namespace API.DTO.UserDTO
{
	public class ChangePasswordDTO
	{
		public string OldPassword { get; set; }
		public string Password { get; set; }
		public string PasswordConfirmed { get; set; }
	}
}
