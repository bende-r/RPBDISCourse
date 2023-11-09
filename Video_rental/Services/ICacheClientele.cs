using VideoRentalModels;

namespace Services;

public interface ICacheClientele
{
    public IEnumerable<Clientele> GetClientele(int rowCount = 20);

    public void AddClient(string cacheKey, int rowCount = 20);

    public IEnumerable<Clientele> GetClientele(string cacheKey, int rowCount = 20);
}