namespace CRUD;
using VideoRentalModels;

public class Update
{
    public void UpdateClientele(VideoRentalContext db, Clientele upd)
    {
        db.Clienteles.Update(upd);
        db.SaveChanges();
    }

    public void UpdateDisk(VideoRentalContext db, Disk upd)
    {
        db.Disks.Update(upd);
        db.SaveChanges();
    }

    public void UpdateGenre(VideoRentalContext db, Genre upd)
    {
        db.Genres.Update(upd);
        db.SaveChanges();
    }

    public void UpdatePosition(VideoRentalContext db, Position upd)
    {
        db.Positions.Update(upd);
        db.SaveChanges();
    }

    public void UpdatePricelist(VideoRentalContext db, Pricelist upd)
    {
        db.Pricelists.Update(upd);
        db.SaveChanges();
    }

    public void UpdateProducer(VideoRentalContext db, Producer upd)
    {
        db.Producers.Update(upd);
        db.SaveChanges();
    }

    public void UpdateStaff(VideoRentalContext db, Staff upd)
    {
        db.Staff.Update(upd);
        db.SaveChanges();
    }

    public void UpdateTaking(VideoRentalContext db, Taking upd)
    {
        db.Takings.Update(upd);
        db.SaveChanges();
    }

    public void UpdateType(VideoRentalContext db, Type upd)
    {
        db.Types.Update(upd);
        db.SaveChanges();
    }
}