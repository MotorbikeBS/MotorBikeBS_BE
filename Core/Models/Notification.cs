namespace Core.Models
{
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public int BookingId { get; set; }
        public int? UserId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int? NotificationTypeId { get; set; }
        public DateTime? Time { get; set; }
        public bool? IsRead { get; set; }

        public virtual Booking Booking { get; set; } = null!;
        public virtual NotificationType? NotificationType { get; set; }
    }
}
