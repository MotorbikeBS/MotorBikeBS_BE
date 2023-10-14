using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class NotificationType
    {
        public NotificationType()
        {
            Notifications = new HashSet<Notification>();
        }

        public int NotificationTypeId { get; set; }
        public string? Title { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
