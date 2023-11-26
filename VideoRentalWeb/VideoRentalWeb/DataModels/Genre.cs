namespace VideoRentalWeb.DataModels
{
    public partial class Genre
    {
        public Genre()
        {
            Disks = new HashSet<Disk>();
        }

        public int GenreId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Disk> Disks { get; set; }

        public override string ToString()
        {
            return GenreId + ", " + Title + ", " + Description;
        }
    }
}