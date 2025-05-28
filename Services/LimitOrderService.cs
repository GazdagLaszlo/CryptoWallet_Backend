using AutoMapper;
using BCrypt.Net;
using CryptoWallet.Dto;
using CryptoWallet.Model;
using CryptoWallet.Repository;
using System.Linq.Expressions;

namespace CryptoWallet.Services
{
    public interface ILimitOrderService
    {
        Task ExecuteLimitOrdersAsync(Cryptocurrency currentPrice);
    }
    public class LimitOrderService : ILimitOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LimitOrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task ExecuteLimitOrdersAsync(Cryptocurrency cryptocurrency)
        {
            var filter = (Expression<Func<LimitOrder, bool>>)(x => x.IsActive);
            var orders = await _unitOfWork.LimitOrderRepository.GetAllAsync(filter);

            foreach (var order in orders)
            {
                if (order.OrderType == OrderType.Buy)
                {
                    if (order.CryptocurrencyId == cryptocurrency.Id)
                    {
                        if (order.LimitPrice >= cryptocurrency.Price)
                        {
                            var user = await _unitOfWork.UserRepository.GetByIdAsync(order.UserId,
                                includeReferences: ["Wallet"],
                                includeCollections: ["Wallet.Cryptocurrencies"]);

                            double expense = order.LimitPrice * order.Amount;
                            if (user.Wallet.Balance >= expense)
                            {
                                user.Wallet.Balance -= expense;

                                order.IsActive = false;

                                TransactionCreateDTO transactionCreate = new TransactionCreateDTO
                                {
                                    UserId = order.UserId,
                                    CryptocurrencyId = cryptocurrency.Id,
                                    Amount = order.Amount,
                                };

                                //Creating transaction
                                var transaction = _mapper.Map<Model.Transaction>(transactionCreate);
                                transaction.TimeStamp = DateTime.UtcNow;
                                transaction.Price = order.LimitPrice;
                                transaction.Type = TransactionType.Buy;
                                await _unitOfWork.TransactionRepository.PostAsync(transaction);

                                //Checking if the user's wallet contains the cryptocurrency
                                var cryptoExistsInWallet = user.Wallet.Cryptocurrencies.FirstOrDefault(x => x.CryptocurrencyId == cryptocurrency.Id);
                                if (cryptoExistsInWallet != null)
                                {
                                    var portfolioItem = await _unitOfWork.PortfolioItemRepository.GetByIdAsync(cryptoExistsInWallet.Id);
                                    portfolioItem.Amount += transactionCreate.Amount;
                                }
                                else
                                {
                                    var portfolioItem = _mapper.Map<PortfolioItem>(new PortfolioItemCreateDto
                                    {
                                        WalletId = user.Wallet.Id,
                                        CryptocurrencyId = cryptocurrency.Id,
                                        Amount = order.Amount,
                                    });
                                    user.Wallet.Cryptocurrencies.Add(portfolioItem);
                                }

                                await _unitOfWork.SaveAsync();                                
                            }
                        }
                    }
                }
                else if (order.OrderType == OrderType.Sell)
                {
                    if (order.CryptocurrencyId == cryptocurrency.Id)
                    {
                        if (order.LimitPrice <= cryptocurrency.Price)
                        {
                            var user = await _unitOfWork.UserRepository.GetByIdAsync(order.UserId,
                                includeReferences: ["Wallet"],
                                includeCollections: ["Wallet.Cryptocurrencies"]);                            

                            order.IsActive = false;
                            
                            TransactionCreateDTO transactionCreate = new TransactionCreateDTO
                            {
                                UserId = order.UserId,
                                CryptocurrencyId = cryptocurrency.Id,
                                Amount = order.Amount,
                            };

                            //Creating transaction
                            var transaction = _mapper.Map<Model.Transaction>(transactionCreate);
                            transaction.TimeStamp = DateTime.UtcNow;
                            transaction.Price = order.LimitPrice;
                            transaction.Type = TransactionType.Sell;
                            await _unitOfWork.TransactionRepository.PostAsync(transaction);                                                      


                            var cryptoExistsInWallet = user.Wallet.Cryptocurrencies.FirstOrDefault(x => x.CryptocurrencyId == cryptocurrency.Id);
                            if (cryptoExistsInWallet != null)
                            {
                                var portfolioItem = await _unitOfWork.PortfolioItemRepository.GetByIdAsync(cryptoExistsInWallet.Id);
                                if (order.Amount <= portfolioItem.Amount)
                                {
                                    portfolioItem.Amount -= order.Amount;
                                    user.Wallet.Balance += order.Amount * cryptocurrency.Price;

                                    if (portfolioItem.Amount == 0)
                                    {
                                        _unitOfWork.PortfolioItemRepository.Delete(portfolioItem);
                                    }
                                }                                
                            }

                            await _unitOfWork.SaveAsync();                            
                        }
                    }
                }
            }
        }
    }
}
