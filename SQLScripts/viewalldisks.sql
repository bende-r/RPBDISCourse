CREATE VIEW [dbo].[View_AllDisks]
AS
SELECT dbo.Disks.DiskID, dbo.Disks."Title" as DiskTitle, dbo.Disks.CreationYear, dbo.Producers.Manufacturer, dbo.Producers.Country, dbo.Disks.MainActor, dbo.Disks.Recording, dbo.Genres.Title AS GenreTitle, dbo."Types".Title AS TypeTitle
FROM dbo.Disks 
INNER JOIN dbo.Genres ON dbo.Disks.GenreID = dbo.Genres.GenreID
INNER JOIN dbo.Producers ON dbo.Disks.Producer = dbo.Producers.ProduceID
INNER JOIN dbo."Types" ON dbo.Disks.DiskType = dbo."Types".TypeID
GO