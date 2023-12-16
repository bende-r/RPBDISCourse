using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using VideoRentalWeb.DataModels;
using VideoRentalWeb.Infrastructure;
using VideoRentalWeb.Models;
using VideoRentalWeb.Models.Entities;
using VideoRentalWeb.Models.Filters;
using VideoRentalWeb.Services;

using Type = VideoRentalWeb.DataModels.Type;

namespace VideoRentalWeb.Controllers;

public class TypesController : Controller
{
    private readonly VideoRentalContext _db;
    private readonly CacheProvider _cache;

    private const string FilterKey = "types";

    private readonly UserManager<User> _userManager;

    public TypesController(UserManager<User> userManager, VideoRentalContext context, CacheProvider cacheProvider)
    {
        _db = context;
        _userManager = userManager;
        _cache = cacheProvider;
    }

    public IActionResult Index(SortState sortState = SortState.TypeTitleAsc, int page = 1)
    {
        TypeFilterViewModel filter = HttpContext.Session.Get<TypeFilterViewModel>(FilterKey);
        if (filter == null)
        {
            filter = new TypeFilterViewModel() { TypeTitle = string.Empty };
            HttpContext.Session.Set(FilterKey, filter);
        }

        string modelKey = $"{typeof(Type).Name}-{page}-{sortState}-{filter.TypeTitle}";
        if (!_cache.TryGetValue(modelKey, out TypeViewModel model))
        {
            model = new TypeViewModel();

            IQueryable<Type> types = GetSortedEntities(sortState, filter.TypeTitle);

            int count = types.Count();
            int pageSize = 10;
            model.PageViewModel = new PageViewModel(page, count, pageSize);

            model.Entities = count == 0 ? new List<Type>() : types.Skip((model.PageViewModel.CurrentPage - 1) * pageSize).Take(pageSize).ToList();
            model.SortViewModel = new SortViewModel(sortState);
            model.TypeFilterViewModel = filter;

            _cache.Set(modelKey, model);
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult Index(TypeFilterViewModel filterModel, int page)
    {
        TypeFilterViewModel filter = HttpContext.Session.Get<TypeFilterViewModel>(FilterKey);
        if (filter != null)
        {
            filter.TypeTitle = filterModel.TypeTitle;

            HttpContext.Session.Remove(FilterKey);
            HttpContext.Session.Set(FilterKey, filter);
        }

        return RedirectToAction("Index", new { page });
    }

    public IActionResult Create(int page)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Проверка наличия роли Admin у текущего пользователя
            if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
            {
                TypeViewModel model = new TypeViewModel()
                {
                    PageViewModel = new PageViewModel { CurrentPage = page }
                };

                return View(model);
            }

            else
            {
                return RedirectToAction("Index", "Types");
            }
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(TypeViewModel model)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Проверка наличия роли Admin у текущего пользователя
            if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
            {
                if (ModelState.IsValid & CheckUniqueValues(model.Entity))
                {
                    await _db.Types.AddAsync(model.Entity);
                    await _db.SaveChangesAsync();

                    _cache.Clean();

                    return RedirectToAction("Index", "Types");
                }

                return View(model);
            }

            else
            {
                return RedirectToAction("Index", "Types");
            }
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> Edit(int id, int page)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Проверка наличия роли Admin у текущего пользователя
            if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
            {
                Type type = await _db.Types.FindAsync(id);
                if (type != null)
                {
                    TypeViewModel model = new TypeViewModel();
                    model.PageViewModel = new PageViewModel { CurrentPage = page };
                    model.Entity = type;

                    return View(model);
                }

                return NotFound();
            }

            else
            {
                return RedirectToAction("Index", "Types");
            }
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TypeViewModel model)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Проверка наличия роли Admin у текущего пользователя
            if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
            {
                if (ModelState.IsValid & CheckUniqueValues(model.Entity))
                {
                    Type type = _db.Types.Find(model.Entity.TypeId);
                    if (type != null)
                    {
                        type.Title = model.Entity.Title;
                        type.Description = model.Entity.Description;

                        _db.Types.Update(type);
                        await _db.SaveChangesAsync();

                        _cache.Clean();

                        return RedirectToAction("Index", "Types", new { page = model.PageViewModel.CurrentPage });
                    }
                    else
                    {
                        return NotFound();
                    }
                }

                return View(model);
            }

            else
            {
                return RedirectToAction("Index", "Types");
            }
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> Delete(int id, int page)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Проверка наличия роли Admin у текущего пользователя
            if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
            {
                Type type = await _db.Types.FindAsync(id);
                if (type == null)
                    return NotFound();

                bool deleteFlag = false;
                string message = "Do you want to delete this entity";

                if (_db.Types.Any(s => s.TypeId == type.TypeId))
                    message = "This entity has entities, which dependents from this. Do you want to delete this entity and other, which dependents from this?";

                TypeViewModel model = new TypeViewModel();
                model.Entity = type;
                model.PageViewModel = new PageViewModel { CurrentPage = page };
                model.DeleteViewModel = new DeleteViewModel { Message = message, IsDeleted = deleteFlag };

                return View(model);
            }

            else
            {
                return RedirectToAction("Index", "Types");
            }
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(TypeViewModel model)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Проверка наличия роли Admin у текущего пользователя
            if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
            {
                Type type = await _db.Types.FindAsync(model.Entity.TypeId);
                if (type == null)
                    return NotFound();

                _db.Types.Remove(type);
                await _db.SaveChangesAsync();

                _cache.Clean();

                model.DeleteViewModel = new DeleteViewModel { Message = "The entity was successfully deleted.", IsDeleted = true };

                return View(model);
            }

            else
            {
                return RedirectToAction("Index", "Types");
            }
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            Type type = await _db.Types.FindAsync(id);
            if (type != null)
            {
                TypeViewModel model = new TypeViewModel()
                {
                    Entity = type,

                    PageViewModel = new PageViewModel()
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

    private bool CheckUniqueValues(Type type)
    {
        bool firstFlag = true;

        Type tempType = _db.Types.FirstOrDefault(g => g.Title == type.Title);
        if (tempType != null)
        {
            if (tempType.TypeId != type.TypeId)
            {
                ModelState.AddModelError(string.Empty, "Another entity have this name. Please replace this to another.");
                firstFlag = false;
            }
        }

        if (firstFlag)
            return true;
        else
            return false;
    }

    private IQueryable<Type> GetSortedEntities(SortState sortState, string typeName)
    {
        IQueryable<Type> types = _db.Types.AsQueryable();

        switch (sortState)
        {
            case SortState.TypeTitleAsc:
                types = types.OrderBy(g => g.Title);
                break;

            case SortState.TypeTitleDesc:
                types = types.OrderByDescending(g => g.Title);
                break;
        }

        if (!string.IsNullOrEmpty(typeName))
            types = types.Where(g => g.Title.Contains(typeName)).AsQueryable();

        return types;
    }
}