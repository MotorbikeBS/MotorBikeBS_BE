using API.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace API.DTO.UserDTO
{
    public class RegisterDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string PasswordConfirmed { get; set; }

    }
}