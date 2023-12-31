﻿using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class Request
    {
        public Request()
        {
            BillConfirms = new HashSet<BillConfirm>();
            BuyerBookings = new HashSet<BuyerBooking>();
            Comments = new HashSet<Comment>();
            Notifications = new HashSet<Notification>();
            Payments = new HashSet<Payment>();
            PointHistories = new HashSet<PointHistory>();
            Reports = new HashSet<Report>();
            Valuations = new HashSet<Valuation>();
        }

        public int RequestId { get; set; }
        public int? MotorId { get; set; }
        public int? ReceiverId { get; set; }
        public int? SenderId { get; set; }
        public DateTime? Time { get; set; }
        public int? RequestTypeId { get; set; }
        public string? Status { get; set; }

        public virtual Motorbike? Motor { get; set; }
        public virtual User? Receiver { get; set; }
        public virtual RequestType? RequestType { get; set; }
        public virtual User? Sender { get; set; }
        public virtual ICollection<BillConfirm> BillConfirms { get; set; }
        public virtual ICollection<BuyerBooking> BuyerBookings { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<PointHistory> PointHistories { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Valuation> Valuations { get; set; }
    }
}
