using VideoRentalModels;

namespace Services;

public interface ICachePricelist
{
    public IEnumerable<Pricelist> GetPricelist(int rowCount = 20);

    public void AddPrice(string cacheKey, int rowCount = 20);

    public IEnumerable<Pricelist> GetPricelist(string cacheKey, int rowCount = 20);
}