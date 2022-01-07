using EmployeeManagement2.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement2.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(UserManager<IdentityUser> userManager)
        {

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel Model)
        {
            return View();
        }
    }
}
