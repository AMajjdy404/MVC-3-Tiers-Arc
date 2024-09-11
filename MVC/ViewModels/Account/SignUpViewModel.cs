using System.ComponentModel.DataAnnotations;

namespace MVC.PL.ViewModels.Account
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "First Name is Required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Usename is Required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [MinLength(5, ErrorMessage = "Minimum Password Length is 5 Char")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirm Password doesn't match Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
    }
}
