using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZEM_Enterprice_WebApp.Migrations
{
    public partial class deliveryIdChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Imie = table.Column<string>(nullable: true),
                    Nazwisko = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MissingFromTech",
                columns: table => new
                {
                    Kod = table.Column<string>(nullable: false),
                    DataDodania = table.Column<DateTime>(nullable: false),
                    User = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissingFromTech", x => x.Kod);
                });

            migrationBuilder.CreateTable(
                name: "PendingChangesTechnical",
                columns: table => new
                {
                    PendingChangesTechnicalId = table.Column<Guid>(nullable: false),
                    CietyWiazka = table.Column<string>(nullable: true),
                    Rodzina = table.Column<string>(nullable: true),
                    Wiazka = table.Column<string>(nullable: true),
                    LiterRodziny = table.Column<string>(nullable: true),
                    KodWiazki = table.Column<string>(nullable: true),
                    IlePrzewodow = table.Column<string>(nullable: true),
                    PrzewodCiety = table.Column<string>(nullable: true),
                    BIN = table.Column<string>(nullable: true),
                    IndeksScala = table.Column<string>(nullable: true),
                    KanBan = table.Column<bool>(nullable: false),
                    Uwagi = table.Column<string>(nullable: true),
                    DataUtworzenia = table.Column<string>(nullable: true),
                    DataModyfikacji = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingChangesTechnical", x => x.PendingChangesTechnicalId);
                });

            migrationBuilder.CreateTable(
                name: "PendingDostawa",
                columns: table => new
                {
                    PendingDostawaId = table.Column<Guid>(nullable: false),
                    Kod = table.Column<string>(nullable: true),
                    Ilosc = table.Column<int>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    Uwagi = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingDostawa", x => x.PendingDostawaId);
                });

            migrationBuilder.CreateTable(
                name: "ScanCache",
                columns: table => new
                {
                    ScanCacheId = table.Column<Guid>(nullable: false),
                    LookedBack = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScanCache", x => x.ScanCacheId);
                });

            migrationBuilder.CreateTable(
                name: "Technical",
                columns: table => new
                {
                    CietyWiazka = table.Column<string>(nullable: false),
                    Rodzina = table.Column<string>(nullable: true),
                    Wiazka = table.Column<string>(nullable: true),
                    LiterRodziny = table.Column<string>(nullable: true),
                    KodWiazki = table.Column<string>(nullable: true),
                    IlePrzewodow = table.Column<string>(nullable: true),
                    PrzewodCiety = table.Column<string>(nullable: true),
                    BIN = table.Column<string>(nullable: true),
                    IndeksScala = table.Column<string>(nullable: true),
                    KanBan = table.Column<bool>(nullable: false),
                    Uwagi = table.Column<string>(nullable: true),
                    DataUtworzenia = table.Column<string>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technical", x => x.CietyWiazka);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dostawa",
                columns: table => new
                {
                    DostawaId = table.Column<Guid>(nullable: false),
                    Kod = table.Column<string>(nullable: true),
                    Ilosc = table.Column<int>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    DataUtworzenia = table.Column<DateTime>(nullable: false),
                    Uwagi = table.Column<string>(nullable: true),
                    TechnicalCietyWiazka = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dostawa", x => x.DostawaId);
                    table.ForeignKey(
                        name: "FK_Dostawa_Technical_TechnicalCietyWiazka",
                        column: x => x.TechnicalCietyWiazka,
                        principalTable: "Technical",
                        principalColumn: "CietyWiazka",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VTMagazyn",
                columns: table => new
                {
                    VTMagazynId = table.Column<Guid>(nullable: false),
                    NumerKompletu = table.Column<int>(nullable: false),
                    SztukiZeskanowane = table.Column<int>(nullable: false),
                    SztukiDeklarowane = table.Column<int>(nullable: false),
                    ZeskanowanychNaKomplet = table.Column<int>(nullable: false),
                    NaKomplet = table.Column<int>(nullable: false),
                    Wiazka = table.Column<string>(nullable: true),
                    KodCiety = table.Column<string>(nullable: true),
                    Pracownik = table.Column<string>(nullable: true),
                    DokDostawy = table.Column<string>(nullable: true),
                    DataUtworzenia = table.Column<DateTime>(nullable: false),
                    DataDostawy = table.Column<DateTime>(nullable: false),
                    Komplet = table.Column<bool>(nullable: false),
                    Deklarowany = table.Column<bool>(nullable: false),
                    autocompleteEnabled = table.Column<bool>(nullable: false),
                    wymuszonaIlosc = table.Column<bool>(nullable: false),
                    DataDopisu = table.Column<DateTime>(nullable: true),
                    DopisanaIlosc = table.Column<int>(nullable: false),
                    Uwagi = table.Column<string>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    TechnicalCietyWiazka = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VTMagazyn", x => x.VTMagazynId);
                    table.ForeignKey(
                        name: "FK_VTMagazyn_Technical_TechnicalCietyWiazka",
                        column: x => x.TechnicalCietyWiazka,
                        principalTable: "Technical",
                        principalColumn: "CietyWiazka",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VtToDostawa",
                columns: table => new
                {
                    DostawaId = table.Column<Guid>(nullable: false),
                    VTMagazynId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VtToDostawa", x => new { x.DostawaId, x.VTMagazynId });
                    table.ForeignKey(
                        name: "FK_VtToDostawa_Dostawa_DostawaId",
                        column: x => x.DostawaId,
                        principalTable: "Dostawa",
                        principalColumn: "DostawaId");
                    table.ForeignKey(
                        name: "FK_VtToDostawa_VTMagazyn_VTMagazynId",
                        column: x => x.VTMagazynId,
                        principalTable: "VTMagazyn",
                        principalColumn: "VTMagazynId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Dostawa_TechnicalCietyWiazka",
                table: "Dostawa",
                column: "TechnicalCietyWiazka");

            migrationBuilder.CreateIndex(
                name: "IX_VTMagazyn_TechnicalCietyWiazka",
                table: "VTMagazyn",
                column: "TechnicalCietyWiazka");

            migrationBuilder.CreateIndex(
                name: "IX_VtToDostawa_VTMagazynId",
                table: "VtToDostawa",
                column: "VTMagazynId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "MissingFromTech");

            migrationBuilder.DropTable(
                name: "PendingChangesTechnical");

            migrationBuilder.DropTable(
                name: "PendingDostawa");

            migrationBuilder.DropTable(
                name: "ScanCache");

            migrationBuilder.DropTable(
                name: "VtToDostawa");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Dostawa");

            migrationBuilder.DropTable(
                name: "VTMagazyn");

            migrationBuilder.DropTable(
                name: "Technical");
        }
    }
}
