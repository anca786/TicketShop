using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketShop.Migrations
{
    /// <inheritdoc />
    public partial class AdaugareRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RatingMediu",
                table: "Evenimente",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatingMediu",
                table: "Evenimente");
        }
    }
}
