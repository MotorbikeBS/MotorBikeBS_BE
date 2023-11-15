using Core.Models;

namespace API.DTO.PostBoostingDTO
{
    public class PointHistoryBoostingResponseDTO
    {
        public int PHistoryId { get; set; }
        public int RequestId { get; set; }
        public int? Qty { get; set; }
        public DateTime? PointUpdatedAt { get; set; }
        public string? Description { get; set; }
        public string? Action { get; set; }
        public int StoreId { get; set; }

        public virtual ICollection<PostBoostingResponseDTO> PostBoostings { get; set; }
    }
}
