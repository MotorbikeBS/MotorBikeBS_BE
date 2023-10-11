namespace API.DTO.BookingDTO
{
	public class BookingResponseDTO
	{
		public int BookingId { get; set; }
		public int RequestId { get; set; }
		public DateTime? DateCreate { get; set; }
		public DateTime? BookingDate { get; set; }
		public string? Note { get; set; }
		public string? Status { get; set; }
	}
}
