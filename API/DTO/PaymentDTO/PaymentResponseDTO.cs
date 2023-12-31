﻿namespace API.DTO.PaymentDTO
{
    public class PaymentResponseDTO
    {
        public int PaymentId { get; set; }
        public int RequestId { get; set; }
        public string? Content { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? PaymentTime { get; set; }
        public string? VnpayOrderId { get; set; }
        public string? PaymentType { get; set; }
        public decimal? Amount { get; set; }
        public int? Point { get; set; }
    }
}
