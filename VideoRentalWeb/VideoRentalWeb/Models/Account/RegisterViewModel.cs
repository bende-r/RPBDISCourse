using System.ComponentModel.DataAnnotations;

namespace VideoRentalMVC.Models.Account
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Login")]
        [DataType(DataType.EmailAddress)]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}