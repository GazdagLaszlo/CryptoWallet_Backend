using AutoMapper;
using CryptoWallet.Dto;
using CryptoWallet.Model;
using CryptoWallet.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace CryptoWallet.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class WalletController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public WalletController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        //GET /api/wallet/{userId}
        [HttpGet("{userId}")]
        public async virtual Task<ActionResult<WalletResponseDTO>> GetWallet(int userId)
        {            
            //Wallet ami a User-hez tartozik
            var filter = (Expression<Func<Wallet, bool>>)(x => x.UserId == userId);
            var wallets = await _unitOfWork.WalletRepository.GetAllAsync(filter, includeCollections: ["Cryptocurrencies.Cryptocurrency"]);
            Wallet? wallet = wallets.FirstOrDefault();
            

            if (wallet == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<WalletResponseDTO>(wallet);

            return Ok(response);
        }
        
        [HttpPut("{userId}")]
        public async virtual Task<IActionResult> UpdateBalance(int userId, [FromBody] WalletBalancePutDTO walletBalancePutDto)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(
                userId,
                includeReferences: ["Wallet"]
            );
            if (user == null)
            {
                return NotFound();
            }
            var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(user.Wallet.Id);

            _mapper.Map(walletBalancePutDto, wallet);
            _unitOfWork.WalletRepository.Update(wallet);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{userId}")]
        public async virtual Task<IActionResult> DeleteWallet(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(
                userId,
                includeReferences: ["Wallet"]
            );
            if (user == null)
            {
                return NotFound();
            }
            var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(user.Wallet.Id);

            _unitOfWork.WalletRepository.Delete(wallet);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
