using API.DTO.UserDTO;
using System;
using System.Collections.Generic;
using Core.Models;

namespace API.DTO.RequestDTO
{
    public partial class RequestResponseDTO
    {
        public int? MotorId { get; set; }
        public int? ReceiverId { get; set; }
        public virtual UserUpdateDTO? Receiver { get; set; }
        public int? SenderId { get; set; }
        public virtual UserUpdateDTO? Sender { get; set; }
        public DateTime? Time { get; set; }
        public int? RequestTypeId { get; set; }
        public virtual Type_RequestRegisterDTO? RequestType { get; set; }
        public string? Status { get; set; }
    }
}
