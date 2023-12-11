using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using VideoRentalWeb.DataModels;
using VideoRentalWeb.Models.Filters;

using Type = VideoRentalWeb.DataModels.Type;

namespace VideoRentalWeb.Models.Entities
{
    public class DiskViewModel : IEntitiesViewModel<Disk>
    {
        public DiskViewModel(){}

        public DiskViewModel(List<Producer> producerList, List<Genre> genres, List<Type> types)
        {
            Producers = new SelectList(producerList, "ProduceId", "Manufacturer");
            Genres = new SelectList(genres, "GenreId", "Title");
            Types = new SelectList(types, "TypeId", "Title");
        }

        [Display(Name = "Disks")]
        public IEnumerable<Disk>? Entities { get; set; }

        [Display(Name = "Disk")]
        public Disk? Entity { get; set; }
      
        public string Title { get; set; } = null!;
        public string? CreationYear { get; set; }
        public string MainActor { get; set; } = null!;
        public DateTime Recording { get; set; }
        public int ProducerId { get; set; }
        public int GenreId { get; set; }
        public int TypeId { get; set; }

        public IEnumerable<SelectListItem>? Producers { get; set; }
        public IEnumerable<SelectListItem>? Genres { get; set; }
        public IEnumerable<SelectListItem>? Types { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public DeleteViewModel? DeleteViewModel { get; set; }
        public SortViewModel? SortViewModel { get; set; }

        public DisksFilterViewModel? DiskFilterViewModel { get; set; }
    }
}