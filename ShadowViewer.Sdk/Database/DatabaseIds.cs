using System;

namespace ShadowViewer.Sdk.Database;

/// <summary>Snowflake IDs compatible with existing long keys (worker 4, epoch 2010-01-01).</summary>
public static class DatabaseIds
{
    private static readonly object Sync = new();
    private static readonly long Epoch = new DateTimeOffset(2010, 1, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();
    private static long lastTimestamp;
    private static long sequence;

    public static long Next()
    {
        lock (Sync)
        {
            var timestamp = Math.Max(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), lastTimestamp);
            sequence = timestamp == lastTimestamp ? sequence + 1 : 0;
            if (sequence > 4095)
            {
                timestamp = lastTimestamp + 1;
                sequence = 0;
            }
            lastTimestamp = timestamp;
            return ((timestamp - Epoch) << 22) | (4L << 12) | sequence;
        }
    }
}
