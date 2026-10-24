using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miru.Migrations
{
    /// <inheritdoc />
    public partial class CreateMangasTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mangas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    japanese_title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    synopsis = table.Column<string>(type: "text", nullable: true),
                    chapters = table.Column<int>(type: "integer", nullable: true),
                    volumes = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    published_from = table.Column<DateOnly>(type: "date", nullable: true),
                    published_to = table.Column<DateOnly>(type: "date", nullable: true),
                    score = table.Column<decimal>(type: "numeric(4,2)", precision: 4, scale: 2, nullable: true),
                    image_source = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mangas", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mangas");
        }
    }
}
