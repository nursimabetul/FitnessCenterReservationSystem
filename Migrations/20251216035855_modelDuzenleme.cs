using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessCenterReservationSystem.Migrations
{
    /// <inheritdoc />
    public partial class modelDuzenleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RandevuId",
                table: "Randevular",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AntrenorUzmanlikAlaniId",
                table: "AntrenorUzmanlikAlanlari",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AntrenorHizmetId",
                table: "AntrenorHizmetler",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Ad",
                table: "UzmanlikAlanlari",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Adres",
                table: "Salonlar",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Ad",
                table: "Salonlar",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UyeId",
                table: "Randevular",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AntrenorId",
                table: "Randevular",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ad",
                table: "Hizmetler",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Randevular",
                newName: "RandevuId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AntrenorUzmanlikAlanlari",
                newName: "AntrenorUzmanlikAlaniId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AntrenorHizmetler",
                newName: "AntrenorHizmetId");

            migrationBuilder.AlterColumn<string>(
                name: "Ad",
                table: "UzmanlikAlanlari",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Adres",
                table: "Salonlar",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Ad",
                table: "Salonlar",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "UyeId",
                table: "Randevular",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AntrenorId",
                table: "Randevular",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Ad",
                table: "Hizmetler",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
