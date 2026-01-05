using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketShop.Migrations
{
    /// <inheritdoc />
    public partial class addFaqtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FAQs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Intrebare = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Raspuns = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FAQs");
        }
    }
}
