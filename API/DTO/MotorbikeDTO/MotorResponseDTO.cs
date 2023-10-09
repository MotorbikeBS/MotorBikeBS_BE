using API.DTO.UserDTO;
using Core.Models;

namespace API.DTO.MotorbikeDTO
{
    public partial class MotorResponseDTO
    {
        public int MotorId { get; set; }
        public string CertificateNumber { get; set; } = null!;
        public string? RegistrationImage { get; set; }
        public string? MotorName { get; set; }
        public int? Odo { get; set; }
        public DateTime? Year { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }      
        public virtual ModelResponseDTO? Model { get; set; }
        public virtual StatusResponseDTO? MotorStatus { get; set; }
        public virtual TypeResponseDTO? MotorType { get; set; }
        public int? StoreId { get; set; }
        public virtual StoreRegisterDTO? Store { get; set; }
        public virtual UserResponseDTO? Owner { get; set; }
        public virtual ICollection<ImageResponseDTO> MotorbikeImages { get; set; }
    }
}
