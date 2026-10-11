using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShadowViewer.Sdk.Backup;

/// <summary>ZIP/Deflate container with a versioned manifest and SHA-256 integrity checks.</summary>
public sealed class BackupService : IBackupService
{
    private const int FormatVersion = 1;
    private const string Signature = "ShadowViewer.Backup";
    private const string ManifestPath = "manifest.json";
    private const int MaxManifestBytes = 4 * 1024 * 1024;
    private readonly IBackupSerializer[] serializers;
    private readonly BackupLimits limits;

    /// <summary>Creates the default table, SQLite, JSON and binary serializers plus optional custom formats.</summary>
    public BackupService(IEnumerable<IBackupSerializer>? additionalSerializers = null, BackupLimits? limits = null)
    {
        this.limits = limits ?? new BackupLimits();
        if (this.limits.MaxEntries <= 0 || this.limits.MaxEntryBytes <= 0 ||
            this.limits.MaxEntryBytes > int.MaxValue || this.limits.MaxTotalBytes < this.limits.MaxEntryBytes)
            throw new ArgumentOutOfRangeException(nameof(limits));
        serializers = (additionalSerializers ?? []).Concat(new IBackupSerializer[]
        {
            new DataTableBackupSerializer(), new SqliteBackupSerializer(), new JsonBackupSerializer(),
            new BinaryBackupSerializer()
        }).ToArray();
        if (serializers.Any(x => x == null || string.IsNullOrWhiteSpace(x.FormatId)) ||
            serializers.Select(x => x.FormatId).Distinct(StringComparer.Ordinal).Count() != serializers.Length)
            throw new ArgumentException("Serializer format identifiers must be nonempty and unique.", nameof(additionalSerializers));
    }

    /// <inheritdoc />
    public async Task ExportAsync(Stream destination, string version, IEnumerable<BackupEntry> entries,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(destination);
        ArgumentException.ThrowIfNullOrWhiteSpace(version);
        ArgumentNullException.ThrowIfNull(entries);
        if (!destination.CanWrite || (destination.CanSeek && (destination.Position != 0 || destination.Length != 0)))
            throw new ArgumentException("Export requires an empty writable stream.", nameof(destination));
        cancellationToken.ThrowIfCancellationRequested();
        var manifest = new Manifest
        {
            Signature = Signature, FormatVersion = FormatVersion, Version = version,
            CreatedAt = DateTimeOffset.UtcNow
        };
        var names = new HashSet<string>(StringComparer.Ordinal);
        long total = 0;
        using var archive = new ZipArchive(destination, ZipArchiveMode.Create, leaveOpen: true);
        foreach (var entry in entries)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (entry == null || string.IsNullOrWhiteSpace(entry.Name) || entry.Data == null || !names.Add(entry.Name))
                throw new ArgumentException("Entries require unique nonempty names and nonnull data.", nameof(entries));
            if (manifest.Entries.Count >= limits.MaxEntries)
                throw new InvalidDataException("The backup has too many entries.");
            var serializer = serializers.FirstOrDefault(x => x.CanSerialize(entry.Data))
                ?? throw new NotSupportedException($"No backup serializer accepts {entry.Data.GetType()}.");
            var bytes = await serializer.SerializeAsync(entry.Data, cancellationToken).ConfigureAwait(false);
            total += bytes.LongLength;
            if (bytes.LongLength > limits.MaxEntryBytes || total > limits.MaxTotalBytes)
                throw new InvalidDataException("The backup exceeds the configured size limit.");
            var path = $"data/{manifest.Entries.Count:D4}.bin";
            var compressed = archive.CreateEntry(path, CompressionLevel.SmallestSize);
            await using (var output = compressed.Open())
                await output.WriteAsync(bytes, cancellationToken).ConfigureAwait(false);
            manifest.Entries.Add(new ManifestEntry
            {
                Name = entry.Name, Path = path, Format = serializer.FormatId, Length = bytes.LongLength,
                Sha256 = Convert.ToHexString(SHA256.HashData(bytes))
            });
        }
        var manifestBytes = JsonSerializer.SerializeToUtf8Bytes(manifest);
        if (manifestBytes.Length > MaxManifestBytes)
            throw new InvalidDataException("The backup manifest is too large.");
        await using var manifestStream = archive.CreateEntry(ManifestPath, CompressionLevel.SmallestSize).Open();
        await manifestStream.WriteAsync(manifestBytes, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<BackupDocument> ImportAsync(Stream source, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (!source.CanRead) throw new ArgumentException("Import requires a readable stream.", nameof(source));
        cancellationToken.ThrowIfCancellationRequested();
        // ZipArchive buffers nonseekable inputs itself. Require seeking to keep resource limits enforceable.
        if (!source.CanSeek) throw new ArgumentException("Import requires a seekable stream.", nameof(source));
        using var archive = new ZipArchive(source, ZipArchiveMode.Read, leaveOpen: true);
        if (archive.Entries.Count > limits.MaxEntries + 1L ||
            archive.Entries.Select(x => x.FullName).Distinct(StringComparer.Ordinal).Count() != archive.Entries.Count)
            throw new InvalidDataException("The backup contains too many or duplicate archive entries.");
        var manifestEntry = archive.GetEntry(ManifestPath) ?? throw new InvalidDataException("Missing backup manifest.");
        Manifest manifest;
        try
        {
            manifest = JsonSerializer.Deserialize<Manifest>(await ReadEntryAsync(manifestEntry, MaxManifestBytes,
                cancellationToken).ConfigureAwait(false)) ?? throw new InvalidDataException("Missing backup metadata.");
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException("Invalid backup manifest.", exception);
        }
        if (manifest.Signature != Signature || manifest.FormatVersion != FormatVersion)
            throw new InvalidDataException("Unsupported backup file or format version. Use a compatible SDK.");
        if (string.IsNullOrWhiteSpace(manifest.Version) || manifest.CreatedAt == default || manifest.Entries == null ||
            manifest.Entries.Count > limits.MaxEntries || archive.Entries.Count != manifest.Entries.Count + 1)
            throw new InvalidDataException("Invalid backup metadata.");

        var names = new HashSet<string>(StringComparer.Ordinal);
        long total = 0;
        // Validate all declarations before allocating or deserializing any data entry.
        for (var index = 0; index < manifest.Entries.Count; index++)
        {
            var item = manifest.Entries[index];
            if (item == null || string.IsNullOrWhiteSpace(item.Name) || !names.Add(item.Name) ||
                item.Path != $"data/{index:D4}.bin" || item.Length < 0 || item.Length > limits.MaxEntryBytes ||
                item.Sha256 == null || item.Sha256.Length != 64 ||
                !serializers.Any(x => x.FormatId == item.Format))
                throw new InvalidDataException("Invalid or unsupported backup entry.");
            total = checked(total + item.Length);
            if (total > limits.MaxTotalBytes || archive.GetEntry(item.Path)?.Length != item.Length)
                throw new InvalidDataException("The backup exceeds its size limit or has an invalid entry length.");
        }
        var result = new List<BackupEntry>();
        foreach (var item in manifest.Entries)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var bytes = await ReadEntryAsync(archive.GetEntry(item.Path)!, limits.MaxEntryBytes,
                cancellationToken).ConfigureAwait(false);
            if (!string.Equals(Convert.ToHexString(SHA256.HashData(bytes)), item.Sha256, StringComparison.OrdinalIgnoreCase))
                throw new InvalidDataException($"Backup entry '{item.Name}' failed its integrity check.");
            var serializer = serializers.First(x => x.FormatId == item.Format);
            if (!serializer.CanDeserialize(bytes))
                throw new InvalidDataException($"Backup entry '{item.Name}' does not match its declared format.");
            var value = await serializer.DeserializeAsync(bytes, cancellationToken).ConfigureAwait(false);
            result.Add(new BackupEntry(item.Name, value));
        }
        return new BackupDocument(manifest.Version, manifest.CreatedAt, result.AsReadOnly());
    }

    private static async Task<byte[]> ReadEntryAsync(ZipArchiveEntry entry, long maximum,
        CancellationToken cancellationToken)
    {
        if (entry.Length < 0 || entry.Length > maximum)
            throw new InvalidDataException("The backup entry exceeds its size limit.");
        await using var input = entry.Open();
        using var output = new MemoryStream();
        var buffer = new byte[81920];
        int count;
        while ((count = await input.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) != 0)
        {
            if (output.Length + count > entry.Length)
                throw new InvalidDataException("The backup entry expanded beyond its declared length.");
            await output.WriteAsync(buffer.AsMemory(0, count), cancellationToken).ConfigureAwait(false);
        }
        if (output.Length != entry.Length) throw new InvalidDataException("Truncated backup entry.");
        return output.ToArray();
    }

    private sealed class Manifest
    {
        public string Signature { get; set; } = "";
        public int FormatVersion { get; set; }
        public string Version { get; set; } = "";
        public DateTimeOffset CreatedAt { get; set; }
        public List<ManifestEntry> Entries { get; set; } = [];
    }

    private sealed class ManifestEntry
    {
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
        public string Format { get; set; } = "";
        public long Length { get; set; }
        public string Sha256 { get; set; } = "";
    }
}
