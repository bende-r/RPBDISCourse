using System.ComponentModel.DataAnnotations;

namespace VideoRentalWeb.Models.Filters
{
    public class ProducersFilterViewModel
    {
        [Display(Name = "Manufacturer")]
        public string Manufacturer { get; set; } = null!;

        [Display(Name = "Country")]
        public string Country { get; set; } 
    }
}