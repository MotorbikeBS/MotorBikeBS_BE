using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Xml.Linq;

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
            if(userId.ToString() == null || storeName == null || storeEmail == null || storePhone == null || address == null || taxCode == null)
            {
                return "Không được bỏ trống!";
            }

            if(storeName.Length <6)
            {
                return "Tên cửa hàng phải từ 6 ký tự trở lên!";
            }

			string vietnamesePattern = @"^[a-zA-ZÀ-ỹ\s]+$";

			if (!Regex.IsMatch(storeName, vietnamesePattern))
			{
				return "Tên không chưa ký tự đặc biệt!";
			}

			string phonePattern = @"^[0-9]{10}$";
			if (!Regex.IsMatch(storePhone, phonePattern))
			{
				return "Số điện thoại không hợp lệ!";
			}

			string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

			if (!Regex.IsMatch(storeEmail, emailPattern))
			{
				return "Email không hợp lệ!";
			}

			if (taxCode.Length != 10)
            {
                if(taxCode.Length != 13)
                {
                    return "Mã số thuế không hợp lệ!";
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
				return "Tên phải từ 6 ký tự trở lên!";
			}

			string vietnamesePattern = @"^[a-zA-ZÀ-ỹ\s]+$";

			if (!Regex.IsMatch(name, vietnamesePattern))
			{
				return "Tên không chưa ký tự đặc biệt!";
			}

            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (!Regex.IsMatch(email, emailPattern))
            {
                return "Email không hợp lệ!";
            }

            if (password.Length < 6 || passwordConfirm.Length < 6)
            {
                return ("Độ dài mật khẩu phải từ 6 ký tự trở lên!");
            }

            if (!password.Equals(passwordConfirm))
            {
                return ("Mật khẩu xác minh không trùng với mật khẩu!");
            }
            return "";
        }

		public static string UserUpdateValidation(string name, string phone, int gender, DateTime dob, string idCard, string address)
		{
			if (name == null || phone == null || gender.ToString() == null || dob.ToString() == null || address == null || idCard == null)
			{
				return "Vui lòng nhập đầy đủ thông tin!";
			}

			if (name.Length < 6)
			{
				return "Tên phải từ 6 ký tự trở lên!";
			}

			string phonePattern = @"^[0-9]{10}$";
			if (!Regex.IsMatch(phone, phonePattern))
			{
				return "Số điện thoại không hợp lệ!";
			}

			if (gender <1 || gender >3)
			{
				return "Giới tính không hợp lệ!";
			}

			int age = DateTime.Now.Year - dob.Year;
			if (age < 16)
			{
				return "Người dùng phải từ 16 tuổi trở lên!";
			}

			string idCardPattern = @"^[0-9]{12}$";
			if (!Regex.IsMatch(idCard, idCardPattern))
			{
				return "Mã căn cước không hợp lệ!";
			}

			string vietnamesePattern = @"^[a-zA-ZÀ-ỹ\s]+$";

			if (!Regex.IsMatch(name, vietnamesePattern))
			{
				return "Tên không chưa ký tự đặc biệt!";
			}

			if (dob > DateTime.Now)
			{
				return ("Ngày sinh không hợp lệ!");
			}

			return "";
		}
	}
}