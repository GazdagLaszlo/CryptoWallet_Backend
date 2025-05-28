using CryptoWallet.Model;

namespace CryptoWallet.Dto
{
    public class LimitOrderCreateDTO
    {
        public int UserId { get; set; }
        public string CryptocurrencyId { get; set; }
        public double Amount { get; set; }
        public double LimitPrice { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class LimitOrderResponseDTO
    {
        public string CryptocurrencyId { get; set; }
        public double Amount { get; set; }
        public double LimitPrice { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string OrderType { get; set; }
    }
}
