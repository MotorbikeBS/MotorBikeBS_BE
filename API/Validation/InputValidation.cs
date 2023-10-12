using API.DTO.MotorbikeDTO;
using API.Utility;
using Core.Models;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
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

		public static string OwnerRegisterValidation(string phone, string idCard, string address)
		{
			if (phone == null || idCard == null || address == null)
			{
				return "Không được bỏ trống!";
			}

			string phonePattern = @"^[0-9]{10}$";
			if (!Regex.IsMatch(phone, phonePattern))
			{
				return "Số điện thoại không hợp lệ!";
			}

			//if(gender <1 || gender >3)
			//{
			//	return "Giới tính không hợp lệ!";
			//}

			//int age = DateTime.Now.Year - dob.Year;
			//if (age < 16)
			//{
			//	return "Người dùng phải từ 16 tuổi trở lên!";
			//}

			string idCardPattern = @"^[0-9]{12}$";
			if (!Regex.IsMatch(idCard, idCardPattern))
			{
				return "Mã căn cước không hợp lệ!";
			}
			return "";
		}

        public static string StoreRegisterValidation(string storeName, string storePhone, string storeEmail, string address, string taxCode)
        {
            if(storeName == null || storeEmail == null || storePhone == null || address == null || taxCode == null)
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
		//MotorBike
        public static string MotorValidation(MotorUpdateDTO motorRegisterDTO)
        {
            if (motorRegisterDTO.CertificateNumber == null || motorRegisterDTO.MotorName == null ||
                motorRegisterDTO.ModelId == null || motorRegisterDTO.Odo == null ||
                motorRegisterDTO.Year == null || motorRegisterDTO.Price == null)
            {
                return "Vui lòng nhập đầy đủ thông tin!";
            }

            if (motorRegisterDTO.MotorName.Length < 6)
            {
                return "Tên phải từ 6 ký tự trở lên!";
            }

            string phonePattern = @"^[0-9]{6}$";
            if (!Regex.IsMatch(motorRegisterDTO.CertificateNumber, phonePattern))
            {
                return "Số chứng nhận đăng ký xe không hợp lệ!";
            }

            if (motorRegisterDTO.Year > DateTime.Now)
            {
                return "Năm đăng ký không được lớn hơn năm hiện tại!";
            }
			
			if (motorRegisterDTO.Price < 0)
            {
                return "Giá tiền không hợp lệ!";
            }

            // Add other validation checks for ModelId, Odo, Price, and other properties as needed

            return "";
        }
        public static string ValidateTitle<TEntity>(TEntity entity, string entityName = "", int minLength = 4)
        {
            PropertyInfo titleProperty = entity.GetType().GetProperty("Title") ?? entity.GetType().GetProperty("BrandName");

            if (titleProperty == null)
            {
                throw new ArgumentException("The 'Title' or 'BrandName' property does not exist on the entity.");
            }

            var titleValue = titleProperty.GetValue(entity, null);

            if (titleValue == null)
            {
                string entityDisplayName = string.IsNullOrEmpty(entityName) ? "entity" : entityName;
                return $"Tên {entityDisplayName} không được bỏ trống!";
            }

            string title = titleValue.ToString();

            if (title.Length < minLength)
            {
                string entityDisplayName = string.IsNullOrEmpty(entityName) ? "entity" : entityName;
                return $"Tên {entityDisplayName} phải có ít nhất {minLength} ký tự!";
            }

            return string.Empty;
        }
        public static void StatusIfAdmin<TEntity>(TEntity entity, int roleid)
        {
            PropertyInfo statusProperty = entity.GetType().GetProperty("Status");

            if (statusProperty == null)
            {
                throw new ArgumentException("The 'Status' property does not exist on the entity.");
            }
            switch (roleid)
            {
				case SD.Role_Admin_Id:
					statusProperty.SetValue(entity, SD.active);
					break;
				default:
                    statusProperty.SetValue(entity, SD.pending);
                    break;
            }

        }





    }
}