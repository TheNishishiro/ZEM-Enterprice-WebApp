using Microsoft.EntityFrameworkCore.Migrations;

namespace ZEM_Enterprice_WebApp.Migrations
{
    public partial class addednakomplet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NaKomplet",
                table: "VTMagazyn",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ZeskanowanychNaKomplet",
                table: "VTMagazyn",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NaKomplet",
                table: "VTMagazyn");

            migrationBuilder.DropColumn(
                name: "ZeskanowanychNaKomplet",
                table: "VTMagazyn");
        }
    }
}
