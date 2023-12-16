using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using VideoRentalWeb.DataModels;
using VideoRentalWeb.Infrastructure;
using VideoRentalWeb.Models;
using VideoRentalWeb.Models.Entities;
using VideoRentalWeb.Models.Filters;
using VideoRentalWeb.Services;

namespace VideoRentalWeb.Controllers;

public class DisksController : Controller
{
    private readonly VideoRentalContext _db;
    private readonly CacheProvider _cache;

    private const string FilterKey = "disks";
    private readonly UserManager<User> _userManager;

    public DisksController(UserManager<User> userManager, VideoRentalContext context, CacheProvider cacheProvider)
    {
        _db = context;
        _cache = cacheProvider;
        _userManager = userManager;
    }

    public IActionResult Index(SortState sortState = SortState.DiskTitleAsc, int page = 1)
    {
        DisksFilterViewModel filter = HttpContext.Session.Get<DisksFilterViewModel>(FilterKey);
        if (filter == null)
        {
            filter = new DisksFilterViewModel() { DiskTitle = string.Empty };
            HttpContext.Session.Set(FilterKey, filter);
        }

        string modelKey = $"{typeof(Disk).Name}-{page}-{sortState}-{filter.DiskTitle}";
        if (!_cache.TryGetValue(modelKey, out DiskViewModel model))
        {
            model = new DiskViewModel();

            IQueryable<Disk> carMarks = GetSortedEntities(sortState, filter.DiskTitle);

            int count = carMarks.Count();
            int pageSize = 10;
            model.PageViewModel = new PageViewModel(page, count, pageSize);

            model.Entities = count == 0 ? new List<Disk>() : carMarks.Skip((model.PageViewModel.CurrentPage - 1) * pageSize).Take(pageSize).ToList();
            model.SortViewModel = new SortViewModel(sortState);
            model.DiskFilterViewModel = filter;

            _cache.Set(modelKey, model);
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult Index(DisksFilterViewModel filterModel, int page)
    {
        DisksFilterViewModel filter = HttpContext.Session.Get<DisksFilterViewModel>(FilterKey);
        if (filter != null)
        {
            filter.DiskTitle = filterModel.DiskTitle;

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
                DiskViewModel model = new DiskViewModel(_db.Producers.ToList(), _db.Genres.ToList(), _db.Types.ToList())
                {
                    PageViewModel = new PageViewModel { CurrentPage = page }
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Disks");
            }
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(DiskViewModel model)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Проверка наличия роли Admin у текущего пользователя
            if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
            {
                if (ModelState.IsValid)
                {
                    Disk newDisk = new Disk()
                    {
                        Title = model.
                            Title,
                        CreationYear = model.CreationYear,

                        Producer = model.ProducerId,

                        MainActor = model.MainActor,

                        Recording = model.Recording,
                        GenreId = model.GenreId,
                        DiskType = model.TypeId,
                    };


                    await _db.Disks.AddAsync(newDisk);
                    await _db.SaveChangesAsync();

                    _cache.Clean();

                    return RedirectToAction("Index", "Disks");
                }

                return View(model);
            }

            else
            {
                return RedirectToAction("Index", "Disks");
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
                Disk disk = await _db.Disks.FindAsync(id);
                if (disk != null)
                {
                    DiskViewModel model = new DiskViewModel()
                    {
                        Genres = new SelectList(_db.Genres.ToList(), "GenreId", "Title", disk.Genre.Title),
                        Producers = new SelectList(_db.Producers.ToList(), "ProduceId", "Manufacturer", disk.ProducerNavigation.Manufacturer),
                        Types = new SelectList(_db.Types.ToList(), "TypeId", "Title", disk.DiskTypeNavigation.Title),
                    };
                    model.DiskId = id;
                    model.PageViewModel = new PageViewModel { CurrentPage = page };
                    model.Title = disk.Title;
                    model.CreationYear = disk.CreationYear;
                    model.ProducerId = disk.Producer;
                    model.MainActor = disk.MainActor;
                    model.Recording = disk.Recording;
                    model.GenreId = disk.GenreId;
                    model.TypeId = disk.DiskType;

                    return View(model);
                }

                return NotFound();
            }

            else
            {
                return RedirectToAction("Index", "Disks");
            }
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(DiskViewModel model)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Проверка наличия роли Admin у текущего пользователя
            if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
            {

                if (ModelState.IsValid)
                {
                    Disk disk = _db.Disks.Find(model.DiskId);
                    if (disk != null)
                    {
                        disk.Title = model.Title;
                        disk.CreationYear = model.CreationYear;
                        disk.Producer = model.ProducerId;
                        disk.MainActor = model.MainActor;
                        disk.Recording = model.Recording;
                        disk.GenreId = model.GenreId;
                        disk.DiskType = model.TypeId;

                        _db.Disks.Update(disk);
                        await _db.SaveChangesAsync();

                        _cache.Clean();

                        return RedirectToAction("Index", "Disks", new { page = model.PageViewModel.CurrentPage });
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
                return RedirectToAction("Index", "Disks");
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
                Disk disk = await _db.Disks.FindAsync(id);
                if (disk == null)
                    return NotFound();

                bool deleteFlag = false;
                string message = "Do you want to delete this entity";

                if (_db.Disks.Any(s => s.DiskId == disk.DiskId))
                    message = "This entity has entities, which dependents from this. Do you want to delete this entity and other, which dependents from this?";

                DiskViewModel model = new DiskViewModel();
                model.Entity = disk;
                model.PageViewModel = new PageViewModel { CurrentPage = page };
                model.DeleteViewModel = new DeleteViewModel { Message = message, IsDeleted = deleteFlag };

                return View(model);
            }

            else
            {
                return RedirectToAction("Index", "Disks");
            }
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DiskViewModel model)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Проверка наличия роли Admin у текущего пользователя
            if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
            {
                Disk disk = await _db.Disks.FindAsync(model.Entity.DiskId);
                if (disk == null)
                    return NotFound();

                _db.Disks.Remove(disk);
                await _db.SaveChangesAsync();

                _cache.Clean();

                model.DeleteViewModel = new DeleteViewModel { Message = "The entity was successfully deleted.", IsDeleted = true };

                return View(model);
            }

            else
            {
                return RedirectToAction("Index", "Disks");
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
            Disk disk = await _db.Disks.FindAsync(id);
            if (disk != null)
            {
                Disk disk1 = disk;
                disk1.Genre = _db.Genres.FirstOrDefault(genre => genre.GenreId == disk1.GenreId);
                disk1.DiskTypeNavigation = _db.Types.FirstOrDefault(type => type.TypeId == disk1.DiskType);
                disk1.ProducerNavigation = _db.Producers.FirstOrDefault(producer => producer.ProduceId == disk1.Producer);

                DiskViewModel model = new DiskViewModel()
                {
                    Entity = disk1,

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

    private IQueryable<Disk> GetSortedEntities(SortState sortState, string title)
    {
        IQueryable<Disk> disks = _db.Disks.AsQueryable();

        switch (sortState)
        {
            case SortState.DiskTitleAsc:
                disks = disks.OrderBy(g => g.Title);
                break;

            case SortState.DiskTitleDesc:
                disks = disks.OrderByDescending(g => g.Title);
                break;

            case SortState.MainActorAsc:
                disks = disks.OrderBy(g => g.MainActor);
                break;

            case SortState.MainActorDesc:
                disks = disks.OrderByDescending(g => g.MainActor);
                break;

            case SortState.DiskCreationYearAsc:
                disks = disks.OrderBy(g => g.CreationYear);
                break;

            case SortState.DiskCreationYearDesc:
                disks = disks.OrderByDescending(g => g.CreationYear);
                break;
        }

        if (!string.IsNullOrEmpty(title))
            disks = disks.Where(g => g.Title.Contains(title)).AsQueryable();

        return disks;
    }
}