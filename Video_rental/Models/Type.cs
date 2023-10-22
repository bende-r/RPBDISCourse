namespace VideoRentalModels
{
    public partial class Type
    {
        public Type()
        {
            Disks = new HashSet<Disk>();
        }

        public int TypeId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Disk> Disks { get; set; }

        public override string ToString()
        {
            return TypeId + ", " + Title + ", " + Description;
        }
    }
}
