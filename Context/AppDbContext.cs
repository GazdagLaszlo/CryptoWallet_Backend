using CryptoWallet.Model;
using Microsoft.EntityFrameworkCore;

namespace CryptoWallet.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }
        public DbSet<PortfolioItem> PortfolioItems { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<PriceHistory> PriceHistories { get; set; }
        public DbSet<LimitOrder> limitOrders { get; set; }

    }
}
