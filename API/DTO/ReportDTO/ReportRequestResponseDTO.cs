using API.DTO.UserDTO;
using Core.Models;

namespace API.DTO.ReportDTO
{
    public class ReportRequestResponseDTO
    {
        public int RequestId { get; set; }
        public int? MotorId { get; set; }
        public int? ReceiverId { get; set; }
        public int? SenderId { get; set; }
        public DateTime? Time { get; set; }
        public int? RequestTypeId { get; set; }
        public string? Status { get; set; }
        
        public virtual ICollection<ReportResponseDTO> Reports { get; set; }
        public virtual UserResponseDTO? Receiver { get; set; }
        public virtual UserResponseDTO? Sender { get; set; }
    }
}
