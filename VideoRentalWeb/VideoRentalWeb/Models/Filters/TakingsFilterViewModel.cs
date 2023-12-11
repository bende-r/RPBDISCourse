using System.ComponentModel.DataAnnotations;

namespace VideoRentalWeb.Models.Filters
{
    public class TakingsFilterViewModel
    {
        [Display(Name = "ClientId")]
        public int ClientId { get; set; } 

        [Display(Name = "DiskId")]
        public int DiskId { get; set; }

        [Display(Name = "DateOfCapture")]
        public DateTime DateOfCapture { get; set; } 

        [Display(Name = "ReturnDate")]
        public DateTime ReturnDate { get; set; } 

        [Display(Name = "PaymentMark")]
        public bool PaymentMark { get; set; }

        [Display(Name = "RefundMark")]
        public bool RefundMark { get; set; }

        [Display(Name = "StaffId")]
        public int StaffId { get; set; }
    }
}