using System;
using System.Collections.Generic;
using API.DTO.MotorbikeDTO;
using API.DTO.UserDTO;
using Core.Models;

namespace API.DTO.RequestDTO
{
    public partial class Store_RequestResponseDTO
    {
        public int RequestId { get; set; }
        public int? MotorId { get; set; }
        public virtual MotorResponseDTO? Motor { get; set; }
        public int? ReceiverId { get; set; }
        public virtual UserResponseDTO? Receiver { get; set; }
        public int? SenderId { get; set; }
        public virtual UserResponseDTO? Sender { get; set; }
        public DateTime? Time { get; set; }
        public int? RequestTypeId { get; set; }
        public virtual Type_RequestRegisterDTO? RequestType { get; set; }
        public string? Status { get; set; }
    }
}
