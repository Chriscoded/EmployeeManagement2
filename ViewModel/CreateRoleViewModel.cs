using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement2.ViewModel
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
