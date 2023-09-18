using API.Validation;
using System.ComponentModel.DataAnnotations;

namespace API.DTO.UserDTO
{
    public class RegisterDTO
    {
        [Required]
        [SingleSpaceBetweenNamesAttribute(ErrorMessage = "Not Valid!")]
        public string? UserName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, MinLength(6)]
        public string Password { get; set; } = null!;
        [Required, Compare("Password")]
        public string PasswordConfirmed { get; set; }
        [Required, Phone(ErrorMessage ="Phone number is not valid!"),
            MinLength(10, ErrorMessage = "Phone number is not valid!"),
            MaxLength(11, ErrorMessage = "Phone number is not valid!")]
        public string? Phone { get; set; }
		public int? RoleId { get; set; }
	}
}
