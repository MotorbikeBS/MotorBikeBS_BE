using System;
using System.Collections.Generic;
using API.DTO.MotorbikeDTO;
using API.DTO.UserDTO;
using Core.Models;

namespace API.DTO.RequestDTO
{
    public partial class Bill_RequestResponseDTO
    {
        public int? MotorId { get; set; }
        public virtual MotorResponseDTO? Motor { get; set; }
        public int? ReceiverId { get; set; }
        public virtual UserResponseDTO? Receiver { get; set; }
        public int? SenderId { get; set; }
        public virtual UserResponseDTO? Sender { get; set; }
    }
}
