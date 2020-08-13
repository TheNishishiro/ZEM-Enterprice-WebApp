using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZEM_Enterprice_WebApp.Migrations
{
    public partial class techDeleteDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "Technical",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "Technical");
        }
    }
}
