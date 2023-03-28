using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotelManagement.Models;
using MotelManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<MyAppUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<MyAppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;

        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    TempData["ErrorMessage"] = "رول جدید اضافه شد";
                    return RedirectToAction("ListRoles", "Administration");
                }
                else
                {
                    TempData["ErrorMessage"] = "عملیه ناموفق بود.";
                    return View(model);
                }
            }
            else
                return View(model);
        }
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                TempData["ErrorMessage"] = $"رول با آی دی نمبر {id} پیدا نشد";
                return View();
            }
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                TempData["ErrorMessage"] = $"رول با آی دی نمبر {model.Id} پیدا نشد";
                return View();
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    TempData["ErrorMessage"] = "موفقانه ویرایش شد";
                    return RedirectToAction("ListRoles");
                }
                else
                {
                    TempData["ErrorMessage"] = "نام قبلا موجود است";
                    return View(model);
                }

            }
        }
        [HttpGet]

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var tempRole = await roleManager.FindByIdAsync(id);
            if (tempRole == null)
            {
                TempData["ErrorMessage"] = "این رول موجود نیست";
                return RedirectToAction("ListRoles");
            }
            else
            {
                await roleManager.DeleteAsync(tempRole);

                TempData["ErrorMessage"] = "رول حذف شد";
                return RedirectToAction("ListRoles");
            }

        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string id)
        {
            var tempRole = await roleManager.FindByIdAsync(id);
            ViewBag.RoleId = id;
            ViewBag.RoleName = tempRole.Name;

            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                TempData["ErrorMessage"] = "اطلاعات برای نمایش موجود نیست";
                return View();
            }
            else
            {
                var model = new List<UserRoleViewModel>();
                foreach (var user in userManager.Users)
                {
                    if(user.Id != "0301fced-ab76-43e5-8e77-f3caadef3bd2")
                    {
                        //Kalim2005 is a super admin other admins must not be able to edit this user.

                        var userRoleViewModel = new UserRoleViewModel()
                        {
                            UserId = user.Id,
                            UserName = user.UserName
                        };

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

                }
                return View(model);
            }

        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                TempData["ErrorMessage"] = "اطلاعات برای نمایش موجود نیست";
                return View();
            }
            else
            {
                for (int i = 0; i < model.Count; i++)
                {
                    var user = await userManager.FindByIdAsync(model[i].UserId);
                    IdentityResult result = null;
                    if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                    {
                        result = await userManager.AddToRoleAsync(user, role.Name);
                    }
                    else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                    {
                        result = await userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }
                    if (result.Succeeded)
                    {
                        if (i < (model.Count - 1))
                            continue;
                        else
                            return RedirectToAction("EditRole", new { Id = id });
                    }
                }
            }
            return RedirectToAction("EditRole", new { Id = id });
        }
    }
}
