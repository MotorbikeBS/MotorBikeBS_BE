using API.DTO.ContractDTO;

namespace API.DTO.BookingNegotiationDTO
{
	public class BookingNegoResponseDTO
	{
		public int BookingId { get; set; }
		public DateTime? DateCreate { get; set; }
		public DateTime? BookingDate { get; set; }
		public string? Note { get; set; }
		public string? Status { get; set; }
		public virtual ICollection<ContractResponseDTO> Contracts { get; set; }
	}
}
