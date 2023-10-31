using Microsoft.Extensions.Caching.Memory;

using VideoRentalModels;

namespace Services;

public class CachePosition : ICachePosition
{
    private VideoRentalContext _db;
    private IMemoryCache _cache;

    public CachePosition(VideoRentalContext context, IMemoryCache memoryCache)
    {
        _db = context;
        _cache = memoryCache;
    }

    public IEnumerable<Position> GetPositions(int rowCount = 20)
    {
        return _db.Positions.Take(rowCount).ToList();
    }

    public void AddPosition(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Position> carModels = null;
        carModels = _db.Positions.Take(rowCount).ToList();

        if (carModels != null)
        {
            _cache.Set(cacheKey, carModels, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
            });
        }
    }

    public IEnumerable<Position> GetPositions(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Position> carModels = null;

        if (!_cache.TryGetValue(cacheKey, out carModels))
        {
            carModels = _db.Positions.Take(rowCount).ToList();

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