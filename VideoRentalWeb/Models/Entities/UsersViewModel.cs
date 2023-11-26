using System.ComponentModel.DataAnnotations;

using VideoRentalModels;

namespace VideoRentalMVC.Models.Entities
{
    public class UsersViewModel : IEntitiesViewModel<User>
    {
        [Display(Name = "Users")]
        public IEnumerable<User> Entities { get; set; }

        [Display(Name = "User")]
        public User Entity { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "New password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public DeleteViewModel DeleteViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
    }
}