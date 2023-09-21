USE [master]
GO
/****** Object:  Database [MotorbikeDB]    Script Date: 09/22/2023 1:12:40 AM ******/
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
ALTER DATABASE [MotorbikeDB] SET  DISABLE_BROKER 
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
ALTER DATABASE [MotorbikeDB] SET DB_CHAINING OFF 
GO

USE [MotorbikeDB]
GO
/****** Object:  Table [dbo].[BillConfirm]    Script Date: 09/22/2023 1:12:40 AM ******/
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
/****** Object:  Table [dbo].[Booking]    Script Date: 09/22/2023 1:12:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking](
	[booking_id] [int] IDENTITY(1,1) NOT NULL,
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
/****** Object:  Table [dbo].[Consignment_Contract]    Script Date: 09/22/2023 1:12:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Consignment_Contract](
	[contract_id] [int] IDENTITY(1,1) NOT NULL,
	[motor_id] [int] NULL,
	[price] [money] NULL,
	[new_owner] [int] NULL,
	[store_id] [int] NULL,
	[content] [nvarchar](100) NULL,
	[created_at] [datetime] NULL,
	[status] [nvarchar](10) NULL,
	[negotiation_id] [int] NULL,
 CONSTRAINT [PK_Consignment_Contract] PRIMARY KEY CLUSTERED 
(
	[contract_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Consignment_ContractImage]    Script Date: 09/22/2023 1:12:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Consignment_ContractImage](
	[contract_image_id] [int] IDENTITY(1,1) NOT NULL,
	[contract_id] [int] NOT NULL,
	[image_link] [nvarchar](100) NULL,
	[description] [nvarchar](200) NULL,
 CONSTRAINT [PK_Consignment_ContractImage] PRIMARY KEY CLUSTERED 
(
	[contract_image_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EarnALiving_Contract]    Script Date: 09/22/2023 1:12:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EarnALiving_Contract](
	[contract_id] [int] IDENTITY(1,1) NOT NULL,
	[motor_id] [int] NULL,
	[price] [money] NULL,
	[new_owner] [int] NULL,
	[store_id] [int] NULL,
	[content] [nvarchar](1000) NULL,
	[created_at] [datetime] NULL,
	[status] [nvarchar](10) NULL,
	[booking_id] [int] NULL,
	[negotiation_id] [int] NULL,
 CONSTRAINT [PK_EarnALiving_Contract] PRIMARY KEY CLUSTERED 
(
	[contract_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EarnALiving_ContractImage]    Script Date: 09/22/2023 1:12:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EarnALiving_ContractImage](
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
/****** Object:  Table [dbo].[LocalAddress]    Script Date: 09/22/2023 1:12:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocalAddress](
	[local_id] [int] NOT NULL,
	[ward_name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_LocalAddress] PRIMARY KEY CLUSTERED 
(
	[local_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Motorbike]    Script Date: 09/22/2023 1:12:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Motorbike](
	[motor_id] [int] IDENTITY(1,1) NOT NULL,
	[certificate_number] [nchar](6) NOT NULL,
	[brand_id] [int] NULL,
	[model_id] [int] NULL,
	[odo] [int] NULL,
	[year] [date] NULL,
	[price] [money] NULL,
	[description] [nvarchar](255) NULL,
	[motor_status_id] [int] NULL,
	[motor_type_id] [int] NULL,
	[store_id] [int] NULL,
	[owner_id] [int] NOT NULL,
 CONSTRAINT [PK_Motorbike] PRIMARY KEY CLUSTERED 
(
	[motor_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotorbikeBrand]    Script Date: 09/22/2023 1:12:40 AM ******/
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
/****** Object:  Table [dbo].[MotorbikeImage]    Script Date: 09/22/2023 1:12:40 AM ******/
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
/****** Object:  Table [dbo].[MotorbikeModel]    Script Date: 09/22/2023 1:12:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MotorbikeModel](
	[model_id] [int] IDENTITY(1,1) NOT NULL,
	[model_name] [nvarchar](50) NULL,
	[description] [nvarchar](200) NULL,
	[status] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_MotorbikeModel] PRIMARY KEY CLUSTERED 
(
	[model_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotorbikeStatus]    Script Date: 09/22/2023 1:12:40 AM ******/
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
/****** Object:  Table [dbo].[MotorbikeType]    Script Date: 09/22/2023 1:12:40 AM ******/
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
/****** Object:  Table [dbo].[Negotiation]    Script Date: 09/22/2023 1:12:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Negotiation](
	[negotiation_id] [int] IDENTITY(1,1) NOT NULL,
	[request_id] [int] NOT NULL,
	[price] [money] NULL,
	[start_time] [datetime] NULL,
	[end_time] [datetime] NULL,
	[description] [nvarchar](200) NULL,
	[status] [nvarchar](50) NULL,
	[from_Seller] [bit] NULL,
 CONSTRAINT [PK_Negotiation] PRIMARY KEY CLUSTERED 
(
	[negotiation_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 09/22/2023 1:12:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[notification_id] [int] IDENTITY(1,1) NOT NULL,
	[booking_id] [int] NOT NULL,
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
/****** Object:  Table [dbo].[NotificationType]    Script Date: 09/22/2023 1:12:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationType](
	[notification_type_id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](50) NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_NotificationType] PRIMARY KEY CLUSTERED 
(
	[notification_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 09/22/2023 1:12:40 AM ******/
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
/****** Object:  Table [dbo].[PointHistory]    Script Date: 09/22/2023 1:12:40 AM ******/
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
/****** Object:  Table [dbo].[PostBoosting]    Script Date: 09/22/2023 1:12:40 AM ******/
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
/****** Object:  Table [dbo].[Request]    Script Date: 09/22/2023 1:12:40 AM ******/
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
/****** Object:  Table [dbo].[RequestType]    Script Date: 09/22/2023 1:12:40 AM ******/
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
/****** Object:  Table [dbo].[Role]    Script Date: 09/22/2023 1:12:40 AM ******/
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
/****** Object:  Table [dbo].[StoreDesciption]    Script Date: 09/22/2023 1:12:40 AM ******/
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
	[local_id] [int] NULL,
	[status] [nvarchar](15) NULL,
 CONSTRAINT [PK_StoreDesciption] PRIMARY KEY CLUSTERED 
(
	[store_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StoreImage]    Script Date: 09/22/2023 1:12:40 AM ******/
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
/****** Object:  Table [dbo].[User]    Script Date: 09/22/2023 1:12:40 AM ******/
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
	[local_id] [int] NULL,
	[role_id] [int] NULL,
	[user_verify_at] [date] NULL,
	[user_updated_at] [date] NULL,
	[status] [nvarchar](10) NOT NULL,
	[password_hash] [varbinary](max) NULL,
	[password_salt] [varbinary](max) NULL,
	[password_reset_token] [nvarchar](max) NULL,
	[reset_token_expires] [datetime] NULL,
	[verifycation_token] [nvarchar](max) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wishlist]    Script Date: 09/22/2023 1:12:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wishlist](
	[user_id] [int] NOT NULL,
	[motor_id] [int] NOT NULL,
 CONSTRAINT [PK_Wishlist] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC,
	[motor_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Motorbike] ON 
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [brand_id], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id]) VALUES (2, N'22333 ', 2, 1, 23233, CAST(N'2011-02-02' AS Date), 222.0000, NULL, 1, 2, 1, 2)
GO
SET IDENTITY_INSERT [dbo].[Motorbike] OFF
GO
SET IDENTITY_INSERT [dbo].[MotorbikeBrand] ON 
GO
INSERT [dbo].[MotorbikeBrand] ([brand_id], [brand_name], [description], [status]) VALUES (2, N'Yamaha', NULL, N'ACTIVE')
GO
INSERT [dbo].[MotorbikeBrand] ([brand_id], [brand_name], [description], [status]) VALUES (3, N'Suzuki', NULL, N'ACTIVE')
GO
SET IDENTITY_INSERT [dbo].[MotorbikeBrand] OFF
GO
SET IDENTITY_INSERT [dbo].[MotorbikeModel] ON 
GO
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status]) VALUES (1, N'Nouvo', NULL, N'ACTIVE')
GO
INSERT [dbo].[MotorbikeModel] ([model_id], [model_name], [description], [status]) VALUES (3, N'WinnerX', NULL, N'ACTIVE')
GO
SET IDENTITY_INSERT [dbo].[MotorbikeModel] OFF
GO
SET IDENTITY_INSERT [dbo].[MotorbikeStatus] ON 
GO
INSERT [dbo].[MotorbikeStatus] ([motorStatus_id], [title], [description]) VALUES (1, N'Posting', NULL)
GO
INSERT [dbo].[MotorbikeStatus] ([motorStatus_id], [title], [description]) VALUES (2, N'Storage', NULL)
GO
INSERT [dbo].[MotorbikeStatus] ([motorStatus_id], [title], [description]) VALUES (3, N'Consignment', NULL)
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
INSERT [dbo].[StoreDesciption] ([store_id], [user_id], [store_name], [description], [store_phone], [store_email], [store_created_at], [store_updated_at], [point], [address], [local_id], [status]) VALUES (1, 2, N'Hoàng Anh Store', N'Hoàng Anh Store là một cửa hàng chuyên buôn bán xe máy cũ, đã trở thành điểm đến đáng tin cậy cho những người yêu thích sự tiện lợi và tiết kiệm của việc sử dụng xe máy. Với nhiều năm kinh nghiệm trong lĩnh vực này, chúng tôi tự hào cung cấp cho khách hàng những chiếc xe máy đã qua sử dụng, được bảo trì và bảo dưỡng kỹ lưỡng để đảm bảo tính an toàn và hiệu suất tối ưu.', N'0978202222', N'anhhoang.store@gmail.com', CAST(N'2023-09-12T00:00:00.000' AS DateTime), CAST(N'2023-09-12T00:00:00.000' AS DateTime), 0, N'23 Tô Hiến Thành', NULL, N'ACTIVE')
GO
INSERT [dbo].[StoreDesciption] ([store_id], [user_id], [store_name], [description], [store_phone], [store_email], [store_created_at], [store_updated_at], [point], [address], [local_id], [status]) VALUES (4, 28, N'NTP Store', NULL, N'1111111111', N'phat@gmail.com', NULL, NULL, NULL, N'asd', NULL, N'ACTIVE')
GO
SET IDENTITY_INSERT [dbo].[StoreDesciption] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token]) VALUES (1, N'Admin_demo', N'admin@gmail.com', NULL, 1, CAST(N'2001-11-29' AS Date), N'123456789012', N'237 Nguyễn Xiển', NULL, 1, CAST(N'2023-09-12' AS Date), CAST(N'2023-09-12' AS Date), N'ACTIVE', 0xEB0DA19FC86576D2288CC54A13D467A19652B9401064DB0821D10A2A63EC75AFFB601494749842ACCACF9B4AA22F297EEC46C0EF765C871F1A390BEB76E41FCD, 0xC573FF90AB3856D9FDC33B3097B236141D7D55C1C0EE8CE38F66061A342C900C3769F624FC8A2C9B089F4F365B2B0CBD9D62490772617D42A77CC38A5381BAD1E682CC219A62B2B6D99EF0F71A803D3A7A47F9C19BE4E703773A4524A11FCAAADF249678BB4B6A926B926EE62F1199997DEE3B33ADBA8C6AA73CFA2E59B785C4, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token]) VALUES (2, N'Store_demo', N'store@gmail.com', NULL, 1, CAST(N'2001-11-29' AS Date), N'123456789023', N'23 Tô Hiến Thành', NULL, 2, CAST(N'2023-09-12' AS Date), CAST(N'2023-09-12' AS Date), N'ACTIVE', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token]) VALUES (3, N'Owner_demo', N'owner@gmail.com', NULL, 2, CAST(N'2001-11-11' AS Date), N'123456789034', N'90 Nguyễn Khuyến', NULL, 3, CAST(N'2023-09-12' AS Date), CAST(N'2023-09-12' AS Date), N'ACTIVE', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token]) VALUES (4, N'Customer_demo', N'customer@gmail.com', NULL, 2, CAST(N'2000-10-02' AS Date), N'123456789056', N'08 Lê Việt', NULL, 4, CAST(N'2023-09-12' AS Date), CAST(N'2023-09-12' AS Date), N'ACTIVE', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token]) VALUES (6, N'123', N'123', N'123       ', 0, CAST(N'2023-09-12' AS Date), N'string      ', N'string', NULL, NULL, CAST(N'2023-09-12' AS Date), CAST(N'2023-09-12' AS Date), N'string', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token]) VALUES (9, N'Nguyen Tien Phat', N'test@gmail.com', N'123       ', NULL, NULL, NULL, NULL, NULL, 4, CAST(N'0001-01-01' AS Date), CAST(N'0001-01-01' AS Date), N'NOT VERIFY', 0xFB38A26C762D6FC10D15072A691E5B0C795626D7F546FA9D424DDD0DEF558325D87502D29D736C936FA0CA5C10A7BD7E62B05A6F1AA0079B083EC6DD88A583CF, 0xF65AC1C90E8DBFE72F8CFB7E31959A0354578C899B75D7A0EDDE615F11529CC4BF71301071ECBA794B2B1105CA87201C4448FCBC44A92038128CAD27804E23827E7AB3AF7C067C2B6F5CCFAAFAECAEBD76C810462B0DC5CCD27C0F107685DA80F7C394F01C135F2D1D7D5FB48D3F213554067E67DA9AB9AD6813C5F9FEE08E97, NULL, NULL, N'35FDBA90367AD13B5B4A71273E88825BEACDAB0C7691548B230BC7B0534F4BA257B514A417667C0BC4F5F83C726A3BA326FC322ADDE5A852696EE4590B198678')
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token]) VALUES (10, N'asd', N'tester@gmail.com', N'123       ', NULL, NULL, NULL, NULL, NULL, 4, CAST(N'2023-09-18' AS Date), NULL, N'ACTIVE', 0x2DB59D62BDCACC6E6085D0191E657ED59BEC7CD44D266E81977FDFA37DE0EC20C0C5ED55303F33470CFA0E8D15E084C741B89B4A245924CCCE9C6A48B9ED238A, 0x49D68229D3E2E32FCDD6743B4A1563CEDC74044A41F577C9684362464D8F4F0C86992C9ACA6A79360D415C02FD6922E59A246234D575B98DD5A1F0A85E1AD6F1FC7E60ED6951ADA9141F0523E641AC8FE9981D2468B7DFDDAA21775F67A5BC88DBF77079154EE85B76265CB6384142A6F81532EC99D59F2564D428C9A6C088F2, NULL, NULL, N'DB8B663BA93A32ED37ED1BA4C69132FAA7925DF82B24B553B29715CD41AD817773A331F2B8FD74AF9327B4C86490E2E5B3D22555DF8D544FEE2162531A48D082')
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token]) VALUES (14, N'NTP', N'phat8a6@gmail.com', N'111       ', NULL, NULL, NULL, NULL, NULL, 4, CAST(N'2023-09-18' AS Date), NULL, N'ACTIVE', 0x09970A7B3E5CCE4DE539B5028F0AD159114F2D057A1335185D979A92BFA52BC1D6601F0A58D17096DB6FD052624FCDCF7B85BEACA96D2914BAD14AEACD0EEA93, 0xD8193C54BFB7DAF582F935CF625871922B7AC2EA52FEB07EA787BC9DA751816BA820EDA0A09AC8BD8A488E73C5F37A2F983EDB2C7093C97BF551EEC6DD0679E5B5297ADBF99BF15686CFA28C27A4F86F87DD9E263EF34066DC38E6F31255F9FCD17BA000DDE08A1F4ADD02A6232E6973D335CA54CC55CF577720BB730F70E0E2, NULL, NULL, N'C6053E2650ECC0BDB68D6F7A197EF9C801F408C76DD755D67B46963F5EED74A170E5982BC7257219FB5D3CDD4AF4397FB2A16D9ED5740A4F5647C492E284AC48')
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token]) VALUES (15, N'N A', N'user@example.com', N'111       ', NULL, NULL, NULL, NULL, NULL, 4, NULL, NULL, N'NOT VERIFY', 0x9F59A48F86983395C1527C18919660811CFE7299F890BF18926579DAA22BA2BD4CF77B04215CDC3CE33703F1A41ADE53E8113D585294FB53292238AB433B196A, 0x261E579D3E60E0EBF4F1C58606984B232A3349C36307716ECFDC9E070251E1588750A1D708C14E531A2E5B0E9F0514B0921EF6B5B78DB297C3694C83F4F234BE017942BD1F2DED677AE47791C0A43F73FE4BA2E1F032259099487D4D9A8E32C3F26080B7B40A1192CAA85696FC5CFCB64C7DFB06DF8F529263A0741D47ECA432, NULL, NULL, N'3ED757A95578EB60EDBB089DFE63C940A4015431926BC8B179FFF8B2C784C5B87EB4AC131C486A8806F6509A353D709E745EC28C33207A3CCE64AA5E153F1AE6')
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token]) VALUES (28, N'NTPReRegister', N'phatnt1132001@gmail.com', N'1111111111', NULL, NULL, NULL, NULL, NULL, 2, CAST(N'2023-09-19' AS Date), NULL, N'ACTIVE', 0x25DDE6125CFB922E9F91431F216D1056E91FCA90CFE21025A0EC24E2CDCDFBC90576019091A7C52627A6A1C65ABDF25E2BBDAE9DD6A2660DA934185F6AFCFED9, 0x832A0EA1C300D9AAA21FCF95FAFE6B79805CEB0F51BF8B08AC975493598A823080FE50D82AB08A6DC9FE7CF2F016A6BAF36A4444A6ED51AFC84A7EF71CECCE57689AA9671FF17C59F5FFE8E2F624E149ACF29565240F2BAEB14D9ADDACA0042FF50D4E7A0DB54B198F7460417883B720AE216F3E4E91E97743B15849ECAB7991, NULL, NULL, N'996BE2174F7F116F5B051D28C6CB11586F1A2C64463512BB55E1CAAF7AA2F07621B72CC36555561F3E66C3A414739A039CC846AB5440374EB3DC021D639B5804')
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
ALTER TABLE [dbo].[BillConfirm]  WITH CHECK ADD  CONSTRAINT [FK_BillConfirm_Request] FOREIGN KEY([request_id])
REFERENCES [dbo].[Request] ([request_id])
GO
ALTER TABLE [dbo].[BillConfirm] CHECK CONSTRAINT [FK_BillConfirm_Request]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_Request] FOREIGN KEY([request_id])
REFERENCES [dbo].[Request] ([request_id])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_Request]
GO
ALTER TABLE [dbo].[Consignment_Contract]  WITH CHECK ADD  CONSTRAINT [FK_Consignment_Contract_Negotiation] FOREIGN KEY([negotiation_id])
REFERENCES [dbo].[Negotiation] ([negotiation_id])
GO
ALTER TABLE [dbo].[Consignment_Contract] CHECK CONSTRAINT [FK_Consignment_Contract_Negotiation]
GO
ALTER TABLE [dbo].[Consignment_ContractImage]  WITH CHECK ADD  CONSTRAINT [FK_Consignment_ContractImage_Consignment_Contract] FOREIGN KEY([contract_id])
REFERENCES [dbo].[Consignment_Contract] ([contract_id])
GO
ALTER TABLE [dbo].[Consignment_ContractImage] CHECK CONSTRAINT [FK_Consignment_ContractImage_Consignment_Contract]
GO
ALTER TABLE [dbo].[EarnALiving_Contract]  WITH CHECK ADD  CONSTRAINT [FK_EarnALiving_Contract_Booking] FOREIGN KEY([booking_id])
REFERENCES [dbo].[Booking] ([booking_id])
GO
ALTER TABLE [dbo].[EarnALiving_Contract] CHECK CONSTRAINT [FK_EarnALiving_Contract_Booking]
GO
ALTER TABLE [dbo].[EarnALiving_Contract]  WITH CHECK ADD  CONSTRAINT [FK_EarnALiving_Contract_Negotiation] FOREIGN KEY([negotiation_id])
REFERENCES [dbo].[Negotiation] ([negotiation_id])
GO
ALTER TABLE [dbo].[EarnALiving_Contract] CHECK CONSTRAINT [FK_EarnALiving_Contract_Negotiation]
GO
ALTER TABLE [dbo].[EarnALiving_ContractImage]  WITH CHECK ADD  CONSTRAINT [FK_EarnALiving_ContractImage_EarnALiving_Contract] FOREIGN KEY([contract_id])
REFERENCES [dbo].[EarnALiving_Contract] ([contract_id])
GO
ALTER TABLE [dbo].[EarnALiving_ContractImage] CHECK CONSTRAINT [FK_EarnALiving_ContractImage_EarnALiving_Contract]
GO
ALTER TABLE [dbo].[Motorbike]  WITH CHECK ADD  CONSTRAINT [FK_Motorbike_MotorbikeBrand] FOREIGN KEY([brand_id])
REFERENCES [dbo].[MotorbikeBrand] ([brand_id])
GO
ALTER TABLE [dbo].[Motorbike] CHECK CONSTRAINT [FK_Motorbike_MotorbikeBrand]
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
ALTER TABLE [dbo].[MotorbikeImage]  WITH CHECK ADD  CONSTRAINT [FK_MotorbikeImage_Motorbike] FOREIGN KEY([motor_id])
REFERENCES [dbo].[Motorbike] ([motor_id])
GO
ALTER TABLE [dbo].[MotorbikeImage] CHECK CONSTRAINT [FK_MotorbikeImage_Motorbike]
GO
ALTER TABLE [dbo].[Negotiation]  WITH CHECK ADD  CONSTRAINT [FK_Negotiation_Request] FOREIGN KEY([request_id])
REFERENCES [dbo].[Request] ([request_id])
GO
ALTER TABLE [dbo].[Negotiation] CHECK CONSTRAINT [FK_Negotiation_Request]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_Booking] FOREIGN KEY([booking_id])
REFERENCES [dbo].[Booking] ([booking_id])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_Booking]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_NotificationType] FOREIGN KEY([notification_type_id])
REFERENCES [dbo].[NotificationType] ([notification_type_id])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_NotificationType]
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
ALTER TABLE [dbo].[StoreDesciption]  WITH CHECK ADD  CONSTRAINT [FK_StoreDesciption_LocalAddress] FOREIGN KEY([local_id])
REFERENCES [dbo].[LocalAddress] ([local_id])
GO
ALTER TABLE [dbo].[StoreDesciption] CHECK CONSTRAINT [FK_StoreDesciption_LocalAddress]
GO
ALTER TABLE [dbo].[StoreDesciption]  WITH CHECK ADD  CONSTRAINT [FK_StoreDesciption_User] FOREIGN KEY([user_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[StoreDesciption] CHECK CONSTRAINT [FK_StoreDesciption_User]
GO
ALTER TABLE [dbo].[StoreImage]  WITH CHECK ADD  CONSTRAINT [FK_StoreImage_StoreDesciption] FOREIGN KEY([store_id])
REFERENCES [dbo].[StoreDesciption] ([store_id])
GO
ALTER TABLE [dbo].[StoreImage] CHECK CONSTRAINT [FK_StoreImage_StoreDesciption]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_LocalAddress] FOREIGN KEY([local_id])
REFERENCES [dbo].[LocalAddress] ([local_id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_LocalAddress]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([role_id])
REFERENCES [dbo].[Role] ([role_id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
ALTER TABLE [dbo].[Wishlist]  WITH CHECK ADD  CONSTRAINT [FK_Wishlist_Motorbike] FOREIGN KEY([motor_id])
REFERENCES [dbo].[Motorbike] ([motor_id])
GO
ALTER TABLE [dbo].[Wishlist] CHECK CONSTRAINT [FK_Wishlist_Motorbike]
GO
ALTER TABLE [dbo].[Wishlist]  WITH CHECK ADD  CONSTRAINT [FK_Wishlist_User] FOREIGN KEY([user_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Wishlist] CHECK CONSTRAINT [FK_Wishlist_User]
GO
USE [master]
GO
ALTER DATABASE [MotorbikeDB] SET  READ_WRITE 
GO
