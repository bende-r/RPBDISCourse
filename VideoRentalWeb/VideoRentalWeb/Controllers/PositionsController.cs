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

namespace VideoRentalWeb.Controllers;

public class PositionsController : Controller
{
    private readonly VideoRentalContext _db;
    private readonly CacheProvider _cache;

    private const string FilterKey = "positionss";

    private readonly UserManager<User> _userManager;

    public PositionsController(UserManager<User> userManager, VideoRentalContext context, CacheProvider cacheProvider)
    {
        _db = context;
        _cache = cacheProvider;
        _userManager = userManager;
    }

    public IActionResult Index(SortState sortState = SortState.GenreTitleAsc, int page = 1)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            PositionsFilterViewModel filter = HttpContext.Session.Get<PositionsFilterViewModel>(FilterKey);
            if (filter == null)
            {
                filter = new PositionsFilterViewModel() { Title = string.Empty };
                HttpContext.Session.Set(FilterKey, filter);
            }

            string modelKey = $"{typeof(Position).Name}-{page}-{sortState}-{filter.Title}";
            if (!_cache.TryGetValue(modelKey, out PositionsViewModel model))
            {
                model = new PositionsViewModel();

                IQueryable<Position> positions = GetSortedEntities(sortState, filter.Title);

                int count = positions.Count();
                int pageSize = 10;
                model.PageViewModel = new PageViewModel(page, count, pageSize);

                model.Entities = count == 0 ? new List<Position>() : positions.Skip((model.PageViewModel.CurrentPage - 1) * pageSize).Take(pageSize).ToList();
                model.SortViewModel = new SortViewModel(sortState);
                model.PositionsFilterViewModel = filter;

                _cache.Set(modelKey, model);
            }

            return View(model);
        }

        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public IActionResult Index(PositionsFilterViewModel filterModel, int page)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            PositionsFilterViewModel filter = HttpContext.Session.Get<PositionsFilterViewModel>(FilterKey);
            if (filter != null)
            {
                filter.Title = filterModel.Title;

                HttpContext.Session.Remove(FilterKey);
                HttpContext.Session.Set(FilterKey, filter);
            }

            return RedirectToAction("Index", new { page });
        }

        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public IActionResult Create(int page)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            PositionsViewModel model = new PositionsViewModel()
            {
                PageViewModel = new PageViewModel { CurrentPage = page }
            };

            return View(model);
        }

        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(PositionsViewModel model)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {

            if (ModelState.IsValid & CheckUniqueValues(model.Entity))
            {
                await _db.Positions.AddAsync(model.Entity);
                await _db.SaveChangesAsync();

                _cache.Clean();

                return RedirectToAction("Index", "Positions");
            }

            return View(model);
        }

        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> Edit(int id, int page)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            Position position = await _db.Positions.FindAsync(id);
            if (position != null)
            {
                PositionsViewModel model = new PositionsViewModel();
                model.PageViewModel = new PageViewModel { CurrentPage = page };
                model.Entity = position;

                return View(model);
            }

            return NotFound();
        }

        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PositionsViewModel model)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            if (ModelState.IsValid & CheckUniqueValues(model.Entity))
            {
                Position position = _db.Positions.Find(model.Entity.PositionId);
                if (position != null)
                {
                    position.Title = model.Entity.Title;

                    _db.Positions.Update(position);
                    await _db.SaveChangesAsync();

                    _cache.Clean();

                    return RedirectToAction("Index", "Positions", new { page = model.PageViewModel.CurrentPage });
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
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> Delete(int id, int page)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            Position position = await _db.Positions.FindAsync(id);
            if (position == null)
                return NotFound();

            bool deleteFlag = false;
            string message = "Do you want to delete this entity";

            PositionsViewModel model = new PositionsViewModel();
            model.Entity = position;
            model.PageViewModel = new PageViewModel { CurrentPage = page };
            model.DeleteViewModel = new DeleteViewModel { Message = message, IsDeleted = deleteFlag };

            return View(model);
        }

        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(PositionsViewModel model)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            Position genre = await _db.Positions.FindAsync(model.Entity.PositionId);
            if (genre == null)
                return NotFound();

            _db.Positions.Remove(genre);
            await _db.SaveChangesAsync();

            _cache.Clean();

            model.DeleteViewModel = new DeleteViewModel { Message = "The entity was successfully deleted.", IsDeleted = true };

            return View(model);
        }

        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    private bool CheckUniqueValues(Position position)
    {
        bool firstFlag = true;

        Position tempgenre = _db.Positions.FirstOrDefault(g => g.Title == position.Title);
        if (tempgenre != null)
        {
            if (tempgenre.PositionId != position.PositionId)
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

    private IQueryable<Position> GetSortedEntities(SortState sortState, string title)
    {
        IQueryable<Position> positions = _db.Positions.AsQueryable();

        switch (sortState)
        {
            case SortState.PositionTitleAsc:
                positions = positions.OrderBy(g => g.Title);
                break;

            case SortState.PositionTitleDesc:
                positions = positions.OrderByDescending(g => g.Title);
                break;
        }

        if (!string.IsNullOrEmpty(title))
            positions = positions.Where(g => g.Title.Contains(title)).AsQueryable();

        return positions;
    }
}