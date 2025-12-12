using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessCenterReservationSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ad",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Boy",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DogumTarihi",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Kilo",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalonId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Soyad",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AntrenorCalismaSaatleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AntrenorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Gun = table.Column<int>(type: "int", nullable: false),
                    BaslangicSaati = table.Column<TimeSpan>(type: "time", nullable: false),
                    BitisSaati = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntrenorCalismaSaatleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntrenorCalismaSaatleri_AspNetUsers_AntrenorId",
                        column: x => x.AntrenorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Salonlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcilisSaati = table.Column<TimeSpan>(type: "time", nullable: false),
                    KapanisSaati = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salonlar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UzmanlikAlanlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UzmanlikAlanlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hizmetler",
                columns: table => new
                {
                    HizmetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SureDakika = table.Column<int>(type: "int", nullable: false),
                    Ucret = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hizmetler", x => x.HizmetId);
                    table.ForeignKey(
                        name: "FK_Hizmetler_Salonlar_SalonId",
                        column: x => x.SalonId,
                        principalTable: "Salonlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AntrenorHizmetler",
                columns: table => new
                {
                    AntrenorHizmetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AntrenorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HizmetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntrenorHizmetler", x => x.AntrenorHizmetId);
                    table.ForeignKey(
                        name: "FK_AntrenorHizmetler_AspNetUsers_AntrenorId",
                        column: x => x.AntrenorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AntrenorHizmetler_Hizmetler_HizmetId",
                        column: x => x.HizmetId,
                        principalTable: "Hizmetler",
                        principalColumn: "HizmetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AntrenorUzmanlikAlanlari",
                columns: table => new
                {
                    AntrenorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UzmanlikAlaniId = table.Column<int>(type: "int", nullable: false),
                    AntrenorUzmanlikAlaniId = table.Column<int>(type: "int", nullable: false),
                    HizmetId = table.Column<int>(type: "int", nullable: true),
                    UzmanlikAlaniId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntrenorUzmanlikAlanlari", x => new { x.AntrenorId, x.UzmanlikAlaniId });
                    table.ForeignKey(
                        name: "FK_AntrenorUzmanlikAlanlari_AspNetUsers_AntrenorId",
                        column: x => x.AntrenorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AntrenorUzmanlikAlanlari_Hizmetler_HizmetId",
                        column: x => x.HizmetId,
                        principalTable: "Hizmetler",
                        principalColumn: "HizmetId");
                    table.ForeignKey(
                        name: "FK_AntrenorUzmanlikAlanlari_UzmanlikAlanlari_UzmanlikAlaniId",
                        column: x => x.UzmanlikAlaniId,
                        principalTable: "UzmanlikAlanlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AntrenorUzmanlikAlanlari_UzmanlikAlanlari_UzmanlikAlaniId1",
                        column: x => x.UzmanlikAlaniId1,
                        principalTable: "UzmanlikAlanlari",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Randevular",
                columns: table => new
                {
                    RandevuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BaslangicSaati = table.Column<TimeSpan>(type: "time", nullable: false),
                    BitisSaati = table.Column<TimeSpan>(type: "time", nullable: false),
                    UyeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AntrenorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SalonId = table.Column<int>(type: "int", nullable: false),
                    HizmetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Randevular", x => x.RandevuId);
                    table.ForeignKey(
                        name: "FK_Randevular_AspNetUsers_AntrenorId",
                        column: x => x.AntrenorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Randevular_AspNetUsers_UyeId",
                        column: x => x.UyeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Randevular_Hizmetler_HizmetId",
                        column: x => x.HizmetId,
                        principalTable: "Hizmetler",
                        principalColumn: "HizmetId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Randevular_Salonlar_SalonId",
                        column: x => x.SalonId,
                        principalTable: "Salonlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SalonId",
                table: "AspNetUsers",
                column: "SalonId");

            migrationBuilder.CreateIndex(
                name: "IX_AntrenorCalismaSaatleri_AntrenorId",
                table: "AntrenorCalismaSaatleri",
                column: "AntrenorId");

            migrationBuilder.CreateIndex(
                name: "IX_AntrenorHizmetler_AntrenorId",
                table: "AntrenorHizmetler",
                column: "AntrenorId");

            migrationBuilder.CreateIndex(
                name: "IX_AntrenorHizmetler_HizmetId",
                table: "AntrenorHizmetler",
                column: "HizmetId");

            migrationBuilder.CreateIndex(
                name: "IX_AntrenorUzmanlikAlanlari_HizmetId",
                table: "AntrenorUzmanlikAlanlari",
                column: "HizmetId");

            migrationBuilder.CreateIndex(
                name: "IX_AntrenorUzmanlikAlanlari_UzmanlikAlaniId",
                table: "AntrenorUzmanlikAlanlari",
                column: "UzmanlikAlaniId");

            migrationBuilder.CreateIndex(
                name: "IX_AntrenorUzmanlikAlanlari_UzmanlikAlaniId1",
                table: "AntrenorUzmanlikAlanlari",
                column: "UzmanlikAlaniId1");

            migrationBuilder.CreateIndex(
                name: "IX_Hizmetler_SalonId",
                table: "Hizmetler",
                column: "SalonId");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_AntrenorId",
                table: "Randevular",
                column: "AntrenorId");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_HizmetId",
                table: "Randevular",
                column: "HizmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_SalonId",
                table: "Randevular",
                column: "SalonId");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_UyeId",
                table: "Randevular",
                column: "UyeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Salonlar_SalonId",
                table: "AspNetUsers",
                column: "SalonId",
                principalTable: "Salonlar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Salonlar_SalonId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AntrenorCalismaSaatleri");

            migrationBuilder.DropTable(
                name: "AntrenorHizmetler");

            migrationBuilder.DropTable(
                name: "AntrenorUzmanlikAlanlari");

            migrationBuilder.DropTable(
                name: "Randevular");

            migrationBuilder.DropTable(
                name: "UzmanlikAlanlari");

            migrationBuilder.DropTable(
                name: "Hizmetler");

            migrationBuilder.DropTable(
                name: "Salonlar");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SalonId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Ad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Boy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DogumTarihi",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Kilo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SalonId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Soyad",
                table: "AspNetUsers");
        }
    }
}
