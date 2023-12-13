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

public class GenresController : Controller
{
    private readonly VideoRentalContext _db;
    private readonly CacheProvider _cache;

    private const string FilterKey = "genres";

    private readonly UserManager<User> _userManager;

    public GenresController(UserManager<User> userManager, VideoRentalContext context, CacheProvider cacheProvider)
    {
        _db = context;
        _cache = cacheProvider;
        _userManager = userManager;
    }

    public IActionResult Index(SortState sortState = SortState.DiskTitleAsc, int page = 1)
    {
        GenresFilterViewModel filter = HttpContext.Session.Get<GenresFilterViewModel>(FilterKey);
        if (filter == null)
        {
            filter = new GenresFilterViewModel() { GenreTitle = string.Empty };
            HttpContext.Session.Set(FilterKey, filter);
        }

        string modelKey = $"{typeof(Genre).Name}-{page}-{sortState}-{filter.GenreTitle}";
        if (!_cache.TryGetValue(modelKey, out GenreViewModel model))
        {
            model = new GenreViewModel();

            IQueryable<Genre> carMarks = GetSortedEntities(sortState, filter.GenreTitle);

            int count = carMarks.Count();
            int pageSize = 10;
            model.PageViewModel = new PageViewModel(page, count, pageSize);

            model.Entities = count == 0 ? new List<Genre>() : carMarks.Skip((model.PageViewModel.CurrentPage - 1) * pageSize).Take(pageSize).ToList();
            model.SortViewModel = new SortViewModel(sortState);
            model.GenresFilterViewModel = filter;

            _cache.Set(modelKey, model);
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult Index(GenresFilterViewModel filterModel, int page)
    {
        GenresFilterViewModel filter = HttpContext.Session.Get<GenresFilterViewModel>(FilterKey);
        if (filter != null)
        {
            filter.GenreTitle = filterModel.GenreTitle;

            HttpContext.Session.Remove(FilterKey);
            HttpContext.Session.Set(FilterKey, filter);
        }

        return RedirectToAction("Index", new { page });
    }

    public IActionResult Create(int page)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            GenreViewModel model = new GenreViewModel()
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
    public async Task<IActionResult> Create(GenreViewModel model)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            if (ModelState.IsValid & CheckUniqueValues(model.Entity))
        {
            await _db.Genres.AddAsync(model.Entity);
            await _db.SaveChangesAsync();

            _cache.Clean();

            return RedirectToAction("Index", "Genres");
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
            Genre genre = await _db.Genres.FindAsync(id);
        if (genre != null)
        {
            GenreViewModel model = new GenreViewModel();
            model.PageViewModel = new PageViewModel { CurrentPage = page };
            model.Entity = genre;

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
    public async Task<IActionResult> Edit(GenreViewModel model)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            if (ModelState.IsValid & CheckUniqueValues(model.Entity))
        {
            Genre genre = _db.Genres.Find(model.Entity.GenreId);
            if (genre != null)
            {
                genre.Title = model.Entity.Title;
                genre.Description = model.Entity.Description;

                _db.Genres.Update(genre);
                await _db.SaveChangesAsync();

                _cache.Clean();

                return RedirectToAction("Index", "Genres", new { page = model.PageViewModel.CurrentPage });
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
            Genre genre = await _db.Genres.FindAsync(id);
        if (genre == null)
            return NotFound();

        bool deleteFlag = false;
        string message = "Do you want to delete this entity";

        if (_db.Disks.Any(s => s.DiskId == genre.GenreId))
            message = "This entity has entities, which dependents from this. Do you want to delete this entity and other, which dependents from this?";

        GenreViewModel model = new GenreViewModel();
        model.Entity = genre;
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
    public async Task<IActionResult> Delete(GenreViewModel model)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            Genre genre = await _db.Genres.FindAsync(model.Entity.GenreId);
        if (genre == null)
            return NotFound();

        _db.Genres.Remove(genre);
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

    public async Task<IActionResult> Details(int id)
    {
        Genre genre = await _db.Genres.FindAsync(id);
        if (genre != null)
        {
            GenreViewModel model = new GenreViewModel()
            {
                Entity = genre,

                PageViewModel = new PageViewModel()
            };

            return View(model);
        }

        return NotFound();
    }

    private bool CheckUniqueValues(Genre genre)
    {
        bool firstFlag = true;

        Genre tempgenre = _db.Genres.FirstOrDefault(g => g.Title == genre.Title);
        if (tempgenre != null)
        {
            if (tempgenre.GenreId != tempgenre.GenreId)
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

    private IQueryable<Genre> GetSortedEntities(SortState sortState, string genreTitle)
    {
        IQueryable<Genre> genres = _db.Genres.AsQueryable();

        switch (sortState)
        {
            case SortState.GenreTitleAsc:
                genres = genres.OrderBy(g => g.Title);
                break;

            case SortState.GenreTitleDesc:
                genres = genres.OrderByDescending(g => g.Title);
                break;
        }

        if (!string.IsNullOrEmpty(genreTitle))
            genres = genres.Where(g => g.Title.Contains(genreTitle)).AsQueryable();

        return genres;
    }
}