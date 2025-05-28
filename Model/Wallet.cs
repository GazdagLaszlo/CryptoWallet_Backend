namespace CryptoWallet.Model
{
    public class Wallet
    {
        public int Id { get; set; }
        public double Balance { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<PortfolioItem> Cryptocurrencies { get; set; } = new List<PortfolioItem>();
    }
}
