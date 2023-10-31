using Type = VideoRentalModels.Type;

namespace Services;

public interface ICacheType
{
    public IEnumerable<Type> GetTypes(int rowCount = 20);
    public void AddType(string cacheKey, int rowCount = 20);
    public IEnumerable<Type> GetTypes(string cacheKey, int rowCount = 20);
}