-- Проверка существования базы данных
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'video_rental')
BEGIN
    -- Создание базы данных, если она не существует
    CREATE DATABASE video_rental;
END

USE video_rental;

-- Проверка существования таблицы
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'genre' )
BEGIN
    -- Создание таблицы, если она не существует
    CREATE TABLE genre (
        id int not null PRIMARY KEY IDENTITY(1,1),-- Определение первичного ключа
        title varchar(30) not null,
		description varchar(100),
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'disc_type')
BEGIN
    CREATE TABLE disc_type (
        id int not null PRIMARY KEY IDENTITY(1,1),
        title varchar(30) not null,
        description varchar(100),
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'discs')
BEGIN
    CREATE TABLE discs (
        id int not null PRIMARY KEY IDENTITY(1,1),
        title varchar(30) not null,
        creation_year varchar(4),
		producer int not null,
		main_actor varchar(90) not null,
		recording date not null,
		genre_id int not null,
		type int not null,
    );
END


IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'pricelist')
BEGIN
    CREATE TABLE pricelist (
         id int not null PRIMARY KEY IDENTITY(1,1),
		 disc int not null,
         price money CHECK(price >0),
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'produce')
BEGIN
    CREATE TABLE produce (
		id int not null PRIMARY KEY IDENTITY(1,1),
        manufacturer varchar(30) not null,
        country varchar(30)
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'positions')
BEGIN
    CREATE TABLE positions (
		id int not null PRIMARY KEY IDENTITY(1,1),
        title varchar(30) not null,
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'staff')
BEGIN
    CREATE TABLE staff (
		id int not null PRIMARY KEY IDENTITY(1,1),
        surname varchar(30) not null,
		name varchar(30) not null,
		middlename varchar(30),
		position int,
		date_of_employment date
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'clientele')
BEGIN
    CREATE TABLE clientele (
		id int not null PRIMARY KEY IDENTITY(1,1),
        surname varchar(30) not null,
		name varchar(30) not null,
		middlename varchar(30),
		addres varchar(50),
		phone varchar(15),
		passport varchar(10),
		position int,
		date_of_employment date
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'taking')
BEGIN
    CREATE TABLE taking (
		id int not null PRIMARY KEY IDENTITY(1,1),
		client_id int not null,
        disk_id int not null,
		date_of_capture date not null,
		return_date date not null,
		payment_mark bit not null,
		refund_mark bit not null CONSTRAINT CK_refund_mark CHECK (refund_mark IN (0, 1)),
		employee_id int not null
    );
END
