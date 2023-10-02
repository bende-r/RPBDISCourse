CREATE PROCEDURE AddOrUpdateProducer
    @ProduceID int,
    @Manufacturer varchar(30),
    @Country varchar(30)
AS
BEGIN
    -- ��������� ������������� ������ �� ProduceID
    IF EXISTS (SELECT 1 FROM dbo.Producers WHERE ProduceID = @ProduceID)
    BEGIN
        -- ������ ����������, ��������� ����������
        UPDATE dbo.Producers
        SET Manufacturer = @Manufacturer,
            Country = @Country
        WHERE ProduceID = @ProduceID;
    END
    ELSE
    BEGIN
        -- ������ �� ����������, ��������� �������
        INSERT INTO dbo.Producers (Manufacturer, Country)
        VALUES (@Manufacturer, @Country);
    END
END;
