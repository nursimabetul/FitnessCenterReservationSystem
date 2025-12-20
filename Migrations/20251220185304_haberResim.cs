using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessCenterReservationSystem.Migrations
{
    /// <inheritdoc />
    public partial class haberResim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResimYolu",
                table: "Haberler",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResimYolu",
                table: "Haberler");
        }
    }
}
