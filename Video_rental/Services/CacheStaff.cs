using Microsoft.Extensions.Caching.Memory;

using VideoRentalModels;

namespace Services;

public class CacheStaff : ICacheStaff
{
    private VideoRentalContext _db;
    private IMemoryCache _cache;

    public CacheStaff(VideoRentalContext context, IMemoryCache memoryCache)
    {
        _db = context;
        _cache = memoryCache;
    }

    public IEnumerable<Staff> GetStaff(int rowCount = 20)
    {
        return _db.Staff.Take(rowCount).ToList();
    }

    public void AddStaff(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Staff> carModels = null;
        carModels = _db.Staff.Take(rowCount).ToList();

        if (carModels != null)
        {
            _cache.Set(cacheKey, carModels, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
            });
        }
    }

    public IEnumerable<Staff> GetStaff(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Staff> carModels = null;

        if (!_cache.TryGetValue(cacheKey, out carModels))
        {
            carModels = _db.Staff.Take(rowCount).ToList();

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