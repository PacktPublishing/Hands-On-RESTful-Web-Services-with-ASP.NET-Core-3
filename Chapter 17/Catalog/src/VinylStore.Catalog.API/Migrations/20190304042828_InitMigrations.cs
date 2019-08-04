using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VinylStore.Catalog.API.Migrations
{
    public partial class InitMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                "catalog");

            migrationBuilder.CreateTable(
                "Artists",
                schema: "catalog",
                columns: table => new
                {
                    ArtistId = table.Column<Guid>(nullable: false),
                    ArtistName = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Artists", x => x.ArtistId); });

            migrationBuilder.CreateTable(
                "Genres",
                schema: "catalog",
                columns: table => new
                {
                    GenreId = table.Column<Guid>(nullable: false),
                    GenreDescription = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Genres", x => x.GenreId); });

            migrationBuilder.CreateTable(
                "Items",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: false),
                    LabelName = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true),
                    PictureUri = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTimeOffset>(nullable: false),
                    Format = table.Column<string>(nullable: true),
                    AvailableStock = table.Column<int>(nullable: false),
                    GenreId = table.Column<Guid>(nullable: false),
                    ArtistId = table.Column<Guid>(nullable: false),
                    IsInactive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        "FK_Items_Artists_ArtistId",
                        x => x.ArtistId,
                        principalSchema: "catalog",
                        principalTable: "Artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Items_Genres_GenreId",
                        x => x.GenreId,
                        principalSchema: "catalog",
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Items_ArtistId",
                schema: "catalog",
                table: "Items",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                "IX_Items_GenreId",
                schema: "catalog",
                table: "Items",
                column: "GenreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Items",
                "catalog");

            migrationBuilder.DropTable(
                "Artists",
                "catalog");

            migrationBuilder.DropTable(
                "Genres",
                "catalog");
        }
    }
}
