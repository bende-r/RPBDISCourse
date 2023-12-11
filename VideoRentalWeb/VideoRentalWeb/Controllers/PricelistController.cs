﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using VideoRentalWeb.DataModels;
using VideoRentalWeb.Infrastructure;
using VideoRentalWeb.Models;
using VideoRentalWeb.Models.Entities;
using VideoRentalWeb.Models.Filters;
using VideoRentalWeb.Services;

namespace VideoRentalWeb.Controllers;

public class PricelistController : Controller
{
    private readonly VideoRentalContext _db;
    private readonly CacheProvider _cache;

    private const string FilterKey = "pricelist";

    public PricelistController(VideoRentalContext context, CacheProvider cacheProvider)
    {
        _db = context;
        _cache = cacheProvider;
    }

    public IActionResult Index(SortState sortState = SortState.PricelistDiscIdAsc, int page = 1)
    {
        PricelistFilterViewModel filter = HttpContext.Session.Get<PricelistFilterViewModel>(FilterKey);
        if (filter == null)
        {
            filter = new PricelistFilterViewModel() { Title = null };
            HttpContext.Session.Set(FilterKey, filter);
        }

        string modelKey = $"{typeof(Pricelist).Name}-{page}-{sortState}-{filter.Title}";
        if (!_cache.TryGetValue(modelKey, out PricelistViewModel model))
        {
            model = new PricelistViewModel();

            IQueryable<Pricelist> pricelists = GetSortedEntities(sortState, filter.Title);

            int count = pricelists.Count();
            int pageSize = 10;
            model.PageViewModel = new PageViewModel(page, count, pageSize);

            model.Entities = count == 0 ? new List<Pricelist>() : pricelists.Skip((model.PageViewModel.CurrentPage - 1) * pageSize).Take(pageSize).ToList();

            foreach (var VARIABLE in model.Entities)
            {
                VARIABLE.Disk =  _db.Disks.Find(VARIABLE.DiskId);
            }

            model.SortViewModel = new SortViewModel(sortState);
            model.PricelistFilterViewModel = filter;

            _cache.Set(modelKey, model);
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult Index(PricelistFilterViewModel filterModel, int page)
    {
        PricelistFilterViewModel filter = HttpContext.Session.Get<PricelistFilterViewModel>(FilterKey);
        if (filter != null)
        {
            filter.Title = filterModel.Title;

            HttpContext.Session.Remove(FilterKey);
            HttpContext.Session.Set(FilterKey, filter);
        }

        return RedirectToAction("Index", new { page });
    }

    public IActionResult Create(int page)
    {
        PricelistViewModel model = new PricelistViewModel(_db.Disks.ToList())
        {
            PageViewModel = new PageViewModel { CurrentPage = page }
        };

      //  ViewBag.Disks = new SelectList(_db.Disks, "DiskId", "Title"); ;

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PricelistViewModel model)
    {
        //   model.Entity.DiskId = model.DiskId;
        foreach (var entry in ModelState)
        {
            var key = entry.Key; // Название свойства
            var errors = entry.Value.Errors.Select(e => e.ErrorMessage).ToList(); // Список ошибок для свойства

            // Далее можно использовать key и errors в соответствии с вашими потребностями
            Console.WriteLine($"Property: {key}, Errors: {string.Join(", ", errors)}");
        }

        if (ModelState.IsValid)
        {
            Pricelist p = new Pricelist()
            {
                DiskId = model.
                    DiskId,
                Price = model.
                    Price,
            };


            await _db.Pricelists.AddAsync(p);
            await _db.SaveChangesAsync();

            _cache.Clean();

            return RedirectToAction("Index", "Pricelist");
        }

        return View(model);
    }

    public async Task<IActionResult> Edit(int id, int page)
    {
        Pricelist pricelist = await _db.Pricelists.FindAsync(id);
        if (pricelist != null)
        {
            PricelistViewModel model = new PricelistViewModel();
            model.PageViewModel = new PageViewModel { CurrentPage = page };
            model.Entity = pricelist;

            return View(model);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PricelistViewModel model)
    {
        if (ModelState.IsValid & CheckUniqueValues(model.Entity))
        {
          Pricelist pricelist = _db.Pricelists.Find(model.Entity.PriceId);
          if (pricelist != null)
            {
               pricelist.Price = model.Entity.Price;

               _db.Pricelists.Update(pricelist);
                await _db.SaveChangesAsync();

                _cache.Clean();

                return RedirectToAction("Index", "Pricelist", new { page = model.PageViewModel.CurrentPage });
            }
           else
            {return NotFound();
           }
        }

        return View(model);
    }

    public async Task<IActionResult> Delete(int id, int page)
    {
        Pricelist pricelist = await _db.Pricelists.FindAsync(id);
        if (pricelist == null)
            return NotFound();

        bool deleteFlag = false;
        string message = "Do you want to delete this entity";

        PricelistViewModel model = new PricelistViewModel();
        model.Entity = pricelist;
        model.PageViewModel = new PageViewModel { CurrentPage = page };
        model.DeleteViewModel = new DeleteViewModel { Message = message, IsDeleted = deleteFlag };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(PricelistViewModel model)
    {
       Pricelist pricelist = await _db.Pricelists.FindAsync(model.Entity.PriceId);
        if (pricelist == null)
            return NotFound();

        _db.Pricelists.Remove(pricelist);
        await _db.SaveChangesAsync();

        _cache.Clean();

        model.DeleteViewModel = new DeleteViewModel { Message = "The entity was successfully deleted.", IsDeleted = true };

        return View(model);
    }

    private bool CheckUniqueValues(Pricelist pricelist)
    {
        bool firstFlag = true;

        Pricelist tempgenre = _db.Pricelists.FirstOrDefault(g => g.DiskId == pricelist.DiskId);
        if (tempgenre != null)
        {
            if (pricelist.PriceId != tempgenre.PriceId)
            {
                ModelState.AddModelError(string.Empty, "Another entity have this disk Id. Please replace this to another.");
                firstFlag = false;
            }
        }

        if (firstFlag)
            return true;
        else
            return false;
    }

    private IQueryable<Pricelist> GetSortedEntities(SortState sortState, string title)
    {
        IQueryable<Pricelist> pricelists = _db.Pricelists.AsQueryable();

        foreach (var pricelist in pricelists)
        {
            Console.WriteLine(pricelist.DiskId.ToString(), pricelist.Price.ToString());
        }

        switch (sortState)
        {
            case SortState.DiskTitleAsc:
                pricelists = pricelists.OrderBy(g => g.Disk.Title);
                break;

            case SortState.DiskTitleDesc:
                pricelists = pricelists.OrderByDescending(g => g.Disk.Title);
                break;

            case SortState.MainActorAsc:
                pricelists = pricelists.OrderBy(g => g.Disk.MainActor);
                break;

            case SortState.MainActorDesc:
                pricelists = pricelists.OrderByDescending(g => g.Disk.MainActor);
                break;

            case SortState.DiskCreationYearAsc:
                pricelists = pricelists.OrderBy(g => g.Disk.CreationYear);
                break;

            case SortState.DiskCreationYearDesc:
                pricelists = pricelists.OrderByDescending(g => g.Disk.CreationYear);
                break;

            case SortState.PricelistDiscIdAsc:
                pricelists = pricelists.OrderBy(g => g.DiskId);
                break;

            case SortState.PricelistDiscIdDesc:
                pricelists = pricelists.OrderByDescending(g => g.DiskId);
                break;

            case SortState.PricelistPriceAsc:
                pricelists = pricelists.OrderBy(g => g.Price);
                break;

            case SortState.PricelistPriceDesc:
                pricelists = pricelists.OrderByDescending(g => g.Price);
                break;
        }

        if (!string.IsNullOrEmpty(title))
            pricelists = pricelists.Where(g => g.Disk.Title.Contains(title)).AsQueryable();

        return pricelists;
    }
}