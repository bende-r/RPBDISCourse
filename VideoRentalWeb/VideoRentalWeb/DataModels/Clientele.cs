namespace VideoRentalWeb.DataModels
{
    public partial class Clientele
    {
        public Clientele()
        {
            Takings = new HashSet<Taking>();
        }

        public int ClientId { get; set; }
        public string Surname { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Middlename { get; set; }
        public string? Addres { get; set; }
        public string? Phone { get; set; }
        public string? Passport { get; set; }

        public virtual ICollection<Taking> Takings { get; set; }

        public override string ToString()
        {
            return ClientId + ", " + Surname + ", " + Name + ", " + Middlename + ", " + Addres + ", " + Phone + ", " + Passport;
        }
    }
}