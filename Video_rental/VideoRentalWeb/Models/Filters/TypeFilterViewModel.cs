using System.ComponentModel.DataAnnotations;

namespace VideoRentalMVC.Models.Filters
{
    public class TypeFilterViewModel
    {
        [Display(Name = "Title")]
        public string TypeTitle { get; set; } = null!;
    }
}