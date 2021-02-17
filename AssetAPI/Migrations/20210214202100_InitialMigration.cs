using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetAPI.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "asset",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset", x => x.id);
                    table.UniqueConstraint("AlternateKey_AssetName", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "asset_property",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    assetid = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    value = table.Column<bool>(type: "bit", nullable: false),
                    time_stamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_property", x => x.id);
                    table.UniqueConstraint("AlternateKey_AssetPropertyName", x => x.name);
                    table.ForeignKey(
                        name: "FK_asset_property_asset_assetid",
                        column: x => x.assetid,
                        principalTable: "asset",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_asset_property_assetid",
                table: "asset_property",
                column: "assetid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "asset_property");

            migrationBuilder.DropTable(
                name: "asset");
        }
    }
}
