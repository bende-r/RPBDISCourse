CREATE PROCEDURE AddOrUpdateType
    @TypeID int,
    @Title varchar(30),
    @Description varchar(100)
AS
BEGIN
    -- ��������� ������������� ������ �� TypeID
    IF EXISTS (SELECT 1 FROM dbo."Types" WHERE TypeID = @TypeID)
    BEGIN
        -- ������ ����������, ��������� ����������
        UPDATE dbo."Types"
        SET Title = @Title,
            "Description" = @Description
        WHERE TypeID = @TypeID;
    END
    ELSE
    BEGIN
        -- ������ �� ����������, ��������� �������
        INSERT INTO dbo."Types" (Title, "Description")
        VALUES (@Title, @Description);
    END
END;
