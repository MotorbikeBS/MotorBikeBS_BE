using Core.Models;
using System.ComponentModel.DataAnnotations;

namespace API.DTO.MotorbikeDTO
{ 
    public partial class MotorRegisterDTO
    {
        [RegularExpression(@"^[0-9]{6}$";, ErrorMessage = "Số chứng nhận đăng ký xe không hợp lệ!")]
        public string CertificateNumber { get; set; } = null!;
        public IFormFile? RegistrationImage { get; set; }
        public string? MotorName { get; set; }
        public int? ModelId { get; set; } 
        public int? Odo { get; set; }
        public DateTime? Year { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public int? MotorTypeId { get; set; }
    }
}

