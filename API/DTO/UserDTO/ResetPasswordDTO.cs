using System.ComponentModel.DataAnnotations;

namespace API.DTO.UserDTO
{
	public class ResetPasswordDTO
	{
		//[Required]
		//public string Token { get; set; }
		[Required, MinLength(6, ErrorMessage = "Mật khẩu phải dài hơn 6 kí tự")]
		public string Password { get; set; }
		[Required, Compare("Password", ErrorMessage = "Không trùng so với mật khẩu")]
		public string PasswordConfirmed { get; set; }
	}
}
