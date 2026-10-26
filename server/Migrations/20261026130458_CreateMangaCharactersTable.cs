using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miru.Migrations
{
    /// <inheritdoc />
    public partial class CreateMangaCharactersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "manga_characters",
                columns: table => new
                {
                    manga_id = table.Column<Guid>(type: "uuid", nullable: false),
                    character_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_manga_characters", x => new { x.manga_id, x.character_id });
                    table.ForeignKey(
                        name: "FK_manga_characters_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_manga_characters_mangas_manga_id",
                        column: x => x.manga_id,
                        principalTable: "mangas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_manga_characters_character_id",
                table: "manga_characters",
                column: "character_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "manga_characters");
        }
    }
}
