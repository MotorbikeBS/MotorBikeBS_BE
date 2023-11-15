using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Core.Models
{
    public partial class MotorbikeDBContext : DbContext
    {
        public MotorbikeDBContext()
        {
        }

        public MotorbikeDBContext(DbContextOptions<MotorbikeDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BillConfirm> BillConfirms { get; set; } = null!;
        public virtual DbSet<BuyerBooking> BuyerBookings { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Motorbike> Motorbikes { get; set; } = null!;
        public virtual DbSet<MotorbikeBrand> MotorbikeBrands { get; set; } = null!;
        public virtual DbSet<MotorbikeImage> MotorbikeImages { get; set; } = null!;
        public virtual DbSet<MotorbikeModel> MotorbikeModels { get; set; } = null!;
        public virtual DbSet<MotorbikeStatus> MotorbikeStatuses { get; set; } = null!;
        public virtual DbSet<MotorbikeType> MotorbikeTypes { get; set; } = null!;
        public virtual DbSet<Negotiation> Negotiations { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<NotificationType> NotificationTypes { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<PointHistory> PointHistories { get; set; } = null!;
        public virtual DbSet<PostBoosting> PostBoostings { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
        public virtual DbSet<ReportImage> ReportImages { get; set; } = null!;
        public virtual DbSet<Request> Requests { get; set; } = null!;
        public virtual DbSet<RequestType> RequestTypes { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<StoreDesciption> StoreDesciptions { get; set; } = null!;
        public virtual DbSet<StoreImage> StoreImages { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Valuation> Valuations { get; set; } = null!;
        public virtual DbSet<Wishlist> Wishlists { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var d = Directory.GetCurrentDirectory();
            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BillConfirm>(entity =>
            {
                entity.ToTable("BillConfirm");

                entity.Property(e => e.BillConfirmId).HasColumnName("bill_confirm_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.MotorId).HasColumnName("motor_id");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .HasColumnName("status");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.BillConfirms)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BillConfirm_Request");
            });

            modelBuilder.Entity<BuyerBooking>(entity =>
            {
                entity.HasKey(e => e.BookingId);

                entity.ToTable("BuyerBooking");

                entity.Property(e => e.BookingId).HasColumnName("booking_id");

                entity.Property(e => e.BookingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("booking_date");

                entity.Property(e => e.DateCreate)
                    .HasColumnType("datetime")
                    .HasColumnName("date_create");

                entity.Property(e => e.Note)
                    .HasMaxLength(100)
                    .HasColumnName("note");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .HasColumnName("status");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.BuyerBookings)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BuyerBooking_Request");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(100)
                    .HasColumnName("content");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.ReplyId).HasColumnName("reply_id");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Reply)
                    .WithMany(p => p.InverseReply)
                    .HasForeignKey(d => d.ReplyId)
                    .HasConstraintName("FK_Comment_Comment");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Request");
            });

            modelBuilder.Entity<Motorbike>(entity =>
            {
                entity.HasKey(e => e.MotorId);

                entity.ToTable("Motorbike");

                entity.Property(e => e.MotorId).HasColumnName("motor_id");

                entity.Property(e => e.CertificateNumber)
                    .HasMaxLength(6)
                    .HasColumnName("certificate_number")
                    .IsFixedLength();

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.ModelId).HasColumnName("model_id");

                entity.Property(e => e.MotorName)
                    .HasMaxLength(50)
                    .HasColumnName("motor_name");

                entity.Property(e => e.MotorStatusId).HasColumnName("motor_status_id");

                entity.Property(e => e.MotorTypeId).HasColumnName("motor_type_id");

                entity.Property(e => e.Odo).HasColumnName("odo");

                entity.Property(e => e.OwnerId).HasColumnName("owner_id");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.RegistrationImage)
                    .HasMaxLength(200)
                    .HasColumnName("registration_image");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Year)
                    .HasColumnType("date")
                    .HasColumnName("year");

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.Motorbikes)
                    .HasForeignKey(d => d.ModelId)
                    .HasConstraintName("FK_Motorbike_MotorbikeModel");

                entity.HasOne(d => d.MotorStatus)
                    .WithMany(p => p.Motorbikes)
                    .HasForeignKey(d => d.MotorStatusId)
                    .HasConstraintName("FK_Motorbike_MotorbikeStatus");

                entity.HasOne(d => d.MotorType)
                    .WithMany(p => p.Motorbikes)
                    .HasForeignKey(d => d.MotorTypeId)
                    .HasConstraintName("FK_Motorbike_MotorbikeType");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Motorbikes)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Motorbike_User");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Motorbikes)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_Motorbike_StoreDesciption");
            });

            modelBuilder.Entity<MotorbikeBrand>(entity =>
            {
                entity.HasKey(e => e.BrandId);

                entity.ToTable("MotorbikeBrand");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .HasColumnName("brand_name");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .HasColumnName("status");
            });

            modelBuilder.Entity<MotorbikeImage>(entity =>
            {
                entity.HasKey(e => e.ImageId);

                entity.ToTable("MotorbikeImage");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.ImageLink)
                    .HasMaxLength(100)
                    .HasColumnName("image_link");

                entity.Property(e => e.MotorId).HasColumnName("motor_id");

                entity.HasOne(d => d.Motor)
                    .WithMany(p => p.MotorbikeImages)
                    .HasForeignKey(d => d.MotorId)
                    .HasConstraintName("FK_MotorbikeImage_Motorbike");
            });

            modelBuilder.Entity<MotorbikeModel>(entity =>
            {
                entity.HasKey(e => e.ModelId);

                entity.ToTable("MotorbikeModel");

                entity.Property(e => e.ModelId).HasColumnName("model_id");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.ModelName)
                    .HasMaxLength(50)
                    .HasColumnName("model_name");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .HasColumnName("status");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.MotorbikeModels)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_MotorbikeModel_MotorbikeBrand");
            });

            modelBuilder.Entity<MotorbikeStatus>(entity =>
            {
                entity.HasKey(e => e.MotorStatusId);

                entity.ToTable("MotorbikeStatus");

                entity.Property(e => e.MotorStatusId).HasColumnName("motorStatus_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description")
                    .IsFixedLength();

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<MotorbikeType>(entity =>
            {
                entity.HasKey(e => e.MotorTypeId);

                entity.ToTable("MotorbikeType");

                entity.Property(e => e.MotorTypeId).HasColumnName("motorType_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .HasColumnName("status");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Negotiation>(entity =>
            {
                entity.ToTable("Negotiation");

                entity.Property(e => e.NegotiationId).HasColumnName("negotiation_id");

                entity.Property(e => e.BaseRequestId).HasColumnName("base_request_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(1000)
                    .HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Deposit)
                    .HasColumnType("money")
                    .HasColumnName("deposit");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("end_time");

                entity.Property(e => e.FinalPrice)
                    .HasColumnType("money")
                    .HasColumnName("final_price");

                entity.Property(e => e.MotorId).HasColumnName("motor_id");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("start_time");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .HasColumnName("status");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.ValuationId).HasColumnName("valuation_id");

                entity.HasOne(d => d.Valuation)
                    .WithMany(p => p.Negotiations)
                    .HasForeignKey(d => d.ValuationId)
                    .HasConstraintName("FK_Negotiation_Valuation");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.NotificationId).HasColumnName("notification_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(200)
                    .HasColumnName("content");

                entity.Property(e => e.IsRead).HasColumnName("is_read");

                entity.Property(e => e.NotificationTypeId).HasColumnName("notification_type_id");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.NotificationType)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.NotificationTypeId)
                    .HasConstraintName("FK_Notification_NotificationType");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_Request");
            });

            modelBuilder.Entity<NotificationType>(entity =>
            {
                entity.ToTable("NotificationType");

                entity.Property(e => e.NotificationTypeId).HasColumnName("notification_type_id");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.Property(e => e.Amount)
                    .HasColumnType("money")
                    .HasColumnName("amount");

                entity.Property(e => e.Content)
                    .HasMaxLength(200)
                    .HasColumnName("content");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("date_created");

                entity.Property(e => e.PaymentTime)
                    .HasColumnType("datetime")
                    .HasColumnName("payment_time");

                entity.Property(e => e.PaymentType)
                    .HasMaxLength(100)
                    .HasColumnName("payment_type");

                entity.Property(e => e.Point).HasColumnName("point");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.VnpayOrderId)
                    .HasMaxLength(50)
                    .HasColumnName("vnpay_order_id");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payment_Request");
            });

            modelBuilder.Entity<PointHistory>(entity =>
            {
                entity.HasKey(e => e.PHistoryId);

                entity.ToTable("PointHistory");

                entity.Property(e => e.PHistoryId).HasColumnName("pHistory_id");

                entity.Property(e => e.Action)
                    .HasMaxLength(50)
                    .HasColumnName("action");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.PointUpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("point_updated_at");

                entity.Property(e => e.Qty).HasColumnName("qty");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.PointHistories)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PointHistory_Request2");
            });

            modelBuilder.Entity<PostBoosting>(entity =>
            {
                entity.HasKey(e => e.BoostId);

                entity.ToTable("PostBoosting");

                entity.Property(e => e.BoostId).HasColumnName("boost_id");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("end_time");

                entity.Property(e => e.HistoryId).HasColumnName("history_id");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.MotorId).HasColumnName("motor_id");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("start_time");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.HasOne(d => d.History)
                    .WithMany(p => p.PostBoostings)
                    .HasForeignKey(d => d.HistoryId)
                    .HasConstraintName("FK_PostBoosting_PointHistory");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report");

                entity.Property(e => e.ReportId).HasColumnName("report_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("description");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .HasColumnName("title");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Report_Request");
            });

            modelBuilder.Entity<ReportImage>(entity =>
            {
                entity.ToTable("ReportImage");

                entity.Property(e => e.ReportImageId).HasColumnName("report_image_id");

                entity.Property(e => e.ImageLink)
                    .HasMaxLength(200)
                    .HasColumnName("image_link");

                entity.Property(e => e.ReportId).HasColumnName("report_id");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.ReportImages)
                    .HasForeignKey(d => d.ReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReportImage_Report");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("Request");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.MotorId).HasColumnName("motor_id");

                entity.Property(e => e.ReceiverId).HasColumnName("receiver_id");

                entity.Property(e => e.RequestTypeId).HasColumnName("request_type_id");

                entity.Property(e => e.SenderId).HasColumnName("sender_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");

                entity.HasOne(d => d.Motor)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.MotorId)
                    .HasConstraintName("FK_Request_Motorbike");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.RequestReceivers)
                    .HasForeignKey(d => d.ReceiverId)
                    .HasConstraintName("FK_Request_User");

                entity.HasOne(d => d.RequestType)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.RequestTypeId)
                    .HasConstraintName("FK_Request_RequestType");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.RequestSenders)
                    .HasForeignKey(d => d.SenderId)
                    .HasConstraintName("FK_Request_User1");
            });

            modelBuilder.Entity<RequestType>(entity =>
            {
                entity.ToTable("RequestType");

                entity.Property(e => e.RequestTypeId).HasColumnName("request_type_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<StoreDesciption>(entity =>
            {
                entity.HasKey(e => e.StoreId);

                entity.ToTable("StoreDesciption");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.BusinessLicense)
                    .HasMaxLength(100)
                    .HasColumnName("business_license");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .HasColumnName("description");

                entity.Property(e => e.Point).HasColumnName("point");

                entity.Property(e => e.Status)
                    .HasMaxLength(15)
                    .HasColumnName("status");

                entity.Property(e => e.StoreCreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("store_created_at");

                entity.Property(e => e.StoreEmail)
                    .HasMaxLength(50)
                    .HasColumnName("store_email");

                entity.Property(e => e.StoreName)
                    .HasMaxLength(50)
                    .HasColumnName("store_name");

                entity.Property(e => e.StorePhone)
                    .HasMaxLength(10)
                    .HasColumnName("store_phone")
                    .IsFixedLength();

                entity.Property(e => e.StoreUpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("store_updated_at");

                entity.Property(e => e.TaxCode)
                    .HasMaxLength(13)
                    .HasColumnName("tax_code");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.WardId)
                    .HasMaxLength(5)
                    .HasColumnName("ward_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.StoreDesciptions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreDesciption_User1");
            });

            modelBuilder.Entity<StoreImage>(entity =>
            {
                entity.ToTable("StoreImage");

                entity.Property(e => e.StoreImageId).HasColumnName("store_image_id");

                entity.Property(e => e.ImageLink)
                    .HasMaxLength(200)
                    .HasColumnName("image_link");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.StoreImages)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreImage_StoreDesciption");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("dob");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.IdCard)
                    .HasMaxLength(12)
                    .HasColumnName("idCard")
                    .IsFixedLength();

                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");

                entity.Property(e => e.PasswordResetToken).HasColumnName("password_reset_token");

                entity.Property(e => e.PasswordSalt).HasColumnName("password_salt");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .HasColumnName("phone")
                    .IsFixedLength();

                entity.Property(e => e.ResetTokenExpires)
                    .HasColumnType("datetime")
                    .HasColumnName("reset_token_expires");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .HasColumnName("status");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserUpdatedAt)
                    .HasColumnType("date")
                    .HasColumnName("user_updated_at");

                entity.Property(e => e.UserVerifyAt)
                    .HasColumnType("date")
                    .HasColumnName("user_verify_at");

                entity.Property(e => e.VerifycationToken).HasColumnName("verifycation_token");

                entity.Property(e => e.VerifycationTokenExpires)
                    .HasColumnType("datetime")
                    .HasColumnName("verifycation_token_expires");

                entity.Property(e => e.WardId)
                    .HasMaxLength(5)
                    .HasColumnName("ward_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role");
            });

            modelBuilder.Entity<Valuation>(entity =>
            {
                entity.ToTable("Valuation");

                entity.Property(e => e.ValuationId).HasColumnName("valuation_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.StorePrice)
                    .HasColumnType("decimal(15, 4)")
                    .HasColumnName("store_price");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.Valuations)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Negotiation_Request");
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.ToTable("Wishlist");

                entity.Property(e => e.WishlistId).HasColumnName("wishlist_id");

                entity.Property(e => e.MotorId).HasColumnName("motor_id");

                entity.Property(e => e.MotorName)
                    .HasMaxLength(50)
                    .HasColumnName("motor_name");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Motor)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.MotorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Motorbike");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
