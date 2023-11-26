using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using VideoRentalWeb.DataModels;
using VideoRentalWeb.Models.Filters;

using Type = VideoRentalWeb.DataModels.Type;

namespace VideoRentalWeb.Models.Entities
{
    public class DiskViewModel : IEntitiesViewModel<Disk>
    {
        [Display(Name = "Disks")]
        public IEnumerable<Disk> Entities { get; set; }

        [Display(Name = "Disk")]
        public Disk Entity { get; set; }

        [Display(Name = "Genres")]
        public IEnumerable<Genre> GenreList { get; set; }

        [Display(Name = "Types")]
        public IEnumerable<Type> TypesList { get; set; }

        public string GenreTitle { get; set; }
        public string TypeTitle { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public DeleteViewModel DeleteViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }

        public DisksFilterViewModel DiskFilterViewModel { get; set; }
    }
}