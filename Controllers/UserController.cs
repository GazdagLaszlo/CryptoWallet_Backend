using AutoMapper;
using CryptoWallet.Context;
using CryptoWallet.Dto;
using CryptoWallet.Model;
using CryptoWallet.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CryptoWallet.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public UserController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("register")]
        public async virtual Task<IActionResult> Register([FromBody] UserCreateDTO userCreateDTO)
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            var emailInUse = users.Any(x => x.Email == userCreateDTO.Email);

            if (emailInUse)
            {
                return BadRequest("Email address already in use!");
            }

            var user = _mapper.Map<User>(userCreateDTO);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userCreateDTO.Password);

            user.Wallet = new Wallet
            {
                Balance = 100,
                //UserId = user.Id
            };

            await _unitOfWork.UserRepository.PostAsync(user);
            await _unitOfWork.SaveAsync();

            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var filter = (Expression<Func<User, bool>>)(x => x.Email == userLoginDTO.Email);
            var user = await _unitOfWork.UserRepository.GetAllAsync(filter);

            if(user == null || !BCrypt.Net.BCrypt.Verify(userLoginDTO.Password, user.First().PasswordHash))
            {
                return NotFound("Invalid credentials!");
            }
            else
            {
                return Ok("Login successful!");
            }
        }

        //GET : api/users/1
        [HttpGet("{id}")]
        public async virtual Task<ActionResult<UserResponseDTO>> GetUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(
                id,
                includeReferences: ["Wallet"]
                //includeCollections: ["Transactions" ]
            );

            var response = _mapper.Map<UserResponseDTO>(user);

            if(user == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async virtual Task<IActionResult> UpdateUser(int id, [FromBody] UserPutDTO userPutDto)
        {
            User? user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _mapper.Map(userPutDto, user);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userPutDto.Password);

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async virtual Task<IActionResult> DeleteUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with Id - {id} not exists!");
            }

            _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
