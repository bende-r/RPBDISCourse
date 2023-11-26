namespace VideoRentalWeb.DataModels
{
    public partial class Producer
    {
        public Producer()
        {
            Disks = new HashSet<Disk>();
        }

        public int ProduceId { get; set; }
        public string Manufacturer { get; set; } = null!;
        public string? Country { get; set; }

        public virtual ICollection<Disk> Disks { get; set; }

        public override string ToString()
        {
            return ProduceId + ", " + Manufacturer + ", " + Country;
        }
    }
}