using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZEM_Enterprice_WebApp.Migrations
{
    public partial class indexing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VTMagazyn_VTMagazynId",
                table: "VTMagazyn",
                column: "VTMagazynId")
                .Annotation("SqlServer:Include", new[] { "KodCiety", "Wiazka", "DataDostawy" });

            migrationBuilder.CreateIndex(
                name: "IX_Technical_CietyWiazka",
                table: "Technical",
                column: "CietyWiazka")
                .Annotation("SqlServer:Include", new[] { "PrzewodCiety", "Wiazka", "BIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VTMagazyn_VTMagazynId",
                table: "VTMagazyn");

            migrationBuilder.DropIndex(
                name: "IX_Technical_CietyWiazka",
                table: "Technical");
        }
    }
}
