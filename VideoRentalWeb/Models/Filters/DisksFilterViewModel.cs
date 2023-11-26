using System.ComponentModel.DataAnnotations;

namespace VideoRentalMVC.Models.Filters
{
    public class DisksFilterViewModel
    {
        [Display(Name = "Title")]
        public string DiskTitle { get; set; } = null!;

        [Display(Name = "CreationYear")]
        public string DiskCreationYear { get; set; } = null!;

        [Display(Name = "Producer")]
        public int DiskProducer { get; set; }

        [Display(Name = "MainActor")]
        public string MainActor { get; set; } = null!;

        [Display(Name = "Recording Date")]
        public DateTime Recording { get; set; }

        [Display(Name = "GenreId")]
        public int GenreId { get; set; }

        [Display(Name = "TypeId")]
        public int DiskType { get; set; }
    }
}