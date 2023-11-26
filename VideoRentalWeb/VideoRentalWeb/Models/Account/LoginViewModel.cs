using System.ComponentModel.DataAnnotations;

namespace VideoRentalWeb.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Login")]
        [DataType(DataType.EmailAddress)]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember?")]
        public bool RememberMe { get; set; }
    }
}