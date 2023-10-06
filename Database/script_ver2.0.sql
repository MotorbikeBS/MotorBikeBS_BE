/****** Object:  Database [motorbikebs-db]    Script Date: 10/07/2023 1:41:13 AM ******/
CREATE DATABASE [motorbikebs-db]  (EDITION = 'GeneralPurpose', SERVICE_OBJECTIVE = 'GP_S_Gen5_1', MAXSIZE = 32 GB) WITH CATALOG_COLLATION = SQL_Latin1_General_CP1_CI_AS;
GO
ALTER DATABASE [motorbikebs-db] SET COMPATIBILITY_LEVEL = 150
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
ALTER DATABASE [motorbikebs-db] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [motorbikebs-db] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [motorbikebs-db] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [motorbikebs-db] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [motorbikebs-db] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [motorbikebs-db] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [motorbikebs-db] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [motorbikebs-db] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [motorbikebs-db] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [motorbikebs-db] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [motorbikebs-db] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [motorbikebs-db] SET  MULTI_USER 
GO
ALTER DATABASE [motorbikebs-db] SET ENCRYPTION ON
GO
ALTER DATABASE [motorbikebs-db] SET QUERY_STORE = ON
GO
ALTER DATABASE [motorbikebs-db] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 100, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
/*** The scripts of database scoped configurations in Azure should be executed inside the target database connection. ***/
GO
-- ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 8;
GO
/****** Object:  Schema [SalesLT]    Script Date: 10/07/2023 1:41:14 AM ******/
CREATE SCHEMA [SalesLT]
GO
/****** Object:  UserDefinedDataType [dbo].[AccountNumber]    Script Date: 10/07/2023 1:41:14 AM ******/
CREATE TYPE [dbo].[AccountNumber] FROM [nvarchar](15) NULL
GO
/****** Object:  UserDefinedDataType [dbo].[Flag]    Script Date: 10/07/2023 1:41:14 AM ******/
CREATE TYPE [dbo].[Flag] FROM [bit] NOT NULL
GO
/****** Object:  UserDefinedDataType [dbo].[Name]    Script Date: 10/07/2023 1:41:14 AM ******/
CREATE TYPE [dbo].[Name] FROM [nvarchar](50) NULL
GO
/****** Object:  UserDefinedDataType [dbo].[NameStyle]    Script Date: 10/07/2023 1:41:14 AM ******/
CREATE TYPE [dbo].[NameStyle] FROM [bit] NOT NULL
GO
/****** Object:  UserDefinedDataType [dbo].[OrderNumber]    Script Date: 10/07/2023 1:41:14 AM ******/
CREATE TYPE [dbo].[OrderNumber] FROM [nvarchar](25) NULL
GO
/****** Object:  UserDefinedDataType [dbo].[Phone]    Script Date: 10/07/2023 1:41:14 AM ******/
CREATE TYPE [dbo].[Phone] FROM [nvarchar](25) NULL
GO
/****** Object:  Sequence [SalesLT].[SalesOrderNumber]    Script Date: 10/07/2023 1:41:14 AM ******/
CREATE SEQUENCE [SalesLT].[SalesOrderNumber] 
 AS [int]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -2147483648
 MAXVALUE 2147483647
 CACHE 
GO
/****** Object:  UserDefinedFunction [dbo].[ufnGetAllCategories]    Script Date: 10/07/2023 1:41:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufnGetAllCategories]()
RETURNS @retCategoryInformation TABLE
(
    -- Columns returned by the function
    [ParentProductCategoryName] nvarchar(50) NULL,
    [ProductCategoryName] nvarchar(50) NOT NULL,
    [ProductCategoryID] int NOT NULL
)
AS
-- Returns the CustomerID, first name, and last name for the specified customer.
BEGIN
    WITH CategoryCTE([ParentProductCategoryID], [ProductCategoryID], [Name]) AS
    (
        SELECT [ParentProductCategoryID], [ProductCategoryID], [Name]
        FROM SalesLT.ProductCategory
        WHERE ParentProductCategoryID IS NULL

    UNION ALL

        SELECT C.[ParentProductCategoryID], C.[ProductCategoryID], C.[Name]
        FROM SalesLT.ProductCategory AS C
        INNER JOIN CategoryCTE AS BC ON BC.ProductCategoryID = C.ParentProductCategoryID
    )

    INSERT INTO @retCategoryInformation
    SELECT PC.[Name] AS [ParentProductCategoryName], CCTE.[Name] as [ProductCategoryName], CCTE.[ProductCategoryID]
    FROM CategoryCTE AS CCTE
    JOIN SalesLT.ProductCategory AS PC
    ON PC.[ProductCategoryID] = CCTE.[ParentProductCategoryID];
    RETURN;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[ufnGetSalesOrderStatusText]    Script Date: 10/07/2023 1:41:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufnGetSalesOrderStatusText](@Status tinyint)
RETURNS nvarchar(15)
AS
-- Returns the sales order status text representation for the status value.
BEGIN
    DECLARE @ret nvarchar(15);

    SET @ret =
        CASE @Status
            WHEN 1 THEN 'In process'
            WHEN 2 THEN 'Approved'
            WHEN 3 THEN 'Backordered'
            WHEN 4 THEN 'Rejected'
            WHEN 5 THEN 'Shipped'
            WHEN 6 THEN 'Cancelled'
            ELSE '** Invalid **'
        END;

    RETURN @ret
END;
GO
/****** Object:  UserDefinedFunction [dbo].[ufnGetCustomerInformation]    Script Date: 10/07/2023 1:41:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufnGetCustomerInformation](@CustomerID int)
RETURNS TABLE
AS
-- Returns the CustomerID, first name, and last name for the specified customer.
RETURN (
    SELECT
        CustomerID,
        FirstName,
        LastName
    FROM [SalesLT].[Customer]
    WHERE [CustomerID] = @CustomerID
);
GO
/****** Object:  Table [dbo].[BillConfirm]    Script Date: 10/07/2023 1:41:14 AM ******/
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
/****** Object:  Table [dbo].[Booking]    Script Date: 10/07/2023 1:41:14 AM ******/
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
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Consignment_Contract]    Script Date: 10/07/2023 1:41:14 AM ******/
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
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Consignment_ContractImage]    Script Date: 10/07/2023 1:41:14 AM ******/
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
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EarnALiving_Contract]    Script Date: 10/07/2023 1:41:14 AM ******/
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
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EarnALiving_ContractImage]    Script Date: 10/07/2023 1:41:14 AM ******/
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
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LocalAddress]    Script Date: 10/07/2023 1:41:14 AM ******/
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
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Motorbike]    Script Date: 10/07/2023 1:41:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Motorbike](
	[motor_id] [int] IDENTITY(1,1) NOT NULL,
	[certificate_number] [nchar](6) NOT NULL,
	[motor_name] [nvarchar](200) NULL,
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
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotorbikeBrand]    Script Date: 10/07/2023 1:41:14 AM ******/
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
/****** Object:  Table [dbo].[MotorbikeImage]    Script Date: 10/07/2023 1:41:14 AM ******/
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
/****** Object:  Table [dbo].[MotorbikeModel]    Script Date: 10/07/2023 1:41:14 AM ******/
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
/****** Object:  Table [dbo].[MotorbikeStatus]    Script Date: 10/07/2023 1:41:14 AM ******/
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
/****** Object:  Table [dbo].[MotorbikeType]    Script Date: 10/07/2023 1:41:14 AM ******/
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
/****** Object:  Table [dbo].[Negotiation]    Script Date: 10/07/2023 1:41:14 AM ******/
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
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 10/07/2023 1:41:14 AM ******/
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
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationType]    Script Date: 10/07/2023 1:41:14 AM ******/
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
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 10/07/2023 1:41:14 AM ******/
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
/****** Object:  Table [dbo].[PointHistory]    Script Date: 10/07/2023 1:41:14 AM ******/
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
/****** Object:  Table [dbo].[PostBoosting]    Script Date: 10/07/2023 1:41:14 AM ******/
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
/****** Object:  Table [dbo].[Request]    Script Date: 10/07/2023 1:41:14 AM ******/
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
/****** Object:  Table [dbo].[RequestType]    Script Date: 10/07/2023 1:41:14 AM ******/
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
/****** Object:  Table [dbo].[Role]    Script Date: 10/07/2023 1:41:14 AM ******/
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
/****** Object:  Table [dbo].[StoreDesciption]    Script Date: 10/07/2023 1:41:14 AM ******/
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
	[tax_code] [nvarchar](13) NULL,
 CONSTRAINT [PK_StoreDesciption] PRIMARY KEY CLUSTERED 
(
	[store_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StoreImage]    Script Date: 10/07/2023 1:41:14 AM ******/
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
/****** Object:  Table [dbo].[User]    Script Date: 10/07/2023 1:41:14 AM ******/
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
	[verifycation_token_expires] [datetime] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wishlist]    Script Date: 10/07/2023 1:41:14 AM ******/
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
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Motorbike] ON 
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id]) VALUES (2005, N'233322', N'Kawasaki Ninja H2SX', 9, 23330, CAST(N'2020-09-09' AS Date), 200.0000, NULL, 1, 3, 1, 2)
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id]) VALUES (2006, N'765555', N'Vision 2023', 5, 7767, CAST(N'2023-09-01' AS Date), 38.9000, NULL, 1, 1, 1, 2)
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id]) VALUES (2007, N'845433', N'Wave RSX FI 110', 4, 23, CAST(N'2011-05-05' AS Date), 15.0000, NULL, 2, 2, NULL, 2)
GO
INSERT [dbo].[Motorbike] ([motor_id], [certificate_number], [motor_name], [model_id], [odo], [year], [price], [description], [motor_status_id], [motor_type_id], [store_id], [owner_id]) VALUES (2008, N'343333', N'Wave Alpha 110cc', 4, 187654, CAST(N'2022-02-02' AS Date), 17.9740, NULL, 2, 2, NULL, 2)
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
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (1, N'https://cdn.honda.com.vn/motorbike-strong-points/July2023/NyeJDaabXKegK3tRHzwy.png', 2008)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (2, N'https://cdn.honda.com.vn/motorbike-strong-points/July2023/z85orAXNqebWITiPOhD1.jpg', 2008)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (10, N'https://cloudfront-us-east-1.images.arcpublishing.com/octane/P4SQCQKVEBEWNKS3YF7NAIV7YA.jpg', 2005)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (17, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/95adfb55-2104-4a89-83f7-a439e98492b0.jpg', 2006)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (18, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/49256018-b351-4f10-85ba-faf701cf5d80.jpg', 2006)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (19, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/0ff77ef4-07dc-4641-b674-66c68ba012aa.jpg', 2007)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (20, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/eefdca29-7c88-4c46-91af-196927684d71.jpg', 2007)
GO
INSERT [dbo].[MotorbikeImage] ([image_id], [image_link], [motor_id]) VALUES (21, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/ea634a94-33a4-43d4-bf11-81dd5479aabc.jpg', 2007)
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
INSERT [dbo].[MotorbikeType] ([motorType_id], [title], [description], [status]) VALUES (1, N'Xe tay ga', NULL, N'ACTIVE')
GO
INSERT [dbo].[MotorbikeType] ([motorType_id], [title], [description], [status]) VALUES (2, N'Xe số', NULL, N'ACTIVE')
GO
INSERT [dbo].[MotorbikeType] ([motorType_id], [title], [description], [status]) VALUES (3, N'Xe côn tay', NULL, N'ACTIVE')
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
INSERT [dbo].[StoreDesciption] ([store_id], [user_id], [store_name], [description], [store_phone], [store_email], [store_created_at], [store_updated_at], [point], [address], [local_id], [status], [tax_code]) VALUES (1, 2, N'Cửa Hàng Xe Máy Cũ Thái Hòa', NULL, N'0369269410', N'nhutminh.itcontact@gmail.com', CAST(N'2023-10-05T09:48:10.753' AS DateTime), NULL, NULL, N'43 Đỗ Xuân Hợp, Phường Phước Long B, Thanh Phố Thủ Đức, Thành Phố Hồ Chí Minh.', NULL, N'ACTIVE', N'4545676789890')
GO
INSERT [dbo].[StoreDesciption] ([store_id], [user_id], [store_name], [description], [store_phone], [store_email], [store_created_at], [store_updated_at], [point], [address], [local_id], [status], [tax_code]) VALUES (2, 3, N'Cửa Hàng Xe Máy Cũ Đức Dũng II', NULL, N'0382694825', N'nhutminh.fpt@gmail.com', CAST(N'2023-10-05T09:54:20.193' AS DateTime), NULL, NULL, N'TL15 Ấp Phú An, Xã Phú Hòa Đông, Huyện Củ Chi, TP.HCM', NULL, N'ACTIVE', N'3434343456567')
GO
INSERT [dbo].[StoreDesciption] ([store_id], [user_id], [store_name], [description], [store_phone], [store_email], [store_created_at], [store_updated_at], [point], [address], [local_id], [status], [tax_code]) VALUES (3, 11, N'Xe Myas', NULL, N'4356789056', N'ee@gmail.com.vn', CAST(N'2023-10-06T16:03:26.767' AS DateTime), NULL, NULL, N'288/3 Man Thiên, phường Tăng Nhơn Phú A, Quận 9, Tp. Hồ Chí Minh.', NULL, N'REFUSE', N'4345453434')
GO
SET IDENTITY_INSERT [dbo].[StoreDesciption] OFF
GO
SET IDENTITY_INSERT [dbo].[StoreImage] ON 
GO
INSERT [dbo].[StoreImage] ([store_image_id], [image_link], [store_id]) VALUES (1, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/2f2d8b63-748c-4279-b4b9-f2920ade842a.jpg', 1)
GO
INSERT [dbo].[StoreImage] ([store_image_id], [image_link], [store_id]) VALUES (2, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/d748b374-3f46-485e-a319-024ce982717b.jpg', 2)
GO
INSERT [dbo].[StoreImage] ([store_image_id], [image_link], [store_id]) VALUES (3, N'https://motorbikeimages.blob.core.windows.net/motorbikebs/2978ade6-da40-41d9-8f1f-14d3481c5666.jpg', 3)
GO
SET IDENTITY_INSERT [dbo].[StoreImage] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (1, N'Nguyễn Tiến Phát', N'phat8a6@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2023-10-05' AS Date), NULL, N'ACTIVE', 0x1C51F0A3E77FD04869A3AD93C555E9822B3C2F3812AB93F0246B2C328FAA91588FD611F623AFFDD8CE2CED8366C6133A4AA4B1115240E1F2EBC7328632C4B763, 0x1A05AE7EF71C1D135CFBC01A60B29963B5E026E740218596265EFF691D2330E2F0AD13C07126252B687F721C3BAEB0734858FBB72C77718BC4D731C4794E273415879D4AA447012FBA60833D7CD8771A021117A2F870A1AADD2F8A5EAE504A4D0299DE653EAA4DF0C9D42BD88C9414785FBB9E56F0C2D5EBCBF68040008D6E8A, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (2, N'Nguyễn Nhựt Minh', N'nhutminh.it19@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 2, CAST(N'2023-10-05' AS Date), NULL, N'ACTIVE', 0xFC4D263550BA97B8775151A71E0D5088571892C7A99FECC77E204E600E759E81A4BFB02ECE2711241A4442A540A62AAD7AC48C6E5D0A635CE77EC236623384F8, 0x63AA969B866826952196CD5A4F129576D9B045B88B02B54CC22B8AEF50CABC3263A600490206171D834793AE3AE6F96DF03CBC133992B635E66F7F9AE3D6A2D4546DB146667D8602008EE111586880F61F0F983A7F09C35D1DFC3E699D1B20BD9B81B5B7A8DB0CC547A134CEADC655A6DE186E5DC4679B96AF7C1163D3D3F725, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (3, N'Minh Nguyễn', N'nhutminh.fpt@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 2, CAST(N'2023-10-05' AS Date), NULL, N'ACTIVE', 0x19C99A17A9C288D09BF82E36426AC62E591CD372618C854BE8C72FC95FB586EA886EBB61D6DA41E234AEF23BEA46D4C88B191A8434A64389F38A7A556475F210, 0x1FB24EF8B7AB1286242B4AA4CA00756509DDB0AD8453F128B904F5286A517AB9F73CEC059643A0E80C600ED2D2448F539EB6E1C2A46876EAA7843CC10269A938541ECDE45BF13B101611C4BDC426EA64285B7C04A1941DB6FD9DAC5830AF6F9536891D58170171201875197BF9C05EEA1D5777DF01D26F360A5B31D14D25F007, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (10, N'Minh Trí', N'phanminhtri269@gmail.com', N'0908660977', 1, CAST(N'2001-09-26' AS Date), N'123123123123', N'Quận 4, Hồ Chí Minh', NULL, 4, CAST(N'2023-10-05' AS Date), CAST(N'2023-10-05' AS Date), N'ACTIVE', 0xA585D4D64DFDB23C80658BA185A70CF6AAE8A53E78437CD394219CAC568837ED754795D418158B795B48B092164CCEDB15CF4709706E6D476E36590530E833E1, 0x6F607BE7009771AE4E9906491FA12B5DB502D738E8207702325DEB52262E85767BCEA9BCEEB6C6826FE88B6A6E9F7C2260A8C7FDB0768DE153729018B9DC5AD55F5B1D8AD1BE36C85F64ED6103CFAF863FA77E2CDD9BB834124B1A247DDAD18E0272683C206CDC51A3A6673D785F846BE98598E9675C8EC2E90FB474799FD07D, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (11, N'Minh Tri FPT', N'tripmse150151@fpt.edu.vn', N'0937500668', NULL, NULL, N'436785096712', N'22 Đường Âu Dương, Phường 3, Quận 8, TP.HCM', NULL, 4, CAST(N'2023-10-05' AS Date), NULL, N'ACTIVE', 0xB376591F2980D24DCE3229337D2799269090CE67CFEA18C27C713408272592456165A86C6279EBE31A5A3D78FBDEA5158DE197D05B6D65B2B056AE70F52AE51D, 0x3A3A7E5C89F4C42B1B7F62C9D9576CFB9EA08F1AF75D94CA01A22940ACFB81FDD3EBD5D8BFDAC0CA33E21F130ACCDF25AB73FD44FC3D91029E8651DA91665D65B2A531395AD927A78657B5E0BD400670ED54C49FCFDC421EE0A2DE5167255192B49D8D409673054FA577EC55E034FE238EDFA3DF696B0C4319273FB6838848E2, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (14, N'Nhut Minh', N'minhnnse150140@fpt.com.vn', NULL, NULL, NULL, NULL, NULL, NULL, 4, NULL, NULL, N'NOT VERIFY', 0xE324DC2B63CA0938F622C96FFE862F25C582400B1DE619F1AFBA065CCDDCDF0E8DB471A1D4FC8C4FE684CEB78E6587460183195498B151776C1CCB58186E00AB, 0xF2913D37CEE620C2180EABFB2D41BE176D33B98022948D9C4A1CFBE6CF12DC80576733EB3BAF83F7C979AC0550BFD57AFEFB27218439AA5E9A936D6B7332E089F59EE48A91CFC0F03F9F5B4AF2D1B9566C1BA748BDAFFF7D139B110EE27A2B531F541A9EC58D6D28578E86CAD01896E00D0EC728608E7C38B9DEBD2C0008F849, NULL, NULL, N'76DE90E1C3C4AF539B7E81C5257FDC7B7B31845A601626DCA89F27750F5F9460', CAST(N'2023-10-05T21:18:47.057' AS DateTime))
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (15, N'Trần Thành Công', N'tranthanhcong652001@gmail.com', N'0855562320', 1, CAST(N'2003-01-14' AS Date), N'545467894522', N'Bến Vân Đồn, Mỹ Tho, Tiền Giang', NULL, 3, CAST(N'2023-10-05' AS Date), CAST(N'2023-10-05' AS Date), N'ACTIVE', 0x9EDD41CC51B6790001988E63F3B9120BC2706FAE56A616B65F8A97621C8DE806F525BFF63A428E30BE3BEAF519A0830626E250614BD2EAEAC87E6151D40AE2BD, 0x61D44F72DFE5077B2D3E1944E3CD4DE2E26D00DEA6B322638FC40FAF5ED0C1947D4FA16D4625F6E127DCE970487CF3BFD91E737007926279C9945B9A1B28671C85C744170B24F3515A3A22427BAD40E86F95E2A9C9BBBEB2A772BE09481BDBE3DCA20747DED8EDB0C0CCAAC4FD57AF0C31BD98D193BC63459470DF3B66E9BEFF, NULL, NULL, N'', NULL)
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (16, N'HungStore', N'hung.trangquang071117@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 4, NULL, NULL, N'NOT VERIFY', 0x9428861020FB2586B4A932A1858BEAB8B03123971056466D90390CA4546CC52E23ABCAF3607663AE176B7806DA597BAA876FC3001B43620A129CEB584BF1797E, 0x591E4658B62BE1EF55AE2F4E9F299AA48D337C594C22843D70B8B380E4B2EC8E238A70FEBD1DECD48CBA341A4CB4C8516C5E49B1D67CAAAB1AA34750DA212122E16E67303A8AFB7CA8F569A015CDA28F1D620725A5B4D9DA3DF53519916AB88E7F613489E1D4F57D1860C0DE088532397B28F3727F8CDAD67E2B22C086AAE49E, NULL, NULL, N'0D8B65B62609373A9517117CCC0C060C5C57A8099E035CE821081698B4CF69D3', CAST(N'2023-10-07T01:03:56.623' AS DateTime))
GO
INSERT [dbo].[User] ([user_id], [user_name], [email], [phone], [gender], [dob], [idCard], [address], [local_id], [role_id], [user_verify_at], [user_updated_at], [status], [password_hash], [password_salt], [password_reset_token], [reset_token_expires], [verifycation_token], [verifycation_token_expires]) VALUES (17, N'Nhựt Minh Nguyễn', N'minhnnse150140@fpt.edu.vn', NULL, NULL, NULL, NULL, NULL, NULL, 4, CAST(N'2023-10-06' AS Date), NULL, N'ACTIVE', 0x36C102C6765BEB61FE222987B688086F101BDDFD3E74C77DCCA38FB84C53071C384071E7933F8A36E10E5BBD732CD6B77F667D6D9FC994D78C8508686886F43E, 0x68F9E7AE84F378708600804E3ABFA2872458B95CA033C46D6742C7F53C6A8C4D878CE51F4263082E25A82545D77E5C5444FF3BF7A851ED817A1CF7E7CAB4DBDA1934DFDF9C9BE29AF1234E0EA4621496068C805C363FE4B9F4A3D5108CDA9F7E62EAD2687A087E3D8989BECAC5FD46EBE11602758D31C034FD0C887F17DDE999, NULL, NULL, N'', NULL)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
/****** Object:  StoredProcedure [dbo].[uspLogError]    Script Date: 10/07/2023 1:41:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- uspLogError logs error information in the ErrorLog table about the
-- error that caused execution to jump to the CATCH block of a
-- TRY...CATCH construct. This should be executed from within the scope
-- of a CATCH block otherwise it will return without inserting error
-- information.
CREATE PROCEDURE [dbo].[uspLogError]
    @ErrorLogID int = 0 OUTPUT -- contains the ErrorLogID of the row inserted
AS                             -- by uspLogError in the ErrorLog table
BEGIN
    SET NOCOUNT ON;

    -- Output parameter value of 0 indicates that error
    -- information was not logged
    SET @ErrorLogID = 0;

    BEGIN TRY
        -- Return if there is no error information to log
        IF ERROR_NUMBER() IS NULL
            RETURN;

        -- Return if inside an uncommittable transaction.
        -- Data insertion/modification is not allowed when
        -- a transaction is in an uncommittable state.
        IF XACT_STATE() = -1
        BEGIN
            PRINT 'Cannot log error since the current transaction is in an uncommittable state. '
                + 'Rollback the transaction before executing uspLogError in order to successfully log error information.';
            RETURN;
        END

        INSERT [dbo].[ErrorLog]
            (
            [UserName],
            [ErrorNumber],
            [ErrorSeverity],
            [ErrorState],
            [ErrorProcedure],
            [ErrorLine],
            [ErrorMessage]
            )
        VALUES
            (
            CONVERT(sysname, CURRENT_USER),
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE()
            );

        -- Pass back the ErrorLogID of the row inserted
        SET @ErrorLogID = @@IDENTITY;
    END TRY
    BEGIN CATCH
        PRINT 'An error occurred in stored procedure uspLogError: ';
        EXECUTE [dbo].[uspPrintError];
        RETURN -1;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[uspPrintError]    Script Date: 10/07/2023 1:41:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- uspPrintError prints error information about the error that caused
-- execution to jump to the CATCH block of a TRY...CATCH construct.
-- Should be executed from within the scope of a CATCH block otherwise
-- it will return without printing any error information.
CREATE PROCEDURE [dbo].[uspPrintError]
AS
BEGIN
    SET NOCOUNT ON;

    -- Print error information.
    PRINT 'Error ' + CONVERT(varchar(50), ERROR_NUMBER()) +
          ', Severity ' + CONVERT(varchar(5), ERROR_SEVERITY()) +
          ', State ' + CONVERT(varchar(5), ERROR_STATE()) +
          ', Procedure ' + ISNULL(ERROR_PROCEDURE(), '-') +
          ', Line ' + CONVERT(varchar(5), ERROR_LINE());
    PRINT ERROR_MESSAGE();
END;
GO
ALTER DATABASE [motorbikebs-db] SET  READ_WRITE 
GO
