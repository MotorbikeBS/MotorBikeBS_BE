using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace API.Validation
{
    public static class InputValidation
    {
		public static bool SpecialCharacters(string? value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return false;
			}

			// Regular expression pattern that allows Vietnamese characters and spaces
			string vietnamesePattern = @"^[a-zA-ZÀ-ỹ\s]+$";

			if (!Regex.IsMatch(value, vietnamesePattern))
			{
				return false; // Input contains invalid characters
			}

			// Additional validation logic can be added as needed

			return true; // Input is valid
		}

		public static bool EmailValidation(string? email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				return false;
			}

			// Regular expression pattern for basic email validation
			string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

			if (!Regex.IsMatch(email, emailPattern))
			{
				return false; // Invalid email format
			}

			return true; // Email is valid
		}
		public static string CleanAndFormatUserName(string? value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return value;
			}

			// Replace consecutive spaces with a single space and trim leading/trailing spaces
			return Regex.Replace(value.Trim(), @"\s+", " ");
		}

		public static string RegisterValidation(string name, string email, string password, string passwordConfirm)
		{
			if(name == null || email == null || password == null || passwordConfirm == null)
			{
				return "Vui lòng nhập đầy đủ thông tin!";
			}

			string vietnamesePattern = @"^[a-zA-ZÀ-ỹ\s]+$";

			if (!Regex.IsMatch(name, vietnamesePattern))
			{
				return "Tên không chưa kí tự đặc biệt"; // Input contains invalid characters
			}

			string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

			if (!Regex.IsMatch(email, emailPattern))
			{
				return "Email không hợp lệ";
			}

			if(password.Length <6 || passwordConfirm.Length <6)
			{
				return ("Độ dài mật khẩu phải từ 6 ký tự trở lên");
			}

			if(!password.Equals(passwordConfirm))
			{
				return ("Mật khẩu xác minh không trùng với mật khẩu");
			}
			return "";
		}
	}
}
