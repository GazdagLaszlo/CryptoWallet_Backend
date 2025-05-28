using CryptoWallet.Context;
using CryptoWallet.Model;

namespace CryptoWallet.Repository
{
    //Menedzseli a Repositoryk típusait, ezáltal használható egy generikus repository. 
    public class BasicUnitOfWork : IUnitOfWork
    {
        private AppDbContext _context;
        private GenericRepository<User> _userRepository;
        private GenericRepository<Wallet> _walletRepository;
        private GenericRepository<PortfolioItem> _portfolioItemRepository;
        private GenericRepository<Cryptocurrency> _cryptocurrencyRepository;
        private GenericRepository<Transaction> _transactionRepository;
        private GenericRepository<PriceHistory> _priceHistory;
        private GenericRepository<LimitOrder> _limitOrder;

        public BasicUnitOfWork(AppDbContext context)
        {
            _context = context;
            _userRepository = new GenericRepository<User>(context);
            _walletRepository = new GenericRepository<Wallet>(context);
            _portfolioItemRepository = new GenericRepository<PortfolioItem>(context);
            _cryptocurrencyRepository = new GenericRepository<Cryptocurrency>(context);
            _transactionRepository = new GenericRepository<Transaction>(context);
            _priceHistory = new GenericRepository<PriceHistory>(context);
            _limitOrder = new GenericRepository<LimitOrder>(context);
        }
        public IGenericRepository<User> UserRepository => _userRepository;
        public IGenericRepository<Wallet> WalletRepository => _walletRepository;
        public IGenericRepository<PortfolioItem> PortfolioItemRepository => _portfolioItemRepository;
        public IGenericRepository<Cryptocurrency> CryptocurrencyRepository => _cryptocurrencyRepository;
        public IGenericRepository<Transaction> TransactionRepository => _transactionRepository;
        public IGenericRepository<PriceHistory> PriceHistoryRepository => _priceHistory;
        public IGenericRepository<LimitOrder> LimitOrderRepository => _limitOrder;

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
