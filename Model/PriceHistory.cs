namespace CryptoWallet.Model
{
    public class PriceHistory
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public DateTime Timestamp { get; set; }
        public string CryptocurrencyId { get; set; }
        public Cryptocurrency Cryptocurrency { get; set; }
    }
}
