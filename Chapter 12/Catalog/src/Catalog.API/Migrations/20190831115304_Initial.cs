using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                "catalog");

            migrationBuilder.CreateTable(
                "Artists",
                columns: table => new
                {
                    ArtistId = table.Column<Guid>(),
                    ArtistName = table.Column<string>(maxLength: 200)
                },
                constraints: table => { table.PrimaryKey("PK_Artists", x => x.ArtistId); });

            migrationBuilder.CreateTable(
                "Genres",
                columns: table => new
                {
                    GenreId = table.Column<Guid>(),
                    GenreDescription = table.Column<string>(maxLength: 1000)
                },
                constraints: table => { table.PrimaryKey("PK_Genres", x => x.GenreId); });

            migrationBuilder.CreateTable(
                "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(),
                    Name = table.Column<string>(),
                    Description = table.Column<string>(maxLength: 1000),
                    LabelName = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true),
                    PictureUri = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTimeOffset>(),
                    Format = table.Column<string>(nullable: true),
                    AvailableStock = table.Column<int>(),
                    GenreId = table.Column<Guid>(),
                    ArtistId = table.Column<Guid>(),
                    IsInactive = table.Column<bool>()
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
                table: "Items",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                "IX_Items_GenreId",
                table: "Items",
                column: "GenreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Items");

            migrationBuilder.DropTable(
                "Artists");

            migrationBuilder.DropTable(
                "Genres");
        }
    }
}