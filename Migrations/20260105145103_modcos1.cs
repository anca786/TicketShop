using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketShop.Migrations
{
    /// <inheritdoc />
    public partial class modcos1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cosuri_CosId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "CosId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cosuri_CosId",
                table: "AspNetUsers",
                column: "CosId",
                principalTable: "Cosuri",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cosuri_CosId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "CosId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cosuri_CosId",
                table: "AspNetUsers",
                column: "CosId",
                principalTable: "Cosuri",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
