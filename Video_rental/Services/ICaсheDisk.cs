using VideoRentalModels;

namespace Services;

public interface ICaсheDisk
{
    public IEnumerable<Disk> GetDisks(int rowCount = 20);
    public void AddDisk(string cacheKey, int rowCount = 20);
    public IEnumerable<Disk> GetDisks(string cacheKey, int rowCount = 20);
}