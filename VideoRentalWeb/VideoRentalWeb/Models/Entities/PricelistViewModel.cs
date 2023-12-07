using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using VideoRentalWeb.DataModels;
using VideoRentalWeb.Models.Filters;

namespace VideoRentalWeb.Models.Entities
{
    public class PricelistViewModel : IEntitiesViewModel<Pricelist>
    {
        [Display(Name = "Pricelist")]
        public IEnumerable<Pricelist>? Entities { get; set; }

        [Display(Name = "Price")]
        public Pricelist Entity
        {
            get; 
            set;
        }

        public IEnumerable<Disk>? Disks {
            get; 
            set; }

        public PageViewModel PageViewModel { get; set; }
        public DeleteViewModel? DeleteViewModel { get; set; }
        public SortViewModel? SortViewModel { get; set; }

        public PricelistFilterViewModel? PricelistFilterViewModel { get; set; }
    }
}