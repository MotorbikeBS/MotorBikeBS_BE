using Core.Models;

namespace API.DTO.PaymentDTO
{
    public class PaymentRequestResponseDTO
    {
        public int RequestId { get; set; }
        public int? MotorId { get; set; }
        public int? ReceiverId { get; set; }
        public int? SenderId { get; set; }
        public DateTime? Time { get; set; }
        public int? RequestTypeId { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<PaymentResponseDTO> Payments { get; set; }
    }
}
