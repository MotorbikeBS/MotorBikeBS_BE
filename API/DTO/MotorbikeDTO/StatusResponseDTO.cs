using Core.Models;

namespace API.DTO.MotorbikeDTO
{
    public partial class StatusResponseDTO
    {
        

        public int MotorStatusId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

    }
}
