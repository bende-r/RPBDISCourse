using VideoRentalModels;

namespace Services;

public interface ICachePosition
{
    public IEnumerable<Position> GetPositions(int rowCount = 20);
    public void AddPosition(string cacheKey, int rowCount = 20);
    public IEnumerable<Position> GetPositions(string cacheKey, int rowCount = 20);
}