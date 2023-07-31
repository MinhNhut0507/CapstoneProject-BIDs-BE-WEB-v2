USE [master]
GO
/****** Object:  Database [BIDsLocal]    Script Date: 7/31/2023 2:20:30 PM ******/
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
/****** Object:  Table [dbo].[Admin]    Script Date: 7/31/2023 2:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admin](
	[ID] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[Status] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AdminNotificationDetail]    Script Date: 7/31/2023 2:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminNotificationDetail](
	[NotificationID] [uniqueidentifier] NOT NULL,
	[TypeID] [int] NOT NULL,
	[AdminID] [uniqueidentifier] NOT NULL,
	[Messages] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BanHistory]    Script Date: 7/31/2023 2:20:30 PM ******/
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
/****** Object:  Table [dbo].[BookingItem]    Script Date: 7/31/2023 2:20:30 PM ******/
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
/****** Object:  Table [dbo].[Category]    Script Date: 7/31/2023 2:20:30 PM ******/
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
/****** Object:  Table [dbo].[Description]    Script Date: 7/31/2023 2:20:30 PM ******/
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
/****** Object:  Table [dbo].[Fee]    Script Date: 7/31/2023 2:20:30 PM ******/
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
/****** Object:  Table [dbo].[Image]    Script Date: 7/31/2023 2:20:30 PM ******/
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
/****** Object:  Table [dbo].[Item]    Script Date: 7/31/2023 2:20:30 PM ******/
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
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemDescription]    Script Date: 7/31/2023 2:20:30 PM ******/
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
/****** Object:  Table [dbo].[Notification]    Script Date: 7/31/2023 2:20:30 PM ******/
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
/****** Object:  Table [dbo].[NotificationType]    Script Date: 7/31/2023 2:20:30 PM ******/
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
/****** Object:  Table [dbo].[PaymentMethodStaff]    Script Date: 7/31/2023 2:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethodStaff](
	[ID] [uniqueidentifier] NOT NULL,
	[StaffID] [uniqueidentifier] NOT NULL,
	[Number] [nvarchar](50) NOT NULL,
	[BankName] [nvarchar](50) NOT NULL,
	[OwnerName] [nvarchar](50) NOT NULL,
	[Status] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethodUser]    Script Date: 7/31/2023 2:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethodUser](
	[ID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[Number] [nvarchar](50) NOT NULL,
	[BankName] [nvarchar](50) NOT NULL,
	[OwnerName] [nvarchar](50) NOT NULL,
	[Status] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentStaff]    Script Date: 7/31/2023 2:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentStaff](
	[ID] [uniqueidentifier] NOT NULL,
	[MethodID] [uniqueidentifier] NOT NULL,
	[StaffID] [uniqueidentifier] NOT NULL,
	[SessionID] [uniqueidentifier] NULL,
	[type] [nvarchar](100) NOT NULL,
	[Detail] [nvarchar](100) NOT NULL,
	[Amount] [float] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Status] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentUser]    Script Date: 7/31/2023 2:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentUser](
	[ID] [uniqueidentifier] NOT NULL,
	[MethodID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[SessionID] [uniqueidentifier] NOT NULL,
	[type] [nvarchar](100) NOT NULL,
	[Detail] [nvarchar](100) NOT NULL,
	[Amount] [float] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Status] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Session]    Script Date: 7/31/2023 2:20:30 PM ******/
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
/****** Object:  Table [dbo].[SessionDetail]    Script Date: 7/31/2023 2:20:30 PM ******/
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
/****** Object:  Table [dbo].[SessionRule]    Script Date: 7/31/2023 2:20:30 PM ******/
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
/****** Object:  Table [dbo].[Staff]    Script Date: 7/31/2023 2:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[ID] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[Status] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StaffNotificationDetail]    Script Date: 7/31/2023 2:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StaffNotificationDetail](
	[NotificationID] [uniqueidentifier] NOT NULL,
	[TypeID] [int] NOT NULL,
	[StaffID] [uniqueidentifier] NOT NULL,
	[Messages] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserNotificationDetail]    Script Date: 7/31/2023 2:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserNotificationDetail](
	[NotificationID] [uniqueidentifier] NOT NULL,
	[TypeID] [int] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[Messages] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 7/31/2023 2:20:30 PM ******/
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
INSERT [dbo].[BookingItem] ([ID], [ItemID], [StaffID], [CreateDate], [Status], [UpdateDate]) VALUES (N'06d4e31a-52e8-47c5-8f2c-1a9bb1d843fb', N'a16e61cc-68c3-4a3f-9d9e-c21b0364ac35', N'9b20198e-9c70-4f22-8714-52a7b6e034d8', CAST(N'2023-07-28T00:00:00.000' AS DateTime), 2, CAST(N'2023-07-28T00:00:00.000' AS DateTime))
INSERT [dbo].[BookingItem] ([ID], [ItemID], [StaffID], [CreateDate], [Status], [UpdateDate]) VALUES (N'6e6e22f7-1425-4b8e-9873-20aa59d1dd7a', N'6e6e22f7-1425-4b8e-9873-20aa59d1dd7a', N'9b20198e-9c70-4f22-8714-52a7b6e034d8', CAST(N'2023-07-28T00:00:00.000' AS DateTime), 2, CAST(N'2023-07-28T00:00:00.000' AS DateTime))
INSERT [dbo].[BookingItem] ([ID], [ItemID], [StaffID], [CreateDate], [Status], [UpdateDate]) VALUES (N'c89520eb-1655-42e1-883d-257602bae1c3', N'527e736a-d74c-4d01-b4cd-8748fb15699e', N'9b20198e-9c70-4f22-8714-52a7b6e034d8', CAST(N'2023-07-28T00:00:00.000' AS DateTime), 2, CAST(N'2023-07-28T00:00:00.000' AS DateTime))
INSERT [dbo].[BookingItem] ([ID], [ItemID], [StaffID], [CreateDate], [Status], [UpdateDate]) VALUES (N'e9e47f34-1dd8-413f-9096-26703600b9ad', N'db6043e1-02c9-4c2b-b8dd-9f0a8574ad5f', N'9b20198e-9c70-4f22-8714-52a7b6e034d8', CAST(N'2023-07-28T00:00:00.000' AS DateTime), 2, CAST(N'2023-07-28T00:00:00.000' AS DateTime))
INSERT [dbo].[BookingItem] ([ID], [ItemID], [StaffID], [CreateDate], [Status], [UpdateDate]) VALUES (N'5f44c399-4f8d-4d02-9759-44b02e2b2d61', N'13e57d05-6b33-418e-9e71-14ebc9700ca1', N'9b20198e-9c70-4f22-8714-52a7b6e034d8', CAST(N'2023-07-28T00:00:00.000' AS DateTime), 2, CAST(N'2023-07-28T00:00:00.000' AS DateTime))
INSERT [dbo].[BookingItem] ([ID], [ItemID], [StaffID], [CreateDate], [Status], [UpdateDate]) VALUES (N'71c6c3ed-4f26-4711-ba59-5c14f9e1d7cb', N'0957b537-36a5-4cb0-97c6-24ecf6c8dbdf', N'9b20198e-9c70-4f22-8714-52a7b6e034d8', CAST(N'2023-07-28T00:00:00.000' AS DateTime), 2, CAST(N'2023-07-28T00:00:00.000' AS DateTime))
INSERT [dbo].[BookingItem] ([ID], [ItemID], [StaffID], [CreateDate], [Status], [UpdateDate]) VALUES (N'84a226c3-346e-4c1a-9f61-94ef96e32075', N'f320c446-9a44-4022-8d26-84c52e688675', N'9b20198e-9c70-4f22-8714-52a7b6e034d8', CAST(N'2023-07-28T00:00:00.000' AS DateTime), 2, CAST(N'2023-07-28T00:00:00.000' AS DateTime))
INSERT [dbo].[BookingItem] ([ID], [ItemID], [StaffID], [CreateDate], [Status], [UpdateDate]) VALUES (N'f3d0a51d-93e8-4b9e-b043-9533c011c378', N'5bf0f3f8-1ee2-42c2-b25f-7b8a5eaa9c38', N'9b20198e-9c70-4f22-8714-52a7b6e034d8', CAST(N'2023-07-28T00:00:00.000' AS DateTime), 2, CAST(N'2023-07-28T00:00:00.000' AS DateTime))
INSERT [dbo].[BookingItem] ([ID], [ItemID], [StaffID], [CreateDate], [Status], [UpdateDate]) VALUES (N'a3b04927-9eaf-4a3d-aedc-9d21630e1e0f', N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', N'9b20198e-9c70-4f22-8714-52a7b6e034d8', CAST(N'2023-07-28T00:00:00.000' AS DateTime), 2, CAST(N'2023-07-28T00:00:00.000' AS DateTime))
INSERT [dbo].[BookingItem] ([ID], [ItemID], [StaffID], [CreateDate], [Status], [UpdateDate]) VALUES (N'd6543d42-9897-465d-a6d2-ee58a15837c7', N'8bf8833a-59aa-4ac5-a80d-cbbbc663dc09', N'9b20198e-9c70-4f22-8714-52a7b6e034d8', CAST(N'2023-07-28T00:00:00.000' AS DateTime), 2, CAST(N'2023-07-28T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[Category] ([ID], [Name], [UpdateDate], [CreateDate], [Status]) VALUES (N'f345f47b-83c3-42f7-9f0d-2342fb4567b4', N'Đồ công nghệ', CAST(N'2023-07-03T01:32:18.013' AS DateTime), CAST(N'2023-07-03T01:32:18.013' AS DateTime), 1)
INSERT [dbo].[Category] ([ID], [Name], [UpdateDate], [CreateDate], [Status]) VALUES (N'890fca36-29ab-4a50-a99e-51f9d3db3ae2', N'Xe đạp', CAST(N'2023-07-03T01:32:18.013' AS DateTime), CAST(N'2023-07-03T01:32:18.013' AS DateTime), 1)
INSERT [dbo].[Category] ([ID], [Name], [UpdateDate], [CreateDate], [Status]) VALUES (N'7d534f13-1d22-4baf-9d67-590d3ae4d07d', N'Xe máy', CAST(N'2023-07-27T23:00:53.103' AS DateTime), CAST(N'2023-07-03T01:32:18.013' AS DateTime), 0)
INSERT [dbo].[Category] ([ID], [Name], [UpdateDate], [CreateDate], [Status]) VALUES (N'6d2e3f84-4e39-41e2-99eb-cba4de0d2e44', N'Đồ cổ', CAST(N'2023-07-03T01:32:18.017' AS DateTime), CAST(N'2023-07-03T01:32:18.017' AS DateTime), 1)
GO
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'b0f5b80d-ff41-4b2f-84db-1530b6018e76', N'7d534f13-1d22-4baf-9d67-590d3ae4d07d', 1, N'Mua vào nam')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'f6a6c63f-56a0-4164-979b-1c6a5f69c512', N'6d2e3f84-4e39-41e2-99eb-cba4de0d2e44', 1, N'Loại sản phẩm cụ thể')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'6340b24d-9257-4d4a-940f-2e34a9e42830', N'6d2e3f84-4e39-41e2-99eb-cba4de0d2e44', 1, N'Niên đại')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'7a41a429-ba57-4d34-92d6-48a12ad0c98c', N'f345f47b-83c3-42f7-9f0d-2342fb4567b4', 1, N'Mua vào năm')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'8d91b4f0-8b59-4ea9-9e42-49c6dd7e098a', N'6d2e3f84-4e39-41e2-99eb-cba4de0d2e44', 1, N'Nhãn hiệu(Nếu có)')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'c87ee0b6-96e1-4bbf-8e8e-4f09fe46a4a8', N'890fca36-29ab-4a50-a99e-51f9d3db3ae2', 1, N'Nhãn hiệu')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'b20e94ce-05f6-41ff-b9de-4fc748dabce0', N'6d2e3f84-4e39-41e2-99eb-cba4de0d2e44', 1, N'Độ hư hại')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'3e6d3540-c9e9-493d-a01b-64a6c7b07f88', N'890fca36-29ab-4a50-a99e-51f9d3db3ae2', 1, N'Mua vào năm')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'9053c700-35d1-4d46-9c5d-6e3288d6ce15', N'f345f47b-83c3-42f7-9f0d-2342fb4567b4', 1, N'Loại sản phẩm cụ thể')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'8e9e6a68-3a45-4135-92d4-85d7ae4e7c82', N'7d534f13-1d22-4baf-9d67-590d3ae4d07d', 1, N'Hãng xe')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'2fb11c2d-2b3c-41a9-82c1-8e1a2445f5c3', N'f345f47b-83c3-42f7-9f0d-2342fb4567b4', 1, N'Màu sắc')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'e2d739f8-ef82-471a-b5a9-98dd3e3a976e', N'7d534f13-1d22-4baf-9d67-590d3ae4d07d', 1, N'Màu sắc')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'4b25a648-32c6-4a95-96d0-9cb02f2da01a', N'890fca36-29ab-4a50-a99e-51f9d3db3ae2', 1, N'Màu sắc')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'52bc7b17-3820-42e9-a775-aa729f2dd4ef', N'7d534f13-1d22-4baf-9d67-590d3ae4d07d', 1, N'Biển số')
INSERT [dbo].[Description] ([ID], [CategoryID], [Status], [Name]) VALUES (N'b79a7df5-af17-4a75-8a4b-ef21c51e9d46', N'f345f47b-83c3-42f7-9f0d-2342fb4567b4', 1, N'Nhãn hiệu')
GO
SET IDENTITY_INSERT [dbo].[Fee] ON 

INSERT [dbo].[Fee] ([ID], [Name], [Min], [Max], [ParticipationFee], [DepositFee], [Surcharge], [CreateDate], [UpdateDate], [Status]) VALUES (10, N'Phân khúc vừa và nhỏ', 1000000, 10000000, 0.005, 0, 0.1, CAST(N'2023-07-03T01:32:17.997' AS DateTime), CAST(N'2023-07-28T17:42:06.470' AS DateTime), 1)
INSERT [dbo].[Fee] ([ID], [Name], [Min], [Max], [ParticipationFee], [DepositFee], [Surcharge], [CreateDate], [UpdateDate], [Status]) VALUES (11, N'Phân khúc trung bình', 10000000, 30000000, 0.004, 0.15, 0.1, CAST(N'2023-07-03T01:32:18.010' AS DateTime), CAST(N'2023-07-03T01:32:18.010' AS DateTime), 1)
INSERT [dbo].[Fee] ([ID], [Name], [Min], [Max], [ParticipationFee], [DepositFee], [Surcharge], [CreateDate], [UpdateDate], [Status]) VALUES (12, N'Phân khúc cao cấp', 30000000, 1000000000, 0.003, 0.25, 0.1, CAST(N'2023-07-03T01:32:18.010' AS DateTime), CAST(N'2023-07-03T01:32:18.010' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Fee] OFF
GO
INSERT [dbo].[Item] ([ID], [UserID], [CategoryID], [Name], [DescriptionDetail], [Quantity], [StepPrice], [Deposit], [CreateDate], [UpdateDate], [FirstPrice]) VALUES (N'13e57d05-6b33-418e-9e71-14ebc9700ca1', N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'890fca36-29ab-4a50-a99e-51f9d3db3ae2', N'Xe Đạp AORGKI', N'Xe đạp nhãn hiệu AORGKI với 2 màu cam và đen thích hợp cho những người muốn tập thể dục bằng xe đạp buổi sáng hoặc đi học đi làm', 1, 200000, 0, CAST(N'2023-07-27T00:00:00.000' AS DateTime), CAST(N'2023-07-27T00:00:00.000' AS DateTime), 2000000)
INSERT [dbo].[Item] ([ID], [UserID], [CategoryID], [Name], [DescriptionDetail], [Quantity], [StepPrice], [Deposit], [CreateDate], [UpdateDate], [FirstPrice]) VALUES (N'6e6e22f7-1425-4b8e-9873-20aa59d1dd7a', N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'f345f47b-83c3-42f7-9f0d-2342fb4567b4', N'Kính thực tế ảo', N'Kính thực tế ảo như công nghệ tương lại có thể cho bạn trải nghiệm xem phim hoặc chơi trò chơi một cách thực tế', 1, 1500000, 0, CAST(N'2023-07-27T00:00:00.000' AS DateTime), CAST(N'2023-07-27T00:00:00.000' AS DateTime), 15000000)
INSERT [dbo].[Item] ([ID], [UserID], [CategoryID], [Name], [DescriptionDetail], [Quantity], [StepPrice], [Deposit], [CreateDate], [UpdateDate], [FirstPrice]) VALUES (N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'890fca36-29ab-4a50-a99e-51f9d3db3ae2', N'Xe Đạp XAMNG', N'Xe đạp nhãn hiệu XAMNG kiểu dáng nhỏ gọn dùng cho các bạn trẻ hoặc các bạn thích bộ môn biểu diễn xe đạp. Ngoài ra với kích thước to hơn so với các sản phẩm cùng loại có thể ứng dụng vào các môn nghệ thuật xe đạp có độ khó cao', 1, 600000, 0, CAST(N'2023-07-27T00:00:00.000' AS DateTime), CAST(N'2023-07-27T00:00:00.000' AS DateTime), 6000000)
INSERT [dbo].[Item] ([ID], [UserID], [CategoryID], [Name], [DescriptionDetail], [Quantity], [StepPrice], [Deposit], [CreateDate], [UpdateDate], [FirstPrice]) VALUES (N'8bf8833a-59aa-4ac5-a80d-cbbbc663dc09', N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', N'f345f47b-83c3-42f7-9f0d-2342fb4567b4', N'Airpod', N'Airpod custom theo tranh Vangosh', 1, 500000, 0, CAST(N'2023-07-27T00:00:00.000' AS DateTime), CAST(N'2023-07-27T00:00:00.000' AS DateTime), 5000000)
GO
INSERT [dbo].[Session] ([ID], [FeeID], [Name], [BeginTime], [EndTime], [UpdateDate], [CreateDate], [Status], [ItemID], [FinalPrice], [SessionRuleID]) VALUES (N'0e3a07d7-e1f8-4973-a4e0-2b27e20285e5', 10, N'Xe Đạp Nghệ Thuật', CAST(N'2023-09-10T12:00:00.000' AS DateTime), CAST(N'2023-09-10T18:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), 1, N'13e57d05-6b33-418e-9e71-14ebc9700ca1', 2000000, N'b7370eb2-3fff-4b91-f31b-08db7d91cd0c')
INSERT [dbo].[Session] ([ID], [FeeID], [Name], [BeginTime], [EndTime], [UpdateDate], [CreateDate], [Status], [ItemID], [FinalPrice], [SessionRuleID]) VALUES (N'a3d687ab-23a1-4e0a-a39b-3d01e3c8a9e8', 10, N'Airpod', CAST(N'2023-07-29T00:30:00.000' AS DateTime), CAST(N'2023-07-29T01:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), 1, N'a16e61cc-68c3-4a3f-9d9e-c21b0364ac35', 5000000, N'b7370eb2-3fff-4b91-f31b-08db7d91cd0c')
INSERT [dbo].[Session] ([ID], [FeeID], [Name], [BeginTime], [EndTime], [UpdateDate], [CreateDate], [Status], [ItemID], [FinalPrice], [SessionRuleID]) VALUES (N'617b1585-34be-4dab-bc46-3f3e5799a1f1', 10, N'Xe Đạp', CAST(N'2023-09-10T12:00:00.000' AS DateTime), CAST(N'2023-09-10T18:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), 1, N'a16e61cc-68c3-4a3f-9d9e-c21b0364ac35', 3000000, N'b7370eb2-3fff-4b91-f31b-08db7d91cd0c')
INSERT [dbo].[Session] ([ID], [FeeID], [Name], [BeginTime], [EndTime], [UpdateDate], [CreateDate], [Status], [ItemID], [FinalPrice], [SessionRuleID]) VALUES (N'7e39a215-cf19-4d71-8c58-66be8b4be30a', 10, N'Xe Đạp', CAST(N'2023-09-10T12:00:00.000' AS DateTime), CAST(N'2023-09-10T18:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), 1, N'36b7f3d4-cc6e-4c07-b46b-22c080168d5a', 6000000, N'b7370eb2-3fff-4b91-f31b-08db7d91cd0c')
INSERT [dbo].[Session] ([ID], [FeeID], [Name], [BeginTime], [EndTime], [UpdateDate], [CreateDate], [Status], [ItemID], [FinalPrice], [SessionRuleID]) VALUES (N'3f5da238-1eb4-4a1c-a1ea-7c1359536e35', 11, N'Kính Thực Tế Ảo', CAST(N'2023-09-10T12:00:00.000' AS DateTime), CAST(N'2023-09-10T18:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), 1, N'6e6e22f7-1425-4b8e-9873-20aa59d1dd7a', 15000000, N'b7370eb2-3fff-4b91-f31b-08db7d91cd0c')
INSERT [dbo].[Session] ([ID], [FeeID], [Name], [BeginTime], [EndTime], [UpdateDate], [CreateDate], [Status], [ItemID], [FinalPrice], [SessionRuleID]) VALUES (N'9bf89ea2-2b5d-4b5e-b1c6-8a52767b2a3c', 10, N'Xe Đạp', CAST(N'2023-09-10T12:00:00.000' AS DateTime), CAST(N'2023-09-10T18:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), 1, N'db6043e1-02c9-4c2b-b8dd-9f0a8574ad5f', 5000000, N'b7370eb2-3fff-4b91-f31b-08db7d91cd0c')
INSERT [dbo].[Session] ([ID], [FeeID], [Name], [BeginTime], [EndTime], [UpdateDate], [CreateDate], [Status], [ItemID], [FinalPrice], [SessionRuleID]) VALUES (N'9dbfd732-c532-4c7c-a7f0-c3d1535e34d9', 10, N'Xe Đạp', CAST(N'2023-09-10T12:00:00.000' AS DateTime), CAST(N'2023-09-10T18:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), 1, N'5bf0f3f8-1ee2-42c2-b25f-7b8a5eaa9c38', 2000000, N'b7370eb2-3fff-4b91-f31b-08db7d91cd0c')
INSERT [dbo].[Session] ([ID], [FeeID], [Name], [BeginTime], [EndTime], [UpdateDate], [CreateDate], [Status], [ItemID], [FinalPrice], [SessionRuleID]) VALUES (N'b34f19c6-1e96-42bb-b15b-cc51354b3e1e', 10, N'Nitendo Swich', CAST(N'2023-09-10T12:00:00.000' AS DateTime), CAST(N'2023-09-10T18:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), 1, N'0957b537-36a5-4cb0-97c6-24ecf6c8dbdf', 10000000, N'b7370eb2-3fff-4b91-f31b-08db7d91cd0c')
INSERT [dbo].[Session] ([ID], [FeeID], [Name], [BeginTime], [EndTime], [UpdateDate], [CreateDate], [Status], [ItemID], [FinalPrice], [SessionRuleID]) VALUES (N'8a176398-8e0d-4851-a119-d29d90da9f45', 11, N'Laptop', CAST(N'2023-07-29T00:00:00.000' AS DateTime), CAST(N'2023-07-29T09:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), 1, N'f320c446-9a44-4022-8d26-84c52e688675', 30000000, N'b7370eb2-3fff-4b91-f31b-08db7d91cd0c')
INSERT [dbo].[Session] ([ID], [FeeID], [Name], [BeginTime], [EndTime], [UpdateDate], [CreateDate], [Status], [ItemID], [FinalPrice], [SessionRuleID]) VALUES (N'e51b8a78-5b17-419e-af7a-d9a646981979', 12, N'Điện Thoại', CAST(N'2023-09-10T12:00:00.000' AS DateTime), CAST(N'2023-09-10T18:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), CAST(N'2023-07-28T00:00:00.000' AS DateTime), 1, N'527e736a-d74c-4d01-b4cd-8748fb15699e', 50000000, N'b7370eb2-3fff-4b91-f31b-08db7d91cd0c')
GO
INSERT [dbo].[SessionRule] ([ID], [Name], [FreeTime], [DelayTime], [DelayFreeTime], [CreateDate], [UpdateDate], [Status]) VALUES (N'b7370eb2-3fff-4b91-f31b-08db7d91cd0c', N'Luật Chính Thức', CAST(N'00:05:00' AS Time), CAST(N'00:10:00' AS Time), CAST(N'00:00:15' AS Time), CAST(N'2023-07-06T02:55:35.100' AS DateTime), CAST(N'2023-07-06T02:55:35.097' AS DateTime), 1)
INSERT [dbo].[SessionRule] ([ID], [Name], [FreeTime], [DelayTime], [DelayFreeTime], [CreateDate], [UpdateDate], [Status]) VALUES (N'139b145a-f4d6-41b8-091e-08db8eb6abf3', N'Luật testing', CAST(N'00:10:00' AS Time), CAST(N'00:00:05' AS Time), CAST(N'00:00:03' AS Time), CAST(N'2023-07-27T22:32:22.270' AS DateTime), CAST(N'2023-07-27T22:32:22.270' AS DateTime), 1)
GO
INSERT [dbo].[Staff] ([ID], [Email], [Name], [Password], [Address], [Phone], [DateOfBirth], [CreateDate], [UpdateDate], [Status]) VALUES (N'36b9e6b1-97df-4fe6-bc10-2c5c163889ed', N'seedstaff1@gmail.com', N'Seed Staff 1', N'05072001', N'115/4/2 phường Trường Thọ quận Thủ Đức', N'0933403842', CAST(N'2001-07-05T00:00:00.000' AS DateTime), CAST(N'2023-07-03T01:32:01.580' AS DateTime), CAST(N'2023-07-03T01:32:01.580' AS DateTime), 1)
INSERT [dbo].[Staff] ([ID], [Email], [Name], [Password], [Address], [Phone], [DateOfBirth], [CreateDate], [UpdateDate], [Status]) VALUES (N'9b20198e-9c70-4f22-8714-52a7b6e034d8', N'seedstaff@gmail.com', N'Seed Staff', N'05072001', N'115/4/2 phường Trường Thọ quận Thủ Đức', N'0933403842', CAST(N'2001-07-05T00:00:00.000' AS DateTime), CAST(N'2023-07-03T01:32:01.580' AS DateTime), CAST(N'2023-07-03T01:32:01.580' AS DateTime), 1)
GO
INSERT [dbo].[Users] ([ID], [Role], [Name], [Email], [Password], [Avatar], [Address], [Phone], [DateOfBirth], [CCCDNumber], [CreateDate], [UpdateDate], [CCCDFrontImage], [CCCDBackImage], [Status]) VALUES (N'b2b8b67c-06a4-44e0-9c15-8953b46e6d43', 2, N'Minh Nhựt', N'minhnhut05072001@gmail.com', N'05072001', N'avatar mẫu', N'115/4/2 đường số 11 phường Trường Thọ quận Thủ Đức', N'0933403842', CAST(N'2001-07-05T00:00:00.000' AS DateTime), N'077201000702', CAST(N'2023-07-27T00:00:00.000' AS DateTime), CAST(N'2023-07-27T00:00:00.000' AS DateTime), N'avatar mẫu', N'avatar mẫu', 1)
GO
ALTER TABLE [dbo].[AdminNotificationDetail]  WITH CHECK ADD FOREIGN KEY([AdminID])
REFERENCES [dbo].[Admin] ([ID])
GO
ALTER TABLE [dbo].[AdminNotificationDetail]  WITH CHECK ADD FOREIGN KEY([NotificationID])
REFERENCES [dbo].[Notification] ([ID])
GO
ALTER TABLE [dbo].[AdminNotificationDetail]  WITH CHECK ADD FOREIGN KEY([TypeID])
REFERENCES [dbo].[NotificationType] ([ID])
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
ALTER TABLE [dbo].[PaymentMethodStaff]  WITH CHECK ADD FOREIGN KEY([StaffID])
REFERENCES [dbo].[Staff] ([ID])
GO
ALTER TABLE [dbo].[PaymentMethodUser]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[PaymentStaff]  WITH CHECK ADD FOREIGN KEY([MethodID])
REFERENCES [dbo].[PaymentMethodStaff] ([ID])
GO
ALTER TABLE [dbo].[PaymentStaff]  WITH CHECK ADD FOREIGN KEY([SessionID])
REFERENCES [dbo].[Session] ([ID])
GO
ALTER TABLE [dbo].[PaymentStaff]  WITH CHECK ADD FOREIGN KEY([StaffID])
REFERENCES [dbo].[Staff] ([ID])
GO
ALTER TABLE [dbo].[PaymentUser]  WITH CHECK ADD FOREIGN KEY([MethodID])
REFERENCES [dbo].[PaymentMethodUser] ([ID])
GO
ALTER TABLE [dbo].[PaymentUser]  WITH CHECK ADD FOREIGN KEY([SessionID])
REFERENCES [dbo].[Session] ([ID])
GO
ALTER TABLE [dbo].[PaymentUser]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Session]  WITH CHECK ADD FOREIGN KEY([FeeID])
REFERENCES [dbo].[Fee] ([ID])
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
USE [master]
GO
ALTER DATABASE [BIDsLocal] SET  READ_WRITE 
GO
