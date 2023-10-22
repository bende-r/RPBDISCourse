namespace VideoRentalModels
{
    public partial class ViewAllDisk
    {
        public int DiskId { get; set; }
        public string DiskTitle { get; set; } = null!;
        public string? CreationYear { get; set; }
        public string Manufacturer { get; set; } = null!;
        public string? Country { get; set; }
        public string MainActor { get; set; } = null!;
        public DateTime Recording { get; set; }
        public string GenreTitle { get; set; } = null!;
        public string TypeTitle { get; set; } = null!;

        public override string ToString()
        {
            return DiskId + ", " + DiskTitle + ", " + CreationYear.ToString() + ", " + Manufacturer + ", " + Country + ", " + MainActor + ", " + Recording.ToString() + ", " + GenreTitle + ", " + TypeTitle;
        }
    }
}