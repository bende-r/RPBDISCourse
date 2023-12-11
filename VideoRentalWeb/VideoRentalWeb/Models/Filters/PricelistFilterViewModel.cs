using System.ComponentModel.DataAnnotations;

namespace VideoRentalWeb.Models.Filters
{
    public class PricelistFilterViewModel
    {
        [Display(Name = "DiskId")]
        public int DiskId { get; set; }

        [Display (Name = "Title")]
        public string Title { get; set; }

        //[Display(Name = "Price")]
        //    public decimal Price { get; set; }
    }
}