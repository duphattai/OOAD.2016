CREATE DATABASE CarManager
GO
USE [CarManager]
GO
/****** Object:  StoredProcedure [dbo].[ReportByDate]    Script Date: 06-Jan-17 12:21:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ReportByDate] 
	-- Add the parameters for the stored procedure here
	@pDate varchar(10) = ''	
AS
BEGIN
	SELECT SUM(ISNULL(B.TOTAL_TICKED,0)) AS TOTAL_TICKED,
		SUM(ISNULL(B.TOTAL_PRICE,0)) AS TOTAL_PRICE
	FROM
	(
		SELECT A.IdOrderDetail, COUNT(A.IdOrderDetail) TOTAL_TICKED, 
		SUM(B.Price) TOTAL_PRICE
		FROM OrderDetail A 
		INNER JOIN Schedule B
		ON A.IdSchedule = B.IdSchedule
		WHERE A.IsPaid = 1 AND (@pDate = '' OR (@pDate <> '' AND CONVERT(date,A.PaymentDate,103) = CONVERT(date,@pDate,103)))
		GROUP BY A.IdOrderDetail		
	) B 
END

GO
/****** Object:  Table [dbo].[Account]    Script Date: 06-Jan-17 12:21:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Account](
	[IdAccount] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](100) NULL,
	[Pass] [varchar](255) NULL,
	[IdRoles] [varchar](100) NULL,
	[FullName] [nvarchar](255) NULL,
	[CMND] [varchar](20) NULL,
	[Phone] [varchar](20) NULL,
	[Address] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdAccount] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusStation]    Script Date: 06-Jan-17 12:21:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusStation](
	[IdBusStation] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdBusStation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Car]    Script Date: 06-Jan-17 12:21:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Car](
	[IdCar] [int] IDENTITY(1,1) NOT NULL,
	[CarNumberPlate] [varchar](20) NOT NULL,
	[TotalSeat] [int] NULL,
	[IdCarDiagram] [int] NOT NULL,
	[Driver1] [nvarchar](max) NULL,
	[Driver2] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCar] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CarDiagram]    Script Date: 06-Jan-17 12:21:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CarDiagram](
	[IdCarDiagram] [int] IDENTITY(1,1) NOT NULL,
	[NumberFloors] [int] NOT NULL,
	[SeatDiagram] [varchar](max) NOT NULL,
	[TypeSeat] [nvarchar](255) NULL,
	[Name] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCarDiagram] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Channel]    Script Date: 06-Jan-17 12:21:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Channel](
	[IdChannel] [int] IDENTITY(1,1) NOT NULL,
	[IdBusStationFrom] [int] NOT NULL,
	[IdBusStationTo] [int] NOT NULL,
	[RunTime] [int] NOT NULL,
	[DefaultPrice] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdChannel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DetailChannel]    Script Date: 06-Jan-17 12:21:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetailChannel](
	[IdChannel] [int] NOT NULL,
	[IdBusStation] [int] NOT NULL,
	[Time] [int] NULL,
	[Note] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdChannel] ASC,
	[IdBusStation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Order]    Script Date: 06-Jan-17 12:21:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Order](
	[IdOrder] [int] IDENTITY(1,1) NOT NULL,
	[OrderName] [nvarchar](255) NULL,
	[PhoneNumber] [varchar](50) NULL,
	[OrderDate] [datetime] NULL,
	[Note] [nvarchar](max) NULL,
	[PickUp] [nvarchar](max) NULL,
	[Destination] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 06-Jan-17 12:21:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[IdOrderDetail] [int] IDENTITY(1,1) NOT NULL,
	[IdOrder] [int] NOT NULL,
	[IdSchedule] [int] NOT NULL,
	[IsPaid] [bit] NULL,
	[SeatNumber] [int] NULL,
	[PaymentDate] [datetime] NULL,
	[FloorNumber] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdOrderDetail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 06-Jan-17 12:21:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Role](
	[IdRole] [int] NOT NULL,
	[RoleName] [varchar](255) NULL,
	[Note] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdRole] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Schedule]    Script Date: 06-Jan-17 12:21:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedule](
	[IdSchedule] [int] IDENTITY(1,1) NOT NULL,
	[IdChannel] [int] NOT NULL,
	[IdCar] [int] NOT NULL,
	[StartTime] [datetime] NULL,
	[ArrivalTime] [datetime] NULL,
	[Price] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdSchedule] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Account] ON 

INSERT [dbo].[Account] ([IdAccount], [UserName], [Pass], [IdRoles], [FullName], [CMND], [Phone], [Address]) VALUES (1, N'admin', N'e10adc3949ba59abbe56e057f20f883e', N'1,2,3', N'Admin', NULL, NULL, NULL)
INSERT [dbo].[Account] ([IdAccount], [UserName], [Pass], [IdRoles], [FullName], [CMND], [Phone], [Address]) VALUES (3, N'tuananh', N'e10adc3949ba59abbe56e057f20f883e', N'2,3', N'Lê Tuấn Anh', N'0123456789', N'0123456789', N'KTX')
INSERT [dbo].[Account] ([IdAccount], [UserName], [Pass], [IdRoles], [FullName], [CMND], [Phone], [Address]) VALUES (4, N'nhanvien', N'e10adc3949ba59abbe56e057f20f883e', N'2', N'Nhân Viên', N'0123456789', N'0123456789', N'KTX')
INSERT [dbo].[Account] ([IdAccount], [UserName], [Pass], [IdRoles], [FullName], [CMND], [Phone], [Address]) VALUES (5, N'quanly', N'e10adc3949ba59abbe56e057f20f883e', N'3', N'Quản Lý', N'0123456789', N'0123456789', N'KTX')
SET IDENTITY_INSERT [dbo].[Account] OFF
SET IDENTITY_INSERT [dbo].[BusStation] ON 

INSERT [dbo].[BusStation] ([IdBusStation], [Name]) VALUES (1, N'Bến xe miền tây')
INSERT [dbo].[BusStation] ([IdBusStation], [Name]) VALUES (2, N'Bến xe sóc trăng')
INSERT [dbo].[BusStation] ([IdBusStation], [Name]) VALUES (5, N'Bến xe tây ninh')
INSERT [dbo].[BusStation] ([IdBusStation], [Name]) VALUES (6, N'Bến xe nha trang')
INSERT [dbo].[BusStation] ([IdBusStation], [Name]) VALUES (7, N'An sương')
INSERT [dbo].[BusStation] ([IdBusStation], [Name]) VALUES (8, N'Củ chi')
SET IDENTITY_INSERT [dbo].[BusStation] OFF
SET IDENTITY_INSERT [dbo].[Car] ON 

INSERT [dbo].[Car] ([IdCar], [CarNumberPlate], [TotalSeat], [IdCarDiagram], [Driver1], [Driver2]) VALUES (1, N'8584888', 40, 2, N'Nguyễn Văn A', NULL)
SET IDENTITY_INSERT [dbo].[Car] OFF
SET IDENTITY_INSERT [dbo].[CarDiagram] ON 

INSERT [dbo].[CarDiagram] ([IdCarDiagram], [NumberFloors], [SeatDiagram], [TypeSeat], [Name]) VALUES (1, 1, N'x41 x37 x33 x29 x25 x21 x17 x13 x09 x05 x01
x42 x38 x34 x30 x26 x22 x18 x14 x10 x06 x02
x43
x44 x39 x35 x31 x27 x23 x19 x15 x11 x07 x03
x45 x40 x36 x32 x28 x24 x20 x16 x12 x08 x04', N'Ghế ngồi', N'Sơ đồ xe ghế ngồi 45 chỗ')
INSERT [dbo].[CarDiagram] ([IdCarDiagram], [NumberFloors], [SeatDiagram], [TypeSeat], [Name]) VALUES (2, 2, N'x16 x13 x10 x07 x04 x01
x17
x18 x14 x11 x08 x05 x02
x19
x20 x15 x12 x09 x06 x03', N'Ghế nằm', N'Sơ đồ xe giường nằm 40 chỗ')
SET IDENTITY_INSERT [dbo].[CarDiagram] OFF
SET IDENTITY_INSERT [dbo].[Channel] ON 

INSERT [dbo].[Channel] ([IdChannel], [IdBusStationFrom], [IdBusStationTo], [RunTime], [DefaultPrice]) VALUES (1, 1, 2, 200, 100000)
INSERT [dbo].[Channel] ([IdChannel], [IdBusStationFrom], [IdBusStationTo], [RunTime], [DefaultPrice]) VALUES (2, 1, 5, 150, 120000)
INSERT [dbo].[Channel] ([IdChannel], [IdBusStationFrom], [IdBusStationTo], [RunTime], [DefaultPrice]) VALUES (6, 1, 6, 200, 150000)
SET IDENTITY_INSERT [dbo].[Channel] OFF
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([IdOrder], [OrderName], [PhoneNumber], [OrderDate], [Note], [PickUp], [Destination]) VALUES (1, N'nam1', NULL, CAST(0x0000A6F101883898 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Order] ([IdOrder], [OrderName], [PhoneNumber], [OrderDate], [Note], [PickUp], [Destination]) VALUES (2, N'nam2', NULL, CAST(0x0000A6F101883898 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Order] ([IdOrder], [OrderName], [PhoneNumber], [OrderDate], [Note], [PickUp], [Destination]) VALUES (5, N'Lê tuấn anh', N'123456789', CAST(0x0000A6F1018838BB AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Order] ([IdOrder], [OrderName], [PhoneNumber], [OrderDate], [Note], [PickUp], [Destination]) VALUES (6, N'Test', N'123456789', CAST(0x0000A6F30000B7E2 AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Order] OFF
SET IDENTITY_INSERT [dbo].[OrderDetail] ON 

INSERT [dbo].[OrderDetail] ([IdOrderDetail], [IdOrder], [IdSchedule], [IsPaid], [SeatNumber], [PaymentDate], [FloorNumber]) VALUES (13, 5, 2, 1, 2, CAST(0x0000A6F300D0BD80 AS DateTime), 1)
INSERT [dbo].[OrderDetail] ([IdOrderDetail], [IdOrder], [IdSchedule], [IsPaid], [SeatNumber], [PaymentDate], [FloorNumber]) VALUES (15, 5, 2, 0, 4, NULL, 1)
INSERT [dbo].[OrderDetail] ([IdOrderDetail], [IdOrder], [IdSchedule], [IsPaid], [SeatNumber], [PaymentDate], [FloorNumber]) VALUES (17, 1, 2, 0, 1, NULL, 1)
INSERT [dbo].[OrderDetail] ([IdOrderDetail], [IdOrder], [IdSchedule], [IsPaid], [SeatNumber], [PaymentDate], [FloorNumber]) VALUES (19, 6, 4, 1, 2, CAST(0x0000A6F300D0BD80 AS DateTime), 1)
INSERT [dbo].[OrderDetail] ([IdOrderDetail], [IdOrder], [IdSchedule], [IsPaid], [SeatNumber], [PaymentDate], [FloorNumber]) VALUES (20, 6, 4, 1, 4, CAST(0x0000A6F300D0BD80 AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[OrderDetail] OFF
INSERT [dbo].[Role] ([IdRole], [RoleName], [Note]) VALUES (1, N'Administration', N'quyền quản trị hệ thống')
INSERT [dbo].[Role] ([IdRole], [RoleName], [Note]) VALUES (2, N'Salesman', N'Quyền nhân viên bán hàng')
INSERT [dbo].[Role] ([IdRole], [RoleName], [Note]) VALUES (3, N'Manager', N'Quản lý')
SET IDENTITY_INSERT [dbo].[Schedule] ON 

INSERT [dbo].[Schedule] ([IdSchedule], [IdChannel], [IdCar], [StartTime], [ArrivalTime], [Price]) VALUES (1, 1, 1, CAST(0x0000A6F000F1B300 AS DateTime), CAST(0x0000A6F00128A180 AS DateTime), 100000)
INSERT [dbo].[Schedule] ([IdSchedule], [IdChannel], [IdCar], [StartTime], [ArrivalTime], [Price]) VALUES (2, 2, 1, CAST(0x0000A6F200F1B300 AS DateTime), CAST(0x0000A6F2011AE5E0 AS DateTime), 60000)
INSERT [dbo].[Schedule] ([IdSchedule], [IdChannel], [IdCar], [StartTime], [ArrivalTime], [Price]) VALUES (4, 1, 1, CAST(0x0000A6F300F1B300 AS DateTime), CAST(0x0000A6F30128A180 AS DateTime), 50000)
INSERT [dbo].[Schedule] ([IdSchedule], [IdChannel], [IdCar], [StartTime], [ArrivalTime], [Price]) VALUES (5, 2, 1, CAST(0x0000A6F000F1B300 AS DateTime), CAST(0x0000A6F00128A180 AS DateTime), 60000)
SET IDENTITY_INSERT [dbo].[Schedule] OFF
ALTER TABLE [dbo].[Car]  WITH CHECK ADD FOREIGN KEY([IdCarDiagram])
REFERENCES [dbo].[CarDiagram] ([IdCarDiagram])
GO
ALTER TABLE [dbo].[Channel]  WITH CHECK ADD FOREIGN KEY([IdBusStationFrom])
REFERENCES [dbo].[BusStation] ([IdBusStation])
GO
ALTER TABLE [dbo].[Channel]  WITH CHECK ADD FOREIGN KEY([IdBusStationTo])
REFERENCES [dbo].[BusStation] ([IdBusStation])
GO
ALTER TABLE [dbo].[DetailChannel]  WITH CHECK ADD FOREIGN KEY([IdBusStation])
REFERENCES [dbo].[BusStation] ([IdBusStation])
GO
ALTER TABLE [dbo].[DetailChannel]  WITH CHECK ADD FOREIGN KEY([IdChannel])
REFERENCES [dbo].[Channel] ([IdChannel])
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([IdOrder])
REFERENCES [dbo].[Order] ([IdOrder])
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([IdSchedule])
REFERENCES [dbo].[Schedule] ([IdSchedule])
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([IdCar])
REFERENCES [dbo].[Car] ([IdCar])
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([IdChannel])
REFERENCES [dbo].[Channel] ([IdChannel])
GO
