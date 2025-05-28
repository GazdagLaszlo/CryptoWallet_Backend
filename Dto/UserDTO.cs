using CryptoWallet.Model;
using System.ComponentModel.DataAnnotations;

namespace CryptoWallet.Dto
{
    public class UserCreateDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }

    public class UserPutDTO
    {
        public string? Username { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [MinLength(8)]
        public string? Password { get; set; }
    }
    public class UserLoginDTO
    {
        public string Email { get; set; }
        public string? Password { get; set; }
    }

    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int WalletId { get; set; }
    }
}
