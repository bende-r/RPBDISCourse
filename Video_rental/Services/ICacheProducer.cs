using VideoRentalModels;

namespace Services;

public interface ICacheProducer
{
    public IEnumerable<Producer> GetProducers(int rowCount = 20);

    public void AddProducer(string cacheKey, int rowCount = 20);

    public IEnumerable<Producer> GetProducers(string cacheKey, int rowCount = 20);
}