namespace API.DTO.NegotiationDTO
{
	public class NegotiationCreateDTO
	{
        public decimal? Price { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Description { get; set; }
    }
}
