using Core.Models;

namespace API.DTO.MotorbikeDTO
{
    public partial class BrandRegisterDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = null!;
    }
}
