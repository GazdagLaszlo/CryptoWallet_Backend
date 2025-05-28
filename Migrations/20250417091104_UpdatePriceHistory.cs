using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWallet.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePriceHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceHistory_Cryptocurrencies_CryptocurrencyId",
                table: "PriceHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceHistory",
                table: "PriceHistory");

            migrationBuilder.RenameTable(
                name: "PriceHistory",
                newName: "priceHistories");

            migrationBuilder.RenameIndex(
                name: "IX_PriceHistory_CryptocurrencyId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_priceHistories_Cryptocurrencies_CryptocurrencyId",
                table: "priceHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_priceHistories",
                table: "priceHistories");

            migrationBuilder.RenameTable(
                name: "priceHistories",
                newName: "PriceHistory");

            migrationBuilder.RenameIndex(
                name: "IX_priceHistories_CryptocurrencyId",
                table: "PriceHistory",
                newName: "IX_PriceHistory_CryptocurrencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceHistory",
                table: "PriceHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceHistory_Cryptocurrencies_CryptocurrencyId",
                table: "PriceHistory",
                column: "CryptocurrencyId",
                principalTable: "Cryptocurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
