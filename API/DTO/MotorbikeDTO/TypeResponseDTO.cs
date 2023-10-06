using Core.Models;

namespace API.DTO.MotorbikeDTO
{
    public partial class TypeResponseDTO
    {

        public int MotorTypeId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
    }
}
