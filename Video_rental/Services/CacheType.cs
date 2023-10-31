using Microsoft.Extensions.Caching.Memory;

using VideoRentalModels;

using Type = VideoRentalModels.Type;

namespace Services;

public class CacheType : ICacheType
{
    private VideoRentalContext _db;
    private IMemoryCache _cache;

    public CacheType(VideoRentalContext context, IMemoryCache memoryCache)
    {
        _db = context;
        _cache = memoryCache;
    }

    public IEnumerable<Type> GetTypes(int rowCount = 20)
    {
        return _db.Types.Take(rowCount).ToList();
    }

    public void AddType(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Type> carModels = null;
        carModels = _db.Types.Take(rowCount).ToList();

        if (carModels != null)
        {
            _cache.Set(cacheKey, carModels, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
            });
        }
    }

    public IEnumerable<Type> GetTypes(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Type> carModels = null;

        if (!_cache.TryGetValue(cacheKey, out carModels))
        {
            carModels = _db.Types.Take(rowCount).ToList();

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