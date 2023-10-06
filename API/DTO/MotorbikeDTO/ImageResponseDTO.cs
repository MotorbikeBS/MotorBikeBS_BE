using Core.Models;

namespace API.DTO.MotorbikeDTO
{
    public partial class ImageResponseDTO
    {
        public int ImageId { get; set; }
        public string? ImageLink { get; set; }
        public int? MotorId { get; set; }
    }
}
