
using API.DTO.MotorbikeDTO;
using API.DTO.UserDTO;

namespace API.DTO.ContractDTO
{
    public class RequestContractResponseDTO
    {
        public int RequestId { get; set; }
        public int? MotorId { get; set; }
        public int? ReceiverId { get; set; }
        public int? SenderId { get; set; }
        public DateTime? Time { get; set; }
        public int? RequestTypeId { get; set; }
        public string? Status { get; set; }

        public virtual MotorResponseDTO? Motor { get; set; }
        public virtual ICollection<NegoRequestResponseDTO> Negotiations { get; set; }
        public virtual UserResponseDTO? Receiver { get; set; }
        public virtual UserResponseDTO? Sender { get; set; }
    }
}
