using System.Text.Json.Serialization;

namespace CryptoWallet.Model
{
    public class Cryptocurrency
    {
        [JsonPropertyName("symbol")]
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("current_price")]
        public double Price { get; set; }
        public long TotalSupply { get; set; }
        public List<PriceHistory> PriceHistories { get; set; } = new List<PriceHistory>();
    }
}
