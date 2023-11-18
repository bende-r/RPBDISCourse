using VideoRentalModels;

using VideoRentalWeb.DataModels;

using Type = VideoRentalModels.Type;

namespace VideoRentalWeb.Data;

public class DbDataInitializer
{
    private static readonly char[] _letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
    private static Random _random = new Random();

    public static void Initialize(VideoRentalContext db)
    {
        int rowCount;
        int rowIndex;

        int minStringLength;
        int maxStringLength;

        db.Database.EnsureCreated();
        if (!db.Genres.Any())
        {
            rowCount = 500;
            string title;
            string description;
            for (rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                minStringLength = 4;
                maxStringLength = 20;
                title = GetString(minStringLength, maxStringLength);
                minStringLength = 50;
                maxStringLength = 1000;
                description = GetString(minStringLength, maxStringLength);
                db.Genres.Add(new Genre() { Title = title, Description = description });
            }
            db.SaveChanges();
        }
        if (!db.Types.Any())
        {
            rowCount = 500;
            string title;
            string description;

            for (rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                minStringLength = 2;
                maxStringLength = 20;
                title = GetString(minStringLength, maxStringLength);
                description = GetString(minStringLength, maxStringLength);
                db.Types.Add(new Type() { Title = title, Description = description });
            }
            db.SaveChanges();
        }
        if (!db.Disks.Any())
        {
            rowCount = 20000;

            string title;
            string creationYear;
            int producer;
            string mainActor;
            DateTime recording;
            int genreId;
            int diskType;

            for (rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                minStringLength = 2;
                maxStringLength = 20;
                title = GetString(minStringLength, maxStringLength);
                creationYear = _random.Next(1940, 2023).ToString();
                producer = _random.Next(1, 40);
                mainActor = GetString(minStringLength, maxStringLength);
                genreId = _random.Next(22, 50);
                diskType = _random.Next(1, 20);
                recording = GetDateTime(2020);

                db.Disks.Add(new Disk()
                {
                    Title = title,
                    CreationYear = creationYear,
                    Producer = producer,
                    MainActor = mainActor,
                    GenreId = genreId,
                    DiskType = diskType,
                    Recording = recording,
                });
            }
            db.SaveChanges();
        }
    }

    private static string GetString(int minStringLength, int maxStringLength)
    {
        string result = "";

        int stringLimit = minStringLength + _random.Next(maxStringLength - minStringLength);

        int stringPosition;
        for (int i = 0; i < stringLimit; i++)
        {
            stringPosition = _random.Next(_letters.Length);

            result += _letters[stringPosition];
        }

        return result;
    }

    private static DateTime GetDateTime()
    {
        DateTime start = new DateTime(1995, 1, 1);
        int range = (DateTime.Today - start).Days;

        return start.AddDays(_random.Next(range));
    }

    private static DateTime GetDateTime(int begin)
    {
        DateTime start = new DateTime(begin, 1, 1);
        int range = (DateTime.Today - start).Days;

        return start.AddDays(_random.Next(range));
    }
}