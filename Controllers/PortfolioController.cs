using AutoMapper;
using CryptoWallet.Repository;
using Microsoft.AspNetCore.Mvc;
using CryptoWallet.Services;
using CryptoWallet.Model;
using CryptoWallet.Dto;

namespace CryptoWallet.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PortfolioController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly IPortfolioService _portfolioService;
        public PortfolioController(IUnitOfWork unitOfWork, IMapper mapper, IPortfolioService portfolioService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _portfolioService = portfolioService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<PortfolioResponseDTO>> GetPortfolio(int userId)
        {
            var totalProfit = await _portfolioService.GetProfit(userId);
            //var profitsOnCryptos = await _portfolioService.DetailedGetProfit(userId);

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId,
                includeCollections: ["Wallet.Cryptocurrencies"]
            );
            if (user == null)
            {
                return NotFound($"User with Id - {userId} not exists!");
            }

            var portfolioItems = _mapper.Map<List<PortfolioItemResponseDTO>>(user.Wallet.Cryptocurrencies);

            var response = new PortfolioResponseDTO
            {
                PortfolioItems = portfolioItems,
                TotalProfit = totalProfit,
                DetailedProfit = await _portfolioService.DetailedGetProfit(userId)
            };

            return Ok(response);
        }

        [HttpGet("profit/{userId}")]
        public async Task<IActionResult> GetProfit(int userId)
        {
            var profit = await _portfolioService.GetProfit(userId);
            return Ok(profit);
        }
        
        [HttpGet("profit/details/{userId}")]
        public async Task<ActionResult<Dictionary<string, double>>> DetailedGetProfit(int userId)
        {
            var profit = await _portfolioService.DetailedGetProfit(userId);
            return Ok(profit);
        }
    }
}
