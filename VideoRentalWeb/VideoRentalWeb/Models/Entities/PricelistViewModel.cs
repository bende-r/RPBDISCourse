using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.Rendering;

using VideoRentalWeb.DataModels;
using VideoRentalWeb.Models.Filters;

namespace VideoRentalWeb.Models.Entities
{
    public class PricelistViewModel : IEntitiesViewModel<Pricelist>
    {
        public PricelistViewModel() { }

        public PricelistViewModel(List<Disk> disks)
        {
            Disks = new SelectList(disks, "DiskId", "Title");
        }

        [Display(Name = "Pricelist")]
        public IEnumerable<Pricelist>? Entities { get; set; }

        [Display(Name = "Price")]
        public Pricelist? Entity { get; set; }

        public int PricelistId { get; set; }
        public int DiskId { get; set; }
        public decimal? Price { get; set; }

        public IEnumerable<SelectListItem>? Disks { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public DeleteViewModel? DeleteViewModel { get; set; }
        public SortViewModel? SortViewModel { get; set; }

        public PricelistFilterViewModel? PricelistFilterViewModel { get; set; }
    }
}