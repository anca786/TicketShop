using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketShop.Migrations
{
    /// <inheritdoc />
    public partial class pretedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Pret",
                table: "Evenimente",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pret",
                table: "Evenimente");
        }
    }
}
