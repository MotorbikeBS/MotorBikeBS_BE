namespace API.DTO.NegotiationDTO
{
	public class NegotiationResponseDTO
	{
        public int NegotiationId { get; set; }
        public int RequestId { get; set; }
        public decimal? Price { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? ExpiredTime { get; set; }
    }
}
