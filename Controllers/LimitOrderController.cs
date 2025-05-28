using AutoMapper;
using Azure;
using CryptoWallet.Dto;
using CryptoWallet.Model;
using CryptoWallet.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace CryptoWallet.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class LimitOrderController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public LimitOrderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("trade/limit-buy")]
        public async Task<ActionResult<LimitOrderCreateDTO>> LimitBuy([FromBody] LimitOrderCreateDTO limitOrderCreateDTO)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(limitOrderCreateDTO.UserId,
                includeReferences: ["Wallet"]);
            if (user == null)
            {
                return NotFound($"User with id - {limitOrderCreateDTO.UserId} not found");
            }
            var userBalance = user.Wallet.Balance;

            double expense = limitOrderCreateDTO.LimitPrice * limitOrderCreateDTO.Amount;
            if(userBalance < expense)
            {
                return BadRequest("Insufficient balance to place the order!");
            }

            var crypto = await _unitOfWork.CryptocurrencyRepository.GetByIdAsync(limitOrderCreateDTO.CryptocurrencyId);
            if (crypto == null)
            {
                return NotFound($"Crypto with id - {limitOrderCreateDTO.CryptocurrencyId} not found");
            }
            if (crypto.Price < limitOrderCreateDTO.LimitPrice)
            {
                return BadRequest("The limit price cannot be higher than the actual cryptocurrency price!");
            }
            var limitOrder = _mapper.Map<LimitOrder>(limitOrderCreateDTO);
            limitOrder.IsActive = true;
            limitOrder.OrderType = OrderType.Buy;
            await _unitOfWork.LimitOrderRepository.PostAsync(limitOrder);
            await _unitOfWork.SaveAsync();

            return Ok(limitOrderCreateDTO);
        }

        [HttpPost("trade/limit-sell")]
        public async Task<ActionResult<LimitOrderCreateDTO>> LimitSell([FromBody] LimitOrderCreateDTO limitOrderCreateDTO)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(limitOrderCreateDTO.UserId,
                includeReferences: ["Wallet"],
                includeCollections: ["Wallet.Cryptocurrencies"]);
            if (user == null)
            {
                return NotFound($"User with id - {limitOrderCreateDTO.UserId} not found");
            }
            var userBalance = user.Wallet.Balance;



            var cryptoExistsInWallet = user.Wallet.Cryptocurrencies.FirstOrDefault(x => x.CryptocurrencyId == limitOrderCreateDTO.CryptocurrencyId);
            if (cryptoExistsInWallet != null)
            {
                if(limitOrderCreateDTO.Amount > cryptoExistsInWallet.Amount)
                {
                    return BadRequest("The amount of cryptocurrency cannot be higher than the amount of the wallet's amount!");
                }

                var crypto = await _unitOfWork.CryptocurrencyRepository.GetByIdAsync(limitOrderCreateDTO.CryptocurrencyId);                
                if (crypto.Price > limitOrderCreateDTO.LimitPrice)
                {
                    return BadRequest("The limit price cannot be lower than the actual cryptocurrency price!");
                }
                else
                {
                    var limitOrder = _mapper.Map<LimitOrder>(limitOrderCreateDTO);
                    limitOrder.IsActive = true;
                    limitOrder.OrderType = OrderType.Sell;
                    await _unitOfWork.LimitOrderRepository.PostAsync(limitOrder);
                    await _unitOfWork.SaveAsync();
                    return Ok(limitOrderCreateDTO);
                }
            }
            else
            {
                return BadRequest("Cryptocurrency not found in user's wallet!");
            }                        
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetOrders(int userId)
        {
            var filter = (Expression<Func<LimitOrder, bool>>)(x => x.UserId == userId && x.IsActive);
            var orders = await _unitOfWork.LimitOrderRepository.GetAllAsync(filter);

            if(orders != null && orders.Any())
            {
                var userOrders = _mapper.Map<List<LimitOrderResponseDTO>>(orders);
                return Ok(userOrders);
            }
            else
            {
                return BadRequest("The user does not have any active orders!");
            }
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var filter = (Expression<Func<LimitOrder, bool>>)(x => x.Id == orderId && x.IsActive);
            var orders = await _unitOfWork.LimitOrderRepository.GetAllAsync(filter);
            var order = orders.FirstOrDefault();

            if (order != null)
            {
                _unitOfWork.LimitOrderRepository.Delete(order);
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            else
            {
                return BadRequest($"Order cannot be found with id - {orderId}");
            }
        }
    }
}
