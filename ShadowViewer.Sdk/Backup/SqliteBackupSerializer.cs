using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShadowViewer.Sdk.Backup;

/// <summary>A complete SQLite snapshot including schema, indexes, triggers and migration history.</summary>
public sealed class SqliteBackup
{
    internal byte[] Bytes { get; }

    internal SqliteBackup(byte[] bytes) => Bytes = bytes;

    /// <summary>Restores this snapshot using SQLite's transactional backup API.</summary>
    /// <remarks>
    /// The destination database is replaced in full. Dispose application DbContexts and pause database
    /// access before restoring. The destination must not have an active transaction. Connection ownership
    /// remains with the caller; a connection opened by this method is closed afterwards.
    /// </remarks>
    public Task RestoreAsync(SqliteConnection destination, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(destination);
        return Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            WithConnection(Bytes, source =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                var wasClosed = destination.State == ConnectionState.Closed;
                try
                {
                    if (wasClosed) destination.Open();
                    // BackupDatabase rolls back the destination if SQLite reports an error.
                    source.BackupDatabase(destination);
                }
                finally
                {
                    if (wasClosed) destination.Close();
                }
            });
        }, cancellationToken);
    }

    internal static void WithConnection(byte[] bytes, Action<SqliteConnection> action)
    {
        var path = Path.Combine(Path.GetTempPath(), "ShadowViewer-backup-" + Guid.NewGuid().ToString("N") + ".sqlite");
        try
        {
            File.WriteAllBytes(path, bytes);
            using var connection = new SqliteConnection(new SqliteConnectionStringBuilder
            {
                DataSource = path, Mode = SqliteOpenMode.ReadOnly, Pooling = false
            }.ToString());
            connection.Open();
            using var check = connection.CreateCommand();
            check.CommandText = "PRAGMA quick_check;";
            using (var reader = check.ExecuteReader())
            {
                if (!reader.Read() || reader.GetString(0) != "ok" || reader.Read())
                    throw new InvalidDataException("The SQLite backup failed its database integrity check.");
            }
            action(connection);
        }
        finally
        {
            DeleteTemporaryDatabase(path);
        }
    }

    internal static void DeleteTemporaryDatabase(string path)
    {
        // A WAL-mode snapshot can create sidecars even when opened read-only.
        foreach (var suffix in new[] { "", "-wal", "-shm", "-journal" })
        {
            var temporaryFile = path + suffix;
            if (File.Exists(temporaryFile)) File.Delete(temporaryFile);
        }
    }
}

/// <summary>Detects SQLite headers and captures live connections through the WAL-aware backup API.</summary>
public sealed class SqliteBackupSerializer : IBackupSerializer
{
    private static readonly byte[] Header = Encoding.ASCII.GetBytes("SQLite format 3\0");

    /// <inheritdoc />
    public string FormatId => "sqlite";

    /// <inheritdoc />
    public bool CanSerialize(object value) => value is SqliteConnection or SqliteBackup ||
        (value is byte[] bytes && CanDeserialize(bytes));

    /// <inheritdoc />
    public bool CanDeserialize(ReadOnlyMemory<byte> payload) => payload.Span.StartsWith(Header);

    /// <inheritdoc />
    /// <remarks>Do not use the source connection concurrently or with an active transaction.</remarks>
    public Task<byte[]> SerializeAsync(object value, CancellationToken cancellationToken = default) => Task.Run(() =>
    {
        cancellationToken.ThrowIfCancellationRequested();
        var bytes = value switch
        {
            SqliteBackup backup => (byte[])backup.Bytes.Clone(),
            byte[] data => (byte[])data.Clone(),
            SqliteConnection connection => Capture(connection, cancellationToken),
            _ => throw new NotSupportedException("Expected a SQLite connection or snapshot.")
        };
        if (!CanDeserialize(bytes)) throw new InvalidDataException("Invalid SQLite backup header.");
        SqliteBackup.WithConnection(bytes, _ => cancellationToken.ThrowIfCancellationRequested());
        return bytes;
    }, cancellationToken);

    /// <inheritdoc />
    public Task<object> DeserializeAsync(ReadOnlyMemory<byte> payload, CancellationToken cancellationToken = default) => Task.Run(() =>
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!CanDeserialize(payload)) throw new InvalidDataException("Invalid SQLite backup header.");
        var bytes = payload.ToArray();
        SqliteBackup.WithConnection(bytes, _ => cancellationToken.ThrowIfCancellationRequested());
        return (object)new SqliteBackup(bytes);
    }, cancellationToken);

    private static byte[] Capture(SqliteConnection source, CancellationToken cancellationToken)
    {
        var path = Path.Combine(Path.GetTempPath(), "ShadowViewer-snapshot-" + Guid.NewGuid().ToString("N") + ".sqlite");
        var wasClosed = source.State == ConnectionState.Closed;
        try
        {
            if (wasClosed) source.Open();
            using (var destination = new SqliteConnection(new SqliteConnectionStringBuilder
            {
                DataSource = path, Pooling = false
            }.ToString()))
            {
                destination.Open();
                source.BackupDatabase(destination);
            }
            cancellationToken.ThrowIfCancellationRequested();
            return File.ReadAllBytes(path);
        }
        finally
        {
            if (wasClosed) source.Close();
            SqliteBackup.DeleteTemporaryDatabase(path);
        }
    }
}
