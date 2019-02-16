using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VinylStore.Catalog.API.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catalog");

            migrationBuilder.CreateTable(
                name: "Artist",
                columns: table => new
                {
                    ArtistId = table.Column<Guid>(nullable: false),
                    ArtistName = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Artist", x => x.ArtistId); });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    GenreId = table.Column<Guid>(nullable: false),
                    GenreDescription = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Genre", x => x.GenreId); });

            migrationBuilder.CreateTable(
                name: "Items",
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
                        name: "FK_Items_Artist_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artist",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Genre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genre",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ArtistId",
                schema: "catalog",
                table: "Items",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_GenreId",
                schema: "catalog",
                table: "Items",
                column: "GenreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "Artist");

            migrationBuilder.DropTable(
                name: "Genre");
        }
    }
}