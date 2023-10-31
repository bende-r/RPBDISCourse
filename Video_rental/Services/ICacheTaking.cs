using VideoRentalModels;

namespace Services;

public interface ICacheTaking
{
    public IEnumerable<Taking> GetTakings(int rowCount = 20);
    public void AddTaking(string cacheKey, int rowCount = 20);
    public IEnumerable<Taking> GetTakings(string cacheKey, int rowCount = 20);
}