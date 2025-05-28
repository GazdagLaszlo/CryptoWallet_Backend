using AutoMapper;
using CryptoWallet.Dto;
using CryptoWallet.Model;
using CryptoWallet.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWallet.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PriceHistoryController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public PriceHistoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{cryptocurrencyId}")]
        public async virtual Task<ActionResult<List<PriceHistoryResponseDTO>>> GetPriceHistory(string cryptocurrencyId)
        {
            var crypto = await _unitOfWork.CryptocurrencyRepository.GetByIdAsync(
                cryptocurrencyId,
                includeCollections: ["PriceHistories"]
            );
            if (crypto == null)
            {
                return NotFound($"Cryptocurrency with id - {cryptocurrencyId} not found!");
            }

            var history = _mapper.Map<List<PriceHistoryResponseDTO>>(crypto.PriceHistories);
            return Ok(history);
        }

        [HttpPut("price")]
        public async virtual Task<IActionResult> UpdatePrice(string cryptocurrencyId, double newPrice)
        {
            var crypto = await _unitOfWork.CryptocurrencyRepository.GetByIdAsync(cryptocurrencyId);
            if(crypto == null)
            {
                return NotFound($"Cryptocurrency with id - {cryptocurrencyId} not found!");
            }
            crypto.Price = newPrice;

            var priceHistory = new PriceHistory
            {
                Price = newPrice,
                Timestamp = DateTime.UtcNow,
                CryptocurrencyId = cryptocurrencyId,
            };
            await _unitOfWork.PriceHistoryRepository.PostAsync(priceHistory);
            

            _unitOfWork.CryptocurrencyRepository.Update(crypto);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
