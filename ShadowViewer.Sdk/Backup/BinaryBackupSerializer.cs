using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShadowViewer.Sdk.Backup;

/// <summary>Fallback for opaque byte arrays; SQLite, tables and JSON are detected first.</summary>
public sealed class BinaryBackupSerializer : IBackupSerializer
{
    private static readonly byte[] Header = Encoding.ASCII.GetBytes("SVBY\x01");

    /// <inheritdoc />
    public string FormatId => "binary";

    /// <inheritdoc />
    public bool CanSerialize(object value) => value is byte[];

    /// <inheritdoc />
    public bool CanDeserialize(ReadOnlyMemory<byte> payload) => payload.Span.StartsWith(Header);

    /// <inheritdoc />
    public Task<byte[]> SerializeAsync(object value, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var bytes = (byte[])value;
        var result = new byte[checked(Header.Length + bytes.Length)];
        Header.CopyTo(result, 0);
        bytes.CopyTo(result, Header.Length);
        return Task.FromResult(result);
    }

    /// <inheritdoc />
    public Task<object> DeserializeAsync(ReadOnlyMemory<byte> payload, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!CanDeserialize(payload)) throw new System.IO.InvalidDataException("Invalid binary backup header.");
        return Task.FromResult<object>(payload[Header.Length..].ToArray());
    }
}
