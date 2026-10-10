using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ShadowViewer.Sdk.Backup;

/// <summary>Exports versioned archives and imports their automatically detected, typed contents.</summary>
public interface IBackupService
{
    /// <summary>Writes a compressed archive. The caller supplies a snapshot version, such as "001".</summary>
    /// <remarks>The output stream remains open. Use a new, empty stream for each archive.</remarks>
    Task ExportAsync(Stream destination, string version, IEnumerable<BackupEntry> entries,
        CancellationToken cancellationToken = default);

    /// <summary>Validates an archive before returning its contents; does not modify application data.</summary>
    /// <remarks>The input stream must be seekable and remains open. Read from the beginning of an archive.</remarks>
    Task<BackupDocument> ImportAsync(Stream source, CancellationToken cancellationToken = default);
}
