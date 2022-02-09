using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement2.ViewModel
{
    public class ChangePasswordVieModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        //[MinLength(10, ErrorMessage = "Password must exceed 10 characters")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        //[MinLength(10, ErrorMessage = "Password must exceed 10 characters")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The Password and Confirm Password did not match")]
        public string ConfirmPassword { get; set; }
    }
}
