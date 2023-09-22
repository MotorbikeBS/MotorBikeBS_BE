using System.ComponentModel.DataAnnotations;

namespace API.DTO.UserDTO
{
    public class StoreRegisterDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string StoreName { get; set; } = null!;
        [Required]
        public string? StorePhone { get; set; }
        [Required]
        public string? StoreEmail { get; set; }
        [Required]
        public string? Address { get; set; }
    }
}
