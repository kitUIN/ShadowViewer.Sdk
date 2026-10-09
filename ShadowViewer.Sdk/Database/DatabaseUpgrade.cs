using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShadowViewer.Sdk.Database;

/// <summary>Backs up SQLite before upgrading and adopts pre-EF tables transactionally.</summary>
public static class DatabaseUpgrade
{
    private static readonly object Sync = new();

    public static void Initialize(DbContext context, string historyTable)
    {
        lock (Sync)
        {
            var unknownMigrations = context.Database.GetAppliedMigrations()
                .Except(context.Database.GetMigrations()).ToArray();
            if (unknownMigrations.Length > 0)
                throw new InvalidOperationException("The database was upgraded by a newer component. Install that version before opening this library.");
            var pending = context.Database.GetPendingMigrations().ToArray();
            if (pending.Length == 0) return;
            var connection = (SqliteConnection)context.Database.GetDbConnection();
            connection.Open();
            try
            {
                var tables = ReadStrings(connection, "SELECT name FROM sqlite_master WHERE type = 'table'");
                if (tables.Any(x => !x.StartsWith("sqlite_", StringComparison.Ordinal)))
                {
                    var backupPath = connection.DataSource + "." + context.GetType().Name + "." +
                                     DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + "." + Guid.NewGuid().ToString("N") + ".bak";
                    using var backup = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = backupPath }.ToString());
                    backup.Open();
                    connection.BackupDatabase(backup);
                }
                if (!context.Database.GetAppliedMigrations().Any())
                {
                    AdoptLegacyTables(context, connection, pending[0], historyTable, tables);
                }
                context.Database.Migrate();
            }
            finally
            {
                connection.Close();
            }
        }
    }

    private static void AdoptLegacyTables(DbContext context, SqliteConnection connection,
        string baseline, string historyTable, List<string> existingTables)
    {
        // Build the *initial migration* schema, never the current model. Later migrations run normally.
        var blueprintPath = Path.Combine(Path.GetTempPath(), "ShadowViewer-schema-" + Guid.NewGuid().ToString("N") + ".sqlite");
        try
        {
            using var blueprint = new SqliteConnection(new SqliteConnectionStringBuilder
            {
                DataSource = blueprintPath, ForeignKeys = false, Pooling = false
            }.ToString());
            blueprint.Open();
            Execute(blueprint, context.GetService<IMigrator>().GenerateScript(toMigration: baseline));
            var ownedTables = ReadStrings(blueprint, "SELECT name FROM sqlite_master WHERE type = 'table'")
                .Where(x => x != historyTable && !x.StartsWith("sqlite_", StringComparison.Ordinal)).ToArray();
            if (!ownedTables.Any(existingTables.Contains)) return;

            Execute(connection, "PRAGMA foreign_keys = OFF;");
            using var transaction = connection.BeginTransaction();
            try
            {
                foreach (var table in ownedTables)
                {
                    using var schemaCommand = blueprint.CreateCommand();
                    schemaCommand.CommandText = "SELECT sql FROM sqlite_master WHERE type = 'table' AND name = $name";
                    schemaCommand.Parameters.AddWithValue("$name", table);
                    var schema = (string)schemaCommand.ExecuteScalar()!;
                    var temporaryTable = "__ef_upgrade_" + table;
                    if (existingTables.Contains(table))
                    {
                        var originalColumns = Columns(connection, table, transaction);
                        var targetColumns = Columns(blueprint, table);
                        var unknown = originalColumns.Except(targetColumns, StringComparer.OrdinalIgnoreCase).ToArray();
                        if (unknown.Length > 0)
                            throw new InvalidOperationException($"Cannot migrate {table}: unknown columns {string.Join(", ", unknown)}.");
                        Execute(connection, schema.Replace("CREATE TABLE " + Quote(table), "CREATE TABLE " + Quote(temporaryTable)), transaction);
                        var columns = string.Join(", ", targetColumns.Intersect(originalColumns, StringComparer.OrdinalIgnoreCase).Select(Quote));
                        Execute(connection, $"INSERT INTO {Quote(temporaryTable)} ({columns}) SELECT {columns} FROM {Quote(table)};", transaction);
                        Execute(connection, $"DROP TABLE {Quote(table)}; ALTER TABLE {Quote(temporaryTable)} RENAME TO {Quote(table)};", transaction);
                    }
                    else
                    {
                        Execute(connection, schema, transaction);
                    }
                }
                foreach (var sql in ReadStrings(blueprint, "SELECT sql FROM sqlite_master WHERE type = 'index' AND sql IS NOT NULL"))
                    Execute(connection, sql, transaction);
                Execute(connection, $"CREATE TABLE IF NOT EXISTS {Quote(historyTable)} (MigrationId TEXT NOT NULL PRIMARY KEY, ProductVersion TEXT NOT NULL);", transaction);
                using var history = blueprint.CreateCommand();
                history.CommandText = $"SELECT ProductVersion FROM {Quote(historyTable)} WHERE MigrationId = $id";
                history.Parameters.AddWithValue("$id", baseline);
                using var record = connection.CreateCommand();
                record.Transaction = transaction;
                record.CommandText = $"INSERT INTO {Quote(historyTable)} (MigrationId, ProductVersion) VALUES ($id, $version)";
                record.Parameters.AddWithValue("$id", baseline);
                record.Parameters.AddWithValue("$version", history.ExecuteScalar()!);
                record.ExecuteNonQuery();
                using var check = connection.CreateCommand();
                check.Transaction = transaction;
                check.CommandText = "PRAGMA foreign_key_check;";
                using (var violations = check.ExecuteReader())
                {
                    if (violations.Read()) throw new InvalidOperationException("Legacy database contains invalid foreign keys; upgrade rolled back.");
                }
                transaction.Commit();
            }
            finally
            {
                // Rollback must happen before reenabling foreign keys.
                transaction.Dispose();
                Execute(connection, "PRAGMA foreign_keys = ON;");
            }
        }
        finally
        {
            if (File.Exists(blueprintPath)) File.Delete(blueprintPath);
        }
    }

    private static List<string> Columns(SqliteConnection connection, string table, SqliteTransaction? transaction = null)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = $"PRAGMA table_info({Quote(table)});";
        using var reader = command.ExecuteReader();
        var columns = new List<string>();
        while (reader.Read()) columns.Add(reader.GetString(1));
        return columns;
    }

    private static List<string> ReadStrings(SqliteConnection connection, string sql)
    {
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        using var reader = command.ExecuteReader();
        var values = new List<string>();
        while (reader.Read()) values.Add(reader.GetString(0));
        return values;
    }

    private static string Quote(string identifier) => "\"" + identifier.Replace("\"", "\"\"") + "\"";

    private static void Execute(SqliteConnection connection, string sql, SqliteTransaction? transaction = null)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }
}
