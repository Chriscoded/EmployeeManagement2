using EmployeeManagement2.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement2.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        [ValidEmailDomainAttribute(allowedDomain: "gmail.com", ErrorMessage = "Email domain must be gmail.com")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        //[MinLength(10, ErrorMessage = "Password must exceed 10 characters")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password did not match")]
        public string ConfirmPassword { get; set; }

        public string City { get; set; }
    }
    
}
