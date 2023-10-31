using Microsoft.Extensions.Caching.Memory;

using VideoRentalModels;

namespace Services;

public class CacheClientele : ICacheClientele
{
    private VideoRentalContext _db;
    private IMemoryCache _cache;

    public CacheClientele(VideoRentalContext context, IMemoryCache memoryCache)
    {
        _db = context;
        _cache = memoryCache;
    }

    public IEnumerable<Clientele> GetClientele(int rowCount = 20)
    {
        return _db.Clienteles.Take(rowCount).ToList();
    }

    public void AddClient(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Clientele> carModels = null;
        carModels = _db.Clienteles.Take(rowCount).ToList();

        if (carModels != null)
        {
            _cache.Set(cacheKey, carModels, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
            });
        }
    }

    public IEnumerable<Clientele> GetClientele(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Clientele> carModels;

        if (!_cache.TryGetValue(cacheKey, out carModels))
        {
            carModels = _db.Clienteles.Take(rowCount).ToList();

            if (carModels != null)
            {
                _cache.Set(cacheKey, carModels, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
        }
        return carModels;
    }
}