CREATE PROCEDURE AddOrUpdateProducer
    @ProduceID int,
    @Manufacturer varchar(30),
    @Country varchar(30)
AS
BEGIN
    -- ѕровер€ем существование записи по ProduceID
    IF EXISTS (SELECT 1 FROM dbo.Producers WHERE ProduceID = @ProduceID)
    BEGIN
        -- «апись существует, выполн€ем обновление
        UPDATE dbo.Producers
        SET Manufacturer = @Manufacturer,
            Country = @Country
        WHERE ProduceID = @ProduceID;
    END
    ELSE
    BEGIN
        -- «апись не существует, выполн€ем вставку
        INSERT INTO dbo.Producers (Manufacturer, Country)
        VALUES (@Manufacturer, @Country);
    END
END;
