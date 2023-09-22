﻿using API.Validation;
using System.ComponentModel.DataAnnotations;

namespace API.DTO.UserDTO
{
    public class RegisterDTO
    {

		[Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the name")]
		[StringLength(maximumLength: 25, MinimumLength = 10, ErrorMessage = "Length must be between 10 to 25")]

		public string? UserName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, MinLength(6)]
        public string Password { get; set; } = null!;
        [Required, Compare("Password")]
        public string PasswordConfirmed { get; set; }
	}
}
