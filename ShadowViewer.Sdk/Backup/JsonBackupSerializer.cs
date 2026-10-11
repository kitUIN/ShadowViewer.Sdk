using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace ShadowViewer.Sdk.Backup;

/// <summary>Recognizes JSON objects, arrays and scalars and restores them as detached JsonElements.</summary>
public sealed class JsonBackupSerializer : IBackupSerializer
{
    /// <inheritdoc />
    public string FormatId => "json";

    /// <inheritdoc />
    public bool CanSerialize(object value) => value is JsonDocument or JsonElement or JsonNode ||
        (value is string text && IsJson(Encoding.UTF8.GetBytes(text))) ||
        (value is byte[] bytes && CanDeserialize(bytes));

    /// <inheritdoc />
    public bool CanDeserialize(ReadOnlyMemory<byte> payload) => IsJson(payload);

    /// <inheritdoc />
    public Task<byte[]> SerializeAsync(object value, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var bytes = value switch
        {
            JsonDocument document => JsonSerializer.SerializeToUtf8Bytes(document.RootElement),
            JsonElement element => JsonSerializer.SerializeToUtf8Bytes(element),
            JsonNode node => JsonSerializer.SerializeToUtf8Bytes(node),
            string text => Encoding.UTF8.GetBytes(text),
            byte[] data => data,
            _ => throw new NotSupportedException("Expected a JSON value.")
        };
        using var parsed = JsonDocument.Parse(RemoveBom(bytes));
        return Task.FromResult(JsonSerializer.SerializeToUtf8Bytes(parsed.RootElement));
    }

    /// <inheritdoc />
    public Task<object> DeserializeAsync(ReadOnlyMemory<byte> payload, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var document = JsonDocument.Parse(RemoveBom(payload));
        return Task.FromResult<object>(document.RootElement.Clone());
    }

    private static bool IsJson(ReadOnlyMemory<byte> payload)
    {
        try
        {
            using var document = JsonDocument.Parse(RemoveBom(payload));
            return true;
        }
        catch (JsonException) { return false; }
    }

    private static ReadOnlyMemory<byte> RemoveBom(ReadOnlyMemory<byte> payload) =>
        payload.Span.StartsWith(new byte[] { 0xEF, 0xBB, 0xBF }) ? payload[3..] : payload;
}
