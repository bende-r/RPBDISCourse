using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services;

using VideoRentalModels;

using VideoRentalMVC.Infrastructure;
using VideoRentalMVC.Models;
using VideoRentalMVC.Models.Entities;
using VideoRentalMVC.Models.Filters;

using VideoRentalWeb.DataModels;

namespace VideoRentalMVC.Controllers;

[Authorize]
public class DisksController : Controller
{
    private readonly VideoRentalContext _db;
    private readonly CacheProvider _cache;

    private const string FilterKey = "disks";

    public DisksController(VideoRentalContext context, CacheProvider cacheProvider)
    {
        _db = context;
        _cache = cacheProvider;
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
        DiskViewModel model = new DiskViewModel()
        {
            PageViewModel = new PageViewModel { CurrentPage = page }
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(DiskViewModel model)
    {
        if (ModelState.IsValid & CheckUniqueValues(model.Entity))
        {
            await _db.Disks.AddAsync(model.Entity);
            await _db.SaveChangesAsync();

            _cache.Clean();

            return RedirectToAction("Index", "Disks");
        }

        return View(model);
    }

    public async Task<IActionResult> Edit(int id, int page)
    {
        Disk disk = await _db.Disks.FindAsync(id);
        if (disk != null)
        {
            DiskViewModel model = new DiskViewModel();
            model.PageViewModel = new PageViewModel { CurrentPage = page };
            model.Entity = disk;

            return View(model);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(DiskViewModel model)
    {
        if (ModelState.IsValid & CheckUniqueValues(model.Entity))
        {
            Disk disk = _db.Disks.Find(model.Entity.DiskId);
            if (disk != null)
            {
                disk.Title = model.Entity.Title;

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

    public async Task<IActionResult> Delete(int id, int page)
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

    [HttpPost]
    public async Task<IActionResult> Delete(DiskViewModel model)
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

    private bool CheckUniqueValues(Disk disk)
    {
        bool firstFlag = true;

        Disk tempDisk = _db.Disks.FirstOrDefault(g => g.DiskId == disk.DiskId);
        if (tempDisk != null)
        {
            if (tempDisk.DiskId != disk.DiskId)
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

    private IQueryable<Disk> GetSortedEntities(SortState sortState, string carMarkName)
    {
        IQueryable<Disk> carMarks = _db.Disks.AsQueryable();

        switch (sortState)
        {
            case SortState.DiskTitleAsc:
                carMarks = carMarks.OrderBy(g => g.Title);
                break;

            case SortState.DiskTitleDesc:
                carMarks = carMarks.OrderByDescending(g => g.Title);
                break;
        }

        if (!string.IsNullOrEmpty(carMarkName))
            carMarks = carMarks.Where(g => g.Title.Contains(carMarkName)).AsQueryable();

        return carMarks;
    }
}