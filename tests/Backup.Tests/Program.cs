using System.Data;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Data.Sqlite;
using ShadowViewer.Sdk.Backup;

var testDirectory = Path.Combine(Path.GetTempPath(), "ShadowViewer-Backup-Tests-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(testDirectory);
var tests = new (string Name, Func<Task> Run)[]
{
    ("001 and 002 snapshots preserve independent versions and mixed formats", VersionsAndDetection),
    ("Binary tables preserve primitive values, schema, nulls and composite keys", TableRoundTrip),
    ("Schema-only tables and empty archives round trip", EmptyData),
    ("JSON values, arrays, scalars and UTF-8 BOM are automatically recognized", JsonDetection),
    ("SQLite backup includes WAL data, indexes, triggers, foreign keys and history", SqliteRoundTrip),
    ("Unknown versions and corrupted payloads are rejected before restoring data", InvalidArchives),
    ("Names, byte limits, malformed data and cancellation are enforced", LimitsAndCancellation),
    ("Additional serializers participate in detection and round trip", CustomFormats)
};
var failed = 0;
try
{
    foreach (var (name, run) in tests)
    {
        try { await run(); Console.WriteLine("PASS " + name); }
        catch (Exception exception) { failed++; Console.Error.WriteLine("FAIL " + name + "\n" + exception); }
    }
}
finally
{
    SqliteConnection.ClearAllPools();
    Directory.Delete(testDirectory, recursive: true);
}
Console.WriteLine($"{tests.Length - failed}/{tests.Length} backup checks passed.");
return failed == 0 ? 0 : 1;

void Check(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
}

async Task ExpectFailure<TException>(Func<Task> action) where TException : Exception
{
    try { await action(); }
    catch (TException) { return; }
    throw new InvalidOperationException($"Expected {typeof(TException).Name}.");
}

async Task<MemoryStream> Export(string version, params BackupEntry[] entries)
{
    var stream = new MemoryStream();
    await new BackupService().ExportAsync(stream, version, entries);
    stream.Position = 0;
    return stream;
}

async Task VersionsAndDetection()
{
    using var json = JsonDocument.Parse("{\"readingMode\":2,\"tag\":\"中文\"}");
    var table = new DataTable("Books");
    table.Columns.Add("Id", typeof(long));
    table.Columns.Add("Name", typeof(string));
    table.PrimaryKey = [table.Columns[0]];
    table.Rows.Add(700000000000000003L, "book");
    var raw = new byte[] { 0, 255, 1, 2, 0 };
    using var first = await Export("001", new("library", table), new("settings", json), new("other/blob", raw));
    var firstDocument = await new BackupService().ImportAsync(first);
    Check(firstDocument.Version == "001" && firstDocument.CreatedAt.Offset == TimeSpan.Zero, "Snapshot metadata changed.");
    Check(firstDocument.Entries.Select(x => x.Name).SequenceEqual(new[] { "library", "settings", "other/blob" }), "Entry names/order changed.");
    Check(firstDocument.Entries[0].Data is DataTable && firstDocument.Entries[1].Data is JsonElement &&
          ((byte[])firstDocument.Entries[2].Data).SequenceEqual(raw), "Mixed format detection failed.");
    table.Rows.Add(700000000000000004L, "second");
    using var second = await Export("002", new BackupEntry("library", table));
    var secondDocument = await new BackupService().ImportAsync(second);
    Check(secondDocument.Version == "002" && ((DataTable)secondDocument.Entries[0].Data).Rows.Count == 2 &&
          ((DataTable)firstDocument.Entries[0].Data).Rows.Count == 1, "Snapshots are not independent.");
    Check(first.CanRead && second.CanRead, "The service closed a caller-owned stream.");
    var repeated = JsonSerializer.Serialize(new { text = new string('x', 200_000) });
    using var compressed = await Export("003", new BackupEntry("large", repeated));
    Check(compressed.Length < Encoding.UTF8.GetByteCount(repeated) / 10, "Repeated data did not compress.");
}

async Task TableRoundTrip()
{
    var values = new object[]
    {
        true, byte.MaxValue, sbyte.MinValue, short.MinValue, ushort.MaxValue, int.MinValue, uint.MaxValue,
        long.MaxValue, ulong.MaxValue, 1.25f, double.Epsilon, 79228162514264337593543950335m, '中', "中文\0text",
        new DateTime(2026, 10, 9, 7, 1, 2, DateTimeKind.Utc), new DateTimeOffset(2026, 10, 9, 15, 1, 2, TimeSpan.FromHours(8)),
        TimeSpan.FromTicks(-12345), Guid.NewGuid(), new byte[] { 0, 255, 0, 128 }
    };
    var table = new DataTable("Typed") { CaseSensitive = true, Namespace = "urn:shadow", Prefix = "sv" };
    for (var index = 0; index < values.Length; index++)
    {
        var column = table.Columns.Add("C" + index, values[index].GetType());
        column.Caption = "Caption " + index;
        if (values[index] is DateTime) column.DateTimeMode = DataSetDateTime.Utc;
        if (values[index] is string) column.MaxLength = 100;
    }
    table.Columns[5].DefaultValue = 42;
    table.PrimaryKey = [table.Columns[5], table.Columns[7]];
    table.Constraints.Add(new UniqueConstraint("unique_text", table.Columns[13]));
    table.Rows.Add(values);
    var nullableValues = (object[])values.Clone();
    nullableValues[5] = 123;
    nullableValues[7] = 456L;
    nullableValues[13] = DBNull.Value;
    table.Rows.Add(nullableValues);
    table.Columns[0].ReadOnly = true;
    using var stream = await Export("001", new BackupEntry("typed", table));
    var restored = (DataTable)(await new BackupService().ImportAsync(stream)).Entries[0].Data;
    Check(restored.TableName == table.TableName && restored.Namespace == table.Namespace && restored.Prefix == table.Prefix &&
          restored.CaseSensitive && restored.Locale.Equals(table.Locale), "Table metadata changed.");
    Check(restored.PrimaryKey.Length == 2 && restored.Constraints.Count == 2 && restored.Columns[0].ReadOnly &&
          restored.Columns[5].DefaultValue.Equals(42) && restored.Columns[13].MaxLength == 100, "Column/key metadata changed.");
    for (var index = 0; index < values.Length; index++)
    {
        Check(restored.Columns[index].DataType == values[index].GetType() && restored.Columns[index].Caption == "Caption " + index, "Column type changed.");
        var actual = restored.Rows[0][index];
        Check(values[index] is byte[] bytes ? ((byte[])actual).SequenceEqual(bytes) : actual.Equals(values[index]), "Table value changed at " + index);
    }
    Check(restored.Rows[1].IsNull(13) && ((DateTime)restored.Rows[0][14]).Kind == DateTimeKind.Utc, "Null/date kind changed.");
    var auto = new DataTable("Auto");
    auto.Columns.Add(new DataColumn("Id", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 100, AutoIncrementStep = 5 });
    auto.Columns.Add("Name", typeof(string));
    auto.Rows.Add(null, "one");
    auto.Rows.Add(null, "two");
    using var autoStream = await Export("001", new BackupEntry("auto", auto));
    var autoRestored = (DataTable)(await new BackupService().ImportAsync(autoStream)).Entries[0].Data;
    var next = autoRestored.Rows.Add(null, "three");
    Check((long)next[0] == 110, "Auto-increment seed/step or next value changed.");
}

async Task EmptyData()
{
    var table = new DataTable("Empty");
    table.Columns.Add("Id", typeof(long));
    table.Columns.Add("Name", typeof(string));
    table.PrimaryKey = [table.Columns[0]];
    using var stream = await Export("001", new BackupEntry("schema", table.Clone()), new("zero-columns", new DataTable()));
    var restored = (DataTable)(await new BackupService().ImportAsync(stream)).Entries[0].Data;
    Check(restored.Rows.Count == 0 && restored.Columns.Count == 2 && restored.PrimaryKey.Length == 1, "Schema-only table changed.");
    using var empty = await Export("002");
    Check((await new BackupService().ImportAsync(empty)).Entries.Count == 0, "Empty archive did not round trip.");
}

async Task JsonDetection()
{
    var sources = new object[]
    {
        " [1, null, true] ", "42", "\"value\"", "null", JsonNode.Parse("{\"mode\":1}")!,
        new byte[] { 0xEF, 0xBB, 0xBF }.Concat(Encoding.UTF8.GetBytes("{\"id\":700000000000000003}")).ToArray()
    };
    using var stream = await Export("001", sources.Select((value, index) => new BackupEntry("json" + index, value)).ToArray());
    var document = await new BackupService().ImportAsync(stream);
    Check(document.Entries.All(x => x.Data is JsonElement), "A JSON representation was not detected.");
    Check(((JsonElement)document.Entries[5].Data).GetProperty("id").GetInt64() == 700000000000000003L, "Large JSON integer lost precision.");
    Check(((JsonElement)document.Entries[3].Data).ValueKind == JsonValueKind.Null, "JSON null changed.");
}

async Task SqliteRoundTrip()
{
    var originalTemporaryFiles = Directory.GetFiles(Path.GetTempPath(), "ShadowViewer-backup-*")
        .Concat(Directory.GetFiles(Path.GetTempPath(), "ShadowViewer-snapshot-*")).ToHashSet();
    var sourcePath = Path.Combine(testDirectory, "source.sqlite");
    using var source = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = sourcePath, Pooling = false }.ToString());
    source.Open();
    using (var command = source.CreateCommand())
    {
        command.CommandText = """
            PRAGMA journal_mode=WAL;
            CREATE TABLE Parent(Id INTEGER PRIMARY KEY, Name TEXT NOT NULL);
            CREATE TABLE Child(Id INTEGER PRIMARY KEY, ParentId INTEGER REFERENCES Parent(Id), Content BLOB);
            CREATE UNIQUE INDEX unique_parent ON Parent(Name);
            CREATE TABLE Audit(Message TEXT);
            CREATE TRIGGER audit_insert AFTER INSERT ON Parent BEGIN INSERT INTO Audit VALUES (NEW.Name); END;
            CREATE TABLE __EFMigrationsHistory_Sdk(MigrationId TEXT PRIMARY KEY, ProductVersion TEXT);
            INSERT INTO __EFMigrationsHistory_Sdk VALUES ('20261009072705_InitialSdk', '8.0.31');
            INSERT INTO Parent VALUES (700000000000000003, 'written-in-WAL');
            INSERT INTO Child VALUES (1, 700000000000000003, X'00FF0100');
            """;
        command.ExecuteNonQuery();
    }
    Check(File.Exists(sourcePath + "-wal"), "WAL fixture was not created.");
    using var stream = await Export("001", new BackupEntry("database", source));
    Check(source.State == ConnectionState.Open, "Export closed an already-open source.");
    var snapshot = (SqliteBackup)(await new BackupService().ImportAsync(stream)).Entries[0].Data;
    using var destination = new SqliteConnection("Data Source=" + Path.Combine(testDirectory, "restored.sqlite"));
    destination.Open();
    using (var obsolete = destination.CreateCommand())
    {
        obsolete.CommandText = "CREATE TABLE Obsolete(Id INTEGER); INSERT INTO Obsolete VALUES (99);";
        obsolete.ExecuteNonQuery();
    }
    destination.Close();
    await snapshot.RestoreAsync(destination);
    Check(destination.State == ConnectionState.Closed, "Restore changed connection ownership.");
    destination.Open();
    object? Scalar(string sql)
    {
        using var command = destination.CreateCommand();
        command.CommandText = sql;
        return command.ExecuteScalar();
    }
    Check((string)Scalar("SELECT Name FROM Parent")! == "written-in-WAL", "Committed WAL data was lost.");
    Check(((byte[])Scalar("SELECT Content FROM Child")!).SequenceEqual(new byte[] { 0, 255, 1, 0 }), "SQLite BLOB changed.");
    Check((long)Scalar("SELECT COUNT(*) FROM sqlite_master WHERE name IN ('unique_parent', 'audit_insert', '__EFMigrationsHistory_Sdk')")! == 3,
        "Database structure was not preserved.");
    Check((long)Scalar("SELECT COUNT(*) FROM pragma_foreign_key_list('Child')")! == 1 &&
          (long)Scalar("SELECT COUNT(*) FROM sqlite_master WHERE name = 'Obsolete'")! == 0, "Full database restore did not preserve schema.");
    using var insertCommand = destination.CreateCommand();
    insertCommand.CommandText = "INSERT INTO Parent VALUES (2, 'trigger-check');";
    insertCommand.ExecuteNonQuery();
    Check((long)Scalar("SELECT COUNT(*) FROM Audit")! == 2, "Restored trigger did not execute.");
    using var again = await Export("002", new BackupEntry("database", snapshot));
    Check((await new BackupService().ImportAsync(again)).Entries[0].Data is SqliteBackup, "Imported snapshot cannot be re-exported.");
    insertCommand.Dispose();
    destination.Close();
    SqliteConnection.ClearPool(destination);
    using var raw = await Export("003", new BackupEntry("raw-db", File.ReadAllBytes(Path.Combine(testDirectory, "restored.sqlite"))));
    Check((await new BackupService().ImportAsync(raw)).Entries[0].Data is SqliteBackup, "Raw SQLite bytes were not detected.");
    var remaining = Directory.GetFiles(Path.GetTempPath(), "ShadowViewer-backup-*")
        .Concat(Directory.GetFiles(Path.GetTempPath(), "ShadowViewer-snapshot-*"));
    Check(!remaining.Except(originalTemporaryFiles).Any(), "SQLite snapshots left temporary database files or WAL sidecars.");
}

async Task<MemoryStream> RewriteArchive(MemoryStream original, Action<JsonObject>? changeManifest = null, bool changePayload = false)
{
    original.Position = 0;
    using var archive = new ZipArchive(original, ZipArchiveMode.Read, leaveOpen: true);
    var result = new MemoryStream();
    using (var changed = new ZipArchive(result, ZipArchiveMode.Create, leaveOpen: true))
    {
        foreach (var entry in archive.Entries)
        {
            using var input = entry.Open();
            using var data = new MemoryStream();
            await input.CopyToAsync(data);
            var bytes = data.ToArray();
            if (entry.FullName == "manifest.json" && changeManifest != null)
            {
                var manifest = JsonNode.Parse(bytes)!.AsObject();
                changeManifest(manifest);
                bytes = JsonSerializer.SerializeToUtf8Bytes(manifest);
            }
            else if (changePayload && entry.FullName == "data/0000.bin") bytes[^1] ^= 1;
            using var output = changed.CreateEntry(entry.FullName).Open();
            await output.WriteAsync(bytes);
        }
    }
    result.Position = 0;
    return result;
}

async Task InvalidArchives()
{
    using var original = await Export("001", new BackupEntry("json", "{\"id\":1}"));
    using var future = await RewriteArchive(original, manifest => manifest["FormatVersion"] = 2);
    await ExpectFailure<InvalidDataException>(() => new BackupService().ImportAsync(future));
    using var tampered = await RewriteArchive(original, changePayload: true);
    await ExpectFailure<InvalidDataException>(() => new BackupService().ImportAsync(tampered));
    using var wrongFormat = await RewriteArchive(original, manifest => manifest["Entries"]![0]!["Format"] = "sqlite");
    await ExpectFailure<InvalidDataException>(() => new BackupService().ImportAsync(wrongFormat));
    using var wrongLength = await RewriteArchive(original, manifest => manifest["Entries"]![0]!["Length"] = 999);
    await ExpectFailure<InvalidDataException>(() => new BackupService().ImportAsync(wrongLength));
    using var wrongPath = await RewriteArchive(original, manifest => manifest["Entries"]![0]!["Path"] = "../outside.bin");
    await ExpectFailure<InvalidDataException>(() => new BackupService().ImportAsync(wrongPath));
    using var unsupported = await RewriteArchive(original, manifest => manifest["Entries"]![0]!["Format"] = "future-format");
    await ExpectFailure<InvalidDataException>(() => new BackupService().ImportAsync(unsupported));
    using var truncated = new MemoryStream(original.ToArray()[..20]);
    await ExpectFailure<InvalidDataException>(() => new BackupService().ImportAsync(truncated));
    using var invalidDatabase = new MemoryStream();
    await ExpectFailure<SqliteException>(() => new BackupService().ExportAsync(invalidDatabase, "001",
        [new("database", Encoding.ASCII.GetBytes("SQLite format 3\0invalid"))]));
}

async Task LimitsAndCancellation()
{
    using var target = new MemoryStream();
    await ExpectFailure<ArgumentException>(() => new BackupService().ExportAsync(target, "001", [new("same", "1"), new("same", "2")]));
    using var original = await Export("001", new BackupEntry("blob", new byte[500]));
    var small = new BackupService(limits: new BackupLimits { MaxEntryBytes = 100, MaxTotalBytes = 200 });
    await ExpectFailure<InvalidDataException>(() => small.ImportAsync(original));
    using var tooBig = new MemoryStream();
    await ExpectFailure<InvalidDataException>(() => small.ExportAsync(tooBig, "001", [new("big", new byte[500])]));
    using var many = await Export("001", new("one", "1"), new("two", "2"));
    await ExpectFailure<InvalidDataException>(() => new BackupService(limits: new BackupLimits { MaxEntries = 1 }).ImportAsync(many));
    using var cancelled = new CancellationTokenSource();
    cancelled.Cancel();
    using var output = new MemoryStream();
    await ExpectFailure<OperationCanceledException>(() => new BackupService().ExportAsync(output, "001", [], cancelled.Token));
    original.Position = 0;
    await ExpectFailure<OperationCanceledException>(() => new BackupService().ImportAsync(original, cancelled.Token));
    Check(output.Length == 0, "Cancelled export changed its destination.");
    using var unsupported = new MemoryStream();
    await ExpectFailure<NotSupportedException>(() => new BackupService().ExportAsync(unsupported, "001", [new("invalid", new object())]));
    using var expression = new MemoryStream();
    var table = new DataTable();
    table.Columns.Add("A", typeof(int));
    table.Columns.Add("B", typeof(int), "A + 1");
    await ExpectFailure<NotSupportedException>(() => new BackupService().ExportAsync(expression, "001", [new("expression", table)]));
    var serializer = new DataTableBackupSerializer();
    var good = await serializer.SerializeAsync(new DataTable("Schema"));
    await ExpectFailure<EndOfStreamException>(() => serializer.DeserializeAsync(good[..^1]));
}

async Task CustomFormats()
{
    var service = new BackupService([new CustomSerializer()]);
    using var stream = new MemoryStream();
    await service.ExportAsync(stream, "001", [new("plugin", new CustomValue(42))]);
    stream.Position = 0;
    var document = await service.ImportAsync(stream);
    Check(document.Entries[0].Data is CustomValue { Number: 42 }, "Custom serializer was not used.");
    stream.Position = 0;
    await ExpectFailure<InvalidDataException>(() => new BackupService().ImportAsync(stream));
    try { _ = new BackupService([new JsonBackupSerializer()]); }
    catch (ArgumentException) { return; }
    throw new InvalidOperationException("Duplicate format identifiers were accepted.");
}

sealed record CustomValue(int Number);

sealed class CustomSerializer : IBackupSerializer
{
    public string FormatId => "test/plugin";
    public bool CanSerialize(object value) => value is CustomValue;
    public bool CanDeserialize(ReadOnlyMemory<byte> payload) => payload.Length == 5 && payload.Span[0] == 0xA5;
    public Task<byte[]> SerializeAsync(object value, CancellationToken cancellationToken = default) =>
        Task.FromResult(new byte[] { 0xA5 }.Concat(BitConverter.GetBytes(((CustomValue)value).Number)).ToArray());
    public Task<object> DeserializeAsync(ReadOnlyMemory<byte> payload, CancellationToken cancellationToken = default) =>
        Task.FromResult<object>(new CustomValue(BitConverter.ToInt32(payload.Span[1..])));
}
