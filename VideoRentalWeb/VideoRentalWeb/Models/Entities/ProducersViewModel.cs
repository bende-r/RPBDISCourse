using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using VideoRentalWeb.DataModels;
using VideoRentalWeb.Models.Filters;

namespace VideoRentalWeb.Models.Entities
{
    public class ProducersViewModel : IEntitiesViewModel<Producer>
    {
        [Display(Name = "Producers")]
        public IEnumerable<Producer>? Entities { get; set; }

        [Display(Name = "Producer")]
        public Producer Entity { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public DeleteViewModel? DeleteViewModel { get; set; }
        public SortViewModel? SortViewModel { get; set; }

        public ProducersFilterViewModel? ProducersFilterViewModel { get; set; }
    }
}