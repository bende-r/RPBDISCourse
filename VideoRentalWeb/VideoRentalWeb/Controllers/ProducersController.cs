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

public class ProducersController : Controller
{
    private readonly VideoRentalContext _db;
    private readonly CacheProvider _cache;

    private const string FilterKey = "producers";

    public ProducersController(VideoRentalContext context, CacheProvider cacheProvider)
    {
        _db = context;
        _cache = cacheProvider;
    }

    public IActionResult Index(SortState sortState = SortState.ManufacturerAsc, int page = 1)
    {
        ProducersFilterViewModel filter = HttpContext.Session.Get<ProducersFilterViewModel>(FilterKey);
        if (filter == null)
        {
            filter = new ProducersFilterViewModel() { Manufacturer = string.Empty };
            HttpContext.Session.Set(FilterKey, filter);
        }

        string modelKey = $"{typeof(Producer).Name}-{page}-{sortState}-{filter.Manufacturer}";
        if (!_cache.TryGetValue(modelKey, out ProducersViewModel model))
        {
            model = new ProducersViewModel();

            IQueryable<Producer> producers = GetSortedEntities(sortState, filter.Manufacturer);

            int count = producers.Count();
            int pageSize = 10;
            model.PageViewModel = new PageViewModel(page, count, pageSize);

            model.Entities = count == 0 ? new List<Producer>() : producers.Skip((model.PageViewModel.CurrentPage - 1) * pageSize).Take(pageSize).ToList();
            model.SortViewModel = new SortViewModel(sortState);
            model.ProducersFilterViewModel = filter;

            _cache.Set(modelKey, model);
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult Index(ProducersFilterViewModel filterModel, int page)
    {
        ProducersFilterViewModel filter = HttpContext.Session.Get<ProducersFilterViewModel>(FilterKey);
        if (filter != null)
        {
            filter.Manufacturer = filterModel.Manufacturer;

            HttpContext.Session.Remove(FilterKey);
            HttpContext.Session.Set(FilterKey, filter);
        }

        return RedirectToAction("Index", new { page });
    }

    public IActionResult Create(int page)
    {
        ProducersViewModel model = new ProducersViewModel()
        {
            PageViewModel = new PageViewModel { CurrentPage = page }
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProducersViewModel model)
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
            await _db.Producers.AddAsync(model.Entity);
            await _db.SaveChangesAsync();

            _cache.Clean();

            return RedirectToAction("Index", "Producers");
        }

        return View(model);
    }

    public async Task<IActionResult> Edit(int id, int page)
    {
        Producer producer = await _db.Producers.FindAsync(id);
        if (producer != null)
        {
            ProducersViewModel model = new ProducersViewModel();
            model.PageViewModel = new PageViewModel { CurrentPage = page };
            model.Entity = producer;

            return View(model);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProducersViewModel model)
    {
        if (ModelState.IsValid & CheckUniqueValues(model.Entity))
        {
            Producer producer = _db.Producers.Find(model.Entity.ProduceId);
            if (producer != null)
            {
                producer.Manufacturer = model.Entity.Manufacturer;
                producer.Country = model.Entity.Country;

                _db.Producers.Update(producer);
                await _db.SaveChangesAsync();

                _cache.Clean();

                return RedirectToAction("Index", "Producers", new { page = model.PageViewModel.CurrentPage });
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
        Producer producer = await _db.Producers.FindAsync(id);
        if (producer == null)
            return NotFound();

        bool deleteFlag = false;
        string message = "Do you want to delete this entity";

        if (_db.Disks.Any(s => s.Producer == producer.ProduceId))
            message = "This entity has entities, which dependents from this. Do you want to delete this entity and other, which dependents from this?";

        ProducersViewModel model = new ProducersViewModel();
        model.Entity = producer;
        model.PageViewModel = new PageViewModel { CurrentPage = page };
        model.DeleteViewModel = new DeleteViewModel { Message = message, IsDeleted = deleteFlag };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(ProducersViewModel model)
    {
        Producer producer = await _db.Producers.FindAsync(model.Entity.ProduceId);
        if (producer == null)
            return NotFound();

        _db.Producers.Remove(producer);
        await _db.SaveChangesAsync();

        _cache.Clean();

        model.DeleteViewModel = new DeleteViewModel { Message = "The entity was successfully deleted.", IsDeleted = true };

        return View(model);
    }

    private bool CheckUniqueValues(Producer producer)
    {
        bool firstFlag = true;

        Producer tempprod = _db.Producers.FirstOrDefault(g => g.Manufacturer == producer.Manufacturer);
        if (tempprod != null)
        {
            if (tempprod.ProduceId != producer.ProduceId)
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

    private IQueryable<Producer> GetSortedEntities(SortState sortState, string genreTitle)
    {
        IQueryable<Producer> producers = _db.Producers.AsQueryable();

        switch (sortState)
        {
            case SortState.DiskTitleAsc:
                producers = producers.OrderBy(g => g.Manufacturer);
                break;

            case SortState.DiskTitleDesc:
                producers = producers.OrderByDescending(g => g.Manufacturer);
                break;
        }

        if (!string.IsNullOrEmpty(genreTitle))
            producers = producers.Where(g => g.Manufacturer.Contains(genreTitle)).AsQueryable();

        return producers;
    }
}