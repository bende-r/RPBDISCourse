using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using VideoRentalWeb.Models.Filters;

using Type = VideoRentalWeb.DataModels.Type;

namespace VideoRentalWeb.Models.Entities
{
    public class TypeViewModel : IEntitiesViewModel<Type>
    {
        [Display(Name = "Types")]
        public IEnumerable<Type>? Entities { get; set; }

        [Display(Name = "Type")]
        public Type Entity { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public DeleteViewModel? DeleteViewModel { get; set; }
        public SortViewModel? SortViewModel { get; set; }

        public TypeFilterViewModel? TypeFilterViewModel { get; set; }
    }
}