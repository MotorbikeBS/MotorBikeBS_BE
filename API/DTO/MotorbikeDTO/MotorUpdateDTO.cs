using Core.Models;

namespace API.DTO.MotorbikeDTO
{ 
    public partial class MotorUpdateDTO
    {
        public string CertificateNumber { get; set; } = null!;
        public IFormFile? RegistrationImage { get; set; }
        public string? MotorName { get; set; }
        public int? ModelId { get; set; }
        public int? Odo { get; set; }
        public DateTime? Year { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public int? MotorStatusId { get; set; }
        public int? MotorTypeId { get; set; }
        public int? StoreId { get; set; }
    }
}

