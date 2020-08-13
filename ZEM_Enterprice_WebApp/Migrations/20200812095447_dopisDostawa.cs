using Microsoft.EntityFrameworkCore.Migrations;

namespace ZEM_Enterprice_WebApp.Migrations
{
    public partial class dopisDostawa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "VTMagazyn");

            migrationBuilder.AddColumn<string>(
                name: "DostawaDopis",
                table: "VTMagazyn",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DostawaDopis",
                table: "VTMagazyn");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "VTMagazyn",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
