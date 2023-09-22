using System.ComponentModel.DataAnnotations;

namespace API.DTO.UserDTO
{
<<<<<<< HEAD
    public class ResetPasswordDTO
    {
        //[Required]
        //public string Token { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string PasswordConfirmed { get; set; }
    }
=======
	public class ResetPasswordDTO
	{
		//[Required]
		//public string Token { get; set; }
		[Required, MinLength(6, ErrorMessage = "Mật khẩu phải dài hơn 6 kí tự")]
		public string Password { get; set; }
		[Required, Compare("Password", ErrorMessage = "Không trùng so với mật khẩu")]
		public string PasswordConfirmed { get; set; }
	}
>>>>>>> 7c8e4723d3f076b00636f75946bbabdf4633d694
}
