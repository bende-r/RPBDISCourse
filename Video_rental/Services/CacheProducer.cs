using Microsoft.Extensions.Caching.Memory;

using VideoRentalModels;

namespace Services;

public class CacheProducer : ICacheProducer
{
    private VideoRentalContext _db;
    private IMemoryCache _cache;

    public CacheProducer(VideoRentalContext context, IMemoryCache memoryCache)
    {
        _db = context;
        _cache = memoryCache;
    }

    public IEnumerable<Producer> GetProducers(int rowCount = 20)
    {
        return _db.Producers.Take(rowCount).ToList();
    }

    public void AddProducer(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Producer> carModels = null;
        carModels = _db.Producers.Take(rowCount).ToList();

        if (carModels != null)
        {
            _cache.Set(cacheKey, carModels, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
            });
        }
    }

    public IEnumerable<Producer> GetProducers(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Producer> carModels = null;

        if (!_cache.TryGetValue(cacheKey, out carModels))
        {
            carModels = _db.Producers.Take(rowCount).ToList();

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