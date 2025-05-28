using AutoMapper;
using CryptoWallet.Dto;
using CryptoWallet.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using CryptoWallet.Model;
using System.Runtime.InteropServices;
using System.Linq.Expressions;

namespace CryptoWallet.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class TransactionController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public TransactionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        
        [HttpPost("buy")]
        public async virtual Task<IActionResult> Buy([FromBody] TransactionCreateDTO transactionCreate)
        {
            //Check if user exists
            var user = await _unitOfWork.UserRepository.GetByIdAsync(transactionCreate.UserId);
            if (user == null)
            {
                return NotFound($"User with id - {transactionCreate.UserId} not found");
            }
            //Check if cryptocurrency exists
            var crypto = await _unitOfWork.CryptocurrencyRepository.GetByIdAsync(transactionCreate.CryptocurrencyId);
            if(crypto == null)
            {
                return NotFound($"Cryptocurrency with id - {transactionCreate.CryptocurrencyId} not found");
            }
            //Check if user's wallet exists
            var filter = (Expression<Func<Wallet, bool>>)(x => x.UserId == transactionCreate.UserId);
            var wallets = await _unitOfWork.WalletRepository.GetAllAsync(filter, includeCollections: ["Cryptocurrencies.Cryptocurrency"]);
            Wallet? wallet = wallets.FirstOrDefault();

            if (wallet == null)
            {
                return NotFound($"Wallet with userId - {transactionCreate.UserId} not found!");
            }
            //Check if the amount is greater than 0
            if(transactionCreate.Amount == 0)
            {
                return BadRequest("The amount must be greater than zero!");
            }

            //Check if the user's balance is enough to complete the transaction
            double totalExpense = transactionCreate.Amount * crypto.Price;
            if(wallet.Balance < totalExpense)
            {
                return BadRequest("Insufficient balance to complete the transaction.");
            }

            wallet.Balance -= totalExpense;

            //Creating transaction
            var transaction = _mapper.Map<Model.Transaction>(transactionCreate);
            transaction.TimeStamp = DateTime.UtcNow;
            transaction.Price = crypto.Price;
            transaction.Type = TransactionType.Buy;
            await _unitOfWork.TransactionRepository.PostAsync(transaction);
            
            //Checking if the user's wallet contains the cryptocurrency
            var cryptoExistsInWallet = wallet.Cryptocurrencies.FirstOrDefault(x => x.CryptocurrencyId == transaction.CryptocurrencyId);
            if (cryptoExistsInWallet != null)
            {
                var portfolioItem = await _unitOfWork.PortfolioItemRepository.GetByIdAsync(cryptoExistsInWallet.Id);
                portfolioItem.Amount += transaction.Amount;
            }
            else
            {
                var portfolioItem = _mapper.Map<PortfolioItem>(new PortfolioItemCreateDto
                {
                    WalletId = wallet.Id,
                    CryptocurrencyId = crypto.Id,
                    Amount = transaction.Amount,
                });
                wallet.Cryptocurrencies.Add(portfolioItem);
            }

            await _unitOfWork.SaveAsync();

            //PortfolioItem-ben Object cycle wallet.user.wallet....

            return Ok($"You have successfully purchased {crypto.Name} on price {crypto.Price} for ${totalExpense}.");
        }

        [HttpPost("sell")]
        public async virtual Task<IActionResult> Sell([FromBody] TransactionCreateDTO transactionCreate)
        {
            //Check if user exists
            var user = await _unitOfWork.UserRepository.GetByIdAsync(transactionCreate.UserId);
            if (user == null)
            {
                return NotFound($"User with id - {transactionCreate.UserId} not found");
            }
            //Check if cryptocurrency exists
            var crypto = await _unitOfWork.CryptocurrencyRepository.GetByIdAsync(transactionCreate.CryptocurrencyId);
            if (crypto == null)
            {
                return NotFound($"Cryptocurrency with id - {transactionCreate.CryptocurrencyId} not found");
            }
            //Check if user's wallet exists
            var filter = (Expression<Func<Wallet, bool>>)(x => x.UserId == transactionCreate.UserId);
            var wallets = await _unitOfWork.WalletRepository.GetAllAsync(filter, includeCollections: ["Cryptocurrencies.Cryptocurrency"]);
            Wallet? wallet = wallets.FirstOrDefault();
            //Console.WriteLine("Balanceeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee"+wallet.Balance);

            if (wallet == null)
            {
                return NotFound($"Wallet with userId - {transactionCreate.UserId} not found!");
            }
            //Check if the amount is greater than 0
            if (transactionCreate.Amount == 0)
            {
                return BadRequest("The amount must be greater than zero!");
            }                        

            //Creating transaction
            var transaction = _mapper.Map<Model.Transaction>(transactionCreate);
            transaction.TimeStamp = DateTime.UtcNow;
            transaction.Price = crypto.Price;
            transaction.Type = TransactionType.Sell;
            await _unitOfWork.TransactionRepository.PostAsync(transaction);

            //Checking if the user's wallet contains the cryptocurrency
            var cryptoExistsInWallet = wallet.Cryptocurrencies.FirstOrDefault(x => x.CryptocurrencyId == transaction.CryptocurrencyId);
            if (cryptoExistsInWallet != null)
            {
                var portfolioItem = await _unitOfWork.PortfolioItemRepository.GetByIdAsync(cryptoExistsInWallet.Id);

                //Check if the user has the amount of cryptocurrency to sell
                if (transactionCreate.Amount <= portfolioItem.Amount)
                {
                    portfolioItem.Amount -= transactionCreate.Amount;
                    wallet.Balance += transactionCreate.Amount * crypto.Price;

                    if(portfolioItem.Amount == 0)
                    {
                        _unitOfWork.PortfolioItemRepository.Delete(portfolioItem);
                    }
                }
                else
                {
                    return BadRequest($"You do not have enough {crypto.Name} to sell.");
                }
            }
            else
            {
                return BadRequest($"You cannot sell {crypto.Name}, because it is not present in your portfolio.");
            }

            await _unitOfWork.SaveAsync();

            //PortfolioItem-ben Object cycle wallet.user.wallet....

            return Ok($"You have successfully sold {crypto.Name} on price {crypto.Price} for ${transactionCreate.Amount * crypto.Price}.");
        }

        [HttpGet("{userId}")]
        public async virtual Task<ActionResult<IEnumerable<TransactionShortResponseDTO>>> GetAllTransaction(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with id - {userId} not found");
            }

            var filter = (Expression<Func<Model.Transaction, bool>>)(x => x.UserId == userId);
            var transactions = await _unitOfWork.TransactionRepository.GetAllAsync(filter);

            var response = _mapper.Map<List<TransactionShortResponseDTO>>(transactions);
            return Ok(response);
        }

        [HttpGet("details/{transactionId}")]
        public async virtual Task<ActionResult<TransactionResponseDTO>> GetTransaction(int transactionId)
        {
            var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(transactionId);
            if(transaction == null)
            {
                return NotFound($"Transaction with id - {transactionId} not found");
            }

            var response = _mapper.Map<TransactionResponseDTO>(transaction);
            return Ok(response);
        }
    }
}
