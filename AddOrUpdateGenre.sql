CREATE PROCEDURE AddOrUpdateGenre
    @GenreID int,
    @Title varchar(30),
    @Description varchar(100)
AS
BEGIN
    -- ��������� ������������� ������ �� GenreID
    IF EXISTS (SELECT 1 FROM dbo.Genres WHERE GenreID = @GenreID)
    BEGIN
        -- ������ ����������, ��������� ����������
        UPDATE dbo.Genres
        SET Title = @Title,
            [Description] = @Description
        WHERE GenreID = @GenreID;
    END
    ELSE
    BEGIN
        -- ������ �� ����������, ��������� �������
        INSERT INTO dbo.Genres (Title, [Description])
        VALUES (@Title, @Description);
    END
END;