using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using System;
using CryptoWallet.Model;

namespace CryptoWallet.Repository
{
    //Menedzseli a Repositoryk típusait, ezáltal használható egy generikus repository. 
    public interface IUnitOfWork
    {
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Wallet> WalletRepository { get; }
        IGenericRepository<PortfolioItem> PortfolioItemRepository { get; }
        IGenericRepository<Cryptocurrency> CryptocurrencyRepository { get; }
        IGenericRepository<Transaction> TransactionRepository { get; }
        IGenericRepository<PriceHistory> PriceHistoryRepository { get; }
        IGenericRepository<LimitOrder> LimitOrderRepository { get; }

        void Save();
        Task SaveAsync();
    }
}
