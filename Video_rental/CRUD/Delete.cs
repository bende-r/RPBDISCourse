using VideoRentalModels;

using Type = VideoRentalModels.Type;

namespace CRUD;
public class Delete
{
    public void RemoveClient(VideoRentalContext db, int index)
    {
        var res = from r in db.Clienteles
                  where index == r.ClientId
                  select r;
        db.Clienteles.Remove(res.First());
        db.SaveChanges();
    }

    public void RemoveDisk(VideoRentalContext db, int index)
    {
        var res = from r in db.Disks
                  where index == r.DiskId
                  select r;
        db.Disks.Remove(res.First());
        db.SaveChanges();
    }

    public void RemoveGenre(VideoRentalContext db, int index)
    {
        var res = from r in db.Genres
                  where index == r.GenreId
                  select r;
        db.Genres.Remove(res.First());
        db.SaveChanges();
    }

    public void RemovePosition(VideoRentalContext db, int index)
    {
        var res = from r in db.Positions
                  where index == r.PositionId
                  select r;
        db.Positions.Remove(res.First());
        db.SaveChanges();
    }

    public void RemovePrice(VideoRentalContext db, int index)
    {
        var res = from r in db.Pricelists
                  where index == r.PriceId
                  select r;
        db.Pricelists.Remove(res.First());
        db.SaveChanges();
    }

    public void RemoveProducer(VideoRentalContext db, int index)
    {
        var res = from r in db.Producers
                  where index == r.ProduceId
                  select r;
        db.Producers.Remove(res.First());
        db.SaveChanges();
    }

    public void RemoveStaff(VideoRentalContext db, int index)
    {
        var res = from r in db.Staff
                  where index == r.StaffId
                  select r;
        db.Staff.Remove(res.First());
        db.SaveChanges();
    }

    public void RemoveTaking(VideoRentalContext db, int index)
    {
        var res = from r in db.Takings
                  where index == r.TakeId
                  select r;
        db.Takings.Remove(res.First());
        db.SaveChanges();
    }

    public void RemoveType(VideoRentalContext db, int index)
    {
        var res = from r in db.Types
                  where index == r.TypeId
                  select r;
        db.Types.Remove(res.First());
        db.SaveChanges();
    }
}