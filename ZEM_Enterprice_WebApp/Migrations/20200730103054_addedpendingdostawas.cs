using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZEM_Enterprice_WebApp.Migrations
{
    public partial class addedpendingdostawas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovedNotifications",
                columns: table => new
                {
                    MovedNotificationsId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovedNotifications", x => x.MovedNotificationsId);
                });

            migrationBuilder.CreateTable(
                name: "PendingDostawa",
                columns: table => new
                {
                    KodIloscData = table.Column<string>(nullable: false),
                    Kod = table.Column<string>(nullable: true),
                    Ilosc = table.Column<int>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    Uwagi = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingDostawa", x => x.KodIloscData);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovedNotifications");

            migrationBuilder.DropTable(
                name: "PendingDostawa");
        }
    }
}
