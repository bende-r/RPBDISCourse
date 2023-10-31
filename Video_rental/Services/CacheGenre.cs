using Microsoft.Extensions.Caching.Memory;

using VideoRentalModels;

namespace Services;

public class CacheGenre : ICacheGenre
{
    private VideoRentalContext _db;
    private IMemoryCache _cache;

    public CacheGenre(VideoRentalContext context, IMemoryCache memoryCache)
    {
        _db = context;
        _cache = memoryCache;
    }

    public void AddGenre(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Genre> carModels = null;
        carModels = _db.Genres.Take(rowCount).ToList();

        if (carModels != null)
        {
            _cache.Set(cacheKey, carModels, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
            });
        }
    }

    public IEnumerable<Genre> GetGenres(int rowCount = 20)
    {
        return _db.Genres.Take(rowCount).ToList();
    }


    public IEnumerable<Genre> GetGenres(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Genre> carModels = null;

        if (!_cache.TryGetValue(cacheKey, out carModels))
        {
            carModels = _db.Genres.Take(rowCount).ToList();

            if (carModels != null)
            {
                _cache.Set(cacheKey, carModels, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
                });
            }
        }

        return carModels;
    }
}