USE [master]
GO
CREATE DATABASE [MotorbikeDB]
GO
ALTER DATABASE [MotorbikeDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MotorbikeDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MotorbikeDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MotorbikeDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MotorbikeDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [MotorbikeDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MotorbikeDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MotorbikeDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MotorbikeDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MotorbikeDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MotorbikeDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MotorbikeDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MotorbikeDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MotorbikeDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MotorbikeDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [MotorbikeDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MotorbikeDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MotorbikeDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MotorbikeDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MotorbikeDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MotorbikeDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MotorbikeDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MotorbikeDB] SET RECOVERY FULL 
GO
ALTER DATABASE [MotorbikeDB] SET  MULTI_USER 
GO
ALTER DATABASE [MotorbikeDB] SET PAGE_VERIFY CHECKSUM  
GO
USE [MotorbikeDB]
GO
/*** The scripts of database scoped configurations in Azure should be executed inside the target database connection. ***/
GO
-- ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 8;
GO
/****** Object:  Table [dbo].[BillConfirm]    Script Date: 11/10/2023 9:44:26 PM ******/
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
/****** Object:  Table [dbo].[BuyerBooking]    Script Date: 11/10/2023 9:44:26 PM ******/
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
/****** Object:  Table [dbo].[Comment]    Script Date: 11/10/2023 9:44:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[comment_id] [int] IDENTITY(1,1) NOT NULL,
	[request_id] [int] NOT NULL,
	[content] [nvarchar](200) NULL,
	[rating] [int] NULL,
	[create_at] [datetime] NULL,
	[update_at] [datetime] NULL,
	[status] [nvarchar](10) NULL,
	[reply_id] [int] NULL,
 CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED 
(
	[comment_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Motorbike]    Script Date: 11/10/2023 9:44:26 PM ******/
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
	[price] [decimal](15, 4) NULL,
	[description] [nvarchar](max) NULL,
	[motor_status_id] [int] NULL,
	[motor_type_id] [int] NULL,
	[store_id] [int] NULL,
	[owner_id] [int] NOT NULL,
	[registration_image] [nvarchar](200) NULL,
 CONSTRAINT [PK_Motorbike] PRIMARY KEY CLUSTERED 
(
	[motor_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotorbikeBrand]    Script Date: 11/10/2023 9:44:26 PM ******/
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
/****** Object:  Table [dbo].[MotorbikeImage]    Script Date: 11/10/2023 9:44:26 PM ******/
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
/****** Object:  Table [dbo].[MotorbikeModel]    Script Date: 11/10/2023 9:44:26 PM ******/
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
/****** Object:  Table [dbo].[MotorbikeStatus]    Script Date: 11/10/2023 9:44:26 PM ******/
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
/****** Object:  Table [dbo].[MotorbikeType]    Script Date: 11/10/2023 9:44:26 PM ******/
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
/****** Object:  Table [dbo].[Negotiation]    Script Date: 11/10/2023 9:44:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Negotiation](
	[negotation_id] [int] IDENTITY(1,1) NOT NULL,
	[motor_id] [int] NULL,
	[final_price] [money] NULL,
	[store_id] [int] NULL,
	[content] [nvarchar](1000) NULL,
	[created_at] [datetime] NULL,
	[status] [nvarchar](10) NULL,
	[valuation_id] [int] NULL,
	[base_request_id] [int] NULL,
	[start_time] [datetime] NULL,
	[end_time] [datetime] NULL,
	[deposit] [money] NULL,
 CONSTRAINT [PK_EarnALiving_Contract] PRIMARY KEY CLUSTERED 
(
	[negotation_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 11/10/2023 9:44:26 PM ******/
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
/****** Object:  Table [dbo].[NotificationType]    Script Date: 11/10/2023 9:44:26 PM ******/
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
/****** Object:  Table [dbo].[Payment]    Script Date: 11/10/2023 9:44:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[payment_id] [int] IDENTITY(1,1) NOT NULL,
	[request_id] [int] NOT NULL,
	[content] [nvarchar](200) NULL,
	[date_created] [datetime] NULL,
	[payment_time] [datetime] NULL,
	[vnpay_order_id] [int] NULL,
	[payment_type] [nvarchar](100) NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[payment_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PointHistory]    Script Date: 11/10/2023 9:44:26 PM ******/
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
/****** Object:  Table [dbo].[PostBoosting]    Script Date: 11/10/2023 9:44:26 PM ******/
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
/****** Object:  Table [dbo].[Request]    Script Date: 11/10/2023 9:44:26 PM ******/
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
/****** Object:  Table [dbo].[RequestType]    Script Date: 11/10/2023 9:44:26 PM ******/
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
/****** Object:  Table [dbo].[Role]    Script Date: 11/10/2023 9:44:26 PM ******/
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
/****** Object:  Table [dbo].[StoreDesciption]    Script Date: 11/10/2023 9:44:26 PM ******/
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
/****** Object:  Table [dbo].[StoreImage]    Script Date: 11/10/2023 9:44:26 PM ******/
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
/****** Object:  Table [dbo].[User]    Script Date: 11/10/2023 9:44:26 PM ******/
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
/****** Object:  Table [dbo].[Valuation]    Script Date: 11/10/2023 9:44:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Valuation](
	[valuation_id] [int] IDENTITY(1,1) NOT NULL,
	[request_id] [int] NOT NULL,
	[store_price] [money] NULL,
	[description] [nvarchar](200) NULL,
	[status] [nvarchar](50) NULL,
 CONSTRAINT [PK_Negotiation] PRIMARY KEY CLUSTERED 
(
	[valuation_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wishlist]    Script Date: 11/10/2023 9:44:26 PM ******/
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
INSERT [dbo].[BillConfirm] ([bill_confirm_id], [motor_id], [user_id], [store_id], [price], [create_at], [status], [request_id]) VALUES (5, 2060, 18, 6, 49800000.0000, CAST(N'2023-11-08T06:30:58.083' AS DateTime), N'ACCEPT', 326)
GO
INSERT [dbo].[BillConfirm] ([bill_confirm_id], [motor_id], [user_id], [store_id], [price], [create_at], [status], [request_id]) VALUES (6, 2056, 2, 5, 80000000.0000, CAST(N'2023-11-09T04:16:28.327' AS DateTime), N'ACCEPT', 341)
GO
INSERT [dbo].[BillConfirm] ([bill_confirm_id], [motor_id], [user_id], [store_id], [price], [create_at], [status], [request_id]) VALUES (7, 2061, 2, 5, 33500000.0000, CAST(N'2023-11-09T16:04:54.117' AS DateTime), N'ACCEPT', 368)
GO
SET IDENTITY_INSERT [dbo].[BillConfirm] OFF
GO
SET IDENTITY_INSERT [dbo].[BuyerBooking] ON 
GO
INSERT [dbo].[BuyerBooking] ([booking_id], [request_id], [date_create], [booking_date], [note], [status]) VALUES (14, 337, CAST(N'2023-11-09T04:09:03.133' AS DateTime), CAST(N'2023-11-16T11:08:00.000' AS DateTime), N'Sẽ tới', N'REJECT')
GO
SET IDENTITY_INSERT [dbo].[BuyerBooking] OFF
GO
SET IDENTITY_INSERT [dbo].[Motorbike] ON 
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2052, N'123511', N'Exciter 135', 15, 123000, CAST(N'2019-05-10' AS Date), CAST(21500000.0000 AS Decimal(15, 4)), NULL, 1, 3, 5, 2, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/d0ad8eab-8057-4944-9fb9-ed86f649dc41.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2053, N'991031', N'GSX S150', 8, 224000, CAST(N'2022-07-07' AS Date), CAST(55300000.0000 AS Decimal(15, 4)), NULL, 1, 4, 5, 2, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/574c4f56-9bee-4311-9060-b70839552008.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2054, N'687654', N'Lead 2019', 6, 76400, CAST(N'2020-10-21' AS Date), CAST(27700000.0000 AS Decimal(15, 4)), NULL, 1, 1, 6, 18, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/f3052484-55b3-4adb-9530-c072877883a7.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2055, N'012152', N'Nouvo 2020', 1, 721003, CAST(N'2018-01-09' AS Date), CAST(18700000.0000 AS Decimal(15, 4)), NULL, 1, 1, 6, 18, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/f355f4fa-951c-46f1-ae41-45d089507587.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2056, N'589310', N'SH 150i', 16, 6000, CAST(N'2023-03-20' AS Date), CAST(80000000.0000 AS Decimal(15, 4)), NULL, 3, 1, NULL, 1, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/46afb579-ab51-4d13-a0a5-c29e4fbe5c0b.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2057, N'029182', N'Sirius', 11, 765000, CAST(N'2020-06-10' AS Date), CAST(21300000.0000 AS Decimal(15, 4)), NULL, 5, 2, NULL, 11, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/52f98f00-ff98-4a77-a758-3f1c2c269f15.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2058, N'886431', N'Vario 160', 10, 85200, CAST(N'2022-09-09' AS Date), CAST(49900000.0000 AS Decimal(15, 4)), NULL, 4, 1, NULL, 15, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/602c1b71-d870-44dc-9b3e-094971b1b429.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2059, N'692024', N'Vespa Sprint 150', 12, 750005, CAST(N'2023-04-09' AS Date), CAST(75000000.0000 AS Decimal(15, 4)), N'Xe còn rất mới, đẹp.', 3, 1, NULL, 15, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/d93d2455-054c-4d7e-9969-a57fd006eb35.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2060, N'623872', N'Satria 2023', 7, 72800, CAST(N'2021-02-08' AS Date), CAST(49800000.0000 AS Decimal(15, 4)), N'Xe dùng để đi phượt.', 3, 3, NULL, 1, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/76cbce7d-1eab-4db9-8a4e-bc9e40f991d9.png')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2061, N'676541', N'Winner X', 3, 521233, CAST(N'2021-04-05' AS Date), CAST(33500000.0000 AS Decimal(15, 4)), NULL, 3, 3, NULL, 1, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/0ad8ccdf-e94f-435c-9654-0161b429a9b3.png')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2062, N'283869', N'Lead 2017', 6, 599900, CAST(N'2018-10-10' AS Date), CAST(22500000.0000 AS Decimal(15, 4)), N'Xe thay nhớt đều đặn.
Còn nguyên zin.', 4, 1, NULL, 11, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/1fae3f24-bd1e-4d1d-a141-8191361fc501.jpg')
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
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (176, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/0332e01e-2a14-4f15-a651-3436c8236092.jpg', 2052)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (177, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/41359bb6-0113-4678-9b9f-9d68613872f2.jpg', 2052)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (178, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/26281fad-e6d2-4e0f-8775-4de2343edebd.jpg', 2052)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (179, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/e5356397-5ade-44d6-bb78-8dbaa97a4b6d.jpg', 2053)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (180, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/caf8290d-f7dd-43a2-ba92-05a4c99760c9.jpg', 2053)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (181, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/f63489b1-d578-45f7-a1a0-f91a40f3152b.jpg', 2054)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (182, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/4d048869-0014-4950-9a36-c4dc655ce108.jpg', 2054)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (183, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/88c44347-5ec0-4f49-a4c1-52cc87161b93.jpg', 2055)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (184, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/d8d2505e-781b-4472-b9f2-5dc7c9b34b75.jpg', 2055)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (185, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/ea105bbb-9c50-407d-8e7e-cd752b6c7f7c.jpg', 2056)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (186, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/a3a38143-128a-44a8-8acc-a629494cc3b0.jpg', 2056)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (187, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/a133a418-dd68-4894-991f-f127cddd852e.jpg', 2056)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (188, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/18b98b78-d614-47f0-9e16-44a4488ca587.jpg', 2057)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (189, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/b61faf0f-1bdd-4284-b3e6-42587345be53.jpg', 2057)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (190, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/fb0ab197-6764-41e7-a26f-1d48dc1a53ba.jpg', 2058)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (191, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/5e1cfad1-80c7-49c3-a0a5-83367e770bbe.png', 2058)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (192, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/a5724ba9-d128-44ae-9cbb-b6f5ef72567b.png', 2058)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (193, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/00b27be6-04d5-48f0-9243-780849cb3428.jpg', 2059)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (194, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/0b423575-5aed-4c6d-b57b-8aac00422041.jpg', 2059)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (195, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/9df9b584-68d0-461c-aef1-d9ea4fee49ec.jpg', 2060)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (196, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/e06aac2b-68cf-4d54-b18f-b28c3864a6d8.jpg', 2060)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (200, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/d2e75bc6-43bb-432e-9b18-4b8238e7ed07.jpg', 2061)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (201, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/5d8519b0-0d6e-4728-adc7-7baf855f6d5e.jpg', 2061)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (202, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/c7cc3f46-9705-4a08-804b-5dcc507f74e1.jpg', 2061)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (203, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/2367730e-4049-4f1b-8138-dbe92593b5ca.jpg', 2062)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (204, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/260e4117-359d-499c-8691-d638d9e1ba2d.jpg', 2062)
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
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status], [brand_id]) VALUES (11, N'Sirius', N'', N'ACTIVE', 2)
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
SET IDENTITY_INSERT [dbo].[Request] ON 
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (283, 2052, 1, 2, CAST(N'2023-11-06T07:12:04.497' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (284, 2052, 1, 2, CAST(N'2023-11-06T07:13:04.133' AS DateTime), 2, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (285, 2053, 1, 2, CAST(N'2023-11-06T07:14:28.583' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (286, 2053, 1, 2, CAST(N'2023-11-06T07:14:40.143' AS DateTime), 2, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (287, 2054, 1, 18, CAST(N'2023-11-06T07:15:53.723' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (288, 2055, 1, 18, CAST(N'2023-11-06T07:16:45.477' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (289, 2054, 1, 18, CAST(N'2023-11-06T07:17:06.440' AS DateTime), 2, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (290, 2055, 1, 18, CAST(N'2023-11-06T07:17:14.593' AS DateTime), 2, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (291, 2056, 1, 11, CAST(N'2023-11-06T07:19:15.390' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (292, 2056, 1, 11, CAST(N'2023-11-06T07:19:31.990' AS DateTime), 3, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (293, 2057, 1, 11, CAST(N'2023-11-06T07:20:33.277' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (294, 2057, 1, 11, CAST(N'2023-11-06T07:20:45.200' AS DateTime), 4, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (295, 2058, 1, 15, CAST(N'2023-11-06T07:21:57.363' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (296, 2058, 1, 15, CAST(N'2023-11-06T07:22:05.290' AS DateTime), 3, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (297, 2059, 1, 15, CAST(N'2023-11-06T07:23:21.343' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (298, 2059, 1, 15, CAST(N'2023-11-06T07:23:28.943' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (301, 2060, 1, 18, CAST(N'2023-11-07T08:55:32.070' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (302, 2060, 1, 18, CAST(N'2023-11-07T08:55:51.810' AS DateTime), 2, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (314, 2060, 1, 18, CAST(N'2023-11-08T02:45:54.827' AS DateTime), 2, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (315, 2054, 1, 18, CAST(N'2023-11-08T02:46:22.397' AS DateTime), NULL, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (316, 2054, 1, 18, CAST(N'2023-11-08T02:46:37.287' AS DateTime), 2, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (326, 2060, 1, 18, CAST(N'2023-11-08T06:30:57.357' AS DateTime), 9, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (335, 2056, 1, 2, CAST(N'2023-11-09T04:02:51.793' AS DateTime), 3, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (336, 2056, 1, 2, CAST(N'2023-11-09T04:08:11.660' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (337, 2056, 2, 17, CAST(N'2023-11-09T04:09:03.117' AS DateTime), 6, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (338, 2056, 1, 2, CAST(N'2023-11-09T04:09:48.283' AS DateTime), 3, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (339, 2056, 1, 2, CAST(N'2023-11-09T04:10:08.333' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (340, 2056, 1, 2, CAST(N'2023-11-09T04:10:24.103' AS DateTime), 3, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (341, 2056, 1, 2, CAST(N'2023-11-09T04:16:28.273' AS DateTime), 9, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (342, 2061, 1, 11, CAST(N'2023-11-09T04:21:27.217' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (343, 2061, 1, 11, CAST(N'2023-11-09T04:21:36.927' AS DateTime), 3, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (345, 2061, 1, 2, CAST(N'2023-11-09T07:21:16.800' AS DateTime), 3, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (346, 2061, 1, 2, CAST(N'2023-11-09T09:41:56.270' AS DateTime), NULL, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (347, 2061, 1, 2, CAST(N'2023-11-09T09:42:22.697' AS DateTime), 3, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (348, 2059, 1, 15, CAST(N'2023-11-09T09:54:42.413' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (349, 2059, 1, 15, CAST(N'2023-11-09T10:04:02.463' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (350, 2059, 1, 15, CAST(N'2023-11-09T10:08:37.453' AS DateTime), NULL, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (351, 2059, 1, 15, CAST(N'2023-11-09T10:10:00.720' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (352, 2059, 1, 15, CAST(N'2023-11-09T10:17:23.623' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (365, 2061, 1, 2, CAST(N'2023-11-09T15:57:51.147' AS DateTime), 3, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (366, 2061, 1, 2, CAST(N'2023-11-09T15:58:59.347' AS DateTime), NULL, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (367, 2061, 1, 2, CAST(N'2023-11-09T16:00:22.797' AS DateTime), 3, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (368, 2061, 1, 2, CAST(N'2023-11-09T16:04:53.530' AS DateTime), 9, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (374, 2062, 1, 11, CAST(N'2023-11-09T16:22:51.680' AS DateTime), 1, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (375, 2062, 1, 11, CAST(N'2023-11-09T16:23:00.030' AS DateTime), NULL, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (376, 2062, 1, 11, CAST(N'2023-11-09T16:23:05.893' AS DateTime), 3, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (386, 2059, 1, 15, CAST(N'2023-11-09T17:12:57.777' AS DateTime), 4, N'CANCEL')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (387, 2059, 1, 15, CAST(N'2023-11-09T17:13:38.647' AS DateTime), NULL, N'ACCEPT')
GO
INSERT [dbo].[Request] ([request_id], [motor_id], [receiver_id], [sender_id], [time], [request_type_id], [status]) VALUES (396, 2062, 11, 2, CAST(N'2023-11-10T10:51:07.143' AS DateTime), 8, N'PENDING')
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
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (10, N'Add-Point', N'Nạp điểm')
GO
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (11, N'Post-Boosting', N'Đẩy bài')
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
INSERT [dbo].[StoreDesciption] ([store_id], [user_id], [store_name], [description], [store_phone], [store_email], [store_created_at], [store_updated_at], [point], [address], [ward_id], [status], [tax_code], [business_license]) VALUES (5, 2, N'Xe Máy Cũ Thái Hòa', NULL, N'0369269410', N'nhutminh.it19@gmail.com', CAST(N'2023-10-10T08:21:07.567' AS DateTime), NULL, 4000040, N'288/3 Man Thiên, phường Tăng Nhơn Phú A, Quận 9, Tp. Hồ Chí Minh.', NULL, N'ACTIVE', N'4536789078456', N'https://motorbikeimages.blob.core.windows.net/motorbikebs/9142e438-368f-4d8b-8e3e-d64772e27e91.png')
GO
INSERT [dbo].[StoreDesciption] ([store_id], [user_id], [store_name], [description], [store_phone], [store_email], [store_created_at], [store_updated_at], [point], [address], [ward_id], [status], [tax_code], [business_license]) VALUES (6, 18, N'Xe Máy Cũ Ðức Dũng II', NULL, N'0937500668', N'phatntse150133@fpt.edu.vn', CAST(N'2023-10-10T08:22:11.503' AS DateTime), NULL, 2000000, N'TL15- Ấp Phú An, Xã Phú Hòa Đông, Huyện Củ Chi, TP.HCM', NULL, N'ACTIVE', N'5566772234512', N'https://motorbikeimages.blob.core.windows.net/motorbikebs/72fe9c06-763e-430d-b710-951836da31f0.jpg')
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
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (2, N'Nguyễn Nhựt Minh', N'nhutminh.it19@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 2, CAST(N'2023-10-05' AS Date), NULL, N'ACTIVE', 0x3CCB908A2AC73C8675ECAB0919DF7D5F5EB428277FC17BF94CB21CC1EAFF1668337A17B897EF67C7157BF21932A1897EEA1EC78B3018CDE3A615089077B1A1BD, 0xEAE5E3F7B92F0FB267ABBD716D23866FB9E3065A3FD68CE995D35D9EC976399D0BF106893A41296E4DE8F3E017B6B924C65826F25A7A3EBBA0C544147011CF302D1176CD8DCBFEED9D871D9010B4B54FF52D0DAB105C00445FEC31936A9653A4B5EDE7442B290297AAAD4108E5FD264E62AFAC96B0212A11BAD15ECA28033F15, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (10, N'Phan Minh Trí', N'phanminhtri269@gmail.com', N'0908660977', 3, CAST(N'2001-09-26' AS Date), N'987867564612', N'33 Bến Vân Đồng, Phường 3,Quận 4, Hồ Chí Minh', NULL, 4, CAST(N'2023-10-05' AS Date), CAST(N'2023-10-31' AS Date), N'ACTIVE', 0xA585D4D64DFDB23C80658BA185A70CF6AAE8A53E78437CD394219CAC568837ED754795D418158B795B48B092164CCEDB15CF4709706E6D476E36590530E833E1, 0x6F607BE7009771AE4E9906491FA12B5DB502D738E8207702325DEB52262E85767BCEA9BCEEB6C6826FE88B6A6E9F7C2260A8C7FDB0768DE153729018B9DC5AD55F5B1D8AD1BE36C85F64ED6103CFAF863FA77E2CDD9BB834124B1A247DDAD18E0272683C206CDC51A3A6673D785F846BE98598E9675C8EC2E90FB474799FD07D, N'485B50FED6BA6F0BA8B19B7BA221CC216A3C84F40843F9603193903C2194DAAC', CAST(N'2023-10-21T21:38:20.527' AS DateTime), N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (11, N'Minh Tri FPT', N'tripmse150151@fpt.edu.vn', N'0937500668', NULL, NULL, N'436785096712', N'225 Đường Âu Dương Lân, Phường 3, Quận 8, TP.HCM', NULL, 3, CAST(N'2023-10-05' AS Date), NULL, N'ACTIVE', 0xB376591F2980D24DCE3229337D2799269090CE67CFEA18C27C713408272592456165A86C6279EBE31A5A3D78FBDEA5158DE197D05B6D65B2B056AE70F52AE51D, 0x3A3A7E5C89F4C42B1B7F62C9D9576CFB9EA08F1AF75D94CA01A22940ACFB81FDD3EBD5D8BFDAC0CA33E21F130ACCDF25AB73FD44FC3D91029E8651DA91665D65B2A531395AD927A78657B5E0BD400670ED54C49FCFDC421EE0A2DE5167255192B49D8D409673054FA577EC55E034FE238EDFA3DF696B0C4319273FB6838848E2, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (14, N'NguyenNhutMinh', N'minhnnse150140@fpt.com.vn', NULL, NULL, NULL, NULL, NULL, NULL, 4, NULL, NULL, N'NOT VERIFY', 0xF59114392EFE348BB1A60179DECED723911BAAD1314109098E4AF43EF7896F976CCADFA7E7DC8E0DAAEE408F8A042DB7F77BA268D122B11D0B1C398119B4C9FF, 0xC0DBB7CDD0670FF880ABBA4F2DFBC5D158AC2E4F2C61A04FF74E68CEC69FB48DA21B90B6948DAFCBD7B15D423F7CDA6C325574D9C90E1EFA3D12A8225584C649850ADD68949269352691C2A8DC5360CD3878CDD4E36D19E0BF8DCDC9494DA967B39C712C50D70BA2307DC445A8C99439A0878FFC68950715D8EBC2605102450C, NULL, NULL, N'3B4127B0529EC8E9969101CBF48D0ABC80353C2E85149125A8EC9BB60AC2020F', CAST(N'2023-10-05T21:18:47.057' AS DateTime))
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
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (22, N'Lê Anh Mỹ', N'lemy222@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 4, NULL, NULL, N'NOT VERIFY', 0xC758A51D54CF34BE58172B153103C1CF6722A1680C02EBCC6579E0CBFD253B1900E2C8C2D03A7040A69A0FE9033C7C1177D23C8AE085F6820E91562DC2E6F7FC, 0xACC66D3CF3ACAAFC8005FC9D1D85AE01CE0BAB3060AD58E1469521BE7A7888D2E97551AF40B99B5B651C2C434C4560B41197B86845BD1FEA33ECCF593896A7A0DB9C2E38ECB4B227D37FE03345FD8BDF795C91AF0F8E7D1F791677F18C7C227BA26D4389353C5C20AB4347BE8590C069190DAD72DFAFE6CF034002AEDFFF4C0A, NULL, NULL, N'E611CE9F352E0363C7BFDB0EF6CF2FCB058E5DCABAC4FA8A36152DEE35A0CFE2', CAST(N'2023-11-02T19:58:12.670' AS DateTime))
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (23, N'Lê Mỹ', N'lemy223@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 4, NULL, NULL, N'NOT VERIFY', 0xB7D1490B349A123A641E6768F492C6F4D633097AB3821C6F776C7B89C49C489242D067AE49EBA549680FF9DD43FE602769AC2FB5E86C426871DA0CCE354BAA9D, 0xB7B614CD2D1C4C3562C9633248011C0B085CF7A26DEDD90BD49175089AE27BEE48BC4E8333B01486381CD3FFFEDD0218A641EE06AB36EE53FA0E4724636A4E0C0FFF81CF505C1D305B6709863B8DBCB62ABE18D6EF887B2856F2120934AD0829F7810D48BE9EFCB2967F9EC8E2CCD5EA129FB77D9BD4178BA8BF0C041C5D76BF, NULL, NULL, N'0CC0AA4920999214D7D24B9A51569E5F9C8D91AFAFEC3B7DAABDB51BCB851196', CAST(N'2023-11-02T19:59:50.947' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[Valuation] ON 
GO
INSERT [dbo].[Valuation] ([valuation_id], [request_id], [store_price], [description], [status]) VALUES (32, 396, 19000000.0000, N'Tôi muốn thương lượng với bạn.', N'PENDING')
GO
SET IDENTITY_INSERT [dbo].[Valuation] OFF
GO
SET IDENTITY_INSERT [dbo].[Wishlist] ON 
GO
INSERT [dbo].[Wishlist] ([wishlist_id], [user_id], [motor_id], [motor_name]) VALUES (77, 17, 2053, N'Suzuki GSX')
GO
INSERT [dbo].[Wishlist] ([wishlist_id], [user_id], [motor_id], [motor_name]) VALUES (78, 17, 2056, N'Honda SH 150i')
GO
SET IDENTITY_INSERT [dbo].[Wishlist] OFF
GO
ALTER TABLE [dbo].[BillConfirm]  WITH CHECK ADD  CONSTRAINT [FK_BillConfirm_Request] FOREIGN KEY([request_id])
REFERENCES [dbo].[Request] ([request_id])
GO
ALTER TABLE [dbo].[BillConfirm] CHECK CONSTRAINT [FK_BillConfirm_Request]
GO
ALTER TABLE [dbo].[BuyerBooking]  WITH CHECK ADD  CONSTRAINT [FK_BuyerBooking_Request] FOREIGN KEY([request_id])
REFERENCES [dbo].[Request] ([request_id])
GO
ALTER TABLE [dbo].[BuyerBooking] CHECK CONSTRAINT [FK_BuyerBooking_Request]
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_Comment] FOREIGN KEY([reply_id])
REFERENCES [dbo].[Comment] ([comment_id])
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_Comment]
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_Request] FOREIGN KEY([request_id])
REFERENCES [dbo].[Request] ([request_id])
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_Request]
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
ALTER TABLE [dbo].[Negotiation]  WITH CHECK ADD  CONSTRAINT [FK_Negotiation_Valuation] FOREIGN KEY([valuation_id])
REFERENCES [dbo].[Valuation] ([valuation_id])
GO
ALTER TABLE [dbo].[Negotiation] CHECK CONSTRAINT [FK_Negotiation_Valuation]
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
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_Request] FOREIGN KEY([request_id])
REFERENCES [dbo].[Request] ([request_id])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_Request]
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
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([role_id])
REFERENCES [dbo].[Role] ([role_id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
ALTER TABLE [dbo].[Valuation]  WITH CHECK ADD  CONSTRAINT [FK_Negotiation_Request] FOREIGN KEY([request_id])
REFERENCES [dbo].[Request] ([request_id])
GO
ALTER TABLE [dbo].[Valuation] CHECK CONSTRAINT [FK_Negotiation_Request]
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
