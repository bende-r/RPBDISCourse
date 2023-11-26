using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using VideoRentalModels;

using VideoRentalMVC.Models;

namespace VideoRentalMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            RoleViewModel model = new RoleViewModel
            {
                Roles = roleManager.Roles.ToList()
            };

            return View(model);
        }

        public IActionResult Create()
        {
            RoleViewModel model = new RoleViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (!string.IsNullOrEmpty(model.RoleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(model.RoleName));

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

        [HttpPost]
        public async Task<IActionResult> Delete(string name)
        {
            IdentityRole role = await roleManager.FindByNameAsync(name);

            if (role != null)
            {
                var result = await roleManager.DeleteAsync(role);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Users()
        {
            return View(userManager.Users.ToList());
        }

        public async Task<IActionResult> Edit(string userId)
        {
            User user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var roles = roleManager.Roles.ToList();

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

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            User user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var allRoles = roleManager.Roles.ToList();
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);

                await userManager.AddToRolesAsync(user, addedRoles);
                await userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("Users");
            }

            return NotFound();
        }
    }
}