USE [BIDsLocal]
GO
/****** Object:  Table [dbo].[Admin]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[BanHistory]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[BookingItem]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[Category]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[Description]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[Fee]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[Image]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[Item]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[ItemDescription]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[Notification]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[NotificationType]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[PaymentStaff]    Script Date: 9/4/2023 2:42:07 AM ******/
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
	[SessionID] [uniqueidentifier] NOT NULL,
	[PayPalRecieveAccount] [nvarchar](100) NULL,
	[UserID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PaymentStaff] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentUser]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[Session]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[SessionDetail]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[SessionRule]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[Staff]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[StaffNotificationDetail]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[UserNotificationDetail]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[UserPaymentInformation]    Script Date: 9/4/2023 2:42:07 AM ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 9/4/2023 2:42:07 AM ******/
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
ALTER TABLE [dbo].[PaymentStaff]  WITH CHECK ADD  CONSTRAINT [FK_PaymentStaff_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[PaymentStaff] CHECK CONSTRAINT [FK_PaymentStaff_Users]
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
