﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZEM_Enterprice_WebApp.Data;

namespace ZEM_Enterprice_WebApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200812095447_dopisDostawa")]
    partial class dopisDostawa
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ZEM_Enterprice_WebApp.Data.MyUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Imie")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Nazwisko")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ZEM_Enterprice_WebApp.Data.Tables.Dostawa", b =>
                {
                    b.Property<Guid>("DostawaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataUtworzenia")
                        .HasColumnType("datetime2");

                    b.Property<int>("Ilosc")
                        .HasColumnType("int");

                    b.Property<string>("Kod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TechnicalCietyWiazka")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Uwagi")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DostawaId");

                    b.HasIndex("TechnicalCietyWiazka");

                    b.ToTable("Dostawa");
                });

            modelBuilder.Entity("ZEM_Enterprice_WebApp.Data.Tables.MissingFromTech", b =>
                {
                    b.Property<string>("Kod")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DataDodania")
                        .HasColumnType("datetime2");

                    b.Property<string>("User")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Kod");

                    b.ToTable("MissingFromTech");
                });

            modelBuilder.Entity("ZEM_Enterprice_WebApp.Data.Tables.PendingChangesTechnical", b =>
                {
                    b.Property<Guid>("PendingChangesTechnicalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BIN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CietyWiazka")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DataModyfikacji")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DataUtworzenia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IlePrzewodow")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IndeksScala")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("KanBan")
                        .HasColumnType("bit");

                    b.Property<string>("KodWiazki")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LiterRodziny")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrzewodCiety")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rodzina")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Uwagi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Wiazka")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PendingChangesTechnicalId");

                    b.ToTable("PendingChangesTechnical");
                });

            modelBuilder.Entity("ZEM_Enterprice_WebApp.Data.Tables.PendingDostawa", b =>
                {
                    b.Property<Guid>("PendingDostawaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<int>("Ilosc")
                        .HasColumnType("int");

                    b.Property<string>("Kod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Uwagi")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PendingDostawaId");

                    b.ToTable("PendingDostawa");
                });

            modelBuilder.Entity("ZEM_Enterprice_WebApp.Data.Tables.ScanCache", b =>
                {
                    b.Property<Guid>("ScanCacheId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("LookedBack")
                        .HasColumnType("bit");

                    b.HasKey("ScanCacheId");

                    b.ToTable("ScanCache");
                });

            modelBuilder.Entity("ZEM_Enterprice_WebApp.Data.Tables.Technical", b =>
                {
                    b.Property<string>("CietyWiazka")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BIN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DataUtworzenia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("IlePrzewodow")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IndeksScala")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("KanBan")
                        .HasColumnType("bit");

                    b.Property<string>("KodWiazki")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LiterRodziny")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrzewodCiety")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rodzina")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Uwagi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Wiazka")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CietyWiazka");

                    b.ToTable("Technical");
                });

            modelBuilder.Entity("ZEM_Enterprice_WebApp.Data.Tables.VTMagazyn", b =>
                {
                    b.Property<Guid>("VTMagazynId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DataDopisu")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataDostawy")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataUtworzenia")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deklarowany")
                        .HasColumnType("bit");

                    b.Property<string>("DokDostawy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DopisanaIlosc")
                        .HasColumnType("int");

                    b.Property<string>("DostawaDopis")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KodCiety")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Komplet")
                        .HasColumnType("bit");

                    b.Property<int>("NaKomplet")
                        .HasColumnType("int");

                    b.Property<int>("NumerKompletu")
                        .HasColumnType("int");

                    b.Property<string>("Pracownik")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SztukiDeklarowane")
                        .HasColumnType("int");

                    b.Property<int>("SztukiZeskanowane")
                        .HasColumnType("int");

                    b.Property<string>("TechnicalCietyWiazka")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Uwagi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Wiazka")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ZeskanowanychNaKomplet")
                        .HasColumnType("int");

                    b.Property<bool>("autocompleteEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("wymuszonaIlosc")
                        .HasColumnType("bit");

                    b.HasKey("VTMagazynId");

                    b.HasIndex("TechnicalCietyWiazka");

                    b.ToTable("VTMagazyn");
                });

            modelBuilder.Entity("ZEM_Enterprice_WebApp.Data.Tables.VtToDostawa", b =>
                {
                    b.Property<Guid>("DostawaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("VTMagazynId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("DostawaId", "VTMagazynId");

                    b.HasIndex("VTMagazynId");

                    b.ToTable("VtToDostawa");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ZEM_Enterprice_WebApp.Data.MyUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ZEM_Enterprice_WebApp.Data.MyUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ZEM_Enterprice_WebApp.Data.MyUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ZEM_Enterprice_WebApp.Data.MyUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ZEM_Enterprice_WebApp.Data.Tables.Dostawa", b =>
                {
                    b.HasOne("ZEM_Enterprice_WebApp.Data.Tables.Technical", "Technical")
                        .WithMany("Dostawas")
                        .HasForeignKey("TechnicalCietyWiazka")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ZEM_Enterprice_WebApp.Data.Tables.VTMagazyn", b =>
                {
                    b.HasOne("ZEM_Enterprice_WebApp.Data.Tables.Technical", "Technical")
                        .WithMany("VTMagazyns")
                        .HasForeignKey("TechnicalCietyWiazka")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ZEM_Enterprice_WebApp.Data.Tables.VtToDostawa", b =>
                {
                    b.HasOne("ZEM_Enterprice_WebApp.Data.Tables.Dostawa", "Dostawa")
                        .WithMany("Skany")
                        .HasForeignKey("DostawaId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ZEM_Enterprice_WebApp.Data.Tables.VTMagazyn", "VTMagazyn")
                        .WithMany("Dostawy")
                        .HasForeignKey("VTMagazynId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
