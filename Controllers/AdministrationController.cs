using EmployeeManagement2.Models;
using EmployeeManagement2.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Collections.Generic;

namespace EmployeeManagement2.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Authorize(Policy = "AdminRolePolicy")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AdministrationController> logger;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager, ILogger<AdministrationController> logger)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.logger = logger;
        }
        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }
        
        [HttpGet]
        [Authorize(Policy = "CreateRolePolicy")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "CreateRolePolicy")]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel Model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole()
                {
                    Name = Model.RoleName,
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(Model);
        }
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditRole(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var model = new EditRoleViewModel()
                {
                    RoleName = role.Name,
                    Id = role.Id
                };
                //var roleName = role.Name;
                foreach (var user in userManager.Users.ToList())
                {
                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        model.Users.Add(user.UserName);
                    }
                }
                return View(model);
            }

        }

        [HttpPost]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditRole(EditRoleViewModel Model)
        {
            var role = await roleManager.FindByIdAsync(Model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {Model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = Model.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(Model);
            }

        }
        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            //find particular role using roleId passed through get (string roleId)
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {roleId} cannot be found";
                return View("NotFound");
            }
            //creating a list object of type UserRoleViewModel
            var model = new List<UserRoleViewModel>();
            //get all users
            foreach (var user in userManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                //check if users is in role if they are in role then true else false
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);

        }
        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> Model, string roleId)
        {
            int a = 0;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < Model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(Model[i].UserId);
                IdentityResult result = null;

                if (Model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!Model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < Model.Count - 1)
                        continue;

                    else
                        return RedirectToAction("EditRole", new { Id = roleId });

                }


            }


            return RedirectToAction("EditRole", new { Id = roleId });

        }

        [HttpGet]
        [Authorize(Policy = "EditUserPolicy")]
        public async Task<IActionResult> EditUser(string userId)
        {
            ViewBag.userId = userId;
            //find particular user using userId passed through get (string userId)
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {userId} cannot be found";
                return View("NotFound");
            }
            var UserClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);
            var model = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                City = user.City,
                Claims = UserClaims.Select(c => c.Type + " : " +  c.Value).ToList(),
                Roles = userRoles
            };
            return View(model);

        }

        [HttpPost]
        [Authorize(Policy = "EditUserPolicy")]
        public async Task<IActionResult> EditUser(EditUserViewModel Model)
        {
            ViewBag.userId = Model.Id;
            //find particular role using userId passed through get (string userId)
            var user = await userManager.FindByIdAsync(Model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {Model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.UserName = Model.UserName;
                user.Email = Model.Email;
                user.City = Model.City;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(Model);
            }

        }
        [HttpPost]
        [Authorize(Policy = "DeleteUserPolicy")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("ListUsers");
            }


        }

        [HttpPost]
        [Authorize(Policy  = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                try
                {
                    var result = await roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("ListRoles");
                }
                catch (DbUpdateException Ex)
                {
                    logger.LogError($"Error deleting role {Ex}");
                    ViewBag.ErrorTitle = $"{role.Name} is in use";
                    ViewBag.ErrorMessage = $"{role.Name} cannot be deleted as there are users in this role," +
                        $"If you want to delete this role, please remove users from " +
                        $"{role.Name} and then try to delete";
                    return View("Error");
                }
            }


        }

        [HttpGet]
        [Authorize(Policy = "EditingAdminNotSameWithEditedPolicy")]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {userId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in roleManager.Roles.ToList())
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "EditingAdminNotSameWithEditedPolicy")]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> Model, string userId)
        {
            ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {userId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < Model.Count; i++)
            {

                IdentityResult result = null;

                if (Model[i].IsSelected && !(await userManager.IsInRoleAsync(user, Model[i].RoleName)))
                {
                    result = await userManager.AddToRoleAsync(user, Model[i].RoleName);
                }
                else if (!Model[i].IsSelected && await userManager.IsInRoleAsync(user, Model[i].RoleName))
                {
                    result = await userManager.RemoveFromRoleAsync(user, Model[i].RoleName);
                }
                else
                {
                    continue;
                }

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "You coudn't add new or remove existing roles");
                    return View("EditUser", new { userId = userId });
                }
                else
                {
                    if (i < Model.Count - 1)
                        continue;

                    else
                        return RedirectToAction("EditUser", new { userId = userId });

                }

            }
            return RedirectToAction("EditUser", new { userId = userId });

        }

        [HttpGet]
        [Authorize(Policy = "EditingAdminNotSameWithEditedPolicy")]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {userId} cannot be found";
                return View("NotFound");
            }

            var existingUserClaims = await userManager.GetClaimsAsync(user);

            var Model = new UserClaimsViewModel()
            {
                UserId = userId
            };
             

            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim()
                {
                    ClaimType = claim.Type
                };

                //if the use has the claim, isSelected property set to true 
                //so the checkbox next to the claim is checked on the UI
               

                if (existingUserClaims.Any(c => c.Type == claim.Type  && c.Value == "true"))
                {
                    userClaim.IsSelected = true;
                }
                
                    Model.Claims.Add(userClaim);
                
            }
            return View(Model);
        }

        [HttpPost]
        [Authorize(Policy = "EditingAdminNotSameWithEditedPolicy")]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel Model)
        {
            var user = await userManager.FindByIdAsync(Model.UserId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {Model.UserId} cannot be found";
                return View("NotFound");
            }
            //get claims in which the user has
            var claims = await userManager.GetClaimsAsync(user);
            //loop through each user claims since it returns IList result (claims)
            for (var i = 0; i < claims.Count; i++)
            {
                var claim = claims[i];
                //remove the claims
                var result = await userManager.RemoveClaimAsync(user, claim);
                //if we couldn't removed claims
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(String.Empty, "Cannot remove user existing claims");
                    return View(Model);
                }
            }
            //lets loop through the claims supplied from the form using Model object of UserClaimsViewModel
            for (var i = 0; i < Model.Claims.Count; i++)
            {
                //if claims is selected for a uswr

                if (Model.Claims[i].IsSelected)
                {
                    var result = await userManager.AddClaimAsync(user, new Claim(Model.Claims[i].ClaimType, Model.Claims[i].IsSelected ? "true" : "false"));

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(String.Empty, "Cannot add selected claim(s) to user");
                        return View(Model);
                    }
                }

               

            }
            return RedirectToAction("EditUser", new { userId = Model.UserId });
            
        }   
    }
}

//[HttpGet]
//public async Task<IActionResult> ManageUserClaims(string userId)
//{

//    var user = await userManager.FindByIdAsync(userId);
//    if (user == null)
//    {
//        ViewBag.ErrorMessage = $"User with id = {userId} cannot be found";
//        return View("NotFound");
//    }

//    var existingUserClaims = await userManager.GetClaimsAsync(user);

//    var model = new List<UserClaimsViewModel>()
//    {
//        UserId = userId
//    };

//    foreach (Claim claim in ClaimsStore.AllClaims)
//    {
//        model.ClaimType = claim.Type;

//        //if the use has the claim, isSelected property set to true 
//        //so the checkbox next to the claim is checked on the UI

//        if (existingUserClaims.Any(c => c.Type == claim.Type))
//        {
//            model.IsSelected = true;
//        }


//    }
//    return View(model);
//}

//[HttpPost]
//public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel Model)
//{

//    var user = await userManager.FindByIdAsync(Model.UserId);
//    if (user == null)
//    {
//        ViewBag.ErrorMessage = $"User with id = {Model.UserId} cannot be found";
//        return View("NotFound");
//    }

//    var Claims = await userManager.GetClaimsAsync(user);
//    foreach (Claim claim in Claims)
//    {
//        var result = await userManager.RemoveClaimAsync(user, claim);



//        if (!result.Succeeded)
//        {
//            ModelState.AddModelError(String.Empty, "Cannot remove user existing claims");
//            return View(Model);
//        }
//        if (Model.IsSelected)
//            result = await userManager.AddClaimAsync(user, new Claim(Model.ClaimType, Model.ClaimType));

//        if (!result.Succeeded)
//        {
//            ModelState.AddModelError(String.Empty, "Cannot add selected claim(s) to user");
//            return View(Model);
//        }

//        return RedirectToAction("EditUser", new { userId = Model.UserId });
//    }
//    return RedirectToAction("EditUser", new { userId = Model.UserId });
//}