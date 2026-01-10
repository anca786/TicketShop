using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketShop.Migrations
{
    /// <inheritdoc />
    public partial class biletUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bilete_AspNetUsers_ApplicationUserId",
                table: "Bilete");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Bilete",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Bilete_ApplicationUserId",
                table: "Bilete",
                newName: "IX_Bilete_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bilete_AspNetUsers_UserId",
                table: "Bilete",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bilete_AspNetUsers_UserId",
                table: "Bilete");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Bilete",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Bilete_UserId",
                table: "Bilete",
                newName: "IX_Bilete_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bilete_AspNetUsers_ApplicationUserId",
                table: "Bilete",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
