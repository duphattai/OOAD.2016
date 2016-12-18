CREATE DATABASE CarManager
GO
USE CarManager
GO


/* Management Account  */
SET DATEFORMAT dmy;

CREATE TABLE [Role]
(
	IdRole		int primary key,
	RoleName	varchar(255),
	Note		nvarchar(Max)
)

CREATE TABLE Account
(
	IdAccount	int PRIMARY KEY IDENTITY (1,1),
	UserName	varchar(100),
	Pass		varchar(20),
	IdRoles		varchar(100),
	FullName    nvarchar(255),
	CMND		varchar(20),
	Phone       varchar(20),
	[Address]	nvarchar(Max)	
)

/* Car diagram */
CREATE TABLE CarDiagram
(
	IdCarDiagram int PRIMARY KEY IDENTITY(1,1),
	NumberFloors int NOT NULL,
	SeatDiagram  varchar(max) NOT NULL,
	TypeSeat     nvarchar(255)
)

CREATE TABLE Car
(
	IdCar int primary key identity(1,1),
	CarNumberPlate varchar(20) NOT NULL,
	TotalSeat int,
	IdCarDiagram int foreign key references CarDiagram(IdCarDiagram) NOT NULL
)


CREATE TABLE BusStation
(
	IdBusStation int PRIMARY KEY IDENTITY(1,1),
	Name nvarchar(255)
)

CREATE TABLE Channel
(
	IdChannel int primary key identity(1,1),
	IdBusStationFrom int foreign key references BusStation(IdBusStation) NOT NULL,
	IdBusStationTo int foreign key references BusStation(IdBusStation) NOT NULL,
	RunTime int not null,
	DefaultPrice float not null
)

CREATE TABLE DetailChannel
(
	IdChannel int foreign key references Channel(IdChannel) NOT NULL,
	IdBusStation int foreign key references BusStation(IdBusStation) NOT NULL,
	[Time]			int,
	Note nvarchar(255),

	PRIMARY KEY (IdChannel, IdBusStation)
)


CREATE TABLE Schedule
(
	IdSchedule	int primary key identity(1,1),
	IdChannel	int foreign key references Channel(IdChannel) NOT NULL,
	IdCar		int foreign key references Car(IdCar) NOT NULL,
	StartTime	DATETIME,
	ArrivalTime DATETIME,
	Price		float NOT NULL
)

CREATE TABLE [Order]
(
	IdOrder		int primary key identity(1,1),
	OrderName	nvarchar(255),
	PhoneNumber varchar(50),
	OrderDate	datetime,
	Note		nvarchar(max)
)

CREATE TABLE OrderDetail
(
	IdOrderDetail int primary key identity(1,1),
	IdOrder int foreign key references [Order](IdOrder) NOT NULL,
	IdSchedule INT FOREIGN KEY REFERENCES Schedule(IdSchedule) not null,
	IsPaid bit,
	SeatNumber int,
	PaymentDate DATETIME,
)