using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using VideoRentalWeb.DataModels;
using VideoRentalWeb.Models.Filters;

namespace VideoRentalWeb.Models.Entities
{
    public class ClienteleViewModel : IEntitiesViewModel<Clientele>
    {
        [Display(Name = "Clientele")]
        public IEnumerable<Clientele>? Entities { get; set; }

        [Display(Name = "Clientele")]
        public Clientele Entity { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public DeleteViewModel? DeleteViewModel { get; set; }
        public SortViewModel? SortViewModel { get; set; }

        public ClienteleFilterViewModel? ClienteleFilterViewModel { get; set; }
    }
}