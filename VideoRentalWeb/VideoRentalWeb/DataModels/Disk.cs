namespace VideoRentalWeb.DataModels
{
    public partial class Disk
    {
        public Disk()
        {
            Pricelists = new HashSet<Pricelist>();
            Takings = new HashSet<Taking>();
        }

        public int DiskId { get; set; }
        public string Title { get; set; } = null!;
        public string? CreationYear { get; set; }
        public int Producer { get; set; }
        public string MainActor { get; set; } = null!;
        public DateTime Recording { get; set; }
        public int GenreId { get; set; }
        public int DiskType { get; set; }

        public virtual Type DiskTypeNavigation { get; set; } = null!;
        public virtual Genre Genre { get; set; } = null!;
        public virtual Producer ProducerNavigation { get; set; } = null!;
        public virtual ICollection<Pricelist> Pricelists { get; set; }
        public virtual ICollection<Taking> Takings { get; set; }

        public override string ToString()
        {
            return DiskId + ", " + Title + ", " + CreationYear + ", " + Producer + ", " + MainActor + ", " + Recording.ToString() + ", " + GenreId + ", " + DiskType;
        }
    }
}