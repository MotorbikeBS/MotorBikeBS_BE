namespace API.DTO.ContractDTO
{
	public class ContractImageResponseDTO
	{
		public int ContractImageId { get; set; }
		public int ContractId { get; set; }
		public string? ImageLink { get; set; }
		public string? Description { get; set; }
	}
}
