using System.ComponentModel.DataAnnotations;

namespace VideoRentalWeb.Models.Filters
{
    public class ClienteleFilterViewModel
    {
        [Display(Name = "Surname")]
        public string Surname { get; set; } = null!;

        [Display(Name = "Name")]
        public string Name { get; set; } = null!;

        [Display(Name = "Middlename")]
        public string  Middlename { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Phone")]
        public string Phone{ get; set; }

        [Display(Name = "Passport")]
        public  string Passport { get; set; }
    }
}