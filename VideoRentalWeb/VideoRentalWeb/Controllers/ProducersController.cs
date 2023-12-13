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

public class ProducersController : Controller
{
    private readonly VideoRentalContext _db;
    private readonly CacheProvider _cache;

    private const string FilterKey = "producers";

    private readonly UserManager<User> _userManager;

    public ProducersController(UserManager<User> userManager, VideoRentalContext context, CacheProvider cacheProvider)
    {
        _db = context;
        _cache = cacheProvider;
        _userManager = userManager;
    }

    public IActionResult Index(SortState sortState = SortState.ManufacturerAsc, int page = 1)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
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

        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public IActionResult Index(ProducersFilterViewModel filterModel, int page)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
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
            ProducersViewModel model = new ProducersViewModel()
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
    public async Task<IActionResult> Create(ProducersViewModel model)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            if (ModelState.IsValid & CheckUniqueValues(model.Entity))
            {
                await _db.Producers.AddAsync(model.Entity);
                await _db.SaveChangesAsync();

                _cache.Clean();

                return RedirectToAction("Index", "Producers");
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

        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProducersViewModel model)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
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

        else
        {
            return RedirectToAction("Index", "Home");
        }
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
            case SortState.ManufacturerAsc:
                producers = producers.OrderBy(g => g.Manufacturer);
                break;

            case SortState.ManufacturerDesc:
                producers = producers.OrderByDescending(g => g.Manufacturer);
                break;

            case SortState.CounrtyAsc:
                producers = producers.OrderBy(g => g.Country);
                break;

            case SortState.CounrtyDesc:
                producers = producers.OrderByDescending(g => g.Country);
                break;
        }

        if (!string.IsNullOrEmpty(genreTitle))
            producers = producers.Where(g => g.Manufacturer.Contains(genreTitle)).AsQueryable();

        return producers;
    }
}