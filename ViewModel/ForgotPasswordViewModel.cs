using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement2.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
