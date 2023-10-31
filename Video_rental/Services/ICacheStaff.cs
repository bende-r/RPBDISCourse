using VideoRentalModels;

namespace Services;

public interface ICacheStaff
{
    public IEnumerable<Staff> GetStaff(int rowCount = 20);
    public void AddStaff(string cacheKey, int rowCount = 20);
    public IEnumerable<Staff> GetStaff(string cacheKey, int rowCount = 20);
}