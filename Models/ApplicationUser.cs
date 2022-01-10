using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public String City { get; set; }
    }
}
