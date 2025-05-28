using CryptoWallet.Model;
using System.Diagnostics;

namespace CryptoWallet.Context
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            List<User> users = null;
            if (!(context.Users.Any()))
            {
                users = new List<User>
                {
                    new User { Username = "GazdagLaszlo", Email = "WMW86J@student.uni-pannon.hu", PasswordHash = BCrypt.Net.BCrypt.HashPassword("11111111") },
                    new User { Username = "KisAndrás", Email = "andris14@gmail.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("11111111") },
                };
                context.Users.AddRange(users);
            }

            if (!(context.Cryptocurrencies.Any()))
            {
                var cryptocurrencies = new List<Cryptocurrency>
                {
                    new Cryptocurrency { Id = "BTC", Name = "Bitcoin", Price = 83500, TotalSupply = 19850000 },
                    new Cryptocurrency { Id = "ETH", Name = "Ethereum", Price = 1600, TotalSupply = 120680000 },
                    new Cryptocurrency { Id = "RNDR", Name = "Render-Token", Price = 3.82, TotalSupply = 517716590 },
                    new Cryptocurrency { Id = "HBAR", Name = "Hedera-Hashgraph", Price = 0.1693, TotalSupply = 42228651564 },
                    new Cryptocurrency { Id = "XRP", Name = "Ripple", Price = 2.05, TotalSupply = 58338141684 },
                    new Cryptocurrency { Id = "INJ", Name = "Injective-Protocol", Price = 8.26, TotalSupply = 99970935 },
                    new Cryptocurrency { Id = "ADA", Name = "Cardano", Price = 0.6304, TotalSupply = 35281021302 },
                    new Cryptocurrency { Id = "DOGE", Name = "Dogecoin", Price = 0.1627, TotalSupply = 148819166384 },
                    new Cryptocurrency { Id = "AAVE", Name = "Aave", Price = 139.28, TotalSupply = 15099582 },
                    new Cryptocurrency { Id = "BNB", Name = "BNB", Price = 589.5, TotalSupply = 14247695 },
                    new Cryptocurrency { Id = "FET", Name = "Artificial Superintelligence Alliance", Price = 0.4648, TotalSupply = 2393166613 },
                    new Cryptocurrency { Id = "LINK", Name = "Chainlink", Price = 12.77, TotalSupply = 657099970 },
                    new Cryptocurrency { Id = "SOL", Name = "Solana", Price = 124.29, TotalSupply = 515973348 },
                    new Cryptocurrency { Id = "AVAX", Name = "Avalanche", Price = 19.08, TotalSupply = 415814749 },
                    new Cryptocurrency { Id = "NEAR", Name = "Near Protocol", Price = 2.12, TotalSupply = 1200919359 },
                };
                context.Cryptocurrencies.AddRange(cryptocurrencies);
            }

            context.SaveChanges();

            List<Wallet> wallets = null;
            if (!(context.Wallets.Any()))
            {
                if(users == null)
                {
                    users = context.Users.ToList();
                }

                wallets = new List<Wallet>
                {
                    new Wallet { Balance = 200, UserId = users[0].Id },
                    new Wallet { Balance = 200, UserId = users[1].Id }
                };
                context.Wallets.AddRange(wallets);
            }

            context.SaveChanges();

            if (!(context.PortfolioItems.Any()))
            {
                if(wallets == null)
                {
                    wallets = context.Wallets.ToList();
                }
                var portfolioitems = new[]
                {
                    new PortfolioItem { WalletId = wallets[0].Id, CryptocurrencyId = "BTC", Amount = 0.6 },
                    new PortfolioItem { WalletId = wallets[0].Id, CryptocurrencyId = "ETH", Amount = 0.6 },
                    new PortfolioItem { WalletId = wallets[1].Id, CryptocurrencyId = "DOGE", Amount = 35 },
                };
                context.PortfolioItems.AddRange(portfolioitems);
            }

            context.SaveChanges();
        }
    }
}
