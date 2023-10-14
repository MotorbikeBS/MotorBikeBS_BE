using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public int RequestId { get; set; }
        public int? UserId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int? NotificationTypeId { get; set; }
        public DateTime? Time { get; set; }
        public bool? IsRead { get; set; }

        public virtual NotificationType? NotificationType { get; set; }
        public virtual Request Request { get; set; } = null!;
    }
}
