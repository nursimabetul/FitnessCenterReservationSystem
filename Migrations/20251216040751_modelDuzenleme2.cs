using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessCenterReservationSystem.Migrations
{
    /// <inheritdoc />
    public partial class modelDuzenleme2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Duyurular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icerik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SalonId = table.Column<int>(type: "int", nullable: true),
                    OlusturanId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duyurular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Duyurular_AspNetUsers_OlusturanId",
                        column: x => x.OlusturanId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Duyurular_Salonlar_SalonId",
                        column: x => x.SalonId,
                        principalTable: "Salonlar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Haberler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icerik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SalonId = table.Column<int>(type: "int", nullable: true),
                    OlusturanId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Haberler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Haberler_AspNetUsers_OlusturanId",
                        column: x => x.OlusturanId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Haberler_Salonlar_SalonId",
                        column: x => x.SalonId,
                        principalTable: "Salonlar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Kampanyalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SalonId = table.Column<int>(type: "int", nullable: true),
                    SalonId1 = table.Column<int>(type: "int", nullable: true),
                    HizmetId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kampanyalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kampanyalar_Hizmetler_HizmetId",
                        column: x => x.HizmetId,
                        principalTable: "Hizmetler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Kampanyalar_Salonlar_SalonId",
                        column: x => x.SalonId,
                        principalTable: "Salonlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kampanyalar_Salonlar_SalonId1",
                        column: x => x.SalonId1,
                        principalTable: "Salonlar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Duyurular_OlusturanId",
                table: "Duyurular",
                column: "OlusturanId");

            migrationBuilder.CreateIndex(
                name: "IX_Duyurular_SalonId",
                table: "Duyurular",
                column: "SalonId");

            migrationBuilder.CreateIndex(
                name: "IX_Haberler_OlusturanId",
                table: "Haberler",
                column: "OlusturanId");

            migrationBuilder.CreateIndex(
                name: "IX_Haberler_SalonId",
                table: "Haberler",
                column: "SalonId");

            migrationBuilder.CreateIndex(
                name: "IX_Kampanyalar_HizmetId",
                table: "Kampanyalar",
                column: "HizmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Kampanyalar_SalonId",
                table: "Kampanyalar",
                column: "SalonId");

            migrationBuilder.CreateIndex(
                name: "IX_Kampanyalar_SalonId1",
                table: "Kampanyalar",
                column: "SalonId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Duyurular");

            migrationBuilder.DropTable(
                name: "Haberler");

            migrationBuilder.DropTable(
                name: "Kampanyalar");
        }
    }
}
