namespace CryptoWallet.Model
{
    public class PortfolioItem
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }
        public string CryptocurrencyId { get; set; }
        public Cryptocurrency Cryptocurrency { get; set; }
        public double Amount { get; set; }
    }
}
