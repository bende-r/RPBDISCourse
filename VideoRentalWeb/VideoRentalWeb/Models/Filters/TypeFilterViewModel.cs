using System.ComponentModel.DataAnnotations;

namespace VideoRentalWeb.Models.Filters
{
    public class TypeFilterViewModel
    {
        [Display(Name = "Title")]
        public string TypeTitle { get; set; } = null!;
    }
}