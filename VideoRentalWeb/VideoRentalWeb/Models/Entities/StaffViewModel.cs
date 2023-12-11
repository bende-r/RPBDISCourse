using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.Rendering;

using VideoRentalWeb.DataModels;
using VideoRentalWeb.Models.Filters;

namespace VideoRentalWeb.Models.Entities
{
    public class StaffViewModel : IEntitiesViewModel<Staff>
    {
        public StaffViewModel() { }

        public StaffViewModel(List<Position> positions)
        {
            Positions  = new SelectList(positions, "PositionId", "Title");
        }

        [Display(Name = "Staff")]
        public IEnumerable<Staff>? Entities { get; set; }

        [Display(Name = "Staff")]
        public Staff? Entity { get; set; }

        public string? Surname { get; set; } = null!;
        public string? Name { get; set; } = null!;
        public string? Middlename { get; set; }
        public int PositionId { get; set; }
        public DateTime DateOfEmployment { get; set; }

        public IEnumerable<SelectListItem>? Positions { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public DeleteViewModel? DeleteViewModel { get; set; }
        public SortViewModel? SortViewModel { get; set; }

        public StaffFilterViewModel? StaffFilterViewModel { get; set; }
    }
}