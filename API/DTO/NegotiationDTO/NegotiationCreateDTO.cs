namespace API.DTO.ContractDTO
{
	public class NegotiationCreateDTO
	{
        public decimal FinalPrice { get; set; }
        public string Content { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Deposit { get; set; }
    }
}
