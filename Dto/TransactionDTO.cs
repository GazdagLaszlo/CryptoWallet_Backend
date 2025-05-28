using CryptoWallet.Model;

namespace CryptoWallet.Dto
{
    public class TransactionCreateDTO
    {        
        public int UserId { get; set; }
        public string CryptocurrencyId { get; set; }
        public double Amount { get; set; }
    }

    public class TransactionResponseDTO
    {
        public int Id { get; set; }
        public string CryptocurrencyId { get; set; }
        public string Type { get; set; }
        public DateTime TimeStamp { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }       
        public double TotalExpense { get; set; }
        
    }

    public class TransactionShortResponseDTO
    {
        public string CryptocurrencyId { get; set; }
        public double Amount { get; set; }
        public string Type { get; set; }
    }
}
