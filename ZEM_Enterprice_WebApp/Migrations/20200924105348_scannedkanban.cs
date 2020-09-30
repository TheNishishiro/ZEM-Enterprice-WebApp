using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZEM_Enterprice_WebApp.Migrations
{
    public partial class scannedkanban : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScannedKanbans",
                columns: table => new
                {
                    ScannedKanbanId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(nullable: true),
                    Wiazka = table.Column<string>(nullable: true),
                    DataDodania = table.Column<DateTime>(nullable: false),
                    User = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScannedKanbans", x => x.ScannedKanbanId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScannedKanbans");
        }
    }
}
