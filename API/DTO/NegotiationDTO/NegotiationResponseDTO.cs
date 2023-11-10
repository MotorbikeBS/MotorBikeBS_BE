using Core.Models;

namespace API.DTO.ContractDTO
{
	public class NegotiationResponseDTO
	{
        public int NegotiationId { get; set; }
        public int? MotorId { get; set; }
        public decimal? FinalPrice { get; set; }
        public int? StoreId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
        public int? ValuationId { get; set; }
        public int? BaseRequestId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? Deposit { get; set; }
    }
}
