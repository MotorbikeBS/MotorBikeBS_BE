namespace API.DTO.UserDTO
{
	public class UserUpdateDTO
	{
		public string? UserName { get; set; }
		public string? Phone { get; set; }
		public int? Gender { get; set; }
		public DateTime? Dob { get; set; }
		public string? IdCard { get; set; }
		public string? Address { get; set; }
		public int? LocalId { get; set; }
	}
}
