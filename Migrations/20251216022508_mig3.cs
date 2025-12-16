using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessCenterReservationSystem.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HizmetId",
                table: "Hizmetler",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Hizmetler",
                newName: "HizmetId");
        }
    }
}
