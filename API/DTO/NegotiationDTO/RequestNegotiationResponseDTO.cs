
using API.DTO.MotorbikeDTO;
using API.DTO.NegotiationDTO;
using API.DTO.UserDTO;

namespace API.DTO.ContractDTO
{
    public class RequestNegotiationResponseDTO
    {
        public int RequestId { get; set; }
        public int? MotorId { get; set; }
        public int? ReceiverId { get; set; }
        public int? SenderId { get; set; }
        public DateTime? Time { get; set; }
        public int? RequestTypeId { get; set; }
        public string? Status { get; set; }

        public virtual MotorResponseDTO? Motor { get; set; }
        public virtual ICollection<ValuationRequestResponseDTO> Valuations { get; set; }
        public virtual UserResponseDTO? Receiver { get; set; }
        public virtual UserResponseDTO? Sender { get; set; }
    }
}
