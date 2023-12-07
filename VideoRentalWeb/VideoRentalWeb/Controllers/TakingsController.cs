using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using VideoRentalWeb.DataModels;
using VideoRentalWeb.Infrastructure;
using VideoRentalWeb.Models;
using VideoRentalWeb.Models.Entities;
using VideoRentalWeb.Models.Filters;
using VideoRentalWeb.Services;

namespace VideoRentalWeb.Controllers;

public class TakingsController : Controller
{
    private readonly VideoRentalContext _db;
    private readonly CacheProvider _cache;

    private const string FilterKey = "takings";

    public TakingsController(VideoRentalContext context, CacheProvider cacheProvider)
    {
        _db = context;
        _cache = cacheProvider;
    }

    public IActionResult Index(SortState sortState = SortState.DiskTitleAsc, int page = 1)
    {
        TakingsFilterViewModel filter = HttpContext.Session.Get<TakingsFilterViewModel>(FilterKey);
        if (filter == null)
        {
            filter = new TakingsFilterViewModel() { DiskId = 0 };
            HttpContext.Session.Set(FilterKey, filter);
        }

        string modelKey = $"{typeof(Taking).Name}-{page}-{sortState}-{filter.DiskId}";
        if (!_cache.TryGetValue(modelKey, out TakingViewModel model))
        {
            model = new TakingViewModel();

            IQueryable<Taking> takings = GetSortedEntities(sortState, filter.DiskId.ToString());

            int count = takings.Count();
            int pageSize = 10;
            model.PageViewModel = new PageViewModel(page, count, pageSize);

            model.Entities = count == 0 ? new List<Taking>() : takings.Skip((model.PageViewModel.CurrentPage - 1) * pageSize).Take(pageSize).ToList();
            model.SortViewModel = new SortViewModel(sortState);
            model.TakingsFilterViewModel = filter;

            _cache.Set(modelKey, model);
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult Index(TakingsFilterViewModel filterModel, int page)
    {
        TakingsFilterViewModel filter = HttpContext.Session.Get<TakingsFilterViewModel>(FilterKey);
        if (filter != null)
        {
            filter.DiskId = filterModel.DiskId;

            HttpContext.Session.Remove(FilterKey);
            HttpContext.Session.Set(FilterKey, filter);
        }

        return RedirectToAction("Index", new { page });
    }

    public IActionResult Create(int page)
    {
        TakingViewModel model = new TakingViewModel()
        {
            PageViewModel = new PageViewModel { CurrentPage = page }
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TakingViewModel model)
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
            await _db.Takings.AddAsync(model.Entity);
            await _db.SaveChangesAsync();

            _cache.Clean();

            return RedirectToAction("Index", "Takings");
        }

        return View(model);
    }

    public async Task<IActionResult> Edit(int id, int page)
    {
        Taking taking = await _db.Takings.FindAsync(id);
        if (taking != null)
        {
            TakingViewModel model = new TakingViewModel();
            model.PageViewModel = new PageViewModel { CurrentPage = page };
            model.Entity = taking;

            return View(model);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TakingViewModel model)
    {
        if (ModelState.IsValid & CheckUniqueValues(model.Entity))
        {
            Taking taking = _db.Takings.Find(model.Entity.TakeId);
            if (taking != null)
            {
                taking.ClientId = model.Entity.ClientId;

                _db.Takings.Update(taking);
                await _db.SaveChangesAsync();

                _cache.Clean();

                return RedirectToAction("Index", "Takings", new { page = model.PageViewModel.CurrentPage });
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
        Taking taking = await _db.Takings.FindAsync(id);
        if (taking == null)
            return NotFound();

        bool deleteFlag = false;
        string message = "Do you want to delete this entity";

       
        TakingViewModel model = new TakingViewModel();
        model.Entity = taking;
        model.PageViewModel = new PageViewModel { CurrentPage = page };
        model.DeleteViewModel = new DeleteViewModel { Message = message, IsDeleted = deleteFlag };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(TakingViewModel model)
    {
        Taking taking = await _db.Takings.FindAsync(model.Entity.TakeId);
        if (taking == null)
            return NotFound();

        _db.Takings.Remove(taking);
        await _db.SaveChangesAsync();

        _cache.Clean();

        model.DeleteViewModel = new DeleteViewModel { Message = "The entity was successfully deleted.", IsDeleted = true };

        return View(model);
    }

    private bool CheckUniqueValues(Taking taking)
    {
        bool firstFlag = true;

        Taking tempgenre = _db.Takings.FirstOrDefault(g => g.TakeId == taking.TakeId);
        if (tempgenre != null)
        {
            if (taking.TakeId != tempgenre.TakeId)
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

    private IQueryable<Taking> GetSortedEntities(SortState sortState, string take)
    {
        IQueryable<Taking> takings = _db.Takings.AsQueryable();

        switch (sortState)
        {
            case SortState.TakingsDiskIdAsc:
                takings = takings.OrderBy(g => g.DiskId);
                break;

            case SortState.TakingsDiskIdDesc:
                takings = takings.OrderByDescending(g => g.DiskId);
                break;
        }

        if (!string.IsNullOrEmpty(take))
            takings = takings.AsQueryable();

        return takings;
    }
}