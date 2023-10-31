using VideoRentalModels;

namespace Services;

public interface ICacheGenre
{
    public IEnumerable<Genre> GetGenres(int rowCount = 20);
    public void AddGenre(string cacheKey, int rowCount = 20);
    public IEnumerable<Genre> GetGenres(string cacheKey, int rowCount = 20);
}