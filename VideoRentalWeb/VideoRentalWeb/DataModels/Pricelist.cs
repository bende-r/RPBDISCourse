namespace VideoRentalWeb.DataModels
{
    public partial class Pricelist
    {
        public int PriceId { get; set; }
        public int DiskId { get; set; }
        public decimal? Price { get; set; }

        public virtual Disk Disk { get; set; } = null!;

        public override string ToString()
        {
            return PriceId + ", " + DiskId + ", " + Price;
        }
    }
}