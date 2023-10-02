CREATE PROCEDURE AddOrUpdateType
    @TypeID int,
    @Title varchar(30),
    @Description varchar(100)
AS
BEGIN
    -- ѕровер€ем существование записи по TypeID
    IF EXISTS (SELECT 1 FROM dbo."Types" WHERE TypeID = @TypeID)
    BEGIN
        -- «апись существует, выполн€ем обновление
        UPDATE dbo."Types"
        SET Title = @Title,
            "Description" = @Description
        WHERE TypeID = @TypeID;
    END
    ELSE
    BEGIN
        -- «апись не существует, выполн€ем вставку
        INSERT INTO dbo."Types" (Title, "Description")
        VALUES (@Title, @Description);
    END
END;
