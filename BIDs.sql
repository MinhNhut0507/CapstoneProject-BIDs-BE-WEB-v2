USE [master]
GO
/****** Object:  Database [BIDsLocal]    Script Date: 8/22/2023 11:53:05 PM ******/
CREATE DATABASE [BIDsLocal]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BIDsLocal', FILENAME = N'D:\SQL\SQLSetUpServer\MSSQL16.SQLEXPRESS\MSSQL\DATA\BIDsLocal.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BIDsLocal_log', FILENAME = N'D:\SQL\SQLSetUpServer\MSSQL16.SQLEXPRESS\MSSQL\DATA\BIDsLocal_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [BIDsLocal] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BIDsLocal].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BIDsLocal] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BIDsLocal] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BIDsLocal] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BIDsLocal] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BIDsLocal] SET ARITHABORT OFF 
GO
ALTER DATABASE [BIDsLocal] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BIDsLocal] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BIDsLocal] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BIDsLocal] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BIDsLocal] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BIDsLocal] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BIDsLocal] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BIDsLocal] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BIDsLocal] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BIDsLocal] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BIDsLocal] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BIDsLocal] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BIDsLocal] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BIDsLocal] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BIDsLocal] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BIDsLocal] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BIDsLocal] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BIDsLocal] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BIDsLocal] SET  MULTI_USER 
GO
ALTER DATABASE [BIDsLocal] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BIDsLocal] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BIDsLocal] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BIDsLocal] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BIDsLocal] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BIDsLocal] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [BIDsLocal] SET QUERY_STORE = ON
GO
ALTER DATABASE [BIDsLocal] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [BIDsLocal]
GO
/****** Object:  Table [dbo].[Admin]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admin](
	[ID] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[Status] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BanHistory]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BanHistory](
	[ID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[Reason] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Status] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookingItem]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingItem](
	[ID] [uniqueidentifier] NOT NULL,
	[ItemID] [uniqueidentifier] NOT NULL,
	[StaffID] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Status] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Description]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Description](
	[ID] [uniqueidentifier] NOT NULL,
	[CategoryID] [uniqueidentifier] NOT NULL,
	[Status] [bit] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fee]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fee](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Min] [float] NOT NULL,
	[Max] [float] NOT NULL,
	[ParticipationFee] [float] NOT NULL,
	[DepositFee] [float] NOT NULL,
	[Surcharge] [float] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[Status] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Image]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Image](
	[ID] [uniqueidentifier] NOT NULL,
	[ItemID] [uniqueidentifier] NOT NULL,
	[DetailImage] [nvarchar](max) NOT NULL,
	[Status] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[ID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[CategoryID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DescriptionDetail] [nvarchar](max) NOT NULL,
	[Quantity] [int] NOT NULL,
	[StepPrice] [float] NOT NULL,
	[Deposit] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[FirstPrice] [float] NOT NULL,
	[AuctionTime] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemDescription]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemDescription](
	[ID] [uniqueidentifier] NOT NULL,
	[ItemID] [uniqueidentifier] NOT NULL,
	[DescriptionID] [uniqueidentifier] NOT NULL,
	[Detail] [nvarchar](50) NOT NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_ItemDescription] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[ID] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ExpireDate] [datetime] NOT NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationType]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentStaff]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentStaff](
	[ID] [uniqueidentifier] NOT NULL,
	[StaffID] [uniqueidentifier] NOT NULL,
	[Amount] [float] NOT NULL,
	[PaymentDate] [datetime] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[PayPalTransactionID] [nvarchar](max) NOT NULL,
	[PaymentDetails] [nvarchar](max) NOT NULL,
	[UserPaymentInformationID] [uniqueidentifier] NULL,
	[SessionID] [uniqueidentifier] NOT NULL,
	[PayPalRecieveAccount] [nvarchar](100) NULL,
 CONSTRAINT [PK_PaymentStaff] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentUser]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentUser](
	[ID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[Amount] [float] NOT NULL,
	[PaymentDate] [datetime] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[PayPalTransactionID] [nvarchar](max) NOT NULL,
	[PaymentDetails] [nvarchar](max) NOT NULL,
	[SessionID] [uniqueidentifier] NOT NULL,
	[PayPalAccount] [nvarchar](100) NULL,
 CONSTRAINT [PK_PaymentUser] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Session]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Session](
	[ID] [uniqueidentifier] NOT NULL,
	[FeeID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[BeginTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[ItemID] [uniqueidentifier] NOT NULL,
	[FinalPrice] [float] NOT NULL,
	[SessionRuleID] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SessionDetail]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SessionDetail](
	[UserID] [uniqueidentifier] NOT NULL,
	[SessionID] [uniqueidentifier] NOT NULL,
	[Price] [float] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Status] [bit] NOT NULL,
	[ID] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SessionRule]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SessionRule](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[FreeTime] [time](7) NOT NULL,
	[DelayTime] [time](7) NOT NULL,
	[DelayFreeTime] [time](7) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_SessionRule] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[ID] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[Status] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StaffNotificationDetail]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StaffNotificationDetail](
	[NotificationID] [uniqueidentifier] NOT NULL,
	[TypeID] [int] NOT NULL,
	[StaffID] [uniqueidentifier] NOT NULL,
	[Messages] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_StaffNotificationDetail] PRIMARY KEY CLUSTERED 
(
	[NotificationID] ASC,
	[TypeID] ASC,
	[StaffID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserNotificationDetail]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserNotificationDetail](
	[NotificationID] [uniqueidentifier] NOT NULL,
	[TypeID] [int] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[Messages] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_UserNotificationDetail] PRIMARY KEY CLUSTERED 
(
	[NotificationID] ASC,
	[TypeID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserPaymentInformation]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPaymentInformation](
	[ID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[PayPalAccount] [nvarchar](max) NOT NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_UserPaymentInfomation] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 8/22/2023 11:53:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [uniqueidentifier] NOT NULL,
	[Role] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Avatar] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[CCCDNumber] [nvarchar](20) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[CCCDFrontImage] [nvarchar](max) NOT NULL,
	[CCCDBackImage] [nvarchar](max) NOT NULL,
	[Status] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Admin] ([ID], [Email], [Name], [Password], [Address], [Phone], [Status]) VALUES (N'd25f5e0c-efb3-435e-8bea-8f4df5e308a5', N'seedadmin@gmail.com', N'Seed Admin', N'05072001', N'115/4/2 phường Trường Thọ quận Thủ Đức', N'0933403842', 1)
GO
INSERT [dbo].[BookingItem] ([ID], [ItemID], [StaffID], [CreateDate], [Status], [UpdateDate]) VALUES (N'6e6e22f7-1425-4b8e-9873-20aa59d1dd7a', N'13e57d05-6b33-418e-9e71-14ebc9700ca1', N'9b20198e-9c70-4f22-8714-52a7b6e034d8', CAST(N'2023-07-28T00:00:00.000' AS DateTime), 2, CAST(N'2023-07-28T00:00:00.000' AS DateTime))
INSERT [dbo].[BookingItem] ([ID], [ItemID], [StaffID], [CreateDate], [Status], [UpdateDate]) VALUES (N'5f44c399-4f8d-4d02-9759-44b02e2b2d61', N'6e6e22f7-1425-4b8e-9873-20aa59d1dd7a', N'9b20198e-9c70-4f22-8714-52a7b6e034d8', CAST(N'2023-07-28T00:00:00.000' AS DateTime), 2, CAST(N'2023-07-28T00:00:00.000' AS DateTime))
INSERT [dbo].[BookingItem] ([ID], [ItemID], [StaffID], [CreateDate], [Status], [UpdateDate]) VALUES (N'52f7a8c5-5e14-4e99-84cc-4540fe9ac56d', N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', N'36b9e6b1-97df-4fe6-bc10-2c5c163889ed', CAST(N'2023-07-31T21:13:41.883' AS DateTime), 2, CAST(N'2023-07-31T21:44:22.083' AS DateTime))
INSERT [dbo].[BookingItem] ([ID], [ItemID], [StaffID], [CreateDate], [Status], [UpdateDate]) VALUES (N'a3b04927-9eaf-4a3d-aedc-9d21630e1e0f', N'690ec794-d61b-4c64-bf11-2f0fbf3187e2', N'9b20198e-9c70-4f22-8714-52a7b6e034d8', CAST(N'2023-07-28T00:00:00.000' AS DateTime), 2, CAST(N'2023-07-28T00:00:00.000' AS DateTime))
INSERT [dbo].[BookingItem] ([ID], [ItemID], [StaffID], [CreateDate], [Status], [UpdateDate]) VALUES (N'd6543d42-9897-465d-a6d2-ee58a15837c7', N'8bf8833a-59aa-4ac5-a80d-cbbbc663dc09', N'9b20198e-9c70-4f22-8714-52a7b6e034d8', CAST(N'2023-07-28T00:00:00.000' AS DateTime), 2, CAST(N'2023-08-03T00:55:58.540' AS DateTime))
GO
INSERT [dbo].[Category] ([ID], [Name], [UpdateDate], [CreateDate], [Status]) VALUES (N'f345f47b-83c3-42f7-9f0d-2342fb4567b4', N'Đồ công nghệ', CAST(N'2023-07-03T01:32:18.013' AS DateTime), CAST(N'2023-07-03T01:32:18.013' AS DateTime), 1)
INSERT [dbo].[Category] ([ID], [Name], [UpdateDate], [CreateDate], [Status]) VALUES (N'890fca36-29ab-4a50-a99e-51f9d3db3ae2', N'Xe đạp', CAST(N'2023-07-03T01:32:18.013' AS DateTime), CAST(N'2023-07-03T01:32:18.013' AS DateTime), 1)
INSERT [dbo].[Category] ([ID], [Name], [UpdateDate], [CreateDate], [Status]) VALUES (N'7d534f13-1d22-4baf-9d67-590d3ae4d07d', N'Xe máy', CAST(N'2023-07-27T23:00:53.103' AS DateTime), CAST(N'2023-07-03T01:32:18.013' AS DateTime), 0)
INSERT [dbo].[Category] ([ID], [Name], [UpdateDate], [CreateDate], [Status]) VALUES (N'6d2e3f84-4e39-41e2-99eb-cba4de0d2e44', N'Đồ sưu tầm, đồ cổ', CAST(N'2023-07-03T01:32:18.017' AS DateTime), CAST(N'2023-07-03T01:32:18.017' AS DateTime), 1)
GO
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'6b29fc40-ca47-1067-b31d-00dd010662da', N'7d534f13-1d22-4baf-9d67-590d3ae4d07d', 1, N'Phân khối')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'3f2504e0-4f89-11d3-9a0c-0305e82c3301', N'7d534f13-1d22-4baf-9d67-590d3ae4d07d', 1, N'Tổng ODO')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'b0f5b80d-ff41-4b2f-84db-1530b6018e76', N'7d534f13-1d22-4baf-9d67-590d3ae4d07d', 1, N'Năm đăng ký')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'f6a6c63f-56a0-4164-979b-1c6a5f69c512', N'6d2e3f84-4e39-41e2-99eb-cba4de0d2e44', 1, N'Loại sản phẩm cụ thể')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'6340b24d-9257-4d4a-940f-2e34a9e42830', N'6d2e3f84-4e39-41e2-99eb-cba4de0d2e44', 1, N'Niên đại')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'7a41a429-ba57-4d34-92d6-48a12ad0c98c', N'f345f47b-83c3-42f7-9f0d-2342fb4567b4', 1, N'Chính sách bảo hành')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'8d91b4f0-8b59-4ea9-9e42-49c6dd7e098a', N'6d2e3f84-4e39-41e2-99eb-cba4de0d2e44', 1, N'Nhãn hiệu(Nếu có)')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'a8a48785-4ff0-42f9-ba19-4e3c85f963e4', N'f345f47b-83c3-42f7-9f0d-2342fb4567b4', 1, N'Sản phẩm cụ thể')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'c87ee0b6-96e1-4bbf-8e8e-4f09fe46a4a8', N'890fca36-29ab-4a50-a99e-51f9d3db3ae2', 1, N'Hãng xe')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'b20e94ce-05f6-41ff-b9de-4fc748dabce0', N'6d2e3f84-4e39-41e2-99eb-cba4de0d2e44', 1, N'Đánh giá mức độ hoàn hảo')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'7d534f13-1d22-4baf-9d67-590d3ae4d07d', N'7d534f13-1d22-4baf-9d67-590d3ae4d07d', 1, N'Loại xe')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'c0dd0b1d-74a2-4b45-ac1d-607cd9d22fed', N'7d534f13-1d22-4baf-9d67-590d3ae4d07d', 1, N'Chính sách bảo hành')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'3e6d3540-c9e9-493d-a01b-64a6c7b07f88', N'890fca36-29ab-4a50-a99e-51f9d3db3ae2', 1, N'Tình trạng')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'9053c700-35d1-4d46-9c5d-6e3288d6ce15', N'f345f47b-83c3-42f7-9f0d-2342fb4567b4', 1, N'Tình trạng')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'eeb65f33-9e54-48fe-bde8-84b2a397c285', N'890fca36-29ab-4a50-a99e-51f9d3db3ae2', 1, N'Loại xe đạp')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'8e9e6a68-3a45-4135-92d4-85d7ae4e7c82', N'7d534f13-1d22-4baf-9d67-590d3ae4d07d', 1, N'Hãng xe')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'2fb11c2d-2b3c-41a9-82c1-8e1a2445f5c3', N'f345f47b-83c3-42f7-9f0d-2342fb4567b4', 1, N'Màu sắc')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'e2d739f8-ef82-471a-b5a9-98dd3e3a976e', N'7d534f13-1d22-4baf-9d67-590d3ae4d07d', 1, N'Màu sắc')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'4b25a648-32c6-4a95-96d0-9cb02f2da01a', N'890fca36-29ab-4a50-a99e-51f9d3db3ae2', 1, N'Màu sắc')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'52bc7b17-3820-42e9-a775-aa729f2dd4ef', N'7d534f13-1d22-4baf-9d67-590d3ae4d07d', 1, N'Biển số')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'9a886674-cc6b-4a3f-bdaa-e3e6083e8c98', N'6d2e3f84-4e39-41e2-99eb-cba4de0d2e44', 1, N'Xuất xứ')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'b79a7df5-af17-4a75-8a4b-ef21c51e9d46', N'f345f47b-83c3-42f7-9f0d-2342fb4567b4', 1, N'Hãng')
GO
SET IDENTITY_INSERT [dbo].[Fee] ON 

INSERT [dbo].[Fee] ([ID], [Name], [Min], [Max], [ParticipationFee], [DepositFee], [Surcharge], [CreateDate], [UpdateDate], [Status]) VALUES (10, N'Phân khúc vừa và nhỏ', 1000000, 10000000, 0.005, 0, 0.1, CAST(N'2023-07-03T01:32:17.997' AS DateTime), CAST(N'2023-07-28T17:42:06.470' AS DateTime), 1)
INSERT [dbo].[Fee] ([ID], [Name], [Min], [Max], [ParticipationFee], [DepositFee], [Surcharge], [CreateDate], [UpdateDate], [Status]) VALUES (11, N'Phân khúc trung bình', 10000000, 30000000, 0.004, 0.15, 0.1, CAST(N'2023-07-03T01:32:18.010' AS DateTime), CAST(N'2023-07-03T01:32:18.010' AS DateTime), 1)
INSERT [dbo].[Fee] ([ID], [Name], [Min], [Max], [ParticipationFee], [DepositFee], [Surcharge], [CreateDate], [UpdateDate], [Status]) VALUES (12, N'Phân khúc cao cấp', 30000000, 1000000000, 0.003, 0.25, 0.1, CAST(N'2023-07-03T01:32:18.010' AS DateTime), CAST(N'2023-07-03T01:32:18.010' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Fee] OFF
GO
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'a8bd49b6-f192-4aec-9b77-026de2f1fe0b', N'6e6e22f7-1425-4b8e-9873-20aa59d1dd7a', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/samsung2-6HGC.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'c173dca6-1dfd-4613-80de-0bdb3808b29e', N'13e57d05-6b33-418e-9e71-14ebc9700ca1', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/dap4-2Qkb.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'3d9ec059-8513-4a0e-8aa6-10fd20041f1c', N'8bf8833a-59aa-4ac5-a80d-cbbbc663dc09', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/lenovo1-46L5.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'db4b0f51-3452-4c80-9fab-21301969801f', N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/zx3-5Xyj.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'06c6c517-44a4-4c16-a039-3d7e5c0e5bdf', N'690ec794-d61b-4c64-bf11-2f0fbf3187e2', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/am2-4bB5.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'0024eeef-d851-4c1e-9ba9-44c66f63258f', N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/zx2-6jGY.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'8449d1fa-0c30-4eed-84a6-4604bb59ca11', N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/zx1-5gSF.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'bec5a092-363d-4bf9-89cb-6d2caec0666a', N'6e6e22f7-1425-4b8e-9873-20aa59d1dd7a', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/samsung4-4RrZ.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'2962f9d9-f6db-41fb-b4c0-75c962066eac', N'13e57d05-6b33-418e-9e71-14ebc9700ca1', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/dap1-7PPe.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'7747d2d6-3515-4c75-9ef6-78a0a9d71743', N'6e6e22f7-1425-4b8e-9873-20aa59d1dd7a', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/samsung1-5GFA.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'f10aebaf-013c-4d16-9aa6-7a38b321e169', N'690ec794-d61b-4c64-bf11-2f0fbf3187e2', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/am4-71jx.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'61d273af-b094-4fe8-b1aa-b682bfc45d91', N'8bf8833a-59aa-4ac5-a80d-cbbbc663dc09', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/lenovo2-3onz.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'6f0a2a08-e726-4318-aa3b-bdcb58fbe3c3', N'690ec794-d61b-4c64-bf11-2f0fbf3187e2', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/am3-3eyF.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'41ccb9ba-1ae3-47c7-aec0-be75544b7371', N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/zx4-k9CE.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'22aa6395-6cdd-4a5f-9b57-d49673b3f98a', N'8bf8833a-59aa-4ac5-a80d-cbbbc663dc09', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/lenovo3-7QuL.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'60437660-74ba-4cda-8df2-d5de63798bec', N'13e57d05-6b33-418e-9e71-14ebc9700ca1', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/dap3-4FuY.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'2595a079-a4a1-40a4-a645-de2accc49a72', N'6e6e22f7-1425-4b8e-9873-20aa59d1dd7a', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/samsung3-72d6.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'a91e77ab-3cc2-4743-9eea-e480e8ee74ab', N'690ec794-d61b-4c64-bf11-2f0fbf3187e2', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/am1-4fpn.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'2c693333-3c53-4ef1-8d2a-ecd458f7e72a', N'13e57d05-6b33-418e-9e71-14ebc9700ca1', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/dap2-6RV9.jpg', 1)
INSERT [dbo].[Image] ([ID], [ItemID], [DetailImage], [Status]) VALUES (N'46c39c06-1ae7-4266-a285-f1ce3c7fe133', N'8bf8833a-59aa-4ac5-a80d-cbbbc663dc09', N'https://upcdn.io/kW15bZB/raw/uploads/2023/08/02/lenovo4-ouWT.jpg', 1)
GO
INSERT [dbo].[Item] ([ID], [UserID], [CategoryID], [Name], [DescriptionDetail], [Quantity], [StepPrice], [Deposit], [CreateDate], [UpdateDate], [FirstPrice], [AuctionTime]) VALUES (N'13e57d05-6b33-418e-9e71-14ebc9700ca1', N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'890fca36-29ab-4a50-a99e-51f9d3db3ae2', N'Xe đạp ASAMA', N'Xe đạp nhãn hiệu ASAMA nhỏ gọn màu lục bảo thích hợp cho các bạn học sinh đi học đi chơi', 1, 100000, 0, CAST(N'2023-07-27T00:00:00.000' AS DateTime), CAST(N'2023-07-27T00:00:00.000' AS DateTime), 1000000, 3)
INSERT [dbo].[Item] ([ID], [UserID], [CategoryID], [Name], [DescriptionDetail], [Quantity], [StepPrice], [Deposit], [CreateDate], [UpdateDate], [FirstPrice], [AuctionTime]) VALUES (N'6e6e22f7-1425-4b8e-9873-20aa59d1dd7a', N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'f345f47b-83c3-42f7-9f0d-2342fb4567b4', N'Điện thoại SAMSUNG JFLIP 4', N'Điện thoại SamSung JFlip 4 màn hình gập hiện đại cá tính nhỏ gọn cho nhân viên, sinh viên thuận lợi cho công việc và học tập', 1, 20000000, 1, CAST(N'2023-07-27T00:00:00.000' AS DateTime), CAST(N'2023-07-27T00:00:00.000' AS DateTime), 200000000, 3)
INSERT [dbo].[Item] ([ID], [UserID], [CategoryID], [Name], [DescriptionDetail], [Quantity], [StepPrice], [Deposit], [CreateDate], [UpdateDate], [FirstPrice], [AuctionTime]) VALUES (N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'7d534f13-1d22-4baf-9d67-590d3ae4d07d', N'Xe máy Kawasaki Ninja 400', N'Moto Kawasaki Ninja 400 có phân khối 400c màu xanh đặc trưng của hãng Kawasaki thích hợp cho những bạn đam mê tốc độ', 1, 10000000, 1, CAST(N'2023-07-27T00:00:00.000' AS DateTime), CAST(N'2023-07-27T00:00:00.000' AS DateTime), 150000000, 3)
INSERT [dbo].[Item] ([ID], [UserID], [CategoryID], [Name], [DescriptionDetail], [Quantity], [StepPrice], [Deposit], [CreateDate], [UpdateDate], [FirstPrice], [AuctionTime]) VALUES (N'690ec794-d61b-4c64-bf11-2f0fbf3187e2', N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'6d2e3f84-4e39-41e2-99eb-cba4de0d2e44', N'Ấm trà tử sa', N'Ấm trà tử sa của nghệ nhân được xuất sứ từ Vũ Hán với độ hoàn hảo 100% ', 1, 5000000, 1, CAST(N'2023-07-31T21:13:41.583' AS DateTime), CAST(N'2023-07-31T21:13:41.583' AS DateTime), 50000000, 3)
INSERT [dbo].[Item] ([ID], [UserID], [CategoryID], [Name], [DescriptionDetail], [Quantity], [StepPrice], [Deposit], [CreateDate], [UpdateDate], [FirstPrice], [AuctionTime]) VALUES (N'8bf8833a-59aa-4ac5-a80d-cbbbc663dc09', N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'f345f47b-83c3-42f7-9f0d-2342fb4567b4', N'Laptop LEGION', N'Laptop Legion sành điệu cho dân văn phòng và sinh viên đặc biệt phù hợp cho sinh viên khối ngành công nghệ', 1, 5000000, 1, CAST(N'2023-07-27T00:00:00.000' AS DateTime), CAST(N'2023-07-27T00:00:00.000' AS DateTime), 50000000, 3)
GO
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'b159a1bf-8ac7-43ec-a0f9-011eeba23094', N'6e6e22f7-1425-4b8e-9873-20aa59d1dd7a', N'7a41a429-ba57-4d34-92d6-48a12ad0c98c', N'Bảo hành hãng', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'775f7ca7-4b2e-4bbd-9786-11d25d713e8c', N'690ec794-d61b-4c64-bf11-2f0fbf3187e2', N'6340b24d-9257-4d4a-940f-2e34a9e42830', N'2011', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'7532ec91-ebe7-45a2-8f87-1abcb8686b93', N'6e6e22f7-1425-4b8e-9873-20aa59d1dd7a', N'2fb11c2d-2b3c-41a9-82c1-8e1a2445f5c3', N'Xanh Đen', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'3e72f409-17e3-4845-93bc-2436e141d9af', N'8bf8833a-59aa-4ac5-a80d-cbbbc663dc09', N'a8a48785-4ff0-42f9-ba19-4e3c85f963e4', N'Laptop', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'1cdd2e9e-adf5-4c7c-a969-243ec8ab0e7e', N'6e6e22f7-1425-4b8e-9873-20aa59d1dd7a', N'9053c700-35d1-4d46-9c5d-6e3288d6ce15', N'Mới 90%', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'3f7b1398-2f78-482a-8ab7-2bbf86a0a9f0', N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', N'e2d739f8-ef82-471a-b5a9-98dd3e3a976e', N'Xanh lá', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'6bdfc728-72a6-4d82-ac14-351d212235ad', N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', N'b0f5b80d-ff41-4b2f-84db-1530b6018e76', N'2022', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'10e1c8cd-9486-481d-b1ef-36ee3451934a', N'6e6e22f7-1425-4b8e-9873-20aa59d1dd7a', N'b79a7df5-af17-4a75-8a4b-ef21c51e9d46', N'Samsung', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'baaddd58-c588-4cd6-88af-3ac7f60e75b5', N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', N'8e9e6a68-3a45-4135-92d4-85d7ae4e7c82', N'Kawasaki', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'7f67014b-bdc9-49d8-bd77-42088f86fb92', N'8bf8833a-59aa-4ac5-a80d-cbbbc663dc09', N'b79a7df5-af17-4a75-8a4b-ef21c51e9d46', N'Legion', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'ecfc77f6-c65b-4c8f-9d6d-7a052cd06616', N'690ec794-d61b-4c64-bf11-2f0fbf3187e2', N'8d91b4f0-8b59-4ea9-9e42-49c6dd7e098a', N'Không', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'77cdc01f-b81f-4dab-9e2a-80938b200024', N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', N'c0dd0b1d-74a2-4b45-ac1d-607cd9d22fed', N'Bảo hành hãng', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'3e64152b-c3a1-4bb4-8403-83e2d75e8c60', N'690ec794-d61b-4c64-bf11-2f0fbf3187e2', N'b20e94ce-05f6-41ff-b9de-4fc748dabce0', N'Hoàn hảo 100%', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'c3d920fb-b317-4d7f-9f8f-8aac48f0939e', N'6e6e22f7-1425-4b8e-9873-20aa59d1dd7a', N'a8a48785-4ff0-42f9-ba19-4e3c85f963e4', N'Điện thoại Samsung JFlip 4', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'6ff9850b-f086-4b60-b86b-b03ac8e83b17', N'13e57d05-6b33-418e-9e71-14ebc9700ca1', N'c87ee0b6-96e1-4bbf-8e8e-4f09fe46a4a8', N'Asama', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'410ae4cf-7d9f-4837-8cda-b42c5103c50a', N'8bf8833a-59aa-4ac5-a80d-cbbbc663dc09', N'7a41a429-ba57-4d34-92d6-48a12ad0c98c', N'Bảo hành hãng', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'a6a49088-8114-4c08-b4fb-bb1302b80a12', N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', N'7d534f13-1d22-4baf-9d67-590d3ae4d07d', N'Moto phân khối lớn', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'43866d84-3875-4750-b738-bd13c190a855', N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', N'3f2504e0-4f89-11d3-9a0c-0305e82c3301', N'10000Km', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'488c66ff-c9de-4416-ad7f-d65125f01362', N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', N'52bc7b17-3820-42e9-a775-aa729f2dd4ef', N'72K1-521.89', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'e24ab980-7703-4fc6-a1a4-df257a2ab59b', N'8bf8833a-59aa-4ac5-a80d-cbbbc663dc09', N'9053c700-35d1-4d46-9c5d-6e3288d6ce15', N'Mới 100%', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'fa7de605-d76c-45e9-95e1-e4cc93e9c7f8', N'690ec794-d61b-4c64-bf11-2f0fbf3187e2', N'9a886674-cc6b-4a3f-bdaa-e3e6083e8c98', N'Vũ Hán Trung Quốc', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'd2de2ba1-8c48-432e-ad19-f084ddc64b2a', N'13e57d05-6b33-418e-9e71-14ebc9700ca1', N'eeb65f33-9e54-48fe-bde8-84b2a397c285', N'Xe đạp cho học sinh', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'71b34c65-672d-471b-a060-f26b63f6c385', N'13e57d05-6b33-418e-9e71-14ebc9700ca1', N'3e6d3540-c9e9-493d-a01b-64a6c7b07f88', N'Đang sử dụng', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'a750881f-0a3c-414b-bdc6-f3e93af1ffbb', N'690ec794-d61b-4c64-bf11-2f0fbf3187e2', N'f6a6c63f-56a0-4164-979b-1c6a5f69c512', N'Ấm trà tử sa', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'8fafd3d5-b37b-4c19-b1d4-f845e60cc6ec', N'8bf8833a-59aa-4ac5-a80d-cbbbc663dc09', N'2fb11c2d-2b3c-41a9-82c1-8e1a2445f5c3', N'Đen bóng', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'25b794b9-fb7e-4a3b-9a09-fd2dee98b3f8', N'13e57d05-6b33-418e-9e71-14ebc9700ca1', N'4b25a648-32c6-4a95-96d0-9cb02f2da01a', N'Xanh đại dương', 1)
INSERT [dbo].[ItemDescription] ([ID], [ItemID], [DescriptionID], [Detail], [Status]) VALUES (N'9584f324-2afd-4022-9ada-ff323075a12c', N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', N'6b29fc40-ca47-1067-b31d-00dd010662da', N'400cc', 1)
GO
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'3b44229c-0272-4e76-a0a3-11166663502f', CAST(N'2023-08-05T02:08:20.940' AS DateTime), CAST(N'2023-08-15T02:08:20.940' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'67bf4762-3e38-429c-ab3f-3abb8daee2a7', CAST(N'2023-08-05T02:26:29.637' AS DateTime), CAST(N'2023-08-15T02:26:29.637' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'b08e5ba6-3911-43f4-a8ee-3f820458979d', CAST(N'2023-08-05T02:24:30.227' AS DateTime), CAST(N'2023-08-15T02:24:30.227' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'1bbd47f3-13ec-4ac9-9957-47940ef18cd5', CAST(N'2023-08-04T23:05:50.593' AS DateTime), CAST(N'2023-08-14T23:05:50.593' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'4f9ab3ae-685c-4ce3-a0c5-557a9bf989fa', CAST(N'2023-08-04T23:56:38.773' AS DateTime), CAST(N'2023-08-14T23:56:38.773' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'6108fded-db92-4717-be2d-696acd6b3f1f', CAST(N'2023-08-04T23:50:58.993' AS DateTime), CAST(N'2023-08-14T23:50:58.993' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'6545391e-f41a-40ff-aada-6c3d51d47fe1', CAST(N'2023-08-05T00:12:17.370' AS DateTime), CAST(N'2023-08-15T00:12:17.370' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'feda4a74-21d1-44ed-af23-7357ba5a23ce', CAST(N'2023-08-04T23:40:07.730' AS DateTime), CAST(N'2023-08-14T23:40:07.730' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'0c199ff0-8e74-4fe5-b251-74a4646fa9cb', CAST(N'2023-08-04T19:40:08.573' AS DateTime), CAST(N'2023-08-14T19:40:08.573' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'fca3164f-c2fc-42f9-b063-7cde4a106e5a', CAST(N'2023-08-05T02:33:24.123' AS DateTime), CAST(N'2023-08-15T02:33:24.123' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'01e1c8ca-1ed9-4d75-9c2e-7eb3e9f78545', CAST(N'2023-08-05T02:33:20.507' AS DateTime), CAST(N'2023-08-15T02:33:20.507' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'18e9391e-1924-48bd-904d-8efc2401495f', CAST(N'2023-08-04T23:05:43.837' AS DateTime), CAST(N'2023-08-14T23:05:43.837' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'df717e58-38d3-43a3-b48e-932548e5cf58', CAST(N'2023-08-05T00:06:50.620' AS DateTime), CAST(N'2023-08-15T00:06:50.620' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'1d1b5142-924b-41c1-beca-9585af61a2cb', CAST(N'2023-08-05T02:08:24.110' AS DateTime), CAST(N'2023-08-15T02:08:24.110' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'b8641f1b-bbbf-4995-a6b1-9edbd768177a', CAST(N'2023-08-05T00:11:50.660' AS DateTime), CAST(N'2023-08-15T00:11:50.660' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'fdc06d11-ad6a-4f2e-96cc-a38c21b9c5e4', CAST(N'2023-08-04T23:09:11.753' AS DateTime), CAST(N'2023-08-14T23:09:11.753' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'6b169926-b9a2-4fda-97b8-ac7f4c3af605', CAST(N'2023-08-04T23:54:29.430' AS DateTime), CAST(N'2023-08-14T23:54:29.430' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'b62c86f5-7f88-4609-8f82-b9530c783ae3', CAST(N'2023-08-04T19:35:16.983' AS DateTime), CAST(N'2023-08-14T19:35:16.983' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'a9e581be-f581-47f8-9047-bdba6311303d', CAST(N'2023-08-05T02:26:49.780' AS DateTime), CAST(N'2023-08-15T02:26:49.780' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'4db5033a-fe54-45a7-b125-c18ad71c2793', CAST(N'2023-08-04T23:10:20.617' AS DateTime), CAST(N'2023-08-14T23:10:20.617' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'7a6d3b24-934e-4ac7-9138-c895c8effcbb', CAST(N'2023-08-04T22:37:40.630' AS DateTime), CAST(N'2023-08-14T22:37:40.630' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'2976889b-2f1c-402f-9449-d3a0555ee1f8', CAST(N'2023-08-18T00:52:34.883' AS DateTime), CAST(N'2023-08-28T00:52:34.883' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'11d749c9-d141-40fc-874f-e07a8aec6a4e', CAST(N'2023-08-05T00:07:15.440' AS DateTime), CAST(N'2023-08-15T00:07:15.440' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'd2c6656a-9e89-401c-b219-eabaa0cc66a7', CAST(N'2023-08-04T23:09:14.707' AS DateTime), CAST(N'2023-08-14T23:09:14.707' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [CreateDate], [ExpireDate], [Status]) VALUES (N'02068573-8845-4214-bda7-edd9de833b55', CAST(N'2023-08-04T23:10:24.050' AS DateTime), CAST(N'2023-08-14T23:10:24.050' AS DateTime), 1)
GO
SET IDENTITY_INSERT [dbo].[NotificationType] ON 

INSERT [dbo].[NotificationType] ([ID], [Name]) VALUES (1, N'Tài Khoản')
INSERT [dbo].[NotificationType] ([ID], [Name]) VALUES (2, N'Sản Phẩm')
INSERT [dbo].[NotificationType] ([ID], [Name]) VALUES (3, N'Đơn Sản Phẩm')
INSERT [dbo].[NotificationType] ([ID], [Name]) VALUES (4, N'Phiên Đấu Giá')
INSERT [dbo].[NotificationType] ([ID], [Name]) VALUES (5, N'Thanh Toán')
SET IDENTITY_INSERT [dbo].[NotificationType] OFF
GO
INSERT [dbo].[PaymentStaff] ([ID], [StaffID], [Amount], [PaymentDate], [Status], [PayPalTransactionID], [PaymentDetails], [UserPaymentInformationID], [SessionID], [PayPalRecieveAccount]) VALUES (N'211ddb2d-1bb7-4927-8a55-30ffe51a0e0f', N'36b9e6b1-97df-4fe6-bc10-2c5c163889ed', 20000000, CAST(N'2023-08-08T00:46:50.240' AS DateTime), N'OK', N'4U369758TB964334K', N'Hoàn trả phí đặt cọc cho sản phẩm đấu giá Laptop LEGION với số tiền hoàn trả là 20000000.', N'6fcb17a2-2185-4b1b-a7b3-8f8d82e17be2', N'17a3e8bb-0dd2-4d9c-afa1-a46ed50b2747', NULL)
GO
INSERT [dbo].[PaymentUser] ([ID], [UserID], [Amount], [PaymentDate], [Status], [PayPalTransactionID], [PaymentDetails], [SessionID], [PayPalAccount]) VALUES (N'7205c785-8c85-4ac2-8f1a-00841f602277', N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', 50000000, CAST(N'2023-08-08T23:42:20.687' AS DateTime), N'CREATED', N'2YP23203MX487644U', N'Thanh toán phí tham gia và phí đặt cọc của sản phẩm Ấm trà tử sa với giá là 532.88.', N'3f5da238-1eb4-4a1c-a1ea-7c1359536e35', NULL)
INSERT [dbo].[PaymentUser] ([ID], [UserID], [Amount], [PaymentDate], [Status], [PayPalTransactionID], [PaymentDetails], [SessionID], [PayPalAccount]) VALUES (N'f809dba3-951a-496a-a11a-3c5939f6b8ed', N'f47ac10b-58cc-4372-a567-0e02b2c3d479', 60000000, CAST(N'2023-08-07T17:09:18.597' AS DateTime), N'APPROVED', N'7PY30846F54629330', N'Thanh toán sản phẩm Laptop LEGION với giá là 60000000.', N'17a3e8bb-0dd2-4d9c-afa1-a46ed50b2747', NULL)
INSERT [dbo].[PaymentUser] ([ID], [UserID], [Amount], [PaymentDate], [Status], [PayPalTransactionID], [PaymentDetails], [SessionID], [PayPalAccount]) VALUES (N'2cda3f2c-ae6a-4cb4-baae-607f4d8a3504', N'f47ac10b-58cc-4372-a567-0e02b2c3d479', 60000000, CAST(N'2023-08-22T22:56:02.607' AS DateTime), N'CREATED', N'7MY04840TE6765246', N'Thanh toán sản phẩm Laptop LEGION với giá là 60000000.', N'17a3e8bb-0dd2-4d9c-afa1-a46ed50b2747', NULL)
INSERT [dbo].[PaymentUser] ([ID], [UserID], [Amount], [PaymentDate], [Status], [PayPalTransactionID], [PaymentDetails], [SessionID], [PayPalAccount]) VALUES (N'743ea13f-58be-4544-bfed-69c2ff701b93', N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', 1000000, CAST(N'2023-08-10T00:49:39.020' AS DateTime), N'APPROVED', N'7DR649339Y4216820', N'Thanh toán sản phẩm Xe đạp ASAMA với giá là 1000000.', N'2f463cee-244c-4222-9338-2b25ea440dfb', NULL)
INSERT [dbo].[PaymentUser] ([ID], [UserID], [Amount], [PaymentDate], [Status], [PayPalTransactionID], [PaymentDetails], [SessionID], [PayPalAccount]) VALUES (N'947c3076-304e-4c0a-baea-a175e4bb96b1', N'f47ac10b-58cc-4372-a567-0e02b2c3d479', 60000000, CAST(N'2023-08-22T23:39:12.973' AS DateTime), N'APPROVED', N'8R3748677G146360H', N'Thanh toán sản phẩm Laptop LEGION với giá là 60000000.', N'17a3e8bb-0dd2-4d9c-afa1-a46ed50b2747', N'minhnhutbid@gmail.com')
INSERT [dbo].[PaymentUser] ([ID], [UserID], [Amount], [PaymentDate], [Status], [PayPalTransactionID], [PaymentDetails], [SessionID], [PayPalAccount]) VALUES (N'7bf61b5f-5b4d-44fb-b7d4-c670f5c769eb', N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', 50000000, CAST(N'2023-08-08T23:41:14.213' AS DateTime), N'CREATED', N'00U9307202145554L', N'Thanh toán sản phẩm Ấm trà tử sa với giá là 50000000.', N'3f5da238-1eb4-4a1c-a1ea-7c1359536e35', NULL)
INSERT [dbo].[PaymentUser] ([ID], [UserID], [Amount], [PaymentDate], [Status], [PayPalTransactionID], [PaymentDetails], [SessionID], [PayPalAccount]) VALUES (N'11ab8f25-d6cc-4bf6-9168-e6adff04fd8f', N'f47ac10b-58cc-4372-a567-0e02b2c3d479', 200000000, CAST(N'2023-08-10T22:23:22.233' AS DateTime), N'CREATED', N'79K84915NS923013F', N'Thanh toán phí tham gia và phí đặt cọc của sản phẩm Điện thoại SAMSUNG JFLIP 4 với giá là 1271.72.', N'0e3a07d7-e1f8-4973-a4e0-2b27e20285e5', NULL)
GO
INSERT [dbo].[Session] ([ID], [FeeID], [Name], [BeginTime], [EndTime], [UpdateDate], [CreateDate], [Status], [ItemID], [FinalPrice], [SessionRuleID]) VALUES (N'2f463cee-244c-4222-9338-2b25ea440dfb', 10, N'Xe đạp Asama', CAST(N'2023-09-01T14:00:00.000' AS DateTime), CAST(N'2023-08-10T00:00:00.000' AS DateTime), CAST(N'2023-08-10T00:50:54.177' AS DateTime), CAST(N'2023-07-31T21:44:21.777' AS DateTime), 4, N'13e57d05-6b33-418e-9e71-14ebc9700ca1', 1000000, N'b7370eb2-3fff-4b91-f31b-08db7d91cd0c')
INSERT [dbo].[Session] ([ID], [FeeID], [Name], [BeginTime], [EndTime], [UpdateDate], [CreateDate], [Status], [ItemID], [FinalPrice], [SessionRuleID]) VALUES (N'0e3a07d7-e1f8-4973-a4e0-2b27e20285e5', 11, N'Điện thoại SAMSUNG JFLIP 4', CAST(N'2023-09-10T12:00:00.000' AS DateTime), CAST(N'2023-09-10T18:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), 1, N'6e6e22f7-1425-4b8e-9873-20aa59d1dd7a', 200000000, N'b7370eb2-3fff-4b91-f31b-08db7d91cd0c')
INSERT [dbo].[Session] ([ID], [FeeID], [Name], [BeginTime], [EndTime], [UpdateDate], [CreateDate], [Status], [ItemID], [FinalPrice], [SessionRuleID]) VALUES (N'7e39a215-cf19-4d71-8c58-66be8b4be30a', 12, N'Xe máy Kawasaki Ninja 400', CAST(N'2023-09-10T12:00:00.000' AS DateTime), CAST(N'2023-09-10T18:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), 1, N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', 150000000, N'b7370eb2-3fff-4b91-f31b-08db7d91cd0c')
INSERT [dbo].[Session] ([ID], [FeeID], [Name], [BeginTime], [EndTime], [UpdateDate], [CreateDate], [Status], [ItemID], [FinalPrice], [SessionRuleID]) VALUES (N'3f5da238-1eb4-4a1c-a1ea-7c1359536e35', 12, N'Ấm trà tử sa', CAST(N'2023-08-02T12:00:00.000' AS DateTime), CAST(N'2023-08-10T00:00:00.000' AS DateTime), CAST(N'2023-08-09T11:43:53.403' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), 3, N'690ec794-d61b-4c64-bf11-2f0fbf3187e2', 50000000, N'b7370eb2-3fff-4b91-f31b-08db7d91cd0c')
INSERT [dbo].[Session] ([ID], [FeeID], [Name], [BeginTime], [EndTime], [UpdateDate], [CreateDate], [Status], [ItemID], [FinalPrice], [SessionRuleID]) VALUES (N'17a3e8bb-0dd2-4d9c-afa1-a46ed50b2747', 12, N'Laptop LEGION', CAST(N'2023-08-02T09:30:00.000' AS DateTime), CAST(N'2023-08-10T00:00:00.000' AS DateTime), CAST(N'2023-08-22T23:50:42.353' AS DateTime), CAST(N'2023-08-03T00:55:58.057' AS DateTime), 4, N'8bf8833a-59aa-4ac5-a80d-cbbbc663dc09', 60000000, N'139b145a-f4d6-41b8-091e-08db8eb6abf3')
GO
INSERT [dbo].[SessionDetail] ([UserID], [SessionID], [Price], [CreateDate], [Status], [ID]) VALUES (N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'2f463cee-244c-4222-9338-2b25ea440dfb', 4000000, CAST(N'2023-08-08T10:00:00.000' AS DateTime), 1, N'c04125a6-7898-4c1f-a9f0-1d7b6eac48e9')
INSERT [dbo].[SessionDetail] ([UserID], [SessionID], [Price], [CreateDate], [Status], [ID]) VALUES (N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'2f463cee-244c-4222-9338-2b25ea440dfb', 5000000, CAST(N'2023-08-09T00:00:00.000' AS DateTime), 1, N'37d8f8c1-9eab-4d4a-8b1e-2e9829f63e68')
INSERT [dbo].[SessionDetail] ([UserID], [SessionID], [Price], [CreateDate], [Status], [ID]) VALUES (N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'3f5da238-1eb4-4a1c-a1ea-7c1359536e35', 50000000, CAST(N'2023-08-03T12:00:00.000' AS DateTime), 1, N'a3c8b33e-9f1d-48e8-af1b-30e417e13f62')
INSERT [dbo].[SessionDetail] ([UserID], [SessionID], [Price], [CreateDate], [Status], [ID]) VALUES (N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'17a3e8bb-0dd2-4d9c-afa1-a46ed50b2747', 55000000, CAST(N'2023-08-02T09:30:00.000' AS DateTime), 1, N'd4b93780-68d0-41dd-8c75-542c88b20b89')
INSERT [dbo].[SessionDetail] ([UserID], [SessionID], [Price], [CreateDate], [Status], [ID]) VALUES (N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'2f463cee-244c-4222-9338-2b25ea440dfb', 4200000, CAST(N'2023-08-08T10:00:00.000' AS DateTime), 1, N'd3fe9c7d-560d-46ca-bec1-7838498d4a5b')
INSERT [dbo].[SessionDetail] ([UserID], [SessionID], [Price], [CreateDate], [Status], [ID]) VALUES (N'f47ac10b-58cc-4372-a567-0e02b2c3d479', N'17a3e8bb-0dd2-4d9c-afa1-a46ed50b2747', 60000000, CAST(N'2023-08-02T09:30:00.000' AS DateTime), 1, N'eab68a26-5f21-4bf5-92a3-83793e18a59f')
INSERT [dbo].[SessionDetail] ([UserID], [SessionID], [Price], [CreateDate], [Status], [ID]) VALUES (N'f47ac10b-58cc-4372-a567-0e02b2c3d479', N'3f5da238-1eb4-4a1c-a1ea-7c1359536e35', 50000000, CAST(N'2023-08-02T12:00:00.000' AS DateTime), 1, N'6e22ef85-7a9d-4e24-af4f-8d30b5e58ec1')
INSERT [dbo].[SessionDetail] ([UserID], [SessionID], [Price], [CreateDate], [Status], [ID]) VALUES (N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'2f463cee-244c-4222-9338-2b25ea440dfb', 3000000, CAST(N'2023-08-08T00:00:00.000' AS DateTime), 1, N'af6e701f-e524-4d79-99d7-982ee2440dcb')
INSERT [dbo].[SessionDetail] ([UserID], [SessionID], [Price], [CreateDate], [Status], [ID]) VALUES (N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'2f463cee-244c-4222-9338-2b25ea440dfb', 4100000, CAST(N'2023-08-08T10:00:00.000' AS DateTime), 1, N'8b9d6cf2-2e77-45ea-b9c0-c1c4dbaa5f6a')
GO
INSERT [dbo].[SessionRule] ([ID], [Name], [FreeTime], [DelayTime], [DelayFreeTime], [CreateDate], [UpdateDate], [Status]) VALUES (N'b7370eb2-3fff-4b91-f31b-08db7d91cd0c', N'Luật Chính Thức', CAST(N'00:05:00' AS Time), CAST(N'00:10:00' AS Time), CAST(N'00:00:15' AS Time), CAST(N'2023-07-06T02:55:35.100' AS DateTime), CAST(N'2023-07-06T02:55:35.097' AS DateTime), 1)
INSERT [dbo].[SessionRule] ([ID], [Name], [FreeTime], [DelayTime], [DelayFreeTime], [CreateDate], [UpdateDate], [Status]) VALUES (N'139b145a-f4d6-41b8-091e-08db8eb6abf3', N'Luật testing', CAST(N'00:10:00' AS Time), CAST(N'00:00:05' AS Time), CAST(N'00:00:03' AS Time), CAST(N'2023-07-27T22:32:22.270' AS DateTime), CAST(N'2023-07-27T22:32:22.270' AS DateTime), 1)
INSERT [dbo].[SessionRule] ([ID], [Name], [FreeTime], [DelayTime], [DelayFreeTime], [CreateDate], [UpdateDate], [Status]) VALUES (N'786b919b-6aa2-4fea-287f-08db96a3ef29', N'test', CAST(N'00:10:00' AS Time), CAST(N'00:00:00' AS Time), CAST(N'00:00:05' AS Time), CAST(N'2023-08-07T00:38:23.887' AS DateTime), CAST(N'2023-08-07T00:38:23.887' AS DateTime), 1)
GO
INSERT [dbo].[Staff] ([ID], [Email], [Name], [Password], [Address], [Phone], [DateOfBirth], [CreateDate], [UpdateDate], [Status]) VALUES (N'36b9e6b1-97df-4fe6-bc10-2c5c163889ed', N'seedstaff1@gmail.com', N'Seed Staff 1', N'05072001', N'115/4/2 phường Trường Thọ quận Thủ Đức', N'0933403842', CAST(N'2001-07-05T00:00:00.000' AS DateTime), CAST(N'2023-07-03T01:32:01.580' AS DateTime), CAST(N'2023-07-03T01:32:01.580' AS DateTime), 1)
INSERT [dbo].[Staff] ([ID], [Email], [Name], [Password], [Address], [Phone], [DateOfBirth], [CreateDate], [UpdateDate], [Status]) VALUES (N'9b20198e-9c70-4f22-8714-52a7b6e034d8', N'seedstaff@gmail.com', N'Seed Staff', N'05072001', N'115/4/2 phường Trường Thọ quận Thủ Đức', N'0933403842', CAST(N'2001-07-05T00:00:00.000' AS DateTime), CAST(N'2023-07-03T01:32:01.580' AS DateTime), CAST(N'2023-07-03T01:32:01.580' AS DateTime), 1)
GO
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'3b44229c-0272-4e76-a0a3-11166663502f', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Ấm trà tử sa của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là minhnhut05072001@gmail.com với mức giá cuối cùng được đưa ra là 50000000. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'67bf4762-3e38-429c-ab3f-3abb8daee2a7', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Ấm trà tử sa của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là nhutdmse151298@fpt.edu.vn với mức giá cuối cùng được đưa ra là 50000000. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'b08e5ba6-3911-43f4-a8ee-3f820458979d', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Ấm trà tử sa của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là nhutdmse151298@fpt.edu.vn với mức giá cuối cùng được đưa ra là 50000000. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'1bbd47f3-13ec-4ac9-9957-47940ef18cd5', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Laptop LEGION của bạn đã bắt đầu. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'4f9ab3ae-685c-4ce3-a0c5-557a9bf989fa', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Ấm trà tử sa của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là minhnhut05072001@gmail.com với mức giá cuối cùng được đưa ra là 50000000. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'6108fded-db92-4717-be2d-696acd6b3f1f', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Ấm trà tử sa của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là minhnhut05072001@gmail.com với mức giá cuối cùng được đưa ra là 50000000. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'6545391e-f41a-40ff-aada-6c3d51d47fe1', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Laptop LEGION của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là minhnhut05072001@gmail.com với mức giá cuối cùng được đưa ra là 50000000. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'feda4a74-21d1-44ed-af23-7357ba5a23ce', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Ấm trà tử sa của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là minhnhut05072001@gmail.com với mức giá cuối cùng được đưa ra là 50000000. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'0c199ff0-8e74-4fe5-b251-74a4646fa9cb', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Laptop LEGION của bạn đã bắt đầu. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'fca3164f-c2fc-42f9-b063-7cde4a106e5a', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Laptop LEGION của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là nhutdmse151298@fpt.edu.vn với mức giá cuối cùng được đưa ra là 60000000. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'01e1c8ca-1ed9-4d75-9c2e-7eb3e9f78545', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Ấm trà tử sa của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là nhutdmse151298@fpt.edu.vn với mức giá cuối cùng được đưa ra là 50000000. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'18e9391e-1924-48bd-904d-8efc2401495f', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Ấm trà tử sa của bạn đã bắt đầu. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'df717e58-38d3-43a3-b48e-932548e5cf58', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Ấm trà tử sa của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là minhnhut05072001@gmail.com với mức giá cuối cùng được đưa ra là 50000000. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'1d1b5142-924b-41c1-beca-9585af61a2cb', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Laptop LEGION của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là minhnhut05072001@gmail.com với mức giá cuối cùng được đưa ra là 60000000. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'b8641f1b-bbbf-4995-a6b1-9edbd768177a', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Ấm trà tử sa của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là minhnhut05072001@gmail.com với mức giá cuối cùng được đưa ra là 50000000. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'fdc06d11-ad6a-4f2e-96cc-a38c21b9c5e4', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Ấm trà tử sa của bạn đã bắt đầu. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'6b169926-b9a2-4fda-97b8-ac7f4c3af605', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Laptop LEGION của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là minhnhut05072001@gmail.com với mức giá cuối cùng được đưa ra là 50000000. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'b62c86f5-7f88-4609-8f82-b9530c783ae3', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Xe đạp ASAMA của bạn đã bắt đầu. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'a9e581be-f581-47f8-9047-bdba6311303d', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Laptop LEGION của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là nhutdmse151298@fpt.edu.vn với mức giá cuối cùng được đưa ra là 60000000. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'4db5033a-fe54-45a7-b125-c18ad71c2793', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Laptop LEGION của bạn đã bắt đầu. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'7a6d3b24-934e-4ac7-9138-c895c8effcbb', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Ấm trà tử sa của bạn đã bắt đầu. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'2976889b-2f1c-402f-9449-d3a0555ee1f8', 1, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Tài khoản Minh Nhựt có email là minhnhut05072001@gmail.com vừa cập nhập được nâng cấp thành người bán. Từ giờ bạn có thêm chức năng đăng bán sản phẩm đấu giá.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'11d749c9-d141-40fc-874f-e07a8aec6a4e', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Laptop LEGION của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là minhnhut05072001@gmail.com với mức giá cuối cùng được đưa ra là 50000000. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'd2c6656a-9e89-401c-b219-eabaa0cc66a7', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Ấm trà tử sa của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là minhnhut05072001@gmail.com với mức giá cuối cùng được đưa ra là 50000000. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
INSERT [dbo].[UserNotificationDetail] ([NotificationID], [TypeID], [UserID], [Messages]) VALUES (N'02068573-8845-4214-bda7-edd9de833b55', 2, N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'Phiên đấu giá vật phẩm Laptop LEGION của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là minhnhut05072001@gmail.com với mức giá cuối cùng được đưa ra là 50000000. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.')
GO
INSERT [dbo].[UserPaymentInformation] ([ID], [UserID], [PayPalAccount], [Status]) VALUES (N'6fcb17a2-2185-4b1b-a7b3-8f8d82e17be2', N'f47ac10b-58cc-4372-a567-0e02b2c3d479', N'minhnhutbid@gmail.com', 1)
GO
INSERT [dbo].[Users] ([ID], [Role], [Name], [Email], [Password], [Avatar], [Address], [Phone], [DateOfBirth], [CCCDNumber], [CreateDate], [UpdateDate], [CCCDFrontImage], [CCCDBackImage], [Status]) VALUES (N'f47ac10b-58cc-4372-a567-0e02b2c3d479', 2, N'Đinh Minh Nhựt', N'nhutdmse151298@fpt.edu.vn', N'05072001', N'avatar mẫu', N'115/4/2 đường số 11 phường trường thọ thành phố thủ đức', N'0933403842', CAST(N'2001-07-05T00:00:00.000' AS DateTime), N'077201000702', CAST(N'2023-08-02T00:00:00.000' AS DateTime), CAST(N'2023-08-02T06:12:59.863' AS DateTime), N'cccd mẫu', N'cccd mẫu', 1)
INSERT [dbo].[Users] ([ID], [Role], [Name], [Email], [Password], [Avatar], [Address], [Phone], [DateOfBirth], [CCCDNumber], [CreateDate], [UpdateDate], [CCCDFrontImage], [CCCDBackImage], [Status]) VALUES (N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', 2, N'Minh Nhựt', N'minhnhut05072001@gmail.com', N'se151298', N'avatar mẫu', N'115/4/2 đường số 11 phường Trường Thọ quận Thủ Đức', N'0933403842', CAST(N'2001-07-05T00:00:00.000' AS DateTime), N'077201000702', CAST(N'2023-07-27T00:00:00.000' AS DateTime), CAST(N'2023-08-18T00:52:31.180' AS DateTime), N'avatar mẫu', N'avatar mẫu', 1)
GO
ALTER TABLE [dbo].[BanHistory]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[BookingItem]  WITH CHECK ADD FOREIGN KEY([StaffID])
REFERENCES [dbo].[Staff] ([ID])
GO
ALTER TABLE [dbo].[Description]  WITH CHECK ADD FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([ID])
GO
ALTER TABLE [dbo].[Image]  WITH CHECK ADD  CONSTRAINT [FK_Image_Item] FOREIGN KEY([ItemID])
REFERENCES [dbo].[Item] ([ID])
GO
ALTER TABLE [dbo].[Image] CHECK CONSTRAINT [FK_Image_Item]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([ID])
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[ItemDescription]  WITH CHECK ADD  CONSTRAINT [FK_ItemDescription_Description] FOREIGN KEY([DescriptionID])
REFERENCES [dbo].[Description] ([ID])
GO
ALTER TABLE [dbo].[ItemDescription] CHECK CONSTRAINT [FK_ItemDescription_Description]
GO
ALTER TABLE [dbo].[ItemDescription]  WITH CHECK ADD  CONSTRAINT [FK_ItemDescription_Item] FOREIGN KEY([ItemID])
REFERENCES [dbo].[Item] ([ID])
GO
ALTER TABLE [dbo].[ItemDescription] CHECK CONSTRAINT [FK_ItemDescription_Item]
GO
ALTER TABLE [dbo].[PaymentStaff]  WITH CHECK ADD  CONSTRAINT [FK_PaymentStaff_Session] FOREIGN KEY([SessionID])
REFERENCES [dbo].[Session] ([ID])
GO
ALTER TABLE [dbo].[PaymentStaff] CHECK CONSTRAINT [FK_PaymentStaff_Session]
GO
ALTER TABLE [dbo].[PaymentStaff]  WITH CHECK ADD  CONSTRAINT [FK_PaymentStaff_Staff] FOREIGN KEY([StaffID])
REFERENCES [dbo].[Staff] ([ID])
GO
ALTER TABLE [dbo].[PaymentStaff] CHECK CONSTRAINT [FK_PaymentStaff_Staff]
GO
ALTER TABLE [dbo].[PaymentStaff]  WITH CHECK ADD  CONSTRAINT [FK_PaymentStaff_UserPaymentInfomation] FOREIGN KEY([UserPaymentInformationID])
REFERENCES [dbo].[UserPaymentInformation] ([ID])
GO
ALTER TABLE [dbo].[PaymentStaff] CHECK CONSTRAINT [FK_PaymentStaff_UserPaymentInfomation]
GO
ALTER TABLE [dbo].[PaymentUser]  WITH CHECK ADD  CONSTRAINT [FK_PaymentUser_Session] FOREIGN KEY([SessionID])
REFERENCES [dbo].[Session] ([ID])
GO
ALTER TABLE [dbo].[PaymentUser] CHECK CONSTRAINT [FK_PaymentUser_Session]
GO
ALTER TABLE [dbo].[PaymentUser]  WITH CHECK ADD  CONSTRAINT [FK_PaymentUser_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[PaymentUser] CHECK CONSTRAINT [FK_PaymentUser_Users]
GO
ALTER TABLE [dbo].[Session]  WITH CHECK ADD FOREIGN KEY([FeeID])
REFERENCES [dbo].[Fee] ([ID])
GO
ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [FK_Session_Item] FOREIGN KEY([ItemID])
REFERENCES [dbo].[Item] ([ID])
GO
ALTER TABLE [dbo].[Session] CHECK CONSTRAINT [FK_Session_Item]
GO
ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [FK_Session_SessionRule] FOREIGN KEY([SessionRuleID])
REFERENCES [dbo].[SessionRule] ([ID])
GO
ALTER TABLE [dbo].[Session] CHECK CONSTRAINT [FK_Session_SessionRule]
GO
ALTER TABLE [dbo].[SessionDetail]  WITH CHECK ADD FOREIGN KEY([SessionID])
REFERENCES [dbo].[Session] ([ID])
GO
ALTER TABLE [dbo].[SessionDetail]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[StaffNotificationDetail]  WITH CHECK ADD FOREIGN KEY([NotificationID])
REFERENCES [dbo].[Notification] ([ID])
GO
ALTER TABLE [dbo].[StaffNotificationDetail]  WITH CHECK ADD FOREIGN KEY([StaffID])
REFERENCES [dbo].[Staff] ([ID])
GO
ALTER TABLE [dbo].[StaffNotificationDetail]  WITH CHECK ADD FOREIGN KEY([TypeID])
REFERENCES [dbo].[NotificationType] ([ID])
GO
ALTER TABLE [dbo].[UserNotificationDetail]  WITH CHECK ADD FOREIGN KEY([NotificationID])
REFERENCES [dbo].[Notification] ([ID])
GO
ALTER TABLE [dbo].[UserNotificationDetail]  WITH CHECK ADD FOREIGN KEY([TypeID])
REFERENCES [dbo].[NotificationType] ([ID])
GO
ALTER TABLE [dbo].[UserNotificationDetail]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[UserPaymentInformation]  WITH CHECK ADD  CONSTRAINT [FK_UserPaymentInfomation_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[UserPaymentInformation] CHECK CONSTRAINT [FK_UserPaymentInfomation_Users]
GO
USE [master]
GO
ALTER DATABASE [BIDsLocal] SET  READ_WRITE 
GO
