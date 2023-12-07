using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using VideoRentalWeb.DataModels;
using VideoRentalWeb.Models.Filters;

namespace VideoRentalWeb.Models.Entities
{
    public class TakingViewModel : IEntitiesViewModel<Taking>
    {
        [Display(Name = "Takings")]
        public IEnumerable<Taking>? Entities { get; set; }

        [Display(Name = "Taking")]
        public Taking Entity { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public DeleteViewModel? DeleteViewModel { get; set; }
        public SortViewModel? SortViewModel { get; set; }

        public TakingsFilterViewModel? TakingsFilterViewModel { get; set; }
    }
}