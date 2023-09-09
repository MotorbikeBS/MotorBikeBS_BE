using System.ComponentModel.DataAnnotations;

namespace MotorBikeBS_API.DTO
{
	public class MotobikeCreateDTO
	{
		public int MotorId { get; set; }
		[Required]
		public string? Brand { get; set; }
		[Required]
		public string? Model { get; set; }
		public DateTime? Year { get; set; }
		public decimal? Price { get; set; }
		public string? Description { get; set; }
		public int? MotorStatusId { get; set; }
		public int? MotorTypeId { get; set; }
		public int? ImageId { get; set; }
		public int? StoreId { get; set; }
		[Required]
		public int OwnerId { get; set; }
	}
}
