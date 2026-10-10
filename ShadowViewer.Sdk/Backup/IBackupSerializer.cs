using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShadowViewer.Sdk.Backup;

/// <summary>Extension point for additional backup formats. Detection must not depend on filenames.</summary>
public interface IBackupSerializer
{
    /// <summary>Stable format identifier persisted in the archive.</summary>
    string FormatId { get; }

    /// <summary>Whether this serializer accepts the supplied runtime value.</summary>
    bool CanSerialize(object value);

    /// <summary>Whether the serialized payload has this format's signature or content.</summary>
    bool CanDeserialize(ReadOnlyMemory<byte> payload);

    /// <summary>Serializes a value. Implementations must observe cancellation.</summary>
    Task<byte[]> SerializeAsync(object value, CancellationToken cancellationToken = default);

    /// <summary>Returns a typed value from a previously validated payload.</summary>
    Task<object> DeserializeAsync(ReadOnlyMemory<byte> payload, CancellationToken cancellationToken = default);
}
