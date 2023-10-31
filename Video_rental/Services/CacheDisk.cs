using Microsoft.Extensions.Caching.Memory;

using VideoRentalModels;

namespace Services;

public class CacheDisk : ICaсheDisk
{
    private VideoRentalContext _db;
    private IMemoryCache _cache;

    public CacheDisk(VideoRentalContext context, IMemoryCache memoryCache)
    {
        _db = context;
        _cache = memoryCache;
    }

    public IEnumerable<Disk> GetDisks(int rowCount = 20)
    {
        return _db.Disks.Take(rowCount).ToList();
    }

    public void AddDisk(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Disk> carModels = null;
        carModels = _db.Disks.Take(rowCount).ToList();

        if (carModels != null)
        {
            _cache.Set(cacheKey, carModels, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
            });
        }
    }

    public IEnumerable<Disk> GetDisks(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Disk> carModels = null;

        if (!_cache.TryGetValue(cacheKey, out carModels))
        {
            carModels = _db.Disks.Take(rowCount).ToList();

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