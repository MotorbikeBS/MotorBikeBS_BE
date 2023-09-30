using System.ComponentModel.DataAnnotations;

namespace API.DTO.UserDTO
{
    public class StoreRegisterDTO
    {
        public string StoreName { get; set; } = null!;
        public string? StorePhone { get; set; }
        public string? StoreEmail { get; set; }
        public string? Address { get; set; }
		public string? TaxCode { get; set; }
	}
}
