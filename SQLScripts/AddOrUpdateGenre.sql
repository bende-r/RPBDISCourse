CREATE PROCEDURE AddOrUpdateGenre
    @GenreID int,
    @Title varchar(30),
    @Description varchar(100)
AS
BEGIN
    -- ѕровер€ем существование записи по GenreID
    IF EXISTS (SELECT 1 FROM dbo.Genres WHERE GenreID = @GenreID)
    BEGIN
        -- «апись существует, выполн€ем обновление
        UPDATE dbo.Genres
        SET Title = @Title,
            [Description] = @Description
        WHERE GenreID = @GenreID;
    END
    ELSE
    BEGIN
        -- «апись не существует, выполн€ем вставку
        INSERT INTO dbo.Genres (Title, [Description])
        VALUES (@Title, @Description);
    END
END;