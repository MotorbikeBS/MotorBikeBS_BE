using Core.Models;

namespace API.DTO.MotorbikeDTO
{
    public partial class ModelRegisterDTO
    {

        public string? ModelName { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = null!;
        public int? BrandId { get; set; }
    }
}
