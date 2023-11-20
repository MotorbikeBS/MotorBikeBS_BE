using Core.Models;

namespace API.DTO.ReportDTO
{
    public class ReportResponseDTO
    {
        public int ReportId { get; set; }
        public int RequestId { get; set; }
        public string? Description { get; set; }
        public string? Title { get; set; }
        public string? Status { get; set; }
        public virtual ICollection<ReportImageResponseDTO> ReportImages { get; set; }

    }
}
