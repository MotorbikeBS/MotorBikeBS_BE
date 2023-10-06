using System;
using System.Collections.Generic;
using Core.Models;

namespace API.DTO.RequestDTO
{
    public partial class RequestRegisterDTO
    {
        public int? ReceiverId { get; set; }
        public int? SenderId { get; set; }
        public DateTime? Time { get; set; }
        public int? RequestTypeId { get; set; }
        public string? Status { get; set; }
    }
}
