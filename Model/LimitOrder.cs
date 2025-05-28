namespace CryptoWallet.Model
{
    public enum OrderType
    {
        Buy = 0,
        Sell = 1
    }
    public class LimitOrder
    {
        public int Id { get; set; }
        public OrderType OrderType { get; set; }
        public double LimitPrice { get; set; }
        public double Amount { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public string CryptocurrencyId { get; set; }

    }
}
