using CryptoWallet.Model;

namespace CryptoWallet.Dto
{
    public class WalletResponseDTO
    {
        public double Balance { get; set; }
        public List<PortfolioItemResponseDTO> Cryptocurrencies { get; set; }
    }
    public class WalletBalancePutDTO
    {
        public double Balance { get; set; }
    }
}
