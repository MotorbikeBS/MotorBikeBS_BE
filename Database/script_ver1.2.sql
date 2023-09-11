USE [master]
GO
/****** Object:  Database [MotorbikeDB]    Script Date: 09/12/2023 1:02:09 AM ******/
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
/****** Object:  Table [dbo].[BillConfirm]    Script Date: 09/12/2023 1:02:09 AM ******/
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
/****** Object:  Table [dbo].[Booking]    Script Date: 09/12/2023 1:02:09 AM ******/
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
/****** Object:  Table [dbo].[Consignment_Contract]    Script Date: 09/12/2023 1:02:09 AM ******/
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
/****** Object:  Table [dbo].[Consignment_ContractImage]    Script Date: 09/12/2023 1:02:09 AM ******/
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
/****** Object:  Table [dbo].[EarnALiving_Contract]    Script Date: 09/12/2023 1:02:09 AM ******/
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
/****** Object:  Table [dbo].[EarnALiving_ContractImage]    Script Date: 09/12/2023 1:02:09 AM ******/
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
/****** Object:  Table [dbo].[Facility]    Script Date: 09/12/2023 1:02:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Facility](
	[facility_id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](100) NULL,
	[description] [nvarchar](200) NULL,
	[status] [nvarchar](50) NULL,
 CONSTRAINT [PK_Facility] PRIMARY KEY CLUSTERED 
(
	[facility_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LocalAddress]    Script Date: 09/12/2023 1:02:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocalAddress](
	[local_id] [int] NOT NULL,
	[ward_name] [nvarchar](50) NOT NULL,
	[district_name] [nvarchar](50) NOT NULL,
	[city_name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_LocalAddress] PRIMARY KEY CLUSTERED 
(
	[local_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Motorbike]    Script Date: 09/12/2023 1:02:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Motorbike](
	[motor_id] [int] NOT NULL,
	[brand] [nvarchar](50) NULL,
	[model] [nvarchar](50) NULL,
	[year] [date] NULL,
	[price] [money] NULL,
	[description] [nvarchar](255) NULL,
	[motor_status_id] [int] NULL,
	[motor_type_id] [int] NULL,
	[image_id] [int] NULL,
	[store_id] [int] NULL,
	[owner_id] [int] NOT NULL,
 CONSTRAINT [PK_Motorbike] PRIMARY KEY CLUSTERED 
(
	[motor_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotorbikeFacility]    Script Date: 09/12/2023 1:02:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MotorbikeFacility](
	[facility_id] [int] IDENTITY(1,1) NOT NULL,
	[motor_id] [int] NOT NULL,
 CONSTRAINT [PK_MotorbikeFacility] PRIMARY KEY CLUSTERED 
(
	[facility_id] ASC,
	[motor_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotorbikeImage]    Script Date: 09/12/2023 1:02:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MotorbikeImage](
	[image_id] [int] IDENTITY(1,1) NOT NULL,
	[image_link] [nvarchar](100) NULL,
	[motor_id] [int] NOT NULL,
 CONSTRAINT [PK_MotorbikeImage] PRIMARY KEY CLUSTERED 
(
	[image_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotorbikeStatus]    Script Date: 09/12/2023 1:02:09 AM ******/
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
/****** Object:  Table [dbo].[MotorbikeType]    Script Date: 09/12/2023 1:02:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MotorbikeType](
	[motorType_id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](100) NULL,
	[description] [nvarchar](200) NULL,
 CONSTRAINT [PK_MotorbikeType] PRIMARY KEY CLUSTERED 
(
	[motorType_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Negotiation]    Script Date: 09/12/2023 1:02:09 AM ******/
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
/****** Object:  Table [dbo].[Notification]    Script Date: 09/12/2023 1:02:09 AM ******/
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
/****** Object:  Table [dbo].[NotificationType]    Script Date: 09/12/2023 1:02:09 AM ******/
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
/****** Object:  Table [dbo].[Payment]    Script Date: 09/12/2023 1:02:09 AM ******/
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
/****** Object:  Table [dbo].[PointHistory]    Script Date: 09/12/2023 1:02:09 AM ******/
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
/****** Object:  Table [dbo].[PostBoosting]    Script Date: 09/12/2023 1:02:09 AM ******/
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
/****** Object:  Table [dbo].[Request]    Script Date: 09/12/2023 1:02:09 AM ******/
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
/****** Object:  Table [dbo].[RequestType]    Script Date: 09/12/2023 1:02:09 AM ******/
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
/****** Object:  Table [dbo].[Role]    Script Date: 09/12/2023 1:02:09 AM ******/
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
/****** Object:  Table [dbo].[StoreDesciption]    Script Date: 09/12/2023 1:02:09 AM ******/
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
	[store_manager_id] [int] NOT NULL,
	[point] [int] NULL,
	[address] [nvarchar](100) NULL,
	[local_id] [int] NULL,
 CONSTRAINT [PK_StoreDesciption] PRIMARY KEY CLUSTERED 
(
	[store_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 09/12/2023 1:02:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[user_name] [nvarchar](100) NULL,
	[email] [nvarchar](100) NOT NULL,
	[password] [nvarchar](100) NOT NULL,
	[phone] [nchar](10) NULL,
	[gender] [int] NULL,
	[dob] [date] NULL,
	[idCard] [nchar](12) NULL,
	[address] [nvarchar](100) NULL,
	[local_id] [int] NULL,
	[role_id] [int] NULL,
	[user_created_at] [date] NOT NULL,
	[user_updated_at] [date] NOT NULL,
	[status] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wishlist]    Script Date: 09/12/2023 1:02:09 AM ******/
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
INSERT [dbo].[StoreDesciption] ([store_id], [user_id], [store_name], [description], [store_phone], [store_email], [store_created_at], [store_updated_at], [store_manager_id], [point], [address], [local_id]) VALUES (1, 2, N'Hoàng Anh Store', N'Hoàng Anh Store là một cửa hàng chuyên buôn bán xe máy cũ, đã trở thành điểm đến đáng tin cậy cho những người yêu thích sự tiện lợi và tiết kiệm của việc sử dụng xe máy. Với nhiều năm kinh nghiệm trong lĩnh vực này, chúng tôi tự hào cung cấp cho khách hàng những chiếc xe máy đã qua sử dụng, được bảo trì và bảo dưỡng kỹ lưỡng để đảm bảo tính an toàn và hiệu suất tối ưu.', N'0978202222', N'anhhoang.store@gmail.com', CAST(N'2023-09-12T00:00:00.000' AS DateTime), CAST(N'2023-09-12T00:00:00.000' AS DateTime), 2, 0, N'23 Tô Hiến Thành', NULL)
GO
SET IDENTITY_INSERT [dbo].[StoreDesciption] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [password], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_created_at], [user_updated_at], [status]) VALUES (1, N'Admin_demo', N'admin@gmail.com', N'123@', NULL, 1, CAST(N'2001-11-29' AS Date), N'123456789012', N'237 Nguyễn Xiển', NULL, 1, CAST(N'2023-09-12' AS Date), CAST(N'2023-09-12' AS Date), N'ACTIVE')
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [password], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_created_at], [user_updated_at], [status]) VALUES (2, N'Store_demo', N'store@gmail.com', N'123@', NULL, 1, CAST(N'2001-11-29' AS Date), N'123456789023', N'23 Tô Hiến Thành', NULL, 2, CAST(N'2023-09-12' AS Date), CAST(N'2023-09-12' AS Date), N'ACTIVE')
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [password], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_created_at], [user_updated_at], [status]) VALUES (3, N'Owner_demo', N'owner@gmail.com', N'123@', NULL, 2, CAST(N'2001-11-11' AS Date), N'123456789034', N'90 Nguyễn Khuyến', NULL, 3, CAST(N'2023-09-12' AS Date), CAST(N'2023-09-12' AS Date), N'ACTIVE')
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [password], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_created_at], [user_updated_at], [status]) VALUES (4, N'Customer_demo', N'customer@gmail.com', N'123@', NULL, 2, CAST(N'2000-10-02' AS Date), N'123456789056', N'08 Lê Việt', NULL, 4, CAST(N'2023-09-12' AS Date), CAST(N'2023-09-12' AS Date), N'ACTIVE')
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
ALTER TABLE [dbo].[Motorbike]  WITH CHECK ADD  CONSTRAINT [FK_Motorbike_User] FOREIGN KEY([owner_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Motorbike] CHECK CONSTRAINT [FK_Motorbike_User]
GO
ALTER TABLE [dbo].[MotorbikeFacility]  WITH CHECK ADD  CONSTRAINT [FK_MotorbikeFacility_Facility] FOREIGN KEY([facility_id])
REFERENCES [dbo].[Facility] ([facility_id])
GO
ALTER TABLE [dbo].[MotorbikeFacility] CHECK CONSTRAINT [FK_MotorbikeFacility_Facility]
GO
ALTER TABLE [dbo].[MotorbikeFacility]  WITH CHECK ADD  CONSTRAINT [FK_MotorbikeFacility_Motorbike] FOREIGN KEY([motor_id])
REFERENCES [dbo].[Motorbike] ([motor_id])
GO
ALTER TABLE [dbo].[MotorbikeFacility] CHECK CONSTRAINT [FK_MotorbikeFacility_Motorbike]
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
ALTER TABLE [dbo].[StoreDesciption]  WITH CHECK ADD  CONSTRAINT [FK_StoreDesciption_User1] FOREIGN KEY([store_manager_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[StoreDesciption] CHECK CONSTRAINT [FK_StoreDesciption_User1]
GO
ALTER TABLE [dbo].[StoreDesciption]  WITH CHECK ADD  CONSTRAINT [FK_StoreDesciption_User2] FOREIGN KEY([store_manager_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[StoreDesciption] CHECK CONSTRAINT [FK_StoreDesciption_User2]
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
