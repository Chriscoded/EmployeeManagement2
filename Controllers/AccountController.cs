﻿
using EmployeeManagement2.Models;
using EmployeeManagement2.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeManagement2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> SignInManager,
            ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            signInManager = SignInManager;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult ResetPassword(string Email, string token)
        {
            if(Email == null || token == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Password reset token");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel Model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(Model.Email);

                if(user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, Model.Token, Model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(Model);
                }
                return View("ResetPasswordConfirmation");
            }
            return View(Model);
        }

            [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel Model)
        {
            ApplicationUser user = null;
            //if input passed all necessary validatio 
            if (ModelState.IsValid)
            {
                user = await userManager.FindByEmailAsync(Model.Email); 
                
                if(user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new
                    { email = Model.Email, token = token}, Request.Scheme);

                    logger.Log(LogLevel.Warning, passwordResetLink);
                    return View("ForgotPasswordConfirmation");
                }
                //when the user is not in our system lets not let them
                //know that they are not in our system rather lets tell them
                //that if they have account we've sent email using 
                //ForgotPasswordConfirmation view
                return View("ForgotPasswordConfirmation");
            }
            //when modelstate is not validate
            else
            {
                ModelState.AddModelError(String.Empty, $"Please ensure your Email is correct, in correct format and case");
                return View(Model);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {Email} is already in use");
            }

        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel Model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {
                    UserName = Model.Email,
                    Email = Model.Email,
                    City = Model.City
                };
                var result = await userManager.CreateAsync(user, Model.Password);

                if (result.Succeeded)
                {
                    //generate confirmation token
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //generate email confirmation link
                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = token }, Request.Scheme);

                    logger.Log(LogLevel.Warning, confirmationLink);

                    //if user is admin adding new user that is the admin is logged in redirect to listuser action in account countroller
                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                    ViewBag.ErrorTitle = $"Registration successful";
                    ViewBag.ErrorMessage = $"Before you can login please confirm your Email," +
                        "By clicking on the confirmation link that we have emailed you";
                    return View("Success");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(Model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if(userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var user = await userManager.FindByIdAsync(userId);

            if(userId == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                 return View("NotFound");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }
            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            LoginViewModel Model = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(Model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel Model, string? ReturnUrl)
        {
            Model.ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(Model.Email);
                //check if email is confirmed
                if (user != null && !user.EmailConfirmed && (await userManager.CheckPasswordAsync(user, Model.Password)))
                {
                    ModelState.AddModelError(String.Empty, $"Email not varified yet");
                    return View(Model);
                }
                var result = await signInManager.PasswordSignInAsync(Model.Email, Model.Password, Model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);

                        //or
                        //return LocalRedirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "home");
                    }

                }
               
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                
            }
            return View(Model);
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            //upon successfull authentication send the user back to ExternalLoginCallback action in account controller
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
                new { ReturnUrl = returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);    
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            //if return url is null initialize it to application root url
            returnUrl = returnUrl ?? Url.Content("~/");
            //lets add values to LoginViewModel incase  external authentication fails we want to 
            //reload login view with return url and externalLogin details that it rquires
            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View("Login", loginViewModel);
            }

            //get login info sent by externalProvider

            var info = await  signInManager.GetExternalLoginInfoAsync();   

            if(info == null)
            {
                ModelState.AddModelError(string.Empty, $"Error loading external login information: {remoteError}");
                return View("Login", loginViewModel);
            }

            var Email = info.Principal.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = null;
            //if email has value
            if(Email != null)
            {
                user  = await userManager.FindByEmailAsync(Email);
               
                    //check if email is confirmed
                    if (user != null && !user.EmailConfirmed)
                    {
                        ModelState.AddModelError(string.Empty, $"Email not confirmed yet");
                        return View("Login", loginViewModel);
                    }
            }
            //else
            //{
            //    ModelState.AddModelError(string.Empty, $"Log into {info.LoginProvider} using your Email to help us identify you");
            //    return View("Login", loginViewModel);
            //}
            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            // if signinResult did not succeed lets check if the external user has local account in
            //in our system using email from info.Principal.FindFirst(ClaimTypes.Email);
            //to get the user details
            else
            {
                if (Email != null)
                {
                    //if the user is not already existing in our local account
                    //then add the user to our local account
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };
                        //adds user to are local database
                        await userManager.CreateAsync(user);

                        //generate confirmation token
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        //generate email confirmation link
                        var confirmationLink = Url.Action("ConfirmEmail", "Account",
                            new { userId = user.Id, token = token }, Request.Scheme);

                        logger.Log(LogLevel.Warning, confirmationLink);

                        ViewBag.SuccessTitle = "Registration successful";
                        ViewBag.SuccessMessage = "Before you can login please confirm your Email," +
                            " By clicking on the confirmation link that we have emailed you";

                        //Adds external user login info to the user when the user with the email is already existing
                        await userManager.AddLoginAsync(user, info);
                        //sign uswr in
                        
                        return View("Success");

                    }
                    //Adds external user login info to the user
                    await userManager.AddLoginAsync(user, info);
                    //sign uswr in
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }
                ViewBag.ErrorTitle = "Email Claim not received from {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support on amaemechris@gmail.com";
                return View("Error");
            }
        }

    }

}
