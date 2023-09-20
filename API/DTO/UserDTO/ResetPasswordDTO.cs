using System.ComponentModel.DataAnnotations;

namespace API.DTO.UserDTO
{
	public class ResetPasswordDTO
	{
		//[Required]
		//public string Token { get; set; }
		[Required, MinLength(6)]
		public string Password { get; set; }
		[Required, Compare("Password")]
		public string PasswordConfirmed { get; set; }
	}
}
