using System;
using System.Collections.Generic;

namespace ShadowViewer.Sdk.Backup;

/// <summary>A named value; its format is detected from the runtime type and serialized content.</summary>
public sealed record BackupEntry(string Name, object Data);

/// <summary>A validated snapshot. Version is independent of the archive's binary format version.</summary>
public sealed record BackupDocument(string Version, DateTimeOffset CreatedAt, IReadOnlyList<BackupEntry> Entries);

/// <summary>Resource limits used for both export and import, measured before compression.</summary>
public sealed record BackupLimits
{
    /// <summary>Maximum number of data entries.</summary>
    public int MaxEntries { get; init; } = 4096;

    /// <summary>Maximum uncompressed bytes in one entry (256 MiB by default).</summary>
    public long MaxEntryBytes { get; init; } = 256L * 1024 * 1024;

    /// <summary>Maximum total uncompressed data bytes (1 GiB by default).</summary>
    public long MaxTotalBytes { get; init; } = 1024L * 1024 * 1024;
}
