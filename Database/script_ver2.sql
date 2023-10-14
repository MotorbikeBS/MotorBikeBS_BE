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
/****** Object:  Table [dbo].[BillConfirm]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Booking]    Script Date: 10/15/2023 12:06:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking](
	[booking_id] [int] IDENTITY(1,1) NOT NULL,
	[negotiation_id] [int] NULL,
	[request_id] [int] NOT NULL,
	[date_create] [datetime] NULL,
	[booking_date] [datetime] NULL,
	[note] [nvarchar](100) NULL,
	[status] [nvarchar](10) NULL,
 CONSTRAINT [PK_Booking] PRIMARY KEY CLUSTERED 
(
	[booking_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contract]    Script Date: 10/15/2023 12:06:53 AM ******/
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
 CONSTRAINT [PK_EarnALiving_Contract] PRIMARY KEY CLUSTERED 
(
	[contract_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContractImage]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Motorbike]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotorbikeBrand]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotorbikeImage]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotorbikeModel]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotorbikeStatus]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotorbikeType]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Negotiation]    Script Date: 10/15/2023 12:06:53 AM ******/
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
 CONSTRAINT [PK_Negotiation] PRIMARY KEY CLUSTERED 
(
	[negotiation_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationType]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PointHistory]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PostBoosting]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Request]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestType]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StoreDesciption]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StoreImage]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ward]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wishlist]    Script Date: 10/15/2023 12:06:53 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Motorbike] ON 
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2009, N'123123', N'Nouvo 2020', 1, 164000, CAST(N'2020-09-28' AS Date), 14800000.0000, NULL, 4, 1, NULL, 15, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/5c8da6a2-6dff-48cb-b530-ea9bde73e5a8.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2010, N'222211', N'Vision 2022', 5, 11000, CAST(N'2022-10-13' AS Date), 23000000.0000, NULL, 4, 1, NULL, 15, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/c3552657-f882-4b59-a288-0af89e6688ae.png')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2011, N'241292', N'Wave Alpha', 4, 75000, CAST(N'2022-07-01' AS Date), 13400000.0000, NULL, 3, 2, NULL, 15, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/2f1a1196-6f52-407b-a3e5-e39d48786546.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2012, N'764422', N'Winner X', 3, 84300, CAST(N'2021-02-19' AS Date), 29500000.0000, NULL, 5, 3, NULL, 15, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/223a10e7-b429-4f62-9adf-fd287509a58c.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2013, N'920412', N'Lead 2019', 6, 67400, CAST(N'2019-09-20' AS Date), 15500000.0000, NULL, 3, 1, NULL, 15, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/6e4c46a4-d70e-430d-9d94-0b488684bffb.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2015, N'215876', N'Satria 2021', 7, 50200, CAST(N'2021-05-10' AS Date), 42500000.0000, NULL, 1, 3, 5, 2, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/bb3f30d3-4b47-41eb-981a-0fbebd09360c.jpg')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2016, N'998909', N'Satria 2023', 7, 11000, CAST(N'2023-02-01' AS Date), 45000000.0000, NULL, 3, 3, NULL, 2, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/e6a32afd-8f55-4f68-830c-737d8e2c4e13.png')
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id], [registration_image]) VALUES (2017, N'536631', N' Vision 2019', 5, 921000, CAST(N'2020-02-10' AS Date), 16700000.0000, NULL, 1, 1, 5, 2, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/d654de8d-b5af-423d-b67f-8aedb69a61b0.png')
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
INSERT [dbo].[MotorbikeBrand] ([brand_id], [brand_name], [description], [status]) VALUES (6, N'Kawasaki', NULL, N'ACTIVE')
GO
INSERT [dbo].[MotorbikeBrand] ([brand_id], [brand_name], [description], [status]) VALUES (7, N'Ducati', NULL, N'ACTIVE')
GO
SET IDENTITY_INSERT [dbo].[MotorbikeBrand] OFF
GO
SET IDENTITY_INSERT [dbo].[MotorbikeImage] ON 
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (6, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/0b419cfb-a274-4cb1-b423-737386d73128.png', 2010)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (7, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/ece974b2-7a1b-4652-852f-594d5c694d9d.jpg', 2010)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (8, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/5b3b7f1d-b8d3-45c9-a4e5-371f0ef0b318.jpg', 2011)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (9, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/2d569e72-09a8-40e6-b848-b60110f119d5.png', 2011)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (10, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/c47b48d4-68ec-4e41-916c-e7550009e8af.jpg', 2012)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (11, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/58668e68-9fac-4752-82cd-43b9e4cb6e5c.jpg', 2012)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (12, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/d15e1c3a-70b4-494d-b412-92c8176e8dbe.jpg', 2012)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (13, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/552e9a8c-03fe-4908-be28-43f0bf726a5d.jpg', 2013)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (14, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/eb699160-a8c5-44ee-a0bd-8bdf4b2fe1f7.jpg', 2013)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (65, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/c0ae5d6a-dc24-47f2-b983-6a9ff4f21bad.jpg', 2009)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (66, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/6844facf-fb47-4af3-880a-44aa2905d5dd.jpg', 2009)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (71, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/3fe91318-0d54-4fbe-b942-91335d616507.jpg', 2016)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (72, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/3210c905-9915-470f-97c2-bb5a35cca9d6.jpg', 2016)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (75, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/8d959c5a-a8b4-4e29-b3d9-e3396d4f610c.jpg', 2015)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (76, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/ddc15ed0-ffbe-4a4d-b09b-36b6b6e9c935.jpg', 2015)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (79, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/8b847488-1b57-4eea-ab4d-e30f96c7d23d.png', 2017)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (80, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/d6739dc4-fb27-429b-8a62-bcc04f6417b8.jpg', 2017)
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
INSERT [dbo].[MotorbikeType] ([motorType_id], [title], [description], [status]) VALUES (2, N'Xe số', NULL, NULL)
GO
INSERT [dbo].[MotorbikeType] ([motorType_id], [title], [description], [status]) VALUES (3, N'Xe côn tay', NULL, NULL)
GO
INSERT [dbo].[MotorbikeType] ([motorType_id], [title], [description], [status]) VALUES (4, N'Xe phân khối lớn', NULL, NULL)
GO
INSERT [dbo].[MotorbikeType] ([motorType_id], [title], [description], [status]) VALUES (5, N'', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[MotorbikeType] OFF
GO
SET IDENTITY_INSERT [dbo].[RequestType] ON 
GO
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (1, N'Motor-Register', N'Đăng ký thông tin xe vào kho')
GO
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (2, N'Motor-Posting', N'Đăng xe lên sàn')
GO
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (3, N'Motor-Update', N'Cập nhật thông tin xe')
GO
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (4, N'Sale-Request', N'Yêu cầu bán xe ')
GO
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (5, N'Purchase-Request', N'Yêu cầu mua xe')
GO
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (6, N'Booking-Request', N'Yêu cầu đặt lịch hẹn')
GO
INSERT [dbo].[RequestType] ([request_type_id], [title], [description]) VALUES (7, N'MotorModel-Register', NULL)
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
INSERT [dbo].[StoreDesciption] ([store_id], [user_id], [store_name], [description], [store_phone], [store_email], [store_created_at], [store_updated_at], [point], [address], [ward_id], [status], [tax_code], [business_license]) VALUES (5, 2, N'Cửa Hàng Xe Máy Cũ Thái Hòa', NULL, N'0369269410', N'nhutminh.it19@gmail.com', CAST(N'2023-10-10T08:21:07.567' AS DateTime), NULL, NULL, N'288/3 Man Thiên, phường Tăng Nhơn Phú A, Quận 9, Tp. Hồ Chí Minh.', NULL, N'ACTIVE', N'4536789078456', N'https://motorbikeimages.blob.core.windows.net/motorbikebs/9142e438-368f-4d8b-8e3e-d64772e27e91.png')
GO
INSERT [dbo].[StoreDesciption] ([store_id], [user_id], [store_name], [description], [store_phone], [store_email], [store_created_at], [store_updated_at], [point], [address], [ward_id], [status], [tax_code], [business_license]) VALUES (6, 18, N'Cửa Hàng Xe Máy Cũ Đức Dũng II', NULL, N'0937500668', N'phatntse150133@fpt.edu.vn', CAST(N'2023-10-10T08:22:11.503' AS DateTime), NULL, NULL, N'TL15- Ấp Phú An, Xã Phú Hòa Đông, Huyện Củ Chi, TP.HCM', NULL, N'ACTIVE', N'5566772234512', N'https://motorbikeimages.blob.core.windows.net/motorbikebs/72fe9c06-763e-430d-b710-951836da31f0.jpg')
GO
SET IDENTITY_INSERT [dbo].[StoreDesciption] OFF
GO
SET IDENTITY_INSERT [dbo].[StoreImage] ON 
GO
INSERT [dbo].[StoreImage] ([store_image_id], [image_link], [store_id]) VALUES (5, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/c1d2a91e-0290-4bed-80f0-9376d8d4dd64.jpg', 5)
GO
INSERT [dbo].[StoreImage] ([store_image_id], [image_link], [store_id]) VALUES (6, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/a38fb08a-593f-4361-9369-554b66840c49.jpg', 6)
GO
SET IDENTITY_INSERT [dbo].[StoreImage] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (1, N'Nguyễn Tiến Phát', N'phat8a6@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2023-10-05' AS Date), NULL, N'ACTIVE', 0x1C51F0A3E77FD04869A3AD93C555E9822B3C2F3812AB93F0246B2C328FAA91588FD611F623AFFDD8CE2CED8366C6133A4AA4B1115240E1F2EBC7328632C4B763, 0x1A05AE7EF71C1D135CFBC01A60B29963B5E026E740218596265EFF691D2330E2F0AD13C07126252B687F721C3BAEB0734858FBB72C77718BC4D731C4794E273415879D4AA447012FBA60833D7CD8771A021117A2F870A1AADD2F8A5EAE504A4D0299DE653EAA4DF0C9D42BD88C9414785FBB9E56F0C2D5EBCBF68040008D6E8A, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (2, N'Nguyễn Nhựt Minh', N'nhutminh.it19@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 2, CAST(N'2023-10-05' AS Date), NULL, N'ACTIVE', 0xFC4D263550BA97B8775151A71E0D5088571892C7A99FECC77E204E600E759E81A4BFB02ECE2711241A4442A540A62AAD7AC48C6E5D0A635CE77EC236623384F8, 0x63AA969B866826952196CD5A4F129576D9B045B88B02B54CC22B8AEF50CABC3263A600490206171D834793AE3AE6F96DF03CBC133992B635E66F7F9AE3D6A2D4546DB146667D8602008EE111586880F61F0F983A7F09C35D1DFC3E699D1B20BD9B81B5B7A8DB0CC547A134CEADC655A6DE186E5DC4679B96AF7C1163D3D3F725, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (10, N'Minh Trí', N'phanminhtri269@gmail.com', N'0908660977', 1, CAST(N'2001-09-26' AS Date), N'123123123123', N'Quận 4, Hồ Chí Minh', NULL, 4, CAST(N'2023-10-05' AS Date), CAST(N'2023-10-05' AS Date), N'ACTIVE', 0xA585D4D64DFDB23C80658BA185A70CF6AAE8A53E78437CD394219CAC568837ED754795D418158B795B48B092164CCEDB15CF4709706E6D476E36590530E833E1, 0x6F607BE7009771AE4E9906491FA12B5DB502D738E8207702325DEB52262E85767BCEA9BCEEB6C6826FE88B6A6E9F7C2260A8C7FDB0768DE153729018B9DC5AD55F5B1D8AD1BE36C85F64ED6103CFAF863FA77E2CDD9BB834124B1A247DDAD18E0272683C206CDC51A3A6673D785F846BE98598E9675C8EC2E90FB474799FD07D, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (11, N'Minh Tri FPT', N'tripmse150151@fpt.edu.vn', N'0937500668', NULL, NULL, N'436785096712', N'22 Đường Âu Dương, Phường 3, Quận 8, TP.HCM', NULL, 4, CAST(N'2023-10-05' AS Date), NULL, N'ACTIVE', 0xB376591F2980D24DCE3229337D2799269090CE67CFEA18C27C713408272592456165A86C6279EBE31A5A3D78FBDEA5158DE197D05B6D65B2B056AE70F52AE51D, 0x3A3A7E5C89F4C42B1B7F62C9D9576CFB9EA08F1AF75D94CA01A22940ACFB81FDD3EBD5D8BFDAC0CA33E21F130ACCDF25AB73FD44FC3D91029E8651DA91665D65B2A531395AD927A78657B5E0BD400670ED54C49FCFDC421EE0A2DE5167255192B49D8D409673054FA577EC55E034FE238EDFA3DF696B0C4319273FB6838848E2, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (14, N'Nhut Minh', N'minhnnse150140@fpt.com.vn', NULL, NULL, NULL, NULL, NULL, NULL, 4, NULL, NULL, N'NOT VERIFY', 0xE324DC2B63CA0938F622C96FFE862F25C582400B1DE619F1AFBA065CCDDCDF0E8DB471A1D4FC8C4FE684CEB78E6587460183195498B151776C1CCB58186E00AB, 0xF2913D37CEE620C2180EABFB2D41BE176D33B98022948D9C4A1CFBE6CF12DC80576733EB3BAF83F7C979AC0550BFD57AFEFB27218439AA5E9A936D6B7332E089F59EE48A91CFC0F03F9F5B4AF2D1B9566C1BA748BDAFFF7D139B110EE27A2B531F541A9EC58D6D28578E86CAD01896E00D0EC728608E7C38B9DEBD2C0008F849, NULL, NULL, N'76DE90E1C3C4AF539B7E81C5257FDC7B7B31845A601626DCA89F27750F5F9460', CAST(N'2023-10-05T21:18:47.057' AS DateTime))
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (15, N'Trần Thành Công', N'tranthanhcong652001@gmail.com', N'0855562320', 1, CAST(N'2003-01-14' AS Date), N'545467894522', N'Bến Vân Đồn, Mỹ Tho, Tiền Giang', NULL, 3, CAST(N'2023-10-05' AS Date), CAST(N'2023-10-05' AS Date), N'ACTIVE', 0x9EDD41CC51B6790001988E63F3B9120BC2706FAE56A616B65F8A97621C8DE806F525BFF63A428E30BE3BEAF519A0830626E250614BD2EAEAC87E6151D40AE2BD, 0x61D44F72DFE5077B2D3E1944E3CD4DE2E26D00DEA6B322638FC40FAF5ED0C1947D4FA16D4625F6E127DCE970487CF3BFD91E737007926279C9945B9A1B28671C85C744170B24F3515A3A22427BAD40E86F95E2A9C9BBBEB2A772BE09481BDBE3DCA20747DED8EDB0C0CCAAC4FD57AF0C31BD98D193BC63459470DF3B66E9BEFF, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (16, N'HungStore', N'hung.trangquang071117@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 4, NULL, NULL, N'NOT VERIFY', 0x9428861020FB2586B4A932A1858BEAB8B03123971056466D90390CA4546CC52E23ABCAF3607663AE176B7806DA597BAA876FC3001B43620A129CEB584BF1797E, 0x591E4658B62BE1EF55AE2F4E9F299AA48D337C594C22843D70B8B380E4B2EC8E238A70FEBD1DECD48CBA341A4CB4C8516C5E49B1D67CAAAB1AA34750DA212122E16E67303A8AFB7CA8F569A015CDA28F1D620725A5B4D9DA3DF53519916AB88E7F613489E1D4F57D1860C0DE088532397B28F3727F8CDAD67E2B22C086AAE49E, NULL, NULL, N'0D8B65B62609373A9517117CCC0C060C5C57A8099E035CE821081698B4CF69D3', CAST(N'2023-10-07T01:03:56.623' AS DateTime))
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (17, N'Nhựt Minh Nguyễn', N'minhnnse150140@fpt.edu.vn', NULL, NULL, NULL, NULL, NULL, NULL, 4, CAST(N'2023-10-06' AS Date), NULL, N'ACTIVE', 0x36C102C6765BEB61FE222987B688086F101BDDFD3E74C77DCCA38FB84C53071C384071E7933F8A36E10E5BBD732CD6B77F667D6D9FC994D78C8508686886F43E, 0x68F9E7AE84F378708600804E3ABFA2872458B95CA033C46D6742C7F53C6A8C4D878CE51F4263082E25A82545D77E5C5444FF3BF7A851ED817A1CF7E7CAB4DBDA1934DFDF9C9BE29AF1234E0EA4621496068C805C363FE4B9F4A3D5108CDA9F7E62EAD2687A087E3D8989BECAC5FD46EBE11602758D31C034FD0C887F17DDE999, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [ward_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (18, N'Nguyễn Nhựt Minh', N'nhutminh.fpt@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 2, CAST(N'2023-10-10' AS Date), NULL, N'ACTIVE', 0x859E4F440D27BD3A06E706CC6D5F46D294901F8751608C8704E0F09B1D43C2A770C099AD046315F51E6FB90D9BA5A9A38D072923CE9899ED18E8262A591A02D2, 0xCB77AAACFC9607953E3BED96DC869630E3F979EC9CA6DD5781ED4D549A980AE6291DABC659E7B4BA8788004DCC79B2B98BE1BDE7DBB0A059A66006DB14B1386E3BABC5AD1DBB8B988FAA06466F67597274C9F5BFB02C933676EB424C28D95F66D757C4649EF85863BEEBEAAA92813BB1A64E026EB4B04C8BE745FCB7A5B37438, NULL, NULL, N'', NULL)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
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
USE [master]
GO
ALTER DATABASE [motorbikebs-db] SET  READ_WRITE 
GO
