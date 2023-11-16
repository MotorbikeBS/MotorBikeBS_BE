using API.DTO.CommentDTO;
using API.DTO.MotorbikeDTO;
using API.Utility;
using Azure;
using Core.Models;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

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

			//int age = DateTime.Now.ToLocalTime().Year - dob.Year;
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

			int age = DateTime.Now.ToLocalTime().Year - dob.Year;
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

			if (dob > DateTime.Now.ToLocalTime())
			{
				return ("Ngày sinh không hợp lệ!");
			}

			return "";
		}

		//public static string ContractValidation(string content, List<IFormFile> images)
		//{
		//	if(content == null)
		//	{
		//		return "Vui lòng nhập nội dung hợp đồng!";
		//	}
		//	if(images.Count() < 1)
		//	{
		//		return "Vui lòng chọn hình ảnh hợp đồng!";
		//	}
		//	if(images.Count() > 5)
		//	{
		//		return "Vui lòng chọn tối đa 5 ảnh!";
		//	}
		//	foreach (var img in images)
		//	{
		//		if (!IsImage(img))
		//		{
		//			return "Vui lòng chọn ảnh hợp lệ";
		//		}
		//	}
		//	return "";
		//}

		public static bool IsImage(IFormFile file)
		{
			if (file != null)
			{
				// Check the file's content type
				if (file.ContentType.StartsWith("image/"))
				{
					return true;
				}

				// Check the file extension (you can add more image extensions if needed)
				var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
				var fileExtension = Path.GetExtension(file.FileName);
				return allowedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase);
			}

			return false;
		}
        public static string ValuationValidation(decimal price, string des)
        {
            if(price == default(decimal) || des.Length == 0)
			{
				return "Vui lòng nhập đầy đủ thông tin";
			}
			if(price < 1000000 || price > 500000000)
			{
				return "Vui lòng nhập giá từ 1.000.000Vnđ đến 500.000.000Vnđ";
			}

            return "";
        }

        public static string NegoBookingTimeValidation(DateTime startDate, DateTime endDate, decimal price)
		{
			if (startDate == default(DateTime))
			{
				return "Vui lòng chọn ngày nhận xe!";
			}
			if(startDate == default(DateTime))
			{
                return "Vui lòng chọn ngày kết thúc!";
            }

			
			if(price == default(int))
			{
				return "Vui lòng nhập giá mong muốn";
			}
			if(price < 1000000 || price > 500000000)
			{
				return "Giá thấp nhất là 1.000.000VNĐ";
			}
			//TimeSpan time = bookingDate.TimeOfDay;

			//TimeSpan startTime = TimeSpan.FromHours(7); // 7:00 AM
			//TimeSpan endTime = TimeSpan.FromHours(21);  // 9:00 PM

			if (startDate.Date < DateTime.Now.ToLocalTime().Date)
			{
				return "Vui lòng chọn thời gian trong tương lai!";
			}
			if(startDate.Date >= endDate.Date)
			{
				return "Vui lòng chọn thời gian kết thúc sau thời gian bắt đầu";
			}
			return "";
		}

		public static string BookingNonConsignmentTimeValidation(DateTime bookingDate)
		{
			if (bookingDate == default(DateTime))
			{
				return "Vui lòng chọn ngày hẹn!";
			}

			TimeSpan time = bookingDate.TimeOfDay;

			TimeSpan startTime = TimeSpan.FromHours(7); // 7:00 AM
			TimeSpan endTime = TimeSpan.FromHours(21);  // 9:00 PM

			if (bookingDate < DateTime.Now.ToLocalTime())
			{
				return "Vui lòng chọn thời gian trong tương lai!";
			}

			if (time <= startTime || time >= endTime)
			{
				return "Vui lòng chọn thời gian trong khung giờ 7h-21h!";
			}
			return "";
		}

		public static string PostBoostingValidation(DateTime startDate, DateTime endDate, int level)
		{
            if (startDate == default(DateTime) || endDate == default(DateTime))
            {
                return "Vui lòng chọn ngày!";
            }
			if(startDate.Date < DateTime.Now.ToLocalTime().Date || endDate.Date < DateTime.Now.ToLocalTime().Date)
			{
                return "Vui lòng chọn thời gian trong tương lai!";
            }
			if(startDate > endDate)
			{
				return "Vui lòng chọn ngày kết thúc sau ngày bắt đầu";
			}
			if(level == default(int))
			{
				return "Vui lòng chọn gói đẩy bài";
			}
			if(level <1 || level >3)
			{
				return "Gói đẩy bài phải từ 1 đến 3!";
			}
			return "";
        }

		public static string PaymentValidate(int amount)
		{
            if(amount == default(int))
			{
				return "Vui lòng nhập số tiền!";
			}
			if (amount < 10000 || amount > 10000000)
			{
				return "Vui lòng nhập số tiền từ 10.000VNĐ đến 10.000.000VNĐ!";
			}
			if(amount % 1000 != 0)
			{
				return "Vui lòng nhập số tiền chẵn!";
			}
			return "";

        }

		public static string ReportValidation(int storeId, string title, string des, List<IFormFile> images)
		{
			if (storeId == default(int))
			{
				return "Vui lòng nhập id cửa hàng";
			}
			if(title.Length <= 0 || des.Length <=0)
			{
				return "Vui lòng nhập đủ thông tin!";
			}
			if(title.Length > 200 || des.Length > 500)
			{
				return "Nội dung quá dài!";
			}
            if (images.Count() < 1)
            {
                return "Vui lòng chọn hình ảnh hợp đồng!";
            }
            if (images.Count() > 5)
            {
                return "Vui lòng chọn tối đa 5 ảnh!";
            }
            foreach (var img in images)
            {
                if (!IsImage(img))
                {
                    return "Vui lòng chọn ảnh hợp lệ";
                }
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

            if (motorRegisterDTO.Year > DateTime.Now.ToLocalTime())
            {
                return "Năm đăng ký không được lớn hơn năm hiện tại!";
            }
			
			if (motorRegisterDTO.Price < 0)
            {
                return "Giá tiền không hợp lệ!";
            }
            return "";
        }
        public static string CommentValidation(CommentRegisterDTO comment, int? ReplyID)
        {
            if (comment.Content == null || comment.Rating == null)
            {
                return "Bình luận không được để trống!";
            }

            if (comment.Content.Length < 6)
            {
                return "Nội dung phải từ 6 ký tự trở lên!";
            }

            if (ReplyID == 0 && (comment.Rating < 1 || comment.Rating > 5))
            {
                return "Vui lòng đánh giá từ 1 - 5 sao!";
            }
			else if ( ReplyID != 0 && (comment.Rating < 0 || comment.Rating > 5))
			{
                return "Vui lòng đánh giá từ 0 - 5 sao!";
            }
            return "";
        }
        public static string ValidateTitle<TEntity>(TEntity entity, string entityName = "", int minLength = 4)
        {
            PropertyInfo titleProperty = entity.GetType().GetProperty("Title") ?? entity.GetType().GetProperty("BrandName") ?? entity.GetType().GetProperty("ModelName");

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
            string statusValue = (string)statusProperty.GetValue(entity);
            switch (roleid)
            {
				case SD.Role_Admin_Id:
					statusProperty.SetValue(entity, statusValue);
					break;
				default:
                    statusProperty.SetValue(entity, SD.pending);
                    break;
            }
        }
        public static string PostingTypeByRole(int roleid, int StatusID)
        {
            switch (roleid)
            {
                case SD.Role_Owner_Id:
                    if (StatusID == SD.Status_Posting) { return "Người sở hữu không được có quyền này"; }
                    break;
            }
			return string.Empty;
        }
        public static string RemoveExtraSpaces(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            return Regex.Replace(input.Trim(), @"\s+", " ");
        }
    }
}