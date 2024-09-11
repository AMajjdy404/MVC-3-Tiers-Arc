using System.ComponentModel.DataAnnotations;

namespace MVC.PL.ViewModels.Account
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
    }
}
