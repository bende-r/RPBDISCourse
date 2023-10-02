USE master
    CREATE DATABASE video_rental;
GO

ALTER DATABASE video_rental SET RECOVERY SIMPLE
GO

USE video_rental;

CREATE TABLE dbo.Genres (
        GenreID int not null PRIMARY KEY IDENTITY(1,1),
        Title varchar(30) not null,
		"Description" varchar(100)
		);

CREATE TABLE dbo."Types" (
        TypeID int not null PRIMARY KEY IDENTITY(1,1),
        Title varchar(30) not null,
        "Description" varchar(100)
		);

CREATE TABLE dbo.Producers (
		ProduceID int not null PRIMARY KEY IDENTITY(1,1),
        Manufacturer varchar(30) not null,
        Country varchar(30)
		);

CREATE TABLE dbo.Disks (
        DiskID int not null PRIMARY KEY IDENTITY(1,1),
        Title varchar(30) not null,
        CreationYear varchar(4),
		Producer int not null  ,
		MainActor varchar(90) not null,
		Recording date not null,
		GenreID int not null,
		DiskType int not null,
		FOREIGN KEY (Producer) REFERENCES Producers(ProduceID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
		FOREIGN KEY (GenreID) REFERENCES Genres(GenreID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
		FOREIGN KEY (DiskType) REFERENCES "Types"(TypeID)
		ON DELETE CASCADE
		ON UPDATE CASCADE)
		;

CREATE TABLE dbo.Pricelist (
        PriceID int not null PRIMARY KEY IDENTITY(1,1),
		DiskID int not null,
        Price money CHECK(price >0),
		FOREIGN KEY (DiskID) REFERENCES Disks(DiskID)
		ON DELETE CASCADE
		ON UPDATE CASCADE
		);

CREATE TABLE dbo.Positions (
		PositionID int not null PRIMARY KEY IDENTITY(1,1),
        Title varchar(30) not null
		);

CREATE TABLE dbo.Staff (
		StaffID int not null PRIMARY KEY IDENTITY(1,1),
        Surname varchar(30) not null,
		"Name" varchar(30) not null,
		Middlename varchar(30),
		PositionID int,
		DateOfEmployment date,
		FOREIGN KEY (PositionID) REFERENCES Positions(PositionID)
		ON DELETE CASCADE
		ON UPDATE CASCADE
		);

CREATE TABLE dbo.Clientele (
		ClientID int not null PRIMARY KEY IDENTITY(1,1),
        Surname varchar(30) not null,
		"Name" varchar(30) not null,
		Middlename varchar(30),
		Addres varchar(50),
		Phone varchar(15),
		Passport varchar(10)
		);

CREATE TABLE dbo.Taking (
		TakeID int not null PRIMARY KEY IDENTITY(1,1),
		ClientID int not null,
        DiskID int not null,
		DateOfCapture date not null,
		ReturnDate date not null,
		PaymentMark bit not null,
		RefundMark bit not null CONSTRAINT CK_refund_mark CHECK (RefundMark IN (0, 1)),
		StaffID int not null,
		FOREIGN KEY (ClientID) REFERENCES Clientele(ClientID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
		FOREIGN KEY (DiskID) REFERENCES Disks(DiskID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
		FOREIGN KEY (StaffID) REFERENCES Staff(StaffId)
		ON DELETE CASCADE
		ON UPDATE CASCADE
		);
GO

CREATE VIEW [dbo].[View_AllDisks]
AS
SELECT dbo.Disks.DiskID, dbo.Disks."Title" as DiskTitle, dbo.Disks.CreationYear, dbo.Producers.Manufacturer, dbo.Producers.Country, dbo.Disks.MainActor, dbo.Disks.Recording, dbo.Genres.Title AS GenreTitle, dbo."Types".Title AS TypeTitle
FROM dbo.Disks 
INNER JOIN dbo.Genres ON dbo.Disks.GenreID = dbo.Genres.GenreID
INNER JOIN dbo.Producers ON dbo.Disks.Producer = dbo.Producers.ProduceID
INNER JOIN dbo."Types" ON dbo.Disks.DiskType = dbo."Types".TypeID
GO