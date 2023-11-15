namespace API.DTO.PostBoostingDTO
{
    public class PostBoostingResponseDTO
    {
        public int BoostId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? TotalPoint { get; set; }
        public int? Level { get; set; }
        public int MotorId { get; set; }
        public int? HistoryId { get; set; }
        public string? Status { get; set; }
    }
}
