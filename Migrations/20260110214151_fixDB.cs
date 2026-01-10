using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TicketShop.Migrations
{
    /// <inheritdoc />
    public partial class fixDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FAQs",
                columns: new[] { "Id", "Intrebare", "Raspuns" },
                values: new object[,]
                {
                    { 1, "retur", "Poți returna biletele cu maxim 24 de ore înainte de eveniment. Banii intră în cont în 3 zile." },
                    { 2, "contact", "Ne poți contacta la support@ticketshop.ro sau la telefon 0770.123.456." },
                    { 3, "buna", "Bună! Sunt asistentul tău roz. Întreabă-mă despre bilete, cont sau evenimente! 💕" },
                    { 4, "cont", "Poți crea un cont gratuit apăsând pe butonul Register din dreapta sus." },
                    { 5, "locatie", "Locația evenimentului este scrisă pe biletul electronic pe care îl primești pe email." }
                });
        }
    }
}
