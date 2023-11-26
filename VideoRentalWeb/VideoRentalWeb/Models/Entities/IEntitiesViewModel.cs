using System.Collections.Generic;
using VideoRentalWeb.DataModels;

namespace VideoRentalWeb.Models.Entities
{
    public interface IEntitiesViewModel<T>
    {
        IEnumerable<T> Entities { get; set; }
        T Entity { get; set; }

        PageViewModel PageViewModel { get; set; }
        DeleteViewModel DeleteViewModel { get; set; }
        SortViewModel SortViewModel { get; set; }
    }
}