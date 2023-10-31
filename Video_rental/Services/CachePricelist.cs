using Microsoft.Extensions.Caching.Memory;

using VideoRentalModels;

namespace Services;

public class CachePricelist : ICachePricelist
{
    private VideoRentalContext _db;
    private IMemoryCache _cache;

    public CachePricelist(VideoRentalContext context, IMemoryCache memoryCache)
    {
        _db = context;
        _cache = memoryCache;
    }


    public IEnumerable<Pricelist> GetPricelist(int rowCount = 20)
    {
        return _db.Pricelists.Take(rowCount).ToList();
    }

    public void AddPrice(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Pricelist> carModels = null;
        carModels = _db.Pricelists.Take(rowCount).ToList();

        if (carModels != null)
        {
            _cache.Set(cacheKey, carModels, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
            });
        }
    }

    public IEnumerable<Pricelist> GetPricelist(string cacheKey, int rowCount = 20)
    {
        IEnumerable<Pricelist> carModels = null;

        if (!_cache.TryGetValue(cacheKey, out carModels))
        {
            carModels = _db.Pricelists.Take(rowCount).ToList();

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