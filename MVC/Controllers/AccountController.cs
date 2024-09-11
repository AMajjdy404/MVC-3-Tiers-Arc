using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Controllers;
using MVC.DAL.Models;
using MVC.PL.Helpers.SendEmail;
using MVC.PL.ViewModels.Account;
using System.Threading.Tasks;

namespace MVC.PL.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMailService _mailService;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
        }


        #region SignUp
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user is null)
                {
                    user = new ApplicationUser()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.Username,
                        Email = model.Email,
                        IsAgree = model.IsAgree,

                    };

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(SignIn));
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
                else
                    ModelState.AddModelError(string.Empty, "Username is Already Exisit!");
            }
            return View(model);
        } 
        #endregion

        #region SignIn
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var result = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (result)
                    {
                        var login = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (login.Succeeded)
                            return RedirectToAction("Index", "Home");
                        else
                            ModelState.AddModelError(string.Empty, "Invalid Login!");
                    }
                    else
                        ModelState.AddModelError(string.Empty, "Invalid Login!");
                }

            }
            return View(model);
        }
        #endregion

        #region SignOut

         public async new Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        #endregion

        #region Forget Password
        public IActionResult ForgetPassword()
        {
            return View();
        }

        public async Task<IActionResult> SendResetPasswordEmail(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    // token => Valid For only one time for this User
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = user.Email, Token = token }, Request.Scheme); // Scheme => stand for dynamic Host and Port

                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = ResetPasswordLink
                    };
                    _mailService.SendEmail(email);
                    return RedirectToAction(nameof(CheckInbox));
                }
                ModelState.AddModelError(string.Empty, "There is No Account with this Email!");

            }
            return View(model);
        }

        public IActionResult CheckInbox()
        {
            return View();
        }

        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var user = await _userManager.FindByEmailAsync(email);
                var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (result.Succeeded)
                {
                    TempData["ResetSucceeded"] = "Reset Password Succeeded";
                    return RedirectToAction(nameof(SignIn));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);

                    }
                }

            }
            return View(model);
        } 
        #endregion

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
