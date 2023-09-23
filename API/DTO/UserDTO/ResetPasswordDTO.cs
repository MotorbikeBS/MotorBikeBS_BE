using System.ComponentModel.DataAnnotations;

namespace API.DTO.UserDTO
{
    public class ResetPasswordDTO
    {
        public string Password { get; set; }
        public string PasswordConfirmed { get; set; }
    }

}
