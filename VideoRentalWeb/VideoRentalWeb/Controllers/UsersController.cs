using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using VideoRentalWeb.DataModels;
using VideoRentalWeb.Models;
using VideoRentalWeb.Models.Entities;

namespace VideoRentalWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> manager;
        private readonly SignInManager<User> inManager;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            manager = userManager;
            inManager = signInManager;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = manager.GetUserAsync(User).Result;

                // Проверка наличия роли Admin у текущего пользователя
                if (manager.IsInRoleAsync(currentUser, "Admin").Result)
                {
                    IEnumerable<User> users = manager.Users.ToList();

                    UsersViewModel model = new UsersViewModel
                    {
                        Entities = users
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
                var currentUser = manager.GetUserAsync(User).Result;

                // Проверка наличия роли Admin у текущего пользователя
                if (manager.IsInRoleAsync(currentUser, "Admin").Result)
                {
                    UsersViewModel model = new UsersViewModel
                    {
                        Entity = new User()
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

        [HttpPost]
        public async Task<IActionResult> Create(UsersViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = manager.GetUserAsync(User).Result;

                // Проверка наличия роли Admin у текущего пользователя
                if (manager.IsInRoleAsync(currentUser, "Admin").Result)
                {
                    foreach (var entry in ModelState)
                    {
                        var key = entry.Key; // Название свойства
                        var errors = entry.Value.Errors.Select(e => e.ErrorMessage).ToList(); // Список ошибок для свойства

                        // Далее можно использовать key и errors в соответствии с вашими потребностями
                        Console.WriteLine($"Property: {key}, Errors: {string.Join(", ", errors)}");
                    }

                    if (ModelState.IsValid & CheckUniqueValues(model.Entity))
                    {
                        model.Entity.UserName = model.Entity.Email;

                        var result = await manager.CreateAsync(model.Entity, model.Password);
                        if (result.Succeeded)
                        {
                            User user = await manager.FindByNameAsync(model.Entity.Email);

                            if (user != null)
                            {
                                await manager.AddToRoleAsync(user, "user");
                            }

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

        public async Task<IActionResult> Edit(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = manager.GetUserAsync(User).Result;

                // Проверка наличия роли Admin у текущего пользователя
                if (manager.IsInRoleAsync(currentUser, "Admin").Result)
                {
                    User user = await manager.FindByIdAsync(id);

                    if (user == null)
                        return NotFound();

                    UsersViewModel model = new UsersViewModel
                    {
                        Entity = user
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

        [HttpPost]
        public async Task<IActionResult> Edit(UsersViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = manager.GetUserAsync(User).Result;

                // Проверка наличия роли Admin у текущего пользователя
                if (manager.IsInRoleAsync(currentUser, "Admin").Result)
                {
                    if (ModelState.IsValid & CheckUniqueValues(model.Entity))
                    {
                        User user = await manager.FindByIdAsync(model.Entity.Id.ToString());

                        if (user != null)
                        {
                            user.Email = model.Entity.Email;
                            user.UserName = model.Entity.Email;

                            var result = await manager.UpdateAsync(user);

                            if (result.Succeeded)
                            {
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                foreach (var error in result.Errors)
                                {
                                    ModelState.AddModelError(string.Empty, error.Description);
                                }
                            }
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

        public async Task<IActionResult> Delete(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = manager.GetUserAsync(User).Result;

                // Проверка наличия роли Admin у текущего пользователя
                if (manager.IsInRoleAsync(currentUser, "Admin").Result)
                {
                    User user = await manager.FindByIdAsync(id);

                    if (user == null)
                        return NotFound();

                    UsersViewModel model = new UsersViewModel
                    {
                        Entity = user,
                        DeleteViewModel = new DeleteViewModel
                        {
                            Message = "Do you want to delete this user?",
                            IsDeleted = true
                        }
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

        [HttpPost]
        public async Task<IActionResult> Delete(UsersViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = manager.GetUserAsync(User).Result;

                // Проверка наличия роли Admin у текущего пользователя
                if (manager.IsInRoleAsync(currentUser, "Admin").Result)
                {
                    User user = await manager.FindByIdAsync(model.Entity.Id.ToString());

                    if (user == null)
                        return NotFound();

                    var result = await manager.DeleteAsync(user);

                    if (result.Succeeded)
                    {
                        model.DeleteViewModel.IsDeleted = false;

                        if (User.Identity.IsAuthenticated & User.Identity.Name == user.UserName)
                        {
                            await inManager.SignOutAsync();
                            return RedirectToAction("Index", "Home");
                        }

                        return View(model);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                    }

                    model.DeleteViewModel.IsDeleted = true;
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

        public async Task<IActionResult> ChangePassword(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = manager.GetUserAsync(User).Result;

                // Проверка наличия роли Admin у текущего пользователя
                if (manager.IsInRoleAsync(currentUser, "Admin").Result)
                {
                    User user = await manager.FindByIdAsync(id);

                    if (user == null)
                        return NotFound();

                    UsersViewModel model = new UsersViewModel
                    {
                        Entity = user
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

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UsersViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = manager.GetUserAsync(User).Result;

                // Проверка наличия роли Admin у текущего пользователя
                if (manager.IsInRoleAsync(currentUser, "Admin").Result)
                {
                    if (ModelState.IsValid)
                    {
                        User user = await manager.FindByIdAsync(model.Entity.Id.ToString());

                        if (user == null)
                            return NotFound();

                        var result = await manager.ChangePasswordAsync(user, model.Password, model.NewPassword);

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

        private bool CheckUniqueValues(User user)
        {
            bool firstFlag = true;

            IEnumerable<User> users = manager.Users.ToList();

            User tempUser = users.FirstOrDefault(u => u.Email == user.Email);
            if (tempUser != null)
            {
                if (tempUser.Id != user.Id)
                {
                    ModelState.AddModelError(string.Empty, "Another entity have this email. Please replace this to another.");
                    firstFlag = false;
                }
            }

            if (firstFlag)
                return true;
            else
                return false;
        }
    }
}