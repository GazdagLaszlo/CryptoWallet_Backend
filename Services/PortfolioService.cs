using AutoMapper;
using BCrypt.Net;
using CryptoWallet.Model;
using CryptoWallet.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWallet.Services
{
    public interface IPortfolioService
    {
        Task<double> GetProfit(int userId);
        Task<Dictionary<string, double>> DetailedGetProfit(int userId);
    }
    public class PortfolioService : IPortfolioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PortfolioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //The profit calculation works based on average purchase price
        public async Task<double> GetProfit(int userId)
        {
            //(aktuális érték - vásárlási ár) * mennyiség minden kriptovalutára.

            double fullProfit = 0;

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId,
                includeReferences: ["Wallet"],
                includeCollections: ["Transactions", "Wallet.Cryptocurrencies", "Wallet.Cryptocurrencies.Cryptocurrency"]
            );

            var portfolio = user.Wallet.Cryptocurrencies;

            foreach (var portfolioItem in portfolio)
            {
                double profit = await GetProfitOnCrypto(userId, portfolioItem.CryptocurrencyId);
                fullProfit += profit;
            }

            return fullProfit;            
        }
        
        public async Task<Dictionary<string, double>> DetailedGetProfit(int userId)
        {
            Dictionary<string, double> profitOnCrypto = new Dictionary<string, double>();

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId,
                includeReferences: ["Wallet"],
                includeCollections: ["Transactions", "Wallet.Cryptocurrencies", "Wallet.Cryptocurrencies.Cryptocurrency"]
            );

            if (user == null)
            {
                throw new KeyNotFoundException($"User with Id - {userId} not exists!");
            }

            var portfolio = user.Wallet.Cryptocurrencies;

            foreach (var portfolioItem in portfolio)
            {
                double profit = await GetProfitOnCrypto(userId, portfolioItem.CryptocurrencyId);
                profitOnCrypto[portfolioItem.Cryptocurrency.Name] = profit;
            }
            return profitOnCrypto;
        }

        public async Task<double> GetProfitOnCrypto(int userId, string cryptocurrencyId)
        {
            double profit = 0;

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId,
                includeReferences: ["Wallet"],
                includeCollections: ["Transactions", "Wallet.Cryptocurrencies", "Wallet.Cryptocurrencies.Cryptocurrency"]
            );

            if (user.Transactions != null)
            {
                List<Transaction> transactions = user.Transactions
                    .Where(x => x.CryptocurrencyId == cryptocurrencyId)
                    .ToList();
                if(transactions == null)
                {
                    throw new KeyNotFoundException($"The user has no cryptocurrency with Id - {cryptocurrencyId}");
                }

                double sumOfPurchasePrice = 0;
                int numberOfPurchase = 0;
                foreach (Transaction transaction in transactions)
                {
                    if(transaction.Type == TransactionType.Buy)
                    {
                        sumOfPurchasePrice += transaction.Price;
                        numberOfPurchase ++;
                    }
                }
                double averagePurchasePrice = 0;
                if (numberOfPurchase > 0)
                {
                    averagePurchasePrice = sumOfPurchasePrice / numberOfPurchase;
                }

                var portfolioItem = user.Wallet.Cryptocurrencies
                        .Where(x => x.CryptocurrencyId == cryptocurrencyId)
                        .FirstOrDefault();
                if (portfolioItem == null || portfolioItem.Cryptocurrency == null)
                {
                    throw new Exception("Crytocurrency not found in user wallet!");
                }

                //Actual price of the cryptocurrency
                double actualValue = portfolioItem.Cryptocurrency.Price;

                //Profit on crypto in portfolio
                profit = (actualValue - averagePurchasePrice) * portfolioItem.Amount;
            }
            else
            {
                throw new KeyNotFoundException("The user has no transactions!");
            }

            return profit;
        }
    }
}
