using System;
using System.Collections.Generic;
using Core.Models;

namespace API.DTO.MotorbikeDTO
{
    public partial class RegisterDTO
    {

        public int MotorId { get; set; }
        public string CertificateNumber { get; set; } = null!;
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? Odo { get; set; }
        public DateTime? Year { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public int? MotorStatusId { get; set; }
        public int? MotorTypeId { get; set; }
        public int? StoreId { get; set; }
    }
}
