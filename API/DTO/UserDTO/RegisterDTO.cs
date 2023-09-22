using API.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace API.DTO.UserDTO
{
    public class RegisterDTO
    {
		private string? _userName;
		[Required(AllowEmptyStrings = false, ErrorMessage = "Tên không được bỏ trống")]
		[StringLength(maximumLength: 30, MinimumLength = 3, ErrorMessage = "Độ dài tên phải từ 3 - 30 ký tự")]
		[RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Tên không được chứa ký tự đặc biệt")]
		//public string? UserName { get; set; }
		public string? UserName
		{
			get => _userName;
			set => _userName = CleanAndFormatUserName(value);
		}
		[Required, EmailAddress(ErrorMessage ="Email không được bỏ trống")]
        public string Email { get; set; } = null!;
        [Required, MinLength(6, ErrorMessage ="Mật khẩu phải dài hơn 6 kí tự")]
        public string Password { get; set; } = null!;
        [Required, Compare("Password", ErrorMessage ="Không trùng so với mật khẩu")]
        public string PasswordConfirmed { get; set; }

		private static string CleanAndFormatUserName(string? value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return value;
			}

			// Replace consecutive spaces with a single space and trim leading/trailing spaces
			return Regex.Replace(value.Trim(), @"\s+", " ");
		}
	}
}
