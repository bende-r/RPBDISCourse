using System.ComponentModel.DataAnnotations;

using VideoRentalMVC.Models.Filters;

using Type = VideoRentalModels.Type;

namespace VideoRentalMVC.Models.Entities
{
    public class TypeViewModel : IEntitiesViewModel<Type>
    {
        [Display(Name = "Types")]
        public IEnumerable<Type> Entities { get; set; }

        [Display(Name = "Type")]
        public Type Entity { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public DeleteViewModel DeleteViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }

        public TypeFilterViewModel TypeFilterViewModel { get; set; }
    }
}