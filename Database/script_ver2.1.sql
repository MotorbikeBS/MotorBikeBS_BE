USE [master]
GO
/****** Object:  Database [motorbikebs-db]    Script Date: 10/15/2023 12:06:53 AM ******/
CREATE DATABASE [motorbikebs-db]
GO
ALTER DATABASE [motorbikebs-db] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [motorbikebs-db] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [motorbikebs-db] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [motorbikebs-db] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [motorbikebs-db] SET ARITHABORT OFF 
GO
ALTER DATABASE [motorbikebs-db] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [motorbikebs-db] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [motorbikebs-db] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [motorbikebs-db] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [motorbikebs-db] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [motorbikebs-db] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [motorbikebs-db] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [motorbikebs-db] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [motorbikebs-db] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [motorbikebs-db] SET  ENABLE_BROKER 
GO
ALTER DATABASE [motorbikebs-db] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [motorbikebs-db] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [motorbikebs-db] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [motorbikebs-db] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [motorbikebs-db] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [motorbikebs-db] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [motorbikebs-db] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [motorbikebs-db] SET RECOVERY FULL 
GO
ALTER DATABASE [motorbikebs-db] SET  MULTI_USER 
GO
ALTER DATABASE [motorbikebs-db] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [motorbikebs-db] SET DB_CHAINING OFF 
GO
USE [motorbikebs-db]
GO

-- If using Azure, uncomment and execute this within the target database connection
-- ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 8;

/****** Object:  Table [dbo].[BillConfirm]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillConfirm](
	[bill_confirm_id] [int] IDENTITY(1,1) NOT NULL,
	[motor_id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
	[store_id] [int] NOT NULL,
	[price] [money] NULL,
	[create_at] [datetime] NULL,
	[status] [nvarchar](10) NULL,
	[request_id] [int] NOT NULL,
 CONSTRAINT [PK_BillConfirm] PRIMARY KEY CLUSTERED 
(
	[bill_confirm_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Booking]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking](
	[booking_id] [int] IDENTITY(1,1) NOT NULL,
	[negotiation_id] [int] NULL,
	[date_create] [datetime] NULL,
	[booking_date] [datetime] NULL,
	[note] [nvarchar](100) NULL,
	[status] [nvarchar](10) NULL,
	[base_request_id] [int] NULL,
 CONSTRAINT [PK_Booking] PRIMARY KEY CLUSTERED 
(
	[booking_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BuyerBooking]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BuyerBooking](
	[booking_id] [int] IDENTITY(1,1) NOT NULL,
	[request_id] [int] NOT NULL,
	[date_create] [datetime] NULL,
	[booking_date] [datetime] NULL,
	[note] [nvarchar](100) NULL,
	[status] [nvarchar](10) NULL,
 CONSTRAINT [PK_BuyerBooking] PRIMARY KEY CLUSTERED 
(
	[booking_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contract]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contract](
	[contract_id] [int] IDENTITY(1,1) NOT NULL,
	[motor_id] [int] NULL,
	[price] [money] NULL,
	[new_owner] [int] NULL,
	[store_id] [int] NULL,
	[content] [nvarchar](1000) NULL,
	[created_at] [datetime] NULL,
	[status] [nvarchar](10) NULL,
	[booking_id] [int] NULL,
	[base_request_id] [int] NULL,
 CONSTRAINT [PK_EarnALiving_Contract] PRIMARY KEY CLUSTERED 
(
	[contract_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContractImage]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractImage](
	[contract_image_id] [int] IDENTITY(1,1) NOT NULL,
	[contract_id] [int] NOT NULL,
	[image_link] [nvarchar](100) NULL,
	[description] [nvarchar](200) NULL,
 CONSTRAINT [PK_EarnALiving_ContractImage] PRIMARY KEY CLUSTERED 
(
	[contract_image_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Motorbike]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Motorbike](
	[motor_id] [int] IDENTITY(1,1) NOT NULL,
	[certificate_number] [nchar](6) NOT NULL,
	[motor_name] [nvarchar](50) NULL,
	[model_id] [int] NULL,
	[odo] [int] NULL,
	[year] [date] NULL,
	[price] [money] NULL,
	[description] [nvarchar](255) NULL,
	[motor_status_id] [int] NULL,
	[motor_type_id] [int] NULL,
	[store_id] [int] NULL,
	[owner_id] [int] NOT NULL,
	[registration_image] [nvarchar](200) NULL,
 CONSTRAINT [PK_Motorbike] PRIMARY KEY CLUSTERED 
(
	[motor_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotorbikeBrand]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MotorbikeBrand](
	[brand_id] [int] IDENTITY(1,1) NOT NULL,
	[brand_name] [nvarchar](50) NULL,
	[description] [nvarchar](200) NULL,
	[status] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_MotorbikeBrand] PRIMARY KEY CLUSTERED 
(
	[brand_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotorbikeImage]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MotorbikeImage](
	[image_id] [int] IDENTITY(1,1) NOT NULL,
	[image_link] [nvarchar](100) NULL,
	[motor_id] [int] NULL,
 CONSTRAINT [PK_MotorbikeImage] PRIMARY KEY CLUSTERED 
(
	[image_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotorbikeModel]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MotorbikeModel](
	[model_id] [int] IDENTITY(1,1) NOT NULL,
	[model_name] [nvarchar](50) NULL,
	[description] [nvarchar](200) NULL,
	[status] [nvarchar](10) NOT NULL,
	[brand_id] [int] NULL,
 CONSTRAINT [PK_MotorbikeModel] PRIMARY KEY CLUSTERED 
(
	[model_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotorbikeStatus]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MotorbikeStatus](
	[motorStatus_id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](100) NULL,
	[description] [nchar](200) NULL,
 CONSTRAINT [PK_MotorbikeStatus] PRIMARY KEY CLUSTERED 
(
	[motorStatus_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotorbikeType]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MotorbikeType](
	[motorType_id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](100) NULL,
	[description] [nvarchar](200) NULL,
	[status] [nvarchar](10) NULL,
 CONSTRAINT [PK_MotorbikeType] PRIMARY KEY CLUSTERED 
(
	[motorType_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Negotiation]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Negotiation](
	[negotiation_id] [int] IDENTITY(1,1) NOT NULL,
	[request_id] [int] NOT NULL,
	[store_price] [decimal](15, 4) NULL,
	[owner_price] [decimal](15, 4) NULL,
	[start_time] [datetime] NULL,
	[end_time] [datetime] NULL,
	[description] [nvarchar](200) NULL,
	[status] [nvarchar](50) NULL,
	[final_price] [decimal](15, 4) NULL,
	[base_request_id] [int] NULL,
	[expired_time] [datetime] NULL,
	[last_change_user_id] [int] NULL,
 CONSTRAINT [PK_Negotiation] PRIMARY KEY CLUSTERED 
(
	[negotiation_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[notification_id] [int] IDENTITY(1,1) NOT NULL,
	[request_id] [int] NOT NULL,
	[user_id] [int] NULL,
	[title] [nvarchar](100) NULL,
	[content] [nvarchar](200) NULL,
	[notification_type_id] [int] NULL,
	[time] [datetime] NULL,
	[is_read] [bit] NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[notification_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationType]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationType](
	[notification_type_id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](50) NULL,
 CONSTRAINT [PK_NotificationType] PRIMARY KEY CLUSTERED 
(
	[notification_type_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[payment_id] [int] IDENTITY(1,1) NOT NULL,
	[history_id] [int] NOT NULL,
	[content] [nvarchar](200) NULL,
	[date_created] [datetime] NULL,
	[payment_time] [datetime] NULL,
	[payment_type] [nvarchar](100) NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[payment_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PointHistory]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PointHistory](
	[pHistory_id] [int] IDENTITY(1,1) NOT NULL,
	[request_id] [int] NOT NULL,
	[qty] [int] NULL,
	[point_updated_at] [datetime] NULL,
	[description] [nvarchar](200) NULL,
	[action] [nvarchar](50) NULL,
	[store_id] [int] NOT NULL,
 CONSTRAINT [PK_PointHistory] PRIMARY KEY CLUSTERED 
(
	[pHistory_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PostBoosting]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostBoosting](
	[boost_id] [int] IDENTITY(1,1) NOT NULL,
	[start_time] [datetime] NULL,
	[end_time] [datetime] NULL,
	[level] [int] NULL,
	[motor_id] [int] NOT NULL,
	[history_id] [int] NULL,
	[status] [nvarchar](50) NULL,
 CONSTRAINT [PK_PostBoosting] PRIMARY KEY CLUSTERED 
(
	[boost_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Request]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Request](
	[request_id] [int] IDENTITY(1,1) NOT NULL,
	[motor_id] [int] NULL,
	[receiver_id] [int] NULL,
	[sender_id] [int] NULL,
	[time] [datetime] NULL,
	[request_type_id] [int] NULL,
	[status] [nvarchar](50) NULL,
 CONSTRAINT [PK_Request] PRIMARY KEY CLUSTERED 
(
	[request_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestType]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestType](
	[request_type_id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](100) NOT NULL,
	[description] [nvarchar](200) NULL,
 CONSTRAINT [PK_RequestType] PRIMARY KEY CLUSTERED 
(
	[request_type_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[role_id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StoreDesciption]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreDesciption](
	[store_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[store_name] [nvarchar](50) NOT NULL,
	[description] [nvarchar](1000) NULL,
	[store_phone] [nchar](10) NULL,
	[store_email] [nvarchar](50) NULL,
	[store_created_at] [datetime] NULL,
	[store_updated_at] [datetime] NULL,
	[point] [int] NULL,
	[address] [nvarchar](100) NULL,
	[ward_id] [nvarchar](5) NULL,
	[status] [nvarchar](15) NULL,
	[tax_code] [nvarchar](13) NULL,
	[business_license] [nvarchar](100) NULL,
 CONSTRAINT [PK_StoreDesciption] PRIMARY KEY CLUSTERED 
(
	[store_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StoreImage]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreImage](
	[store_image_id] [int] IDENTITY(1,1) NOT NULL,
	[image_link] [nvarchar](200) NOT NULL,
	[store_id] [int] NOT NULL,
 CONSTRAINT [PK_StoreImage] PRIMARY KEY CLUSTERED 
(
	[store_image_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[user_name] [nvarchar](100) NULL,
	[email] [nvarchar](100) NOT NULL,
	[phone] [nchar](10) NULL,
	[gender] [int] NULL,
	[dob] [date] NULL,
	[idCard] [nchar](12) NULL,
	[address] [nvarchar](100) NULL,
	[ward_id] [nvarchar](5) NULL,
	[role_id] [int] NULL,
	[user_verify_at] [date] NULL,
	[user_updated_at] [date] NULL,
	[status] [nvarchar](10) NOT NULL,
	[password_hash] [varbinary](max) NULL,
	[password_salt] [varbinary](max) NULL,
	[password_reset_token] [nvarchar](max) NULL,
	[reset_token_expires] [datetime] NULL,
	[verifycation_token] [nvarchar](max) NULL,
	[verifycation_token_expires] [datetime] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ward]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ward](
	[ward_id] [nvarchar](5) NOT NULL,
	[ward_name] [nvarchar](100) NOT NULL,
	[type] [nvarchar](50) NULL,
	[district_id] [nvarchar](5) NULL,
 CONSTRAINT [PK_LocalAddress] PRIMARY KEY CLUSTERED 
(
	[ward_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wishlist]    Script Date: 10/30/2023 10:03:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wishlist](
	[wishlist_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[motor_id] [int] NOT NULL,
	[motor_name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Wishlist_1] PRIMARY KEY CLUSTERED 
(
	[wishlist_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[BillConfirm] ON 
GO
INSERT [dbo].[BillConfirm] ([bill_confirm_id], [motor_id], [user_id], [store_id], [price], [create_at], [status], [request_id]) VALUES (2, 2040, 2, 5, 45300000.0000, CAST(N'2023-10-25T09:27:29.773' AS DateTime), N'ACCEPT', 198)
GO
SET IDENTITY_INSERT [dbo].[BillConfirm] OFF
GO
SET IDENTITY_INSERT [dbo].[Booking] ON 
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (26, 103, CAST(N'2023-10-25T08:42:46.660' AS DateTime), CAST(N'2023-10-28T00:00:00.000' AS DateTime), N'Test flow booking', N'PENDING', 194)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (27, 104, CAST(N'2023-10-25T08:51:11.683' AS DateTime), CAST(N'2023-10-30T00:00:00.000' AS DateTime), N'Test Booking v2', N'PENDING', 195)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (28, 105, CAST(N'2023-10-25T08:52:26.587' AS DateTime), CAST(N'2023-10-29T00:00:00.000' AS DateTime), N'test thôi', N'PENDING', 196)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (29, 106, CAST(N'2023-10-25T09:46:29.433' AS DateTime), CAST(N'2023-11-01T00:00:00.000' AS DateTime), N'đặt lịch k ký gửi', N'PENDING', 200)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (30, 107, CAST(N'2023-10-25T09:57:04.723' AS DateTime), CAST(N'2023-11-01T00:00:00.000' AS DateTime), N'12312312', N'PENDING', 202)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (31, 108, CAST(N'2023-10-25T09:57:53.717' AS DateTime), CAST(N'2023-10-27T00:00:00.000' AS DateTime), N'3213123', N'PENDING', 203)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (32, 109, CAST(N'2023-10-25T10:21:56.813' AS DateTime), CAST(N'2023-10-30T00:00:00.000' AS DateTime), N'Test mua SH', N'PENDING', 208)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (33, 110, CAST(N'2023-10-25T10:23:55.780' AS DateTime), CAST(N'2023-10-31T00:00:00.000' AS DateTime), N'Tới đây với anh!!', N'PENDING', 209)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (34, 111, CAST(N'2023-10-25T14:53:50.780' AS DateTime), CAST(N'2023-10-29T00:00:00.000' AS DateTime), N'Vespa Sprint 150
', N'CANCEL', 210)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (35, 112, CAST(N'2023-10-25T15:40:23.710' AS DateTime), CAST(N'2023-10-30T00:00:00.000' AS DateTime), N'12312312312', N'CANCEL', 213)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (36, 113, CAST(N'2023-10-25T15:41:19.643' AS DateTime), CAST(N'2023-11-08T00:00:00.000' AS DateTime), N'12312312312', N'CANCEL', 214)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (37, 114, CAST(N'2023-10-25T16:05:51.270' AS DateTime), CAST(N'2023-11-02T00:00:00.000' AS DateTime), N'1231231231', N'CANCEL', 215)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (38, 115, CAST(N'2023-10-25T17:27:58.303' AS DateTime), CAST(N'2023-10-31T00:00:00.000' AS DateTime), N'', N'CANCEL', 216)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (39, 116, CAST(N'2023-10-25T17:29:38.430' AS DateTime), CAST(N'2023-10-30T00:00:00.000' AS DateTime), N'', N'CANCEL', 217)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (40, 117, CAST(N'2023-10-25T17:31:00.577' AS DateTime), CAST(N'2023-10-30T00:00:00.000' AS DateTime), N'', N'CANCEL', 218)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (41, 118, CAST(N'2023-10-26T15:47:13.827' AS DateTime), CAST(N'2023-11-08T00:00:00.000' AS DateTime), N'', N'CANCEL', 228)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (42, 119, CAST(N'2023-10-26T16:02:59.377' AS DateTime), CAST(N'2023-10-30T00:00:00.000' AS DateTime), N'2321312312', N'CANCEL', 233)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (43, 120, CAST(N'2023-10-26T16:19:32.940' AS DateTime), CAST(N'2023-10-30T00:00:00.000' AS DateTime), N'231231233', N'CANCEL', 236)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (44, 121, CAST(N'2023-10-28T17:42:30.570' AS DateTime), CAST(N'2023-11-07T00:00:00.000' AS DateTime), N'12312312', N'CANCEL', 253)
GO
INSERT [dbo].[Booking] ([booking_id], [negotiation_id], [date_create], [booking_date], [note], [status], [base_request_id]) VALUES (45, 122, CAST(N'2023-10-29T06:46:56.033' AS DateTime), CAST(N'2023-10-31T00:00:00.000' AS DateTime), N'Hi booking with owner
', N'PENDING', 256)
GO
SET IDENTITY_INSERT [dbo].[Booking] OFF
GO
SET IDENTITY_INSERT [dbo].[BuyerBooking] ON 
GO
INSERT [dbo].[BuyerBooking] ([booking_id], [request_id], [date_create], [booking_date], [note], [status]) VALUES (3, 251, CAST(N'2023-10-28T10:42:38.693' AS DateTime), CAST(N'2023-10-31T17:42:00.000' AS DateTime), N'test Customer bookinh', N'CANCEL')
GO
INSERT [dbo].[BuyerBooking] ([booking_id], [request_id], [date_create], [booking_date], [note], [status]) VALUES (4, 252, CAST(N'2023-10-28T10:44:19.140' AS DateTime), CAST(N'2023-10-31T17:44:00.000' AS DateTime), N'12312312312', N'REJECT')
GO
INSERT [dbo].[BuyerBooking] ([booking_id], [request_id], [date_create], [booking_date], [note], [status]) VALUES (5, 254, CAST(N'2023-10-28T19:13:53.840' AS DateTime), CAST(N'2023-11-01T15:15:00.000' AS DateTime), N'Tôi sẽ tới', N'CANCEL')
GO
INSERT [dbo].[BuyerBooking] ([booking_id], [request_id], [date_create], [booking_date], [note], [status]) VALUES (6, 255, CAST(N'2023-10-29T06:37:30.203' AS DateTime), CAST(N'2023-10-30T15:37:00.000' AS DateTime), N'', N'CANCEL')
GO
INSERT [dbo].[BuyerBooking] ([booking_id], [request_id], [date_create], [booking_date], [note], [status]) VALUES (7, 257, CAST(N'2023-10-29T08:21:19.943' AS DateTime), CAST(N'2023-10-30T20:20:00.000' AS DateTime), N'Account Minh Trí Booking test', N'PENDING')
GO
INSERT [dbo].[BuyerBooking] ([booking_id], [request_id], [date_create], [booking_date], [note], [status]) VALUES (8, 258, CAST(N'2023-10-29T08:22:21.177' AS DateTime), CAST(N'2023-11-02T15:21:00.000' AS DateTime), N'Test Booking account NhutMinh', N'CANCEL')
GO
SET IDENTITY_INSERT [dbo].[BuyerBooking] OFF
GO
SET IDENTITY_INSERT [dbo].[Contract] ON 
GO
INSERT [dbo].[Contract] ([contract_id], [motor_id], [price], [new_owner], [store_id], [content], [created_at], [status], [booking_id], [base_request_id]) VALUES (27, 2038, 17500000.0000, 18, 6, N'Test Re-upContract', CAST(N'2023-10-25T08:43:11.237' AS DateTime), N'CANCEL', 26, 194)
GO
INSERT [dbo].[Contract] ([contract_id], [motor_id], [price], [new_owner], [store_id], [content], [created_at], [status], [booking_id], [base_request_id]) VALUES (28, 2036, 16700000.0000, 2, 5, N'Nouvou', CAST(N'2023-10-25T08:51:43.010' AS DateTime), N'ACCEPT', 27, 195)
GO
INSERT [dbo].[Contract] ([contract_id], [motor_id], [price], [new_owner], [store_id], [content], [created_at], [status], [booking_id], [base_request_id]) VALUES (29, 2038, 19800000.0000, 2, 5, N'Test contract ', CAST(N'2023-10-25T08:52:46.643' AS DateTime), N'ACCEPT', 28, 196)
GO
INSERT [dbo].[Contract] ([contract_id], [motor_id], [price], [new_owner], [store_id], [content], [created_at], [status], [booking_id], [base_request_id]) VALUES (30, 2039, 29500000.0000, 18, 6, N'tải hợp đồng nè :)))', CAST(N'2023-10-25T09:46:59.473' AS DateTime), N'ACCEPT', 29, 200)
GO
INSERT [dbo].[Contract] ([contract_id], [motor_id], [price], [new_owner], [store_id], [content], [created_at], [status], [booking_id], [base_request_id]) VALUES (31, 2037, 14300000.0000, 2, 5, N'23423423', CAST(N'2023-10-25T09:57:21.250' AS DateTime), N'CANCEL', 30, 202)
GO
INSERT [dbo].[Contract] ([contract_id], [motor_id], [price], [new_owner], [store_id], [content], [created_at], [status], [booking_id], [base_request_id]) VALUES (32, 2037, 14300000.0000, 18, 6, N'sdasdsadas', CAST(N'2023-10-25T09:58:12.543' AS DateTime), N'ACCEPT', 31, 203)
GO
INSERT [dbo].[Contract] ([contract_id], [motor_id], [price], [new_owner], [store_id], [content], [created_at], [status], [booking_id], [base_request_id]) VALUES (33, 2045, 80000000.0000, 2, 5, N'Hợp đồng SH 150I', CAST(N'2023-10-25T10:22:35.263' AS DateTime), N'ACCEPT', 32, 208)
GO
INSERT [dbo].[Contract] ([contract_id], [motor_id], [price], [new_owner], [store_id], [content], [created_at], [status], [booking_id], [base_request_id]) VALUES (34, 2045, 75000000.0000, 18, 6, N'HiHI', CAST(N'2023-10-25T10:24:24.807' AS DateTime), N'CANCEL', 33, 209)
GO
INSERT [dbo].[Contract] ([contract_id], [motor_id], [price], [new_owner], [store_id], [content], [created_at], [status], [booking_id], [base_request_id]) VALUES (35, 2044, 63000000.0000, 18, 6, N'tải lại hợp đồng', CAST(N'2023-10-25T15:42:12.527' AS DateTime), N'CANCEL', 36, 214)
GO
INSERT [dbo].[Contract] ([contract_id], [motor_id], [price], [new_owner], [store_id], [content], [created_at], [status], [booking_id], [base_request_id]) VALUES (36, 2044, 63000000.0000, 18, 6, N'2123123', CAST(N'2023-10-26T16:18:28.420' AS DateTime), N'CANCEL', 42, 233)
GO
INSERT [dbo].[Contract] ([contract_id], [motor_id], [price], [new_owner], [store_id], [content], [created_at], [status], [booking_id], [base_request_id]) VALUES (37, 2044, 63000000.0000, 18, 6, N'12123312312', CAST(N'2023-10-26T16:19:45.233' AS DateTime), N'PENDING', 43, 236)
GO
SET IDENTITY_INSERT [dbo].[Contract] OFF
GO
SET IDENTITY_INSERT [dbo].[ContractImage] ON 
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (65, 27, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/dd78130b-380f-48c6-8221-7de6592a33db.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (66, 27, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/3e7a937b-4c8e-4fa8-b595-74c3d8c5e362.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (67, 27, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/b4fe15aa-12d9-49ff-a206-1592bcf0659f.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (70, 29, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/82feea20-9d86-4f79-a4eb-3ba951d93b0f.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (71, 29, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/ac30241f-09a7-481a-9738-f59d3036b083.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (72, 28, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/b8c3724f-f418-4d3d-b826-0a4a47a9395d.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (73, 28, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/1050a24c-af29-4804-b7a1-3f2f84310bcf.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (74, 30, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/09b75ef0-5487-4470-82f8-cc6787982765.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (75, 30, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/b64c6561-a8c8-4c01-bac2-e0ee7cd3cb97.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (76, 31, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/2aecd7cb-9e39-4e04-811c-3805626289bd.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (77, 31, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/62b7e496-75a5-40e7-813d-26cedf599c1e.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (78, 31, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/1bbf45f4-8c35-4081-8441-4539b282bb1d.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (79, 32, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/fd967a8f-57c0-4af5-b628-389da830fb49.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (80, 32, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/383bedcf-a173-48a2-aa9d-0f32726379a2.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (81, 32, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/567c3e5a-5397-4f02-ac9c-8dc42231a0f7.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (82, 33, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/b0f77a8a-cbc5-4850-8f03-770c46345418.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (83, 33, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/7d59b0dc-434f-4bd0-82b1-b5a865b059c5.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (84, 34, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/75e08f4a-3dcf-4d4d-8220-9413609dfc95.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (85, 34, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/99236468-bb51-4f61-87ac-301b7dcfb0f2.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (88, 35, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/69c77e9f-68ab-4ea5-a7f8-e35cc4549541.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (89, 35, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/50b430a4-1161-4d87-b2c1-9e536410a04b.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (90, 36, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/558e77bf-0c0f-4df0-a15b-1744c43cd1c1.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (91, 36, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/cea70dc7-65de-418c-9a23-d034183c995d.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (92, 36, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/c84a4089-1105-47eb-aafa-c33344e85a2a.png', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (93, 36, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/500845b0-3f06-4491-a058-970d2ae3ccaa.png', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (94, 37, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/59409361-820a-4b10-bcff-58b523712210.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (95, 37, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/47a734df-8b0e-473f-b778-3a04594f1899.jpg', NULL)
GO
INSERT [dbo].[ContractImage] ([contract_image_id], [contract_id], [image_link], [description]) VALUES (96, 37, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/667f4313-ab14-4d08-a60b-7d311c9f62e5.png', NULL)
GO
SET IDENTITY_INSERT [dbo].[ContractImage] OFF
GO
SET IDENTITY_INSERT [dbo].[Motorbike] ON 
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2036, N'032492', N'Nouvo 2020', 1, 720888, CAST(N'2020-10-10' AS Date), 16700000.0000, N'Xe cũ nhưng vẫn còn zin.', 3, 1, 5, 15, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/e628c8e2-de0a-4976-8516-8b872061d2f3.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2037, N'567854', N'Wave Alpha', 4, 230100, CAST(N'2021-03-09' AS Date), 14300000.0000, NULL, 3, 2, 6, 15, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/224c4e70-cc57-4caa-a0ba-97f7d14fed7e.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2038, N'866751', N'Vision 2022', 5, 152000, CAST(N'2022-12-01' AS Date), 19800000.0000, NULL, 3, 1, 5, 11, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/94e400de-8c38-4360-9ab5-e20a31abc768.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2039, N'786541', N'Winner X', 3, 810222, CAST(N'2022-02-01' AS Date), 29500000.0000, N'Xe còn mới, thay nhớt xe nhớt máy đều đặn.', 3, 3, 6, 11, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/2cb89318-4aed-4d00-8f06-46c386cc185a.png')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2040, N'676541', N'GSX S150', 8, 194000, CAST(N'2022-09-09' AS Date), 45300000.0000, N'Xe phân phối lớn dành cho đi phượt.', 3, 4, NULL, 1, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/05a52277-e00b-49ff-9fd0-6df8d69505e4.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2041, N'492123', N'Lead 2017', 6, 687300, CAST(N'2018-06-09' AS Date), 20100000.0000, NULL, 1, 1, 5, 2, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/c4cbb90f-4980-4d8a-b707-f7bc96436289.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2042, N'029112', N'Vario 160', 10, 654710, CAST(N'2022-12-12' AS Date), 42000000.0000, N'Xe con mới.', 1, 1, 6, 18, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/b20d966a-e706-4fb0-85bd-df610099e59c.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2043, N'676543', N'Exciter 135', 15, 758310, CAST(N'2018-02-01' AS Date), 17500000.0000, NULL, 1, 3, 6, 18, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/84510a89-e429-452e-9c44-2c2844386e6f.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2044, N'582910', N'Vespa Sprint 150', 12, 72600, CAST(N'2023-01-10' AS Date), 63000000.0000, N'Xe còn rất mới', 4, 1, NULL, 11, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/92de29cc-cc8d-4033-a2e2-abf1cf9683ae.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2045, N'897830', N'SH 150', 16, 20000, CAST(N'2023-09-12' AS Date), 80000000.0000, N'Xe SH150i cũ. Chủ đi giữ kỹ như mới mua !
Có fix giá nhẹ cho anh em thật lòng muốn mua!', 5, 1, 5, 15, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/7f77a16d-eca9-4810-ad97-38f66577d949.jpeg')
GO
SET IDENTITY_INSERT [dbo].[Motorbike] OFF
GO
SET IDENTITY_INSERT [dbo].[MotorbikeBrand] ON 
GO
INSERT [dbo].[MotorbikeBrand] ([brand_id], [brand_name], [description], [status]) VALUES (2, N'Yamaha', NULL, N'ACTIVE')
GO
INSERT [dbo].[MotorbikeBrand] ([brand_id], [brand_name], [description], [status]) VALUES (3, N'Suzuki', NULL, N'ACTIVE')
GO
INSERT [dbo].[MotorbikeBrand] ([brand_id], [brand_name], [description], [status]) VALUES (4, N'Honda', NULL, N'ACTIVE')
GO
INSERT [dbo].[MotorbikeBrand] ([brand_id], [brand_name], [description], [status]) VALUES (5, N'BMW', NULL, N'ACTIVE')
GO
INSERT [dbo].[MotorbikeBrand] ([brand_id], [brand_name], [description], [status]) VALUES (6, N'Kawasaki', N'Chưa có mô tả.', N'ACTIVE')
GO
INSERT [dbo].[MotorbikeBrand] ([brand_id], [brand_name], [description], [status]) VALUES (7, N'Ducati', NULL, N'ACTIVE')
GO
INSERT [dbo].[MotorbikeBrand] ([brand_id], [brand_name], [description], [status]) VALUES (8, N'Piaggio', N'', N'ACTIVE')
GO
INSERT [dbo].[MotorbikeBrand] ([brand_id], [brand_name], [description], [status]) VALUES (9, N'Piaggio 2', N'', N'PENDING')
GO
INSERT [dbo].[MotorbikeBrand] ([brand_id], [brand_name], [description], [status]) VALUES (10, N'234234', N'', N'PENDING')
GO
SET IDENTITY_INSERT [dbo].[MotorbikeBrand] OFF
GO
SET IDENTITY_INSERT [dbo].[MotorbikeImage] ON 
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (131, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/c9aa31a3-a90b-49cc-babc-8a6314eb3d97.jpg', 2036)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (132, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/93a8ca75-df00-4aab-82e6-91ddcea14ed4.jpg', 2036)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (133, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/90d5b520-31a6-499b-9a30-8cad47e46cb8.jpg', 2037)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (134, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/f6b183b7-b49c-4596-bfcc-cacd11d155bc.png', 2037)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (135, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/b5ebd947-a323-4738-8244-cfabcce8bcd4.png', 2038)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (136, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/557896da-9309-4aea-b837-93e1755ad5ae.jpg', 2038)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (137, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/8d67f541-262e-4ebf-ac81-5cc713d81d5f.jpg', 2039)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (138, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/32c72a73-8289-45a6-991a-291201a46233.jpg', 2039)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (139, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/d7b36c94-1dbf-4575-9684-f03e8fdfb43e.jpg', 2039)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (140, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/9326e327-b642-4068-a330-49819bf36108.jpg', 2040)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (141, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/d225c6c4-7b5e-4ab8-9d23-b346a87748d2.jpg', 2040)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (142, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/ec172b62-c367-4e60-82fc-3129be3410ca.jpg', 2041)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (143, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/74e23eab-ac98-46ac-be2e-09622f0b139c.jpg', 2041)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (144, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/e4e64315-38fd-44d8-8593-dc9c4b960f61.jpg', 2042)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (145, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/5e9161d7-9cdd-485e-aec7-04f01e139174.png', 2042)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (146, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/0a321060-0fad-4c8b-b862-837c05c50543.png', 2042)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (147, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/43738ac4-c69c-4323-b351-62b9dbf478f2.jpg', 2043)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (148, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/beedf3d8-07fd-4240-979f-3bf41f4841bb.jpg', 2043)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (149, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/85ec7ede-476f-4f80-8365-eb5adcb51eb3.jpg', 2043)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (150, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/c368b4e6-2065-4aa4-8dcd-61e64e909ea0.jpg', 2044)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (151, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/d271a8c9-07b0-4c7f-b755-80f9253d1bc6.jpg', 2044)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (152, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/9f821df8-7113-4690-a5ef-ad11ca744c90.jpg', 2045)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (153, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/43880087-7560-42cb-a058-92bb82c5cbb2.jpg', 2045)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (154, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/db057a59-f0be-4ec6-8de7-cb056fdf330d.jpg', 2045)
GO
SET IDENTITY_INSERT [dbo].[MotorbikeImage] OFF
GO
SET IDENTITY_INSERT [dbo].[MotorbikeModel] ON 
GO
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status], [brand_id]) VALUES (1, N'Nouvo', NULL, N'ACTIVE', 2)
GO
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status], [brand_id]) VALUES (3, N'WinnerX', NULL, N'ACTIVE', 2)
GO
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status], [brand_id]) VALUES (4, N'Wave', NULL, N'ACTIVE', 4)
GO
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status], [brand_id]) VALUES (5, N'Vision', NULL, N'ACTIVE', 4)
GO
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status], [brand_id]) VALUES (6, N'Lead', NULL, N'ACTIVE', 4)
GO
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status], [brand_id]) VALUES (7, N'Satria', NULL, N'ACTIVE', 3)
GO
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status], [brand_id]) VALUES (8, N'GSX', NULL, N'ACTIVE', 3)
GO
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status], [brand_id]) VALUES (9, N'Ninja', NULL, N'ACTIVE', 6)
GO
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status], [brand_id]) VALUES (10, N'Vario', N'', N'ACTIVE', 4)
GO
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status], [brand_id]) VALUES (11, N'Sirius 2020', N'', N'PENDING', 2)
GO
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status], [brand_id]) VALUES (12, N'Vespa', N'', N'ACTIVE', 8)
GO
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status], [brand_id]) VALUES (13, N'1111', N'', N'PENDING', 2)
GO
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status], [brand_id]) VALUES (14, N'Air Blade', N'', N'ACTIVE', 4)
GO
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status], [brand_id]) VALUES (15, N'Exciter', N'', N'ACTIVE', 2)
GO
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status], [brand_id]) VALUES (16, N'SH 150i', N'', N'ACTIVE', 4)
GO
SET IDENTITY_INSERT [dbo].[MotorbikeModel] OFF
GO
SET IDENTITY_INSERT [dbo].[MotorbikeStatus] ON 
GO
INSERT [dbo].[MotorbikeStatus] ([motorStatus_id], [title], [description]) VALUES (1, N'POSTING', NULL)
GO
INSERT [dbo].[MotorbikeStatus] ([motorStatus_id], [title], [description]) VALUES (3, N'STORAGE', NULL)
GO
INSERT [dbo].[MotorbikeStatus] ([motorStatus_id], [title], [description]) VALUES (4, N'CONSIGNMENT', NULL)
GO
INSERT [dbo].[MotorbikeStatus] ([motorStatus_id], [title], [description]) VALUES (5, N'LIVELIHOOD', NULL)
GO
SET IDENTITY_INSERT [dbo].[MotorbikeStatus] OFF
GO
SET IDENTITY_INSERT [dbo].[MotorbikeType] ON 
GO
INSERT [dbo].[MotorbikeType] ([motorType_id], [title], [description], [status]) VALUES (1, N'Xe tay ga', NULL, NULL)
GO
INSERT [dbo].[MotorbikeType] ([motorType_id], [title], [description], [status]) VALUES (2, N'Xe số', N'', N'')
GO
INSERT [dbo].[MotorbikeType] ([motorType_id], [title], [description], [status]) VALUES (3, N'Xe côn tay', N'Chưa có mô tả.', N'')
GO
INSERT [dbo].[MotorbikeType] ([motorType_id], [title], [description], [status]) VALUES (4, N'Xe phân khối lớn', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[MotorbikeType] OFF
GO
SET IDENTITY_INSERT [dbo].[Negotiation] ON 
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (103, 194, CAST(17500000.0000 AS Decimal(15, 4)), NULL, CAST(N'2023-10-25T08:41:54.133' AS DateTime), CAST(N'2023-10-25T08:42:21.433' AS DateTime), N'Test Flow thương lượng', N'ACCEPT', CAST(17500000.0000 AS Decimal(15, 4)), 194, CAST(N'2023-10-28T08:41:54.133' AS DateTime), 18)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (104, 195, NULL, NULL, CAST(N'2023-10-25T08:50:59.017' AS DateTime), CAST(N'2023-10-25T08:50:59.017' AS DateTime), NULL, N'ACCEPT', CAST(16700000.0000 AS Decimal(15, 4)), 195, NULL, NULL)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (105, 196, NULL, NULL, CAST(N'2023-10-25T08:52:12.743' AS DateTime), CAST(N'2023-10-25T08:52:12.743' AS DateTime), NULL, N'ACCEPT', CAST(19800000.0000 AS Decimal(15, 4)), 196, NULL, NULL)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (106, 200, CAST(26000000.0000 AS Decimal(15, 4)), CAST(29500000.0000 AS Decimal(15, 4)), CAST(N'2023-10-25T09:45:08.067' AS DateTime), CAST(N'2023-10-25T09:46:04.177' AS DateTime), N'26 triệu thui nghen', N'ACCEPT', CAST(29500000.0000 AS Decimal(15, 4)), 200, CAST(N'2023-10-28T09:45:08.067' AS DateTime), 11)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (107, 202, NULL, NULL, CAST(N'2023-10-25T09:56:52.930' AS DateTime), CAST(N'2023-10-25T09:56:52.930' AS DateTime), NULL, N'ACCEPT', CAST(14300000.0000 AS Decimal(15, 4)), 202, NULL, NULL)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (108, 203, NULL, NULL, CAST(N'2023-10-25T09:57:46.213' AS DateTime), CAST(N'2023-10-25T09:57:46.213' AS DateTime), NULL, N'ACCEPT', CAST(14300000.0000 AS Decimal(15, 4)), 203, NULL, NULL)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (109, 208, NULL, NULL, CAST(N'2023-10-25T10:21:42.310' AS DateTime), CAST(N'2023-10-25T10:21:42.310' AS DateTime), NULL, N'ACCEPT', CAST(80000000.0000 AS Decimal(15, 4)), 208, NULL, NULL)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (110, 209, CAST(75000000.0000 AS Decimal(15, 4)), NULL, CAST(N'2023-10-25T10:23:15.233' AS DateTime), CAST(N'2023-10-25T10:23:39.437' AS DateTime), N'75 dc hơm', N'ACCEPT', CAST(75000000.0000 AS Decimal(15, 4)), 209, CAST(N'2023-10-28T10:23:15.233' AS DateTime), 18)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (111, 210, NULL, NULL, CAST(N'2023-10-25T14:53:27.407' AS DateTime), CAST(N'2023-10-25T14:53:27.407' AS DateTime), NULL, N'ACCEPT', CAST(63000000.0000 AS Decimal(15, 4)), 210, NULL, NULL)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (112, 213, NULL, NULL, CAST(N'2023-10-25T15:40:13.660' AS DateTime), CAST(N'2023-10-25T15:40:13.660' AS DateTime), NULL, N'ACCEPT', CAST(63000000.0000 AS Decimal(15, 4)), 213, NULL, NULL)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (113, 214, NULL, NULL, CAST(N'2023-10-25T15:40:59.003' AS DateTime), CAST(N'2023-10-25T15:40:59.003' AS DateTime), NULL, N'ACCEPT', CAST(63000000.0000 AS Decimal(15, 4)), 214, NULL, NULL)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (114, 215, NULL, NULL, CAST(N'2023-10-25T16:05:42.217' AS DateTime), CAST(N'2023-10-25T16:05:42.217' AS DateTime), NULL, N'ACCEPT', CAST(63000000.0000 AS Decimal(15, 4)), 215, NULL, NULL)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (115, 216, NULL, NULL, CAST(N'2023-10-25T17:27:49.443' AS DateTime), CAST(N'2023-10-25T17:27:49.443' AS DateTime), NULL, N'ACCEPT', CAST(63000000.0000 AS Decimal(15, 4)), 216, NULL, NULL)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (116, 217, NULL, NULL, CAST(N'2023-10-25T17:29:32.330' AS DateTime), CAST(N'2023-10-25T17:29:32.330' AS DateTime), NULL, N'ACCEPT', CAST(63000000.0000 AS Decimal(15, 4)), 217, NULL, NULL)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (117, 218, NULL, NULL, CAST(N'2023-10-25T17:30:52.870' AS DateTime), CAST(N'2023-10-25T17:30:52.870' AS DateTime), NULL, N'ACCEPT', CAST(63000000.0000 AS Decimal(15, 4)), 218, NULL, NULL)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (118, 228, NULL, NULL, CAST(N'2023-10-26T15:47:07.200' AS DateTime), CAST(N'2023-10-26T15:47:07.200' AS DateTime), NULL, N'ACCEPT', CAST(63000000.0000 AS Decimal(15, 4)), 228, NULL, NULL)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (119, 233, NULL, NULL, CAST(N'2023-10-26T16:02:46.770' AS DateTime), CAST(N'2023-10-26T16:02:46.770' AS DateTime), NULL, N'ACCEPT', CAST(63000000.0000 AS Decimal(15, 4)), 233, NULL, NULL)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (120, 236, NULL, NULL, CAST(N'2023-10-26T16:19:25.377' AS DateTime), CAST(N'2023-10-26T16:19:25.377' AS DateTime), NULL, N'ACCEPT', CAST(63000000.0000 AS Decimal(15, 4)), 236, NULL, NULL)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (121, 253, CAST(40000000.0000 AS Decimal(15, 4)), CAST(50000000.0000 AS Decimal(15, 4)), CAST(N'2023-10-28T17:39:26.137' AS DateTime), CAST(N'2023-10-28T17:42:21.493' AS DateTime), N'hi', N'ACCEPT', CAST(50000000.0000 AS Decimal(15, 4)), 253, CAST(N'2023-10-31T17:39:26.137' AS DateTime), 11)
GO
INSERT [dbo].[Negotiation] ([negotiation_id], [request_id], [store_price], [owner_price], [start_time], [end_time], [description], [status], [final_price], [base_request_id], [expired_time], [last_change_user_id]) VALUES (122, 256, NULL, NULL, CAST(N'2023-10-29T06:46:29.020' AS DateTime), CAST(N'2023-10-29T06:46:29.020' AS DateTime), NULL, N'ACCEPT', CAST(63000000.0000 AS Decimal(15, 4)), 256, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Negotiation] OFF
GO
SET IDENTITY_INSERT [dbo].[Request] ON 
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (178, 2036, 1, 15, CAST(N'2023-10-25T08:24:55.600' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (179, 2037, 1, 15, CAST(N'2023-10-25T08:27:38.180' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (180, 2036, 1, 15, CAST(N'2023-10-25T08:28:00.640' AS DateTime), 3, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (181, 2037, 1, 15, CAST(N'2023-10-25T08:28:06.567' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (182, 2038, 1, 11, CAST(N'2023-10-25T08:30:11.807' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (183, 2039, 1, 11, CAST(N'2023-10-25T08:31:54.757' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (184, 2038, 1, 11, CAST(N'2023-10-25T08:32:00.910' AS DateTime), 3, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (185, 2039, 1, 11, CAST(N'2023-10-25T08:32:06.807' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (186, 2040, 1, 2, CAST(N'2023-10-25T08:34:07.300' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (187, 2041, 1, 2, CAST(N'2023-10-25T08:36:34.410' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (188, 2041, 1, 2, CAST(N'2023-10-25T08:36:40.673' AS DateTime), 2, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (189, 2040, 1, 2, CAST(N'2023-10-25T08:37:38.210' AS DateTime), 2, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (190, 2042, 1, 18, CAST(N'2023-10-25T08:39:02.207' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (191, 2043, 1, 18, CAST(N'2023-10-25T08:40:08.160' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (192, 2042, 1, 18, CAST(N'2023-10-25T08:40:15.220' AS DateTime), 2, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (193, 2043, 1, 18, CAST(N'2023-10-25T08:40:25.440' AS DateTime), 2, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (194, 2038, 11, 18, CAST(N'2023-10-25T08:41:54.110' AS DateTime), 8, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (195, 2036, 15, 2, CAST(N'2023-10-25T08:50:59.003' AS DateTime), 8, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (196, 2038, 11, 2, CAST(N'2023-10-25T08:52:12.737' AS DateTime), 8, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (197, 2041, 1, 2, CAST(N'2023-10-25T09:14:10.597' AS DateTime), 2, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (198, 2040, 1, 2, CAST(N'2023-10-25T09:27:29.200' AS DateTime), 9, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (199, 2036, 1, 2, CAST(N'2023-10-25T09:42:27.077' AS DateTime), 2, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (200, 2039, 11, 18, CAST(N'2023-10-25T09:45:08.043' AS DateTime), 8, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (201, 2039, 1, 18, CAST(N'2023-10-25T09:53:00.260' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (202, 2037, 15, 2, CAST(N'2023-10-25T09:56:52.900' AS DateTime), 8, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (203, 2037, 15, 18, CAST(N'2023-10-25T09:57:46.203' AS DateTime), 8, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (204, 2044, 1, 11, CAST(N'2023-10-25T10:16:26.543' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (205, 2044, 1, 11, CAST(N'2023-10-25T10:16:34.383' AS DateTime), 3, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (206, 2045, 1, 15, CAST(N'2023-10-25T10:21:02.713' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (207, 2045, 1, 15, CAST(N'2023-10-25T10:21:14.103' AS DateTime), 3, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (208, 2045, 15, 2, CAST(N'2023-10-25T10:21:42.297' AS DateTime), 8, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (209, 2045, 15, 18, CAST(N'2023-10-25T10:23:15.223' AS DateTime), 8, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (210, 2044, 11, 18, CAST(N'2023-10-25T14:53:26.847' AS DateTime), 8, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (211, 2036, 1, 2, CAST(N'2023-10-25T15:11:51.567' AS DateTime), 2, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (212, 2036, 1, 2, CAST(N'2023-10-25T15:19:37.460' AS DateTime), 2, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (213, 2044, 11, 18, CAST(N'2023-10-25T15:40:13.643' AS DateTime), 8, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (214, 2044, 11, 18, CAST(N'2023-10-25T15:40:58.990' AS DateTime), 8, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (215, 2044, 11, 2, CAST(N'2023-10-25T16:05:42.183' AS DateTime), 8, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (216, 2044, 11, 2, CAST(N'2023-10-25T17:27:48.863' AS DateTime), 8, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (217, 2044, 11, 2, CAST(N'2023-10-25T17:29:32.320' AS DateTime), 8, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (218, 2044, 11, 18, CAST(N'2023-10-25T17:30:52.860' AS DateTime), 8, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (219, 2045, 1, 2, CAST(N'2023-10-26T02:41:06.607' AS DateTime), 2, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (220, 2045, 1, 2, CAST(N'2023-10-26T02:42:13.860' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (221, 2045, 1, 2, CAST(N'2023-10-26T06:21:06.427' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (222, 2045, 1, 2, CAST(N'2023-10-26T06:25:30.633' AS DateTime), 2, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (223, 2038, 1, 2, CAST(N'2023-10-26T06:25:55.307' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (224, 2041, 1, 2, CAST(N'2023-10-26T06:37:51.237' AS DateTime), 2, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (225, 2041, 1, 2, CAST(N'2023-10-26T06:38:49.767' AS DateTime), 2, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (226, 2045, 1, 2, CAST(N'2023-10-26T07:28:05.823' AS DateTime), 4, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (227, 2036, 1, 2, CAST(N'2023-10-26T07:36:13.110' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (228, 2044, 11, 18, CAST(N'2023-10-26T15:47:06.640' AS DateTime), 8, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (229, 2037, 1, 18, CAST(N'2023-10-26T15:50:01.607' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (230, 2043, 1, 18, CAST(N'2023-10-26T15:51:21.517' AS DateTime), 2, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (231, 2043, 1, 18, CAST(N'2023-10-26T15:51:22.177' AS DateTime), 2, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (232, 2038, 1, 2, CAST(N'2023-10-26T16:00:24.967' AS DateTime), 3, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (233, 2044, 11, 18, CAST(N'2023-10-26T16:02:46.743' AS DateTime), 8, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (234, 2038, 1, 2, CAST(N'2023-10-26T16:06:23.837' AS DateTime), 3, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (235, 2038, 1, 2, CAST(N'2023-10-26T16:09:25.327' AS DateTime), 3, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (236, 2044, 11, 18, CAST(N'2023-10-26T16:19:25.350' AS DateTime), 8, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (237, 2036, 1, 2, CAST(N'2023-10-26T16:21:41.933' AS DateTime), 3, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (239, 2037, 1, 18, CAST(N'2023-10-27T10:29:50.917' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (240, 2043, 1, 18, CAST(N'2023-10-27T10:32:12.673' AS DateTime), 2, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (241, 2039, 1, 18, CAST(N'2023-10-27T10:54:42.880' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (251, 2045, 2, 17, CAST(N'2023-10-28T10:42:38.143' AS DateTime), 6, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (252, 2045, 2, 10, CAST(N'2023-10-28T10:44:19.123' AS DateTime), 6, N'REJECT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (253, 2044, 11, 18, CAST(N'2023-10-28T17:39:25.910' AS DateTime), 8, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (254, 2045, 2, 17, CAST(N'2023-10-28T19:13:53.783' AS DateTime), 6, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (255, 2045, 2, 17, CAST(N'2023-10-29T06:37:29.627' AS DateTime), 6, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (256, 2044, 11, 2, CAST(N'2023-10-29T06:46:28.987' AS DateTime), 8, N'PENDING')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (257, 2045, 2, 10, CAST(N'2023-10-29T08:21:19.737' AS DateTime), 6, N'PENDING')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (258, 2045, 2, 17, CAST(N'2023-10-29T08:22:21.150' AS DateTime), 6, N'CANCEL')
GO
SET IDENTITY_INSERT [dbo].[Request] OFF
GO
SET IDENTITY_INSERT [dbo].[RequestType] ON 
GO
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (1, N'Motor-Register', N'Ðang ký xe vào kho')
GO
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (2, N'Motor-Posting', N'Đăng xe lên sàn')
GO
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (3, N'Motor-Consignment', N'Owner dang xe ký g?i')
GO
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (4, N'Motor-nonConsignment', N'Owner k ky gui')
GO
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (5, N'Purchase-Request', N'Yêu cầu mua xe')
GO
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (6, N'Booking-Request', N'Yêu cầu đặt lịch hẹn')
GO
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (7, N'MotorModel-Register', NULL)
GO
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (8, N'Negotiation-Request', N'Yêu cầu thương lượng giá cả')
GO
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (9, N'Store-TransferOwnership', NULL)
GO
SET IDENTITY_INSERT [dbo].[RequestType] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 
GO
INSERT [dbo].[Role] ([role_id], [title]) VALUES (1, N'Admin')
GO
INSERT [dbo].[Role] ([role_id], [title]) VALUES (2, N'Store')
GO
INSERT [dbo].[Role] ([role_id], [title]) VALUES (3, N'Owner')
GO
INSERT [dbo].[Role] ([role_id], [title]) VALUES (4, N'Customer')
GO
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
SET IDENTITY_INSERT [dbo].[StoreDesciption] ON 
GO
INSERT [dbo].[StoreDesciption] ([store_id], [user_id], [store_name], [description], [store_phone], [store_email], [store_created_at], [store_updated_at], [point], [address], [ward_id], [status], [tax_code], [business_license]) VALUES (5, 2, N'Xe Máy Cũ Thái Hòa', NULL, N'0369269410', N'nhutminh.it19@gmail.com', CAST(N'2023-10-10T08:21:07.567' AS DateTime), NULL, NULL, N'288/3 Man Thiên, phường Tăng Nhơn Phú A, Quận 9, Tp. Hồ Chí Minh.', NULL, N'ACTIVE', N'4536789078456', N'https://motorbikeimages.blob.core.windows.net/motorbikebs/9142e438-368f-4d8b-8e3e-d64772e27e91.png')
GO
INSERT [dbo].[StoreDesciption] ([store_id], [user_id], [store_name], [description], [store_phone], [store_email], [store_created_at], [store_updated_at], [point], [address], [ward_id], [status], [tax_code], [business_license]) VALUES (6, 18, N'Xe Máy Cũ Ðức Dũng II', NULL, N'0937500668', N'phatntse150133@fpt.edu.vn', CAST(N'2023-10-10T08:22:11.503' AS DateTime), NULL, NULL, N'TL15- Ấp Phú An, Xã Phú Hòa Đông, Huyện Củ Chi, TP.HCM', NULL, N'ACTIVE', N'5566772234512', N'https://motorbikeimages.blob.core.windows.net/motorbikebs/72fe9c06-763e-430d-b710-951836da31f0.jpg')
GO
INSERT [dbo].[StoreDesciption] ([store_id], [user_id], [store_name], [description], [store_phone], [store_email], [store_created_at], [store_updated_at], [point], [address], [ward_id], [status], [tax_code], [business_license]) VALUES (7, 17, N'NhutMinh Biker', NULL, N'0987835426', N'rwer@gmail.com', CAST(N'2023-10-20T08:37:14.293' AS DateTime), NULL, NULL, N'Lo E1 khu CNC', NULL, N'REFUSE', N'1231231231212', N'https://motorbikeimages.blob.core.windows.net/motorbikebs/d89dab22-6e44-4017-b0ec-c2307cd4fb65.jpg')
GO
INSERT [dbo].[StoreDesciption] ([store_id], [user_id], [store_name], [description], [store_phone], [store_email], [store_created_at], [store_updated_at], [point], [address], [ward_id], [status], [tax_code], [business_license]) VALUES (8, 10, N'Cửa hàng Lizins', NULL, N'0908660977', N'phanminhtri@gmail.com', CAST(N'2023-10-24T18:30:17.187' AS DateTime), NULL, NULL, N'360A, Quận 4, TP. HCM', NULL, N'REFUSE', N'0924120214', N'https://motorbikeimages.blob.core.windows.net/motorbikebs/edb2d27a-ecbd-4982-8dae-fb7f40151635.jpg')
GO
SET IDENTITY_INSERT [dbo].[StoreDesciption] OFF
GO
SET IDENTITY_INSERT [dbo].[StoreImage] ON 
GO
INSERT [dbo].[StoreImage] ([store_image_id], [image_link], [store_id]) VALUES (5, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/c1d2a91e-0290-4bed-80f0-9376d8d4dd64.jpg', 5)
GO
INSERT [dbo].[StoreImage] ([store_image_id], [image_link], [store_id]) VALUES (6, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/a38fb08a-593f-4361-9369-554b66840c49.jpg', 6)
GO
INSERT [dbo].[StoreImage] ([store_image_id], [image_link], [store_id]) VALUES (7, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/3dcc391e-c5a0-4c1c-b2f1-e1260b7d2b1b.jpg', 7)
GO
INSERT [dbo].[StoreImage] ([store_image_id], [image_link], [store_id]) VALUES (8, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/8be93754-9c8d-48fe-b9ee-2bb4313c0ba0.png', 8)
GO
SET IDENTITY_INSERT [dbo].[StoreImage] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (1, N'Nguyễn Tiến Phát', N'phat8a6@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2023-10-05' AS Date), NULL, N'ACTIVE', 0x1C51F0A3E77FD04869A3AD93C555E9822B3C2F3812AB93F0246B2C328FAA91588FD611F623AFFDD8CE2CED8366C6133A4AA4B1115240E1F2EBC7328632C4B763, 0x1A05AE7EF71C1D135CFBC01A60B29963B5E026E740218596265EFF691D2330E2F0AD13C07126252B687F721C3BAEB0734858FBB72C77718BC4D731C4794E273415879D4AA447012FBA60833D7CD8771A021117A2F870A1AADD2F8A5EAE504A4D0299DE653EAA4DF0C9D42BD88C9414785FBB9E56F0C2D5EBCBF68040008D6E8A, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (2, N'Nguyễn Nhựt Minh', N'nhutminh.it19@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 2, CAST(N'2023-10-05' AS Date), NULL, N'ACTIVE', 0xFC4D263550BA97B8775151A71E0D5088571892C7A99FECC77E204E600E759E81A4BFB02ECE2711241A4442A540A62AAD7AC48C6E5D0A635CE77EC236623384F8, 0x63AA969B866826952196CD5A4F129576D9B045B88B02B54CC22B8AEF50CABC3263A600490206171D834793AE3AE6F96DF03CBC133992B635E66F7F9AE3D6A2D4546DB146667D8602008EE111586880F61F0F983A7F09C35D1DFC3E699D1B20BD9B81B5B7A8DB0CC547A134CEADC655A6DE186E5DC4679B96AF7C1163D3D3F725, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (10, N'Minh Trí', N'phanminhtri269@gmail.com', N'0908660977', 1, CAST(N'2001-09-26' AS Date), N'123123123123', N'Quận 4, Hồ Chí Minh', NULL, 4, CAST(N'2023-10-05' AS Date), CAST(N'2023-10-05' AS Date), N'ACTIVE', 0xA585D4D64DFDB23C80658BA185A70CF6AAE8A53E78437CD394219CAC568837ED754795D418158B795B48B092164CCEDB15CF4709706E6D476E36590530E833E1, 0x6F607BE7009771AE4E9906491FA12B5DB502D738E8207702325DEB52262E85767BCEA9BCEEB6C6826FE88B6A6E9F7C2260A8C7FDB0768DE153729018B9DC5AD55F5B1D8AD1BE36C85F64ED6103CFAF863FA77E2CDD9BB834124B1A247DDAD18E0272683C206CDC51A3A6673D785F846BE98598E9675C8EC2E90FB474799FD07D, N'485B50FED6BA6F0BA8B19B7BA221CC216A3C84F40843F9603193903C2194DAAC', CAST(N'2023-10-21T21:38:20.527' AS DateTime), N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (11, N'Minh Tri FPT', N'tripmse150151@fpt.edu.vn', N'0937500668', NULL, NULL, N'436785096712', N'225 Đường Âu Dương Lân, Phường 3, Quận 8, TP.HCM', NULL, 3, CAST(N'2023-10-05' AS Date), NULL, N'ACTIVE', 0xB376591F2980D24DCE3229337D2799269090CE67CFEA18C27C713408272592456165A86C6279EBE31A5A3D78FBDEA5158DE197D05B6D65B2B056AE70F52AE51D, 0x3A3A7E5C89F4C42B1B7F62C9D9576CFB9EA08F1AF75D94CA01A22940ACFB81FDD3EBD5D8BFDAC0CA33E21F130ACCDF25AB73FD44FC3D91029E8651DA91665D65B2A531395AD927A78657B5E0BD400670ED54C49FCFDC421EE0A2DE5167255192B49D8D409673054FA577EC55E034FE238EDFA3DF696B0C4319273FB6838848E2, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (14, N'Nhut Minh', N'minhnnse150140@fpt.com.vn', NULL, NULL, NULL, NULL, NULL, NULL, 4, NULL, NULL, N'NOT VERIFY', 0xE324DC2B63CA0938F622C96FFE862F25C582400B1DE619F1AFBA065CCDDCDF0E8DB471A1D4FC8C4FE684CEB78E6587460183195498B151776C1CCB58186E00AB, 0xF2913D37CEE620C2180EABFB2D41BE176D33B98022948D9C4A1CFBE6CF12DC80576733EB3BAF83F7C979AC0550BFD57AFEFB27218439AA5E9A936D6B7332E089F59EE48A91CFC0F03F9F5B4AF2D1B9566C1BA748BDAFFF7D139B110EE27A2B531F541A9EC58D6D28578E86CAD01896E00D0EC728608E7C38B9DEBD2C0008F849, NULL, NULL, N'76DE90E1C3C4AF539B7E81C5257FDC7B7B31845A601626DCA89F27750F5F9460', CAST(N'2023-10-05T21:18:47.057' AS DateTime))
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (15, N'Trần Thành Công', N'tranthanhcong652001@gmail.com', N'0855562320', 1, CAST(N'2003-01-14' AS Date), N'545467894522', N'Bến Vân Đồn, Mỹ Tho, Tiền Giang', NULL, 3, CAST(N'2023-10-05' AS Date), CAST(N'2023-10-05' AS Date), N'ACTIVE', 0x9EDD41CC51B6790001988E63F3B9120BC2706FAE56A616B65F8A97621C8DE806F525BFF63A428E30BE3BEAF519A0830626E250614BD2EAEAC87E6151D40AE2BD, 0x61D44F72DFE5077B2D3E1944E3CD4DE2E26D00DEA6B322638FC40FAF5ED0C1947D4FA16D4625F6E127DCE970487CF3BFD91E737007926279C9945B9A1B28671C85C744170B24F3515A3A22427BAD40E86F95E2A9C9BBBEB2A772BE09481BDBE3DCA20747DED8EDB0C0CCAAC4FD57AF0C31BD98D193BC63459470DF3B66E9BEFF, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (16, N'HungStore', N'hung.trangquang071117@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 4, NULL, NULL, N'NOT VERIFY', 0x9428861020FB2586B4A932A1858BEAB8B03123971056466D90390CA4546CC52E23ABCAF3607663AE176B7806DA597BAA876FC3001B43620A129CEB584BF1797E, 0x591E4658B62BE1EF55AE2F4E9F299AA48D337C594C22843D70B8B380E4B2EC8E238A70FEBD1DECD48CBA341A4CB4C8516C5E49B1D67CAAAB1AA34750DA212122E16E67303A8AFB7CA8F569A015CDA28F1D620725A5B4D9DA3DF53519916AB88E7F613489E1D4F57D1860C0DE088532397B28F3727F8CDAD67E2B22C086AAE49E, NULL, NULL, N'0D8B65B62609373A9517117CCC0C060C5C57A8099E035CE821081698B4CF69D3', CAST(N'2023-10-07T01:03:56.623' AS DateTime))
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (17, N'Nhựt Minh Nguyễn', N'minhnnse150140@fpt.edu.vn', N'0369269410', 1, CAST(N'2001-01-16' AS Date), N'261567883654', N'288/3 Man Thiên, phường Tăng Nhơn Phú A, Quận 9, Tp. Hồ Chí Minh.', NULL, 4, CAST(N'2023-10-06' AS Date), CAST(N'2023-10-29' AS Date), N'ACTIVE', 0x36C102C6765BEB61FE222987B688086F101BDDFD3E74C77DCCA38FB84C53071C384071E7933F8A36E10E5BBD732CD6B77F667D6D9FC994D78C8508686886F43E, 0x68F9E7AE84F378708600804E3ABFA2872458B95CA033C46D6742C7F53C6A8C4D878CE51F4263082E25A82545D77E5C5444FF3BF7A851ED817A1CF7E7CAB4DBDA1934DFDF9C9BE29AF1234E0EA4621496068C805C363FE4B9F4A3D5108CDA9F7E62EAD2687A087E3D8989BECAC5FD46EBE11602758D31C034FD0C887F17DDE999, N'2ABD00E3F8827B67204FF541F6552AB3E6B25EE4B0F5844C7F19DF455B3FB05C', CAST(N'2023-10-21T21:38:52.847' AS DateTime), N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (18, N'Nguyễn Nhựt Minh', N'nhutminh.fpt@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 2, CAST(N'2023-10-10' AS Date), NULL, N'ACTIVE', 0x859E4F440D27BD3A06E706CC6D5F46D294901F8751608C8704E0F09B1D43C2A770C099AD046315F51E6FB90D9BA5A9A38D072923CE9899ED18E8262A591A02D2, 0xCB77AAACFC9607953E3BED96DC869630E3F979EC9CA6DD5781ED4D549A980AE6291DABC659E7B4BA8788004DCC79B2B98BE1BDE7DBB0A059A66006DB14B1386E3BABC5AD1DBB8B988FAA06466F67597274C9F5BFB02C933676EB424C28D95F66D757C4649EF85863BEEBEAAA92813BB1A64E026EB4B04C8BE745FCB7A5B37438, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (19, N'Phan Trí', N'phantri@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 4, NULL, NULL, N'NOT VERIFY', 0xF48BCF1D5A53BB57D6CF2574B0BEA02519F4340E531AE4B0F86EC618A3AFED685AADD06D00D63D3179A58B619436CA4708CE9B550E09FA220296FE5823444055, 0x816EC2906AD9B7AAB424D45DA2B710F1D563A3ACB8C900E0AE4EAC25C8F68B14940C9FE146446D72F7B319158A973E1093521E142A599A7CEA844F781F030EA4C14372E1F09D97FAD28C9638AACE889152B61A0CE7EC58BFA776610ECF7E7F1F449143DF8EE35E74B2E65EC001A25E7A256D60416A8EC4FE729BE05364D0170E, NULL, NULL, N'BD3F7D35EF2794738807154A1760B2CAB1BF1415627766BDFE3481B64628CD14', CAST(N'2023-10-17T19:32:02.507' AS DateTime))
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (20, N'Nguyễn Tiến Phát', N'phatntse150133@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 4, NULL, NULL, N'NOT VERIFY', 0x8B70575F0C7A0648A5FF507A77FAEB029E9A6C62BB9A3F3395C03B866E4D7646EAEB9851712295FA83D128B9BCE1908545B9D6D87E148B2CAA1C1B1DBE65A80C, 0xC8A4E866ADC1FB56FB6B912924C41199E861AC44382C96A1D2A9D6EC2B7F94E064FA52FA3EED0CA4B171D5772034501164FD8B8B8CFD384258236A64B04631C17DA58411F18063BE15F017CDC348B188C06925B304D59915758F6583EA7FAF82DA57FD1FF2A262ADF4B1BFD82B737B3465E2BDAB68C1964C4CD0FE2C859ACA1F, NULL, NULL, N'7590C9F91189269B56C5E624AE77D2ABC0DA0ACB63E8052040647C897E734871', CAST(N'2023-10-21T21:40:41.823' AS DateTime))
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (21, N'Nguyễn Tiến Phát', N'phatntse150133@fpt.edu.vn', NULL, NULL, NULL, NULL, NULL, NULL, 4, CAST(N'2023-10-21' AS Date), NULL, N'ACTIVE', 0xD20CF99458D9402440806DB637CDE10F0AE09BAB205C435B907EF81E0206E1D448E91BA2C26154C10E8743785562D230DF5F658F1283EAAC3962E9121ED79982, 0xBCB8C33D5ED84989647C349602455004072ECB45FFBD50431B9DDAA023D8A1353B5B400C41672CB2D60D6E9F5ECF6F81FFD1C02FB6EB6645A0F21724D214684AF3859C534F66A66D2C860C2E270ADA798D8ADD163BCCF2A377780952483D5AC5F8CED218D453F72CDBFCEC012D372200D6D50D09A04E2A5B4E323FB8D7CB406D, NULL, NULL, N'', NULL)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[Wishlist] ON 
GO
INSERT [dbo].[Wishlist] ([wishlist_id], [user_id], [motor_id], [motor_name]) VALUES (54, 10, 2045, N'Honda SH 150i')
GO
INSERT [dbo].[Wishlist] ([wishlist_id], [user_id], [motor_id], [motor_name]) VALUES (60, 17, 2042, N'Honda Vario')
GO
INSERT [dbo].[Wishlist] ([wishlist_id], [user_id], [motor_id], [motor_name]) VALUES (61, 17, 2041, N'Honda Lead')
GO
SET IDENTITY_INSERT [dbo].[Wishlist] OFF
GO
ALTER TABLE [dbo].[BillConfirm]  WITH CHECK ADD  CONSTRAINT [FK_BillConfirm_Request] FOREIGN KEY([request_id])
REFERENCES [dbo].[Request] ([request_id])
GO
ALTER TABLE [dbo].[BillConfirm] CHECK CONSTRAINT [FK_BillConfirm_Request]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_Negotiation] FOREIGN KEY([negotiation_id])
REFERENCES [dbo].[Negotiation] ([negotiation_id])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_Negotiation]
GO
ALTER TABLE [dbo].[BuyerBooking]  WITH CHECK ADD  CONSTRAINT [FK_BuyerBooking_Request] FOREIGN KEY([request_id])
REFERENCES [dbo].[Request] ([request_id])
GO
ALTER TABLE [dbo].[BuyerBooking] CHECK CONSTRAINT [FK_BuyerBooking_Request]
GO
ALTER TABLE [dbo].[Contract]  WITH CHECK ADD  CONSTRAINT [FK_EarnALiving_Contract_Booking] FOREIGN KEY([booking_id])
REFERENCES [dbo].[Booking] ([booking_id])
GO
ALTER TABLE [dbo].[Contract] CHECK CONSTRAINT [FK_EarnALiving_Contract_Booking]
GO
ALTER TABLE [dbo].[ContractImage]  WITH CHECK ADD  CONSTRAINT [FK_EarnALiving_ContractImage_EarnALiving_Contract] FOREIGN KEY([contract_id])
REFERENCES [dbo].[Contract] ([contract_id])
GO
ALTER TABLE [dbo].[ContractImage] CHECK CONSTRAINT [FK_EarnALiving_ContractImage_EarnALiving_Contract]
GO
ALTER TABLE [dbo].[Motorbike]  WITH CHECK ADD  CONSTRAINT [FK_Motorbike_MotorbikeModel] FOREIGN KEY([model_id])
REFERENCES [dbo].[MotorbikeModel] ([model_id])
GO
ALTER TABLE [dbo].[Motorbike] CHECK CONSTRAINT [FK_Motorbike_MotorbikeModel]
GO
ALTER TABLE [dbo].[Motorbike]  WITH CHECK ADD  CONSTRAINT [FK_Motorbike_MotorbikeStatus] FOREIGN KEY([motor_status_id])
REFERENCES [dbo].[MotorbikeStatus] ([motorStatus_id])
GO
ALTER TABLE [dbo].[Motorbike] CHECK CONSTRAINT [FK_Motorbike_MotorbikeStatus]
GO
ALTER TABLE [dbo].[Motorbike]  WITH CHECK ADD  CONSTRAINT [FK_Motorbike_MotorbikeType] FOREIGN KEY([motor_type_id])
REFERENCES [dbo].[MotorbikeType] ([motorType_id])
GO
ALTER TABLE [dbo].[Motorbike] CHECK CONSTRAINT [FK_Motorbike_MotorbikeType]
GO
ALTER TABLE [dbo].[Motorbike]  WITH CHECK ADD  CONSTRAINT [FK_Motorbike_StoreDesciption] FOREIGN KEY([store_id])
REFERENCES [dbo].[StoreDesciption] ([store_id])
GO
ALTER TABLE [dbo].[Motorbike] CHECK CONSTRAINT [FK_Motorbike_StoreDesciption]
GO
ALTER TABLE [dbo].[Motorbike]  WITH CHECK ADD  CONSTRAINT [FK_Motorbike_User] FOREIGN KEY([owner_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Motorbike] CHECK CONSTRAINT [FK_Motorbike_User]
GO
ALTER TABLE [dbo].[MotorbikeImage]  WITH CHECK ADD  CONSTRAINT [FK_MotorbikeImage_Motorbike] FOREIGN KEY([motor_id])
REFERENCES [dbo].[Motorbike] ([motor_id])
GO
ALTER TABLE [dbo].[MotorbikeImage] CHECK CONSTRAINT [FK_MotorbikeImage_Motorbike]
GO
ALTER TABLE [dbo].[MotorbikeModel]  WITH CHECK ADD  CONSTRAINT [FK_MotorbikeModel_MotorbikeBrand] FOREIGN KEY([brand_id])
REFERENCES [dbo].[MotorbikeBrand] ([brand_id])
GO
ALTER TABLE [dbo].[MotorbikeModel] CHECK CONSTRAINT [FK_MotorbikeModel_MotorbikeBrand]
GO
ALTER TABLE [dbo].[Negotiation]  WITH CHECK ADD  CONSTRAINT [FK_Negotiation_Request] FOREIGN KEY([request_id])
REFERENCES [dbo].[Request] ([request_id])
GO
ALTER TABLE [dbo].[Negotiation] CHECK CONSTRAINT [FK_Negotiation_Request]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_NotificationType] FOREIGN KEY([notification_type_id])
REFERENCES [dbo].[NotificationType] ([notification_type_id])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_NotificationType]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_Request] FOREIGN KEY([request_id])
REFERENCES [dbo].[Request] ([request_id])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_Request]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_PointHistory] FOREIGN KEY([history_id])
REFERENCES [dbo].[PointHistory] ([pHistory_id])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_PointHistory]
GO
ALTER TABLE [dbo].[PointHistory]  WITH CHECK ADD  CONSTRAINT [FK_PointHistory_Request] FOREIGN KEY([store_id])
REFERENCES [dbo].[Request] ([request_id])
GO
ALTER TABLE [dbo].[PointHistory] CHECK CONSTRAINT [FK_PointHistory_Request]
GO
ALTER TABLE [dbo].[PointHistory]  WITH CHECK ADD  CONSTRAINT [FK_PointHistory_Request1] FOREIGN KEY([request_id])
REFERENCES [dbo].[Request] ([request_id])
GO
ALTER TABLE [dbo].[PointHistory] CHECK CONSTRAINT [FK_PointHistory_Request1]
GO
ALTER TABLE [dbo].[PostBoosting]  WITH CHECK ADD  CONSTRAINT [FK_PostBoosting_PointHistory] FOREIGN KEY([history_id])
REFERENCES [dbo].[PointHistory] ([pHistory_id])
GO
ALTER TABLE [dbo].[PostBoosting] CHECK CONSTRAINT [FK_PostBoosting_PointHistory]
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD  CONSTRAINT [FK_Request_Motorbike] FOREIGN KEY([motor_id])
REFERENCES [dbo].[Motorbike] ([motor_id])
GO
ALTER TABLE [dbo].[Request] CHECK CONSTRAINT [FK_Request_Motorbike]
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD  CONSTRAINT [FK_Request_RequestType] FOREIGN KEY([request_type_id])
REFERENCES [dbo].[RequestType] ([request_type_id])
GO
ALTER TABLE [dbo].[Request] CHECK CONSTRAINT [FK_Request_RequestType]
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD  CONSTRAINT [FK_Request_User] FOREIGN KEY([receiver_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Request] CHECK CONSTRAINT [FK_Request_User]
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD  CONSTRAINT [FK_Request_User1] FOREIGN KEY([sender_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Request] CHECK CONSTRAINT [FK_Request_User1]
GO
ALTER TABLE [dbo].[StoreDesciption]  WITH CHECK ADD  CONSTRAINT [FK_StoreDesciption_LocalAddress] FOREIGN KEY([ward_id])
REFERENCES [dbo].[Ward] ([ward_id])
GO
ALTER TABLE [dbo].[StoreDesciption] CHECK CONSTRAINT [FK_StoreDesciption_LocalAddress]
GO
ALTER TABLE [dbo].[StoreDesciption]  WITH CHECK ADD  CONSTRAINT [FK_StoreDesciption_User1] FOREIGN KEY([user_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[StoreDesciption] CHECK CONSTRAINT [FK_StoreDesciption_User1]
GO
ALTER TABLE [dbo].[StoreImage]  WITH CHECK ADD  CONSTRAINT [FK_StoreImage_StoreDesciption] FOREIGN KEY([store_id])
REFERENCES [dbo].[StoreDesciption] ([store_id])
GO
ALTER TABLE [dbo].[StoreImage] CHECK CONSTRAINT [FK_StoreImage_StoreDesciption]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_LocalAddress] FOREIGN KEY([ward_id])
REFERENCES [dbo].[Ward] ([ward_id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_LocalAddress]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([role_id])
REFERENCES [dbo].[Role] ([role_id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
ALTER TABLE [dbo].[Wishlist]  WITH CHECK ADD  CONSTRAINT [FK_Motorbike] FOREIGN KEY([motor_id])
REFERENCES [dbo].[Motorbike] ([motor_id])
GO
ALTER TABLE [dbo].[Wishlist] CHECK CONSTRAINT [FK_Motorbike]
GO
ALTER TABLE [dbo].[Wishlist]  WITH CHECK ADD  CONSTRAINT [FK_User] FOREIGN KEY([user_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Wishlist] CHECK CONSTRAINT [FK_User]
GO
ALTER DATABASE [motorbikebs-db] SET  READ_WRITE 
GO
