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

			string vietnamesePattern = @"^[a-zA-ZÀ-ỹ\s]+$";

			if (!Regex.IsMatch(value, vietnamesePattern))
			{
				return false;
			}

			return true;
		}

        public static bool EmailValidation(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

			string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

			if (!Regex.IsMatch(email, emailPattern))
			{
				return false; 
			}

			return true; 
		}
		public static string CleanAndFormatUserName(string? value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return value;
			}

			return Regex.Replace(value.Trim(), @"\s+", " ");
		}

        public static string StoreRegisterValidation(int userId, string storeName, string storePhone, string storeEmail, string address, string taxCode)
        {
            if(userId == null || storeName == null || storeEmail == null || storePhone == null || address == null || taxCode == null)
            {
                return "Không được bỏ trộng!";
            }

            if(storeName.Length <6)
            {
                return "Tên cửa hàng phải từ 6 ký tự trở lên";
            }

			string phonePattern = @"^[0-9]{10}$";
			if (!Regex.IsMatch(storePhone, phonePattern))
			{
				return "Số điện thoại không hợp lệ";
			}

			string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

			if (!Regex.IsMatch(storeEmail, emailPattern))
			{
				return "Email không hợp lệ";
			}

			if (taxCode.Length != 10)
            {
                if(taxCode.Length != 13)
                {
                    return "Mã số thuế không hợp lệ";
                }
            }

            return "";
		}

        public static string RegisterValidation(string name, string email, string password, string passwordConfirm)
        {
            if (name == null || email == null || password == null || passwordConfirm == null)
            {
                return "Vui lòng nhập đầy đủ thông tin!";
            }

			if(name.Length <6)
			{
				return "Tên phải từ 6 ký tự trở lên";
			}

			string vietnamesePattern = @"^[a-zA-ZÀ-ỹ\s]+$";

			if (!Regex.IsMatch(name, vietnamesePattern))
			{
				return "Tên không chưa ký tự đặc biệt";
			}

            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (!Regex.IsMatch(email, emailPattern))
            {
                return "Email không hợp lệ";
            }

            if (password.Length < 6 || passwordConfirm.Length < 6)
            {
                return ("Độ dài mật khẩu phải từ 6 ký tự trở lên");
            }

            if (!password.Equals(passwordConfirm))
            {
                return ("Mật khẩu xác minh không trùng với mật khẩu");
            }
            return "";
        }
    }
}