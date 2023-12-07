using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using VideoRentalWeb.DataModels;
using VideoRentalWeb.Models.Filters;

namespace VideoRentalWeb.Models.Entities
{
    public class PositionsViewModel : IEntitiesViewModel<Position>
    {
        [Display(Name = "Positions")]
        public IEnumerable<Position>? Entities { get; set; }

        [Display(Name = "Position")]
        public Position Entity { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public DeleteViewModel? DeleteViewModel { get; set; }
        public SortViewModel? SortViewModel { get; set; }

        public PositionsFilterViewModel? PositionsFilterViewModel{ get; set; }
    }
}