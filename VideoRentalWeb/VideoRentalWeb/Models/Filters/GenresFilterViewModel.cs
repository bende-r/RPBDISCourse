using System.ComponentModel.DataAnnotations;

namespace VideoRentalWeb.Models.Filters
{
    public class GenresFilterViewModel
    {
        [Display(Name = "Title")]
        public string GenreTitle { get; set; } = null!;
    }
}