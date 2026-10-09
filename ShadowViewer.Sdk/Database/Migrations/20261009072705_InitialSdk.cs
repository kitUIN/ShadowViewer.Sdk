using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShadowViewer.Sdk.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialSdk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CacheZip",
                columns: table => new
                {
                    Md5 = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Sha1 = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    CachePath = table.Column<string>(type: "TEXT", nullable: true),
                    ComicId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CacheZip", x => new { x.Md5, x.Sha1 });
                });

            migrationBuilder.CreateTable(
                name: "ShadowTag",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    BackgroundHex = table.Column<string>(type: "TEXT", nullable: false),
                    ForegroundHex = table.Column<string>(type: "TEXT", nullable: false),
                    Icon = table.Column<string>(type: "TEXT", nullable: true),
                    PluginId = table.Column<string>(type: "TEXT", nullable: false),
                    TagType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShadowTag", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "unique_shadow_tag_name",
                table: "ShadowTag",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CacheZip");

            migrationBuilder.DropTable(
                name: "ShadowTag");
        }
    }
}
