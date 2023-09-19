using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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
        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<ConsignmentContract> ConsignmentContracts { get; set; } = null!;
        public virtual DbSet<ConsignmentContractImage> ConsignmentContractImages { get; set; } = null!;
        public virtual DbSet<EarnAlivingContract> EarnAlivingContracts { get; set; } = null!;
        public virtual DbSet<EarnAlivingContractImage> EarnAlivingContractImages { get; set; } = null!;
        public virtual DbSet<Facility> Facilities { get; set; } = null!;
        public virtual DbSet<LocalAddress> LocalAddresses { get; set; } = null!;
        public virtual DbSet<Motorbike> Motorbikes { get; set; } = null!;
        public virtual DbSet<MotorbikeImage> MotorbikeImages { get; set; } = null!;
        public virtual DbSet<MotorbikeStatus> MotorbikeStatuses { get; set; } = null!;
        public virtual DbSet<MotorbikeType> MotorbikeTypes { get; set; } = null!;
        public virtual DbSet<Negotiation> Negotiations { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<NotificationType> NotificationTypes { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<PointHistory> PointHistories { get; set; } = null!;
        public virtual DbSet<PostBoosting> PostBoostings { get; set; } = null!;
        public virtual DbSet<Request> Requests { get; set; } = null!;
        public virtual DbSet<RequestType> RequestTypes { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<StoreDesciption> StoreDesciptions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(local);Uid=ntp;Pwd=123456;Database=MotorbikeDB");
            }
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

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

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
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Booking_Request");
            });

            modelBuilder.Entity<ConsignmentContract>(entity =>
            {
                entity.HasKey(e => e.ContractId);

                entity.ToTable("Consignment_Contract");

                entity.Property(e => e.ContractId).HasColumnName("contract_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(100)
                    .HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.MotorId).HasColumnName("motor_id");

                entity.Property(e => e.NegotiationId).HasColumnName("negotiation_id");

                entity.Property(e => e.NewOwner).HasColumnName("new_owner");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .HasColumnName("status");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.HasOne(d => d.Negotiation)
                    .WithMany(p => p.ConsignmentContracts)
                    .HasForeignKey(d => d.NegotiationId)
                    .HasConstraintName("FK_Consignment_Contract_Negotiation");
            });

            modelBuilder.Entity<ConsignmentContractImage>(entity =>
            {
                entity.HasKey(e => e.ContractImageId);

                entity.ToTable("Consignment_ContractImage");

                entity.Property(e => e.ContractImageId).HasColumnName("contract_image_id");

                entity.Property(e => e.ContractId).HasColumnName("contract_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.ImageLink)
                    .HasMaxLength(100)
                    .HasColumnName("image_link");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.ConsignmentContractImages)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Consignment_ContractImage_Consignment_Contract");
            });

            modelBuilder.Entity<EarnAlivingContract>(entity =>
            {
                entity.HasKey(e => e.ContractId);

                entity.ToTable("EarnALiving_Contract");

                entity.Property(e => e.ContractId).HasColumnName("contract_id");

                entity.Property(e => e.BookingId).HasColumnName("booking_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(1000)
                    .HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.MotorId).HasColumnName("motor_id");

                entity.Property(e => e.NegotiationId).HasColumnName("negotiation_id");

                entity.Property(e => e.NewOwner).HasColumnName("new_owner");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .HasColumnName("status");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.EarnAlivingContracts)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK_EarnALiving_Contract_Booking");

                entity.HasOne(d => d.Negotiation)
                    .WithMany(p => p.EarnAlivingContracts)
                    .HasForeignKey(d => d.NegotiationId)
                    .HasConstraintName("FK_EarnALiving_Contract_Negotiation");
            });

            modelBuilder.Entity<EarnAlivingContractImage>(entity =>
            {
                entity.HasKey(e => e.ContractImageId);

                entity.ToTable("EarnALiving_ContractImage");

                entity.Property(e => e.ContractImageId).HasColumnName("contract_image_id");

                entity.Property(e => e.ContractId).HasColumnName("contract_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.ImageLink)
                    .HasMaxLength(100)
                    .HasColumnName("image_link");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.EarnAlivingContractImages)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EarnALiving_ContractImage_EarnALiving_Contract");
            });

            modelBuilder.Entity<Facility>(entity =>
            {
                entity.ToTable("Facility");

                entity.Property(e => e.FacilityId).HasColumnName("facility_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.HasMany(d => d.Motors)
                    .WithMany(p => p.Facilities)
                    .UsingEntity<Dictionary<string, object>>(
                        "MotorbikeFacility",
                        l => l.HasOne<Motorbike>().WithMany().HasForeignKey("MotorId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_MotorbikeFacility_Motorbike"),
                        r => r.HasOne<Facility>().WithMany().HasForeignKey("FacilityId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_MotorbikeFacility_Facility"),
                        j =>
                        {
                            j.HasKey("FacilityId", "MotorId");

                            j.ToTable("MotorbikeFacility");

                            j.IndexerProperty<int>("FacilityId").ValueGeneratedOnAdd().HasColumnName("facility_id");

                            j.IndexerProperty<int>("MotorId").HasColumnName("motor_id");
                        });
            });

            modelBuilder.Entity<LocalAddress>(entity =>
            {
                entity.HasKey(e => e.LocalId);

                entity.ToTable("LocalAddress");

                entity.Property(e => e.LocalId)
                    .ValueGeneratedNever()
                    .HasColumnName("local_id");

                entity.Property(e => e.CityName)
                    .HasMaxLength(50)
                    .HasColumnName("city_name");

                entity.Property(e => e.DistrictName)
                    .HasMaxLength(50)
                    .HasColumnName("district_name");

                entity.Property(e => e.WardName)
                    .HasMaxLength(50)
                    .HasColumnName("ward_name");
            });

            modelBuilder.Entity<Motorbike>(entity =>
            {
                entity.HasKey(e => e.MotorId);

                entity.ToTable("Motorbike");

                entity.Property(e => e.MotorId).HasColumnName("motor_id");

                entity.Property(e => e.Brand)
                    .HasMaxLength(50)
                    .HasColumnName("brand");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.Model)
                    .HasMaxLength(50)
                    .HasColumnName("model");

                entity.Property(e => e.MotorStatusId).HasColumnName("motor_status_id");

                entity.Property(e => e.MotorTypeId).HasColumnName("motor_type_id");

                entity.Property(e => e.OwnerId).HasColumnName("owner_id");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Year)
                    .HasColumnType("date")
                    .HasColumnName("year");

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
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MotorbikeImage_Motorbike");
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

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Negotiation>(entity =>
            {
                entity.ToTable("Negotiation");

                entity.Property(e => e.NegotiationId).HasColumnName("negotiation_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("end_time");

                entity.Property(e => e.FromSeller).HasColumnName("from_Seller");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("start_time");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.Negotiations)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Negotiation_Request");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.NotificationId).HasColumnName("notification_id");

                entity.Property(e => e.BookingId).HasColumnName("booking_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(200)
                    .HasColumnName("content");

                entity.Property(e => e.IsRead).HasColumnName("is_read");

                entity.Property(e => e.NotificationTypeId).HasColumnName("notification_type_id");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_Booking");

                entity.HasOne(d => d.NotificationType)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.NotificationTypeId)
                    .HasConstraintName("FK_Notification_NotificationType");
            });

            modelBuilder.Entity<NotificationType>(entity =>
            {
                entity.ToTable("NotificationType");

                entity.Property(e => e.NotificationTypeId).HasColumnName("notification_type_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(200)
                    .HasColumnName("content");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("date_created");

                entity.Property(e => e.HistoryId).HasColumnName("history_id");

                entity.Property(e => e.PaymentTime)
                    .HasColumnType("datetime")
                    .HasColumnName("payment_time");

                entity.Property(e => e.PaymentType)
                    .HasMaxLength(100)
                    .HasColumnName("payment_type");

                entity.HasOne(d => d.History)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.HistoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payment_PointHistory");
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
                    .WithMany(p => p.PointHistoryRequests)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PointHistory_Request1");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.PointHistoryStores)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PointHistory_Request");
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

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .HasColumnName("description");

                entity.Property(e => e.LocalId).HasColumnName("local_id");

                entity.Property(e => e.Point).HasColumnName("point");

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

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Local)
                    .WithMany(p => p.StoreDesciptions)
                    .HasForeignKey(d => d.LocalId)
                    .HasConstraintName("FK_StoreDesciption_LocalAddress");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.StoreDesciptions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreDesciption_User");
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

                entity.Property(e => e.LocalId).HasColumnName("local_id");

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

                entity.HasOne(d => d.Local)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.LocalId)
                    .HasConstraintName("FK_User_LocalAddress");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role");

                entity.HasMany(d => d.Motors)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "Wishlist",
                        l => l.HasOne<Motorbike>().WithMany().HasForeignKey("MotorId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Wishlist_Motorbike"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Wishlist_User"),
                        j =>
                        {
                            j.HasKey("UserId", "MotorId");

                            j.ToTable("Wishlist");

                            j.IndexerProperty<int>("UserId").HasColumnName("user_id");

                            j.IndexerProperty<int>("MotorId").HasColumnName("motor_id");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
