using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miru.Migrations
{
    /// <inheritdoc />
    public partial class CreateCharactersAndAnimeCharactersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    last_name = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    japanese_name = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    japanese_last_name = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    age = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    gender = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    role = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    image_source = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AnimeCharacters",
                columns: table => new
                {
                    anime_id = table.Column<Guid>(type: "uuid", nullable: false),
                    character_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeCharacters", x => new { x.anime_id, x.character_id });
                    table.ForeignKey(
                        name: "FK_AnimeCharacters_Animes_anime_id",
                        column: x => x.anime_id,
                        principalTable: "Animes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeCharacters_Characters_character_id",
                        column: x => x.character_id,
                        principalTable: "Characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeCharacters_character_id",
                table: "AnimeCharacters",
                column: "character_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeCharacters");

            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
