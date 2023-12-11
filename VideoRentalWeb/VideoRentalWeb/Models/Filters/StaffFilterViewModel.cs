using System.ComponentModel.DataAnnotations;

namespace VideoRentalWeb.Models.Filters
{
    public class StaffFilterViewModel
    {
        [Display(Name = "Surname")]
        public string Surname { get; set; } = null!;

        [Display(Name = "Name")]
        public string Name { get; set; } = null!;

        [Display(Name = "Middlename")]
        public string Middlename { get; set; }

        [Display(Name = "PositionId")]
        public int PositionId { get; set; }

        [Display(Name = "DateOfEmployment Date")]
        public DateTime ReDateOfEmploymentcording { get; set; }
    }
}