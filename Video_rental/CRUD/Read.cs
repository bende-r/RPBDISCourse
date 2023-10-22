using System.Xml.Serialization;

using VideoRentalModels;

namespace CRUD
{
    public class Read
    {
        public List<Clientele> ReadClienteles(VideoRentalContext db)
        {
            var result = from res in db.Clienteles
                         select res;
            return result.ToList();
        }

        public List<Disk> ReadDisks(VideoRentalContext db)
        {
            var result = from res in db.Disks
                         select res;
            return result.ToList();
        }

        public List<Genre> ReadGenres(VideoRentalContext db)
        {
            var result = from res in db.Genres
                         select res;
            return result.ToList();
        }

        public List<Position> ReadPositions(VideoRentalContext db)
        {
            var result = from res in db.Positions
                         select res;
            return result.ToList();
        }

        public List<Pricelist> ReadPricelist(VideoRentalContext db)
        {
            var result = from res in db.Pricelists
                         select res;
            return result.ToList();
        }

        public List<Producer> ReadProducers(VideoRentalContext db)
        {
            var result = from res in db.Producers
                         select res;
            return result.ToList();
        }

        public List<Staff> ReadStaff(VideoRentalContext db)
        {
            var result = from res in db.Staff
                         select res;
            return result.ToList();
        }

        public List<Taking> ReadTaking(VideoRentalContext db)
        {
            var result = from res in db.Takings
                         select res;
            return result.ToList();
        }

        public List<VideoRentalModels.Type> ReadTypes(VideoRentalContext db)
        {
            var result = from res in db.Types
                         select res;
            return result.ToList();
        }
        public List<ViewAllDisk> ViewAllDisks(VideoRentalContext db)
        {
            var result = from res in db.ViewAllDisks
                         select res;
            return result.ToList();
        }
    }
}

