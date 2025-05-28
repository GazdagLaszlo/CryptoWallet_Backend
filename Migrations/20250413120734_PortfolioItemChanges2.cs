using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWallet.Migrations
{
    /// <inheritdoc />
    public partial class PortfolioItemChanges2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioItems_Cryptocurrencies_CryptoCurrencyId",
                table: "PortfolioItems");

            migrationBuilder.RenameColumn(
                name: "CryptoCurrencyId",
                table: "PortfolioItems",
                newName: "CryptocurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioItems_CryptoCurrencyId",
                table: "PortfolioItems",
                newName: "IX_PortfolioItems_CryptocurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioItems_Cryptocurrencies_CryptocurrencyId",
                table: "PortfolioItems",
                column: "CryptocurrencyId",
                principalTable: "Cryptocurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioItems_Cryptocurrencies_CryptocurrencyId",
                table: "PortfolioItems");

            migrationBuilder.RenameColumn(
                name: "CryptocurrencyId",
                table: "PortfolioItems",
                newName: "CryptoCurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioItems_CryptocurrencyId",
                table: "PortfolioItems",
                newName: "IX_PortfolioItems_CryptoCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioItems_Cryptocurrencies_CryptoCurrencyId",
                table: "PortfolioItems",
                column: "CryptoCurrencyId",
                principalTable: "Cryptocurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
