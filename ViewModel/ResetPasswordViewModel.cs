using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement2.ViewModel
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name ="New Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage ="Password and Confirm Password must match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
