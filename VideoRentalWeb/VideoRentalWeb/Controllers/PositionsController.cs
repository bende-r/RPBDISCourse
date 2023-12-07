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

public class PositionsController : Controller
{
    private readonly VideoRentalContext _db;
    private readonly CacheProvider _cache;

    private const string FilterKey = "positionss";

    public PositionsController(VideoRentalContext context, CacheProvider cacheProvider)
    {
        _db = context;
        _cache = cacheProvider;
    }

    public IActionResult Index(SortState sortState = SortState.GenreTitleAsc, int page = 1)
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

    [HttpPost]
    public IActionResult Index(PositionsFilterViewModel filterModel, int page)
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

    public IActionResult Create(int page)
    {
        PositionsViewModel model = new PositionsViewModel()
        {
            PageViewModel = new PageViewModel { CurrentPage = page }
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PositionsViewModel model)
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
            await _db.Positions.AddAsync(model.Entity);
            await _db.SaveChangesAsync();

            _cache.Clean();

            return RedirectToAction("Index", "Positions");
        }

        return View(model);
    }

    public async Task<IActionResult> Edit(int id, int page)
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

    [HttpPost]
    public async Task<IActionResult> Edit(PositionsViewModel model)
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

    public async Task<IActionResult> Delete(int id, int page)
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

    [HttpPost]
    public async Task<IActionResult> Delete(PositionsViewModel model)
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

    private bool CheckUniqueValues(Position position)
    {
        bool firstFlag = true;

        Position tempgenre = _db.Positions.FirstOrDefault(g => g.PositionId == position.PositionId);
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