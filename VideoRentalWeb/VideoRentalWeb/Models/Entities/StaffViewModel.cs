using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using VideoRentalWeb.DataModels;
using VideoRentalWeb.Models.Filters;

namespace VideoRentalWeb.Models.Entities
{
    public class StaffViewModel : IEntitiesViewModel<Staff>
    {
        [Display(Name = "Staff")]
        public IEnumerable<Staff>? Entities { get; set; }

        [Display(Name = "Staff")]
        public Staff Entity { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public DeleteViewModel? DeleteViewModel { get; set; }
        public SortViewModel? SortViewModel { get; set; }

        public StaffFilterViewModel? StaffFilterViewModel { get; set; }
    }
}