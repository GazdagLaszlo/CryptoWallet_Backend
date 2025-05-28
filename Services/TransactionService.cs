using AutoMapper;
using CryptoWallet.Dto;
using CryptoWallet.Model;
using CryptoWallet.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWallet.Services
{
    public class TransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /*
        public async Task<ActionResult<>> TransactionValidation(TransactionCreateDTO transactionCreate)
        {

        }
        */
    }
}
