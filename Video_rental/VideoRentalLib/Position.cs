namespace VideoRentalModels
{
    public partial class Position
    {
        public Position()
        {
            staff = new HashSet<Staff>();
        }

        public int PositionId { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<Staff> staff { get; set; }

        public override string ToString()
        {
            return PositionId + ", " + Title;
        }
    }
}