using System.ComponentModel.DataAnnotations;

namespace VideoRentalWeb.Models.Filters
{
    public class PositionsFilterViewModel
    {
        [Display(Name = "Title")]
        public string Title { get; set; } = null!;
    }
}