using MotelManagement.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotelManagement.ViewModels;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

namespace MotelManagement.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager<MyAppUser> _userManager;
        private readonly SignInManager<MyAppUser> _signInManager;
        private RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<MyAppUser> userManager,
                             SignInManager<MyAppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult Register()
        {            
            return View();
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                using SqlConnection sql = new(Startup.ConnectionString);
                var temp = new RegisterViewModel() { };
                temp = sql.Query<RegisterViewModel>("dbo.IsUserExist", new { UserName = model.UserName }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (temp != null)
                {
                    TempData["ErrorMessage"] = " .نام یوزر از قبل موجود است "+temp.UserName;
                    return View(model); 
                }
               
                var user = new MyAppUser { UserName = model.UserName, Email = model.Email, FullName = model.FullName};
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // place the user in a specific Role
                    if (model.Role == 1)
                    {
                        await _userManager.AddToRoleAsync(user, "Manager");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "Administrator");
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "اطلاعات هویت درست نیست");
                    TempData["ErrorMessage"] = "عملیه ناموفق بود.";
                }
                AddErrors(result);

                

            }
            else
            {
                TempData["ErrorMessage"] = "لطفا معلومات را کامل درج نمایید.";
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    TempData["ErrorMessage"] = "معلومات درج شده نادرست است.";
                }

            }
            else
            {
                TempData["ErrorMessage"] = "لطفا معلومات را کامل درج نمایید.";
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

       
        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
