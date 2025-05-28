using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWallet.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_priceHistories_Cryptocurrencies_CryptocurrencyId",
                table: "priceHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_priceHistories",
                table: "priceHistories");

            migrationBuilder.RenameTable(
                name: "priceHistories",
                newName: "PriceHistories");

            migrationBuilder.RenameIndex(
                name: "IX_priceHistories_CryptocurrencyId",
                table: "PriceHistories",
                newName: "IX_PriceHistories_CryptocurrencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceHistories",
                table: "PriceHistories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceHistories_Cryptocurrencies_CryptocurrencyId",
                table: "PriceHistories",
                column: "CryptocurrencyId",
                principalTable: "Cryptocurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceHistories_Cryptocurrencies_CryptocurrencyId",
                table: "PriceHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceHistories",
                table: "PriceHistories");

            migrationBuilder.RenameTable(
                name: "PriceHistories",
                newName: "priceHistories");

            migrationBuilder.RenameIndex(
                name: "IX_PriceHistories_CryptocurrencyId",
                table: "priceHistories",
                newName: "IX_priceHistories_CryptocurrencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_priceHistories",
                table: "priceHistories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_priceHistories_Cryptocurrencies_CryptocurrencyId",
                table: "priceHistories",
                column: "CryptocurrencyId",
                principalTable: "Cryptocurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
