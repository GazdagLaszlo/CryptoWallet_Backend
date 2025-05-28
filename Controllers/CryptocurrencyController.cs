using AutoMapper;
using BCrypt.Net;
using CryptoWallet.Dto;
using CryptoWallet.Model;
using CryptoWallet.Repository;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace CryptoWallet.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CryptocurrencyController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public CryptocurrencyController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async virtual Task<ActionResult<IEnumerable<CryptocurrencyShortResponseDTO>>> ListCryptos()
        {
            var cryptos = await _unitOfWork.CryptocurrencyRepository.GetAllAsync();
            if (cryptos == null)
            {
                return NotFound();
            }

            return _mapper.Map<List<CryptocurrencyShortResponseDTO>>(cryptos);
        }

        [HttpGet("{cryptocurrencyId}")]
        public async virtual Task<ActionResult<Cryptocurrency>> GetCryptocurrency(string cryptocurrencyId)
        {
            var crypto = await _unitOfWork.CryptocurrencyRepository.GetByIdAsync(cryptocurrencyId);
            if(crypto == null)
            {
                return NotFound();
            }

            return _mapper.Map<Cryptocurrency>(crypto);
        }

        [HttpPost]
        public async virtual Task<ActionResult<Cryptocurrency>> CreateCryptocurrency([FromBody] Cryptocurrency cryptocurrency)
        {
            var cryptoExists = await _unitOfWork.CryptocurrencyRepository.GetByIdAsync(cryptocurrency.Id);            
            if (cryptoExists != null)
            {
                return BadRequest($"Cryptocurrency with id - {cryptocurrency.Id} already exists!");
            }
            
            await _unitOfWork.CryptocurrencyRepository.PostAsync(cryptocurrency);
            await _unitOfWork.SaveAsync();

            return Ok(cryptocurrency);
        }

        [HttpDelete("{cryptocurrencyId}")]
        public async virtual Task<IActionResult> DeleteCryptocurrency(string cryptocurrencyId)
        {
            var crypto = await _unitOfWork.CryptocurrencyRepository.GetByIdAsync(cryptocurrencyId);
            if(crypto == null)
            {
                return NotFound();
            }

            _unitOfWork.CryptocurrencyRepository.Delete(crypto);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
