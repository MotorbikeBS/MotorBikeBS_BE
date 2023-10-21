using Core.Models;

namespace API.DTO.ContractDTO
{
	public class ContractResponseDTO
	{
		public int ContractId { get; set; }
		public int? MotorId { get; set; }
		public decimal? Price { get; set; }
		public int? NewOwner { get; set; }
		public int? StoreId { get; set; }
		public string? Content { get; set; }
		public DateTime? CreatedAt { get; set; }
		public string? Status { get; set; }
		public int? BookingId { get; set; }
		public int? BaseRequestId { get; set; }

		public virtual ICollection<ContractImageResponseDTO> ContractImages { get; set; }
	}
}
