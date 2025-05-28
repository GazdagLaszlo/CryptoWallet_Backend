using CryptoWallet.Model;

namespace CryptoWallet.Dto
{
    public class PortfolioItemResponseDTO
    {
        public string CryptoName { get; set; }
        public double Amount { get; set; }
    }

    public class PortfolioResponseDTO
    {
        public double TotalProfit { get; set; }
        public List<PortfolioItemResponseDTO> PortfolioItems { get; set; }                
        public Dictionary<string, double> DetailedProfit { get; set; }
    }


    public class PortfolioItemCreateDto
    {
        public int WalletId { get; set; }
        public string CryptocurrencyId { get; set; }
        public double Amount { get; set; }
    }
}
