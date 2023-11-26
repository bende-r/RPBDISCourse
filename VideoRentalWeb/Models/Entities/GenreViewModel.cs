using System.ComponentModel.DataAnnotations;

using VideoRentalModels;

using VideoRentalMVC.Models.Filters;

namespace VideoRentalMVC.Models.Entities
{
    public class GenreViewModel : IEntitiesViewModel<Genre>
    {
        [Display(Name = "Genres")]
        public IEnumerable<Genre> Entities { get; set; }

        [Display(Name = "Genre")]
        public Genre Entity { get; set; }

        public string GenreTitle { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public DeleteViewModel DeleteViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }

        public GenresFilterViewModel GenresFilterViewModel { get; set; }
    }
}