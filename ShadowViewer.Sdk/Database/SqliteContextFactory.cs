using DryIoc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace ShadowViewer.Sdk.Database;

/// <summary>Immutable options are shared; DbContext instances are never shared.</summary>
public sealed class SqliteContextFactory<TContext> : IDbContextFactory<TContext> where TContext : DbContext
{
    private readonly DbContextOptions<TContext> options;
    private readonly Func<DbContextOptions<TContext>, TContext> create;

    public SqliteContextFactory(string databasePath, string historyTable,
        Func<DbContextOptions<TContext>, TContext> create)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(databasePath))!);
        var connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = databasePath,
            ForeignKeys = true,
            DefaultTimeout = 30
        }.ToString();
        options = new DbContextOptionsBuilder<TContext>()
            .UseSqlite(connectionString, sqlite => sqlite.MigrationsHistoryTable(historyTable))
            .Options;
        this.create = create;
    }

    public TContext CreateDbContext() => create(options);
}

public static class DatabaseRegistration
{
    public static void Register<TContext>(IContainer container, string databasePath, string historyTable,
        Func<DbContextOptions<TContext>, TContext> create) where TContext : DbContext
    {
        container.RegisterInstance<IDbContextFactory<TContext>>(
            new SqliteContextFactory<TContext>(databasePath, historyTable, create));
    }
}
