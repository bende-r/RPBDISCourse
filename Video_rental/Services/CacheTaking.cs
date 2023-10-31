using Microsoft.Extensions.Caching.Memory;

using VideoRentalModels;

namespace Services;

public class CacheTaking : ICacheTaking
{
    private VideoRentalContext _db;
    private IMemoryCache _cache;

    public CacheTaking(VideoRentalContext context, IMemoryCache memoryCache)
    {
        _db = context;
        _cache = memoryCache;
    }

    public void AddTaking(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Taking> carModels = null;
        carModels = _db.Takings.Take(rowCount).ToList();

        if (carModels != null)
        {
            _cache.Set(cacheKey, carModels, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
            });
        }
        throw new NotImplementedException();
    }

    public IEnumerable<Taking> GetTakings(int rowCount = 20)
    {
        return _db.Takings.Take(rowCount).ToList();
    }

    public IEnumerable<Taking> GetTakings(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Taking> carModels = null;

        if (!_cache.TryGetValue(cacheKey, out carModels))
        {
            carModels = _db.Takings.Take(rowCount).ToList();

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