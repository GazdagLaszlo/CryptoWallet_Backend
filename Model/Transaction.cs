namespace CryptoWallet.Model
{
    public enum TransactionType
    {
        Buy = 0,
        Sell = 1
    }
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }
        public TransactionType Type { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string CryptocurrencyId { get; set; }
        public Cryptocurrency Cryptocurrency { get; set; }
    }
}
