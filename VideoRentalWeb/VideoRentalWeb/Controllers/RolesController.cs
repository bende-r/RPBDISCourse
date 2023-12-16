using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using VideoRentalWeb.DataModels;
using VideoRentalWeb.Models;

namespace VideoRentalWeb.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = _userManager.GetUserAsync(User).Result;

                // Проверка наличия роли Admin у текущего пользователя
                if (_userManager.IsInRoleAsync(currentUser, "Admin").Result)
                {
                    RoleViewModel model = new RoleViewModel
                    {
                        Roles = _roleManager.Roles.ToList()
                    };

                    return View(model);
                }

                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = _userManager.GetUserAsync(User).Result;

                // Проверка наличия роли Admin у текущего пользователя
                if (_userManager.IsInRoleAsync(currentUser, "Admin").Result)
                {
                    RoleViewModel model = new RoleViewModel();

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = _userManager.GetUserAsync(User).Result;

                // Проверка наличия роли Admin у текущего пользователя
                if (_userManager.IsInRoleAsync(currentUser, "Admin").Result)
                {
                    if (!string.IsNullOrEmpty(model.RoleName))
                    {
                        var result = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));

                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                                ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string name)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = _userManager.GetUserAsync(User).Result;

                // Проверка наличия роли Admin у текущего пользователя
                if (_userManager.IsInRoleAsync(currentUser, "Admin").Result)
                {
                    IdentityRole role = await _roleManager.FindByNameAsync(name);

                    if (role != null)
                    {
                        var result = await _roleManager.DeleteAsync(role);
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Users()
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = _userManager.GetUserAsync(User).Result;

                // Проверка наличия роли Admin у текущего пользователя
                if (_userManager.IsInRoleAsync(currentUser, "Admin").Result)
                {
                    return View(_userManager.Users.ToList());
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Edit(string userId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = _userManager.GetUserAsync(User).Result;

                // Проверка наличия роли Admin у текущего пользователя
                if (_userManager.IsInRoleAsync(currentUser, "Admin").Result)
                {
                    User user = await _userManager.FindByIdAsync(userId);

                    if (user != null)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        var roles = _roleManager.Roles.ToList();

                        RoleViewModel model = new RoleViewModel
                        {
                            Roles = roles,
                            UserRoles = userRoles,
                            User = user
                        };

                        return View(model);
                    }

                    return NotFound();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = _userManager.GetUserAsync(User).Result;

                // Проверка наличия роли Admin у текущего пользователя
                if (_userManager.IsInRoleAsync(currentUser, "Admin").Result)
                {
                    User user = await _userManager.FindByIdAsync(userId);

                    if (user != null)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        var allRoles = _roleManager.Roles.ToList();
                        var addedRoles = roles.Except(userRoles);
                        var removedRoles = userRoles.Except(roles);

                        await _userManager.AddToRolesAsync(user, addedRoles);
                        await _userManager.RemoveFromRolesAsync(user, removedRoles);

                        return RedirectToAction("Users");
                    }

                    return NotFound();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}