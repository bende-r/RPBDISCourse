namespace VideoRentalWeb.DataModels
{
    public partial class Taking
    {
        public int TakeId { get; set; }
        public int ClientId { get; set; }
        public int DiskId { get; set; }
        public DateTime DateOfCapture { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool PaymentMark { get; set; }
        public bool RefundMark { get; set; }
        public int StaffId { get; set; }

        public virtual Clientele Client { get; set; } = null!;
        public virtual Disk Disk { get; set; } = null!;
        public virtual Staff Staff { get; set; } = null!;

        public override string ToString()
        {
            return TakeId + ", " + ClientId + ", " + DiskId + ", " + DateOfCapture.ToString() + ", " +
                   ReturnDate.ToString() + ", " + PaymentMark.ToString() + ", " + RefundMark.ToString() + ", " +
                   StaffId.ToString();
        }
    }
}