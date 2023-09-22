namespace API.DTO.MotorbikeDTO
{
    public partial class MotorRegisterDTO
    {

        public int MotorId { get; set; }
        public string CertificateNumber { get; set; } = null!;
        public int? BrandId { get; set; }
        public int? ModelId { get; set; }
        public int? Odo { get; set; }
        public DateTime? Year { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public int? MotorStatusId { get; set; }
        public int? MotorTypeId { get; set; }
        public int? StoreId { get; set; }
        public int OwnerId { get; set; }
    }
}
