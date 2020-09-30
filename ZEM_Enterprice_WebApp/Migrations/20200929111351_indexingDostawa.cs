using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZEM_Enterprice_WebApp.Migrations
{
    public partial class indexingDostawa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Dostawa_DostawaId",
                table: "Dostawa",
                column: "DostawaId")
                .Annotation("SqlServer:Include", new[] { "Data", "Kod" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Dostawa_DostawaId",
                table: "Dostawa");
        }
    }
}
