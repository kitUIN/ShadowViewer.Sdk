using Microsoft.EntityFrameworkCore.Design;

namespace ShadowViewer.Sdk.Database;

public sealed class ShadowDbContextDesignFactory : IDesignTimeDbContextFactory<ShadowDbContext>
{
    public ShadowDbContext CreateDbContext(string[] args) =>
        new SqliteContextFactory<ShadowDbContext>(args.Length > 0 ? args[0] : "ShadowViewer.design.sqlite",
            "__EFMigrationsHistory_Sdk", options => new ShadowDbContext(options)).CreateDbContext();
}
