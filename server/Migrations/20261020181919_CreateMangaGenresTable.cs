using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miru.Migrations
{
    /// <inheritdoc />
    public partial class CreateMangaGenresTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MangaGenres",
                columns: table => new
                {
                    manga_id = table.Column<Guid>(type: "uuid", nullable: false),
                    genre_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaGenres", x => new { x.manga_id, x.genre_id });
                    table.ForeignKey(
                        name: "FK_MangaGenres_Genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "Genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaGenres_Mangas_manga_id",
                        column: x => x.manga_id,
                        principalTable: "Mangas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MangaGenres_genre_id",
                table: "MangaGenres",
                column: "genre_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MangaGenres");
        }
    }
}
