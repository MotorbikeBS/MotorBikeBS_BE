using API.DTO.NegotiationDTO;
using API.DTO.RequestDTO;
using API.DTO.UserDTO;
using Core.Models;
using System;
using System.Collections.Generic;

namespace API.DTO.BillDTO
{
    public partial class BillResponseDTO
    {
        public int BillConfirmId { get; set; }
        public int MotorId { get; set; }
        public int UserId { get; set; }
        public int StoreId { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? Status { get; set; }
        public int RequestId { get; set; }
        public virtual Bill_RequestResponseDTO Request { get; set; }

    }
}
