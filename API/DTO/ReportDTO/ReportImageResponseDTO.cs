namespace API.DTO.ReportDTO
{
    public class ReportImageResponseDTO
    {
        public int ReportImageId { get; set; }
        public int ReportId { get; set; }
        public string ImageLink { get; set; } = null!;
    }
}
