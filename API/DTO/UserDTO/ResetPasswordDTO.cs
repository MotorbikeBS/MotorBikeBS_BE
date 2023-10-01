using System.ComponentModel.DataAnnotations;

namespace API.DTO.UserDTO
{
    public class ResetPasswordDTO
    {
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmed { get; set; }
    }

}