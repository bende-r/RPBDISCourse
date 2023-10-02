USE video_rental

DECLARE @Symbol CHAR(52)= 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz',
		@Position int,
		@i int,
		@NameLimit int,
		@GenreTitle varchar(30),
		@GenreDescription varchar(100),
		@TypeTitle varchar(30),
		@TypeDescription varchar(100),
		@Manufacturer varchar(30),
		@Country varchar(30),
	    @DiscTitle varchar(30),
        @CreationYear varchar(4),
		@Producer int,
		@MainActor varchar(90),
		@Recording date,
		@GenreID int,
		@DiskType int,
		
		@RowCount INT,
		@NumberGenres int,
		@NumberTypes int,
		@NumberDisks int,
		@NumberProdusers int,
		@MinNumberSymbols int,
		@MaxNumberSymbols int

SET @NumberGenres =10
SET @NumberProdusers =20
SET @NumberTypes =  10
SET @NumberDisks =  100

BEGIN TRAN

SELECT @i=0 FROM dbo.Genres WITH (TABLOCKX) WHERE 1=0

-- Заполнение жанров
	SET @RowCount=1
	SET @MinNumberSymbols=5
	SET @MaxNumberSymbols=50	
	WHILE @RowCount<=@NumberGenres
	BEGIN		

		SET @NameLimit=@MinNumberSymbols+RAND()*(@MaxNumberSymbols-@MinNumberSymbols) -- имя от 5 до 50 символов
		SET @i=1
        SET @GenreTitle=''
		SET @GenreDescription=''
		WHILE @i<=@NameLimit
		BEGIN
			SET @Position=RAND()*52
			SET @GenreTitle = @GenreTitle + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @GenreDescription = @GenreDescription + SUBSTRING(@Symbol, @Position, 1)
			SET @i=@i+1
		END

		INSERT INTO dbo.Genres(Title, Description) VALUES (ISNULL(@GenreTitle, ''), ISNULL(@GenreDescription, ''))
		
		SET @RowCount +=1
	END

-- Заполнение типов дисков 
SELECT @i=0 FROM dbo."Types" WITH (TABLOCKX) WHERE 1=0
	SET @RowCount=1
	SET @MinNumberSymbols=5
	SET @MaxNumberSymbols=20	
	
	WHILE @RowCount<=@NumberTypes
	BEGIN	

		SET @NameLimit=@MinNumberSymbols+RAND()*(@MaxNumberSymbols-@MinNumberSymbols)  -- имя от 5 до 20 символов
		SET @i=1
		SET @TypeTitle=''
		SET @TypeDescription=''
		WHILE @i<=@NameLimit
		BEGIN
			SET @Position=RAND()*52
			SET @TypeTitle = @TypeTitle + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @TypeDescription=@TypeDescription + SUBSTRING(@Symbol, @Position, 1)
			SET @i=@i+1
		END

		INSERT INTO dbo."Types"(Title, "Description") 
		SELECT @TypeTitle, @TypeDescription
		
		SET @RowCount +=1
	END

-- Заполнение типов дисков 
SELECT @i=0 FROM dbo.Producers WITH (TABLOCKX) WHERE 1=0
	SET @RowCount=1
	SET @MinNumberSymbols=5
	SET @MaxNumberSymbols=20	
	
	WHILE @RowCount<=@NumberProdusers
	BEGIN	

		SET @NameLimit=@MinNumberSymbols+RAND()*(@MaxNumberSymbols-@MinNumberSymbols)  -- имя от 5 до 20 символов
		SET @i=1
		SET @Country=''
		SET @Manufacturer=''
		WHILE @i<=@NameLimit
		BEGIN
			SET @Position=RAND()*52
			SET @Country = @Country + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @Manufacturer = @Manufacturer + SUBSTRING(@Symbol, @Position, 1)
			SET @i=@i+1
		END

		INSERT INTO dbo.Producers(Manufacturer, Country) SELECT @Manufacturer, @Country
		
		SET @RowCount +=1
	END

-- Заполнение дисков


SET @RowCount=1
	SET @MinNumberSymbols=5
	SET @MaxNumberSymbols=50	
	WHILE @RowCount<=@NumberGenres
	BEGIN		

		SET @NameLimit=@MinNumberSymbols+RAND()*(@MaxNumberSymbols-@MinNumberSymbols) -- имя от 5 до 50 символов
		SET @i=1
        SET @GenreTitle=''
		SET @GenreDescription=''
		WHILE @i<=@NameLimit
		BEGIN
			SET @Position=RAND()*52
			SET @GenreTitle = @GenreTitle + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @GenreDescription = @GenreDescription + SUBSTRING(@Symbol, @Position, 1)
			SET @i=@i+1
		END

		INSERT INTO dbo.Genres(Title, Description) VALUES (ISNULL(@GenreTitle, ''), ISNULL(@GenreDescription, ''))
		
		SET @RowCount +=1
	END


SELECT @RowCount=1 FROM dbo.Disks WITH (TABLOCKX) WHERE 1=0
	SET @MinNumberSymbols=5
	SET @MaxNumberSymbols=50	
	
	WHILE @RowCount<=@NumberDisks
	BEGIN
		SET @NameLimit=@MinNumberSymbols+RAND()*(@MaxNumberSymbols-@MinNumberSymbols) -- имя от 5 до 50 символов
		SET @i=1
		SET @DiscTitle = ''
		SET @MainActor = ''
		WHILE @i<=@NameLimit
		BEGIN
			SET @Position=RAND()*52
			SET @DiscTitle = @DiscTitle + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @MainActor = @MainActor + SUBSTRING(@Symbol, @Position, 1)
			SET @i=@i+1
		END

		SET @CreationYear = FLOOR(RAND() * (2024 - 1980 + 1)) + 1980
		SET @Recording = dateadd(day,-RAND()*15000,GETDATE())
	
		SET @Producer=CAST( (1+RAND()*(@NumberProdusers-1)) as int)
		SET @DiskType=CAST( (1+RAND()*(@NumberTypes-1)) as int)
		SET @GenreID =CAST( (1+RAND()*(@NumberGenres-1)) as int)

		INSERT INTO dbo.Disks(Title,CreationYear,Producer,MainActor,Recording,GenreID,DiskType)
		SELECT @DiscTitle, @CreationYear, @Producer, @MainActor, @Recording, @GenreID, @DiskType
	
		SET @RowCount +=1
	END

COMMIT TRAN
GO