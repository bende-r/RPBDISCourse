namespace VideoRentalModels
{
    public partial class Staff
    {
        public Staff()
        {
            Takings = new HashSet<Taking>();
        }

        public int StaffId { get; set; }
        public string Surname { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Middlename { get; set; }
        public int? PositionId { get; set; }
        public DateTime? DateOfEmployment { get; set; }

        public virtual Position? Position { get; set; }
        public virtual ICollection<Taking> Takings { get; set; }

        public override string ToString()
        {
            return StaffId + ", " + Surname + ", " + Name + ", " + Middlename + ", " + PositionId + ", " +
                   DateOfEmployment.ToString();
        }
    }
}