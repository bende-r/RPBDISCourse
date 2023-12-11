using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using VideoRentalWeb.DataModels;
using VideoRentalWeb.Models.Filters;

namespace VideoRentalWeb.Models.Entities
{
    public class TakingViewModel : IEntitiesViewModel<Taking>
    {
        public TakingViewModel(){}

        public TakingViewModel(List<Clientele> clienteles, List<Disk> disks, List<Staff> staff)
        {
            Clientele = new SelectList(clienteles, "ClientId", "Surname");
           Disks =  new SelectList(disks, "DiskId", "Title");
          Staff =  new SelectList(staff, "StaffId", "Surname");
        }

        [Display(Name = "Takings")]
        public IEnumerable<Taking>? Entities { get; set; }

        [Display(Name = "Taking")]
        public Taking? Entity { get; set; }

        public int ClientId { get; set; }
        public int DiskId { get; set; }
        public DateTime DateOfCapture { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool PaymentMark { get; set; }
        public bool RefundMark { get; set; }
        public int StaffId { get; set; }

        public IEnumerable<SelectListItem>? Clientele { get; set; }
        public IEnumerable<SelectListItem>? Disks { get; set; }
        public IEnumerable<SelectListItem>? Staff { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public DeleteViewModel? DeleteViewModel { get; set; }
        public SortViewModel? SortViewModel { get; set; }

        public TakingsFilterViewModel? TakingsFilterViewModel { get; set; }
    }
}