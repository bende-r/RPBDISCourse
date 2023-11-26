using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

using VideoRentalModels;

namespace VideoRentalMVC.Models
{
    public class RoleViewModel
    {
        public User User { get; set; }

        [Display(Name = "Roles")]
        public IEnumerable<IdentityRole> Roles { get; set; }

        [Display(Name = "User roles")]
        public IList<string> UserRoles { get; set; }

        [Required]
        [Display(Name = "Role")]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "The role name length must be more 4")]
        public string RoleName { get; set; }

        [Display(Name = "Modification")]
        public string Modification { get; set; }
    }
}