using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miru.Migrations
{
    /// <inheritdoc />
    public partial class CreateGenresAndAnimeGenresTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "anime_genres",
                columns: table => new
                {
                    anime_id = table.Column<Guid>(type: "uuid", nullable: false),
                    genre_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_anime_genres", x => new { x.anime_id, x.genre_id });
                    table.ForeignKey(
                        name: "FK_anime_genres_animes_anime_id",
                        column: x => x.anime_id,
                        principalTable: "animes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_anime_genres_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_anime_genres_genre_id",
                table: "anime_genres",
                column: "genre_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "anime_genres");

            migrationBuilder.DropTable(
                name: "genres");
        }
    }
}
