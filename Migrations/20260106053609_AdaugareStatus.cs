using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketShop.Migrations
{
    /// <inheritdoc />
    public partial class AdaugareStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MotivRespingere",
                table: "Evenimente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizatorId",
                table: "Evenimente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Evenimente",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotivRespingere",
                table: "Evenimente");

            migrationBuilder.DropColumn(
                name: "OrganizatorId",
                table: "Evenimente");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Evenimente");
        }
    }
}
