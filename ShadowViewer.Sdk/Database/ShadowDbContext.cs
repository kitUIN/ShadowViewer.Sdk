using Microsoft.EntityFrameworkCore;
using ShadowViewer.Sdk.Cache;
using ShadowViewer.Sdk.Models;

namespace ShadowViewer.Sdk.Database;

/// <summary>SDK-owned SQLite tables. Create a context per operation using IDbContextFactory.</summary>
public class ShadowDbContext : DbContext
{
    public ShadowDbContext(DbContextOptions<ShadowDbContext> options) : base(options) { }

    protected ShadowDbContext(DbContextOptions options) : base(options) { }

    public DbSet<ShadowTag> ShadowTags => Set<ShadowTag>();
    public DbSet<CacheZip> CacheZips => Set<CacheZip>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureCoreModel(modelBuilder);
    }

    public static void ConfigureCoreModel(ModelBuilder modelBuilder, bool excludeFromMigrations = false)
    {
        var tag = modelBuilder.Entity<ShadowTag>();
        tag.ToTable("ShadowTag", table => table.ExcludeFromMigrations(excludeFromMigrations));
        tag.HasKey(x => x.Id);
        tag.Property(x => x.Id).ValueGeneratedOnAdd().HasValueGenerator<DatabaseIdValueGenerator>();
        tag.Property(x => x.Name).HasMaxLength(255);
        tag.HasIndex(x => x.Name).IsUnique().HasDatabaseName("unique_shadow_tag_name");
        tag.Ignore(x => x.Background);
        tag.Ignore(x => x.Foreground);
        tag.Ignore(x => x.AllowClick);

        var zip = modelBuilder.Entity<CacheZip>();
        zip.ToTable("CacheZip", table => table.ExcludeFromMigrations(excludeFromMigrations));
        zip.HasKey(x => new { x.Md5, x.Sha1 });
        zip.Property(x => x.Md5).HasMaxLength(255);
        zip.Property(x => x.Sha1).HasMaxLength(255);
        zip.Property(x => x.Name).HasMaxLength(1000);
        zip.Property(x => x.Password).HasMaxLength(255);
    }
}
