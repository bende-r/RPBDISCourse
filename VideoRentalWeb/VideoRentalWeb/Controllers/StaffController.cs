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

public class StaffController : Controller
{
    private readonly VideoRentalContext _db;
    private readonly CacheProvider _cache;

    private const string FilterKey = "staff";

    public StaffController(VideoRentalContext context, CacheProvider cacheProvider)
    {
        _db = context;
        _cache = cacheProvider;
    }

    public IActionResult Index(SortState sortState = SortState.StaffNameAsc, int page = 1)
    {
        StaffFilterViewModel filter = HttpContext.Session.Get<StaffFilterViewModel>(FilterKey);
        if (filter == null)
        {
            filter = new StaffFilterViewModel() { Name = string.Empty };
            HttpContext.Session.Set(FilterKey, filter);
        }

        string modelKey = $"{typeof(Staff).Name}-{page}-{sortState}-{filter.Name}";
        if (!_cache.TryGetValue(modelKey, out StaffViewModel model))
        {
            model = new StaffViewModel();

            IQueryable<Staff> carMarks = GetSortedEntities(sortState, filter.Name);

            int count = carMarks.Count();
            int pageSize = 10;
            model.PageViewModel = new PageViewModel(page, count, pageSize);

            model.Entities = count == 0 ? new List<Staff>() : carMarks.Skip((model.PageViewModel.CurrentPage - 1) * pageSize).Take(pageSize).ToList();
            model.SortViewModel = new SortViewModel(sortState);
            model.StaffFilterViewModel = filter;

            _cache.Set(modelKey, model);
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult Index(StaffFilterViewModel filterModel, int page)
    {
        StaffFilterViewModel filter = HttpContext.Session.Get<StaffFilterViewModel>(FilterKey);
        if (filter != null)
        {
            filter.Name = filterModel.Name;

            HttpContext.Session.Remove(FilterKey);
            HttpContext.Session.Set(FilterKey, filter);
        }

        return RedirectToAction("Index", new { page });
    }

    public IActionResult Create(int page)
    {
        StaffViewModel model = new StaffViewModel()
        {
            PageViewModel = new PageViewModel { CurrentPage = page }
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(StaffViewModel model)
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
            await _db.Staff.AddAsync(model.Entity);
            await _db.SaveChangesAsync();

            _cache.Clean();

            return RedirectToAction("Index", "Genres");
        }

        return View(model);
    }

    public async Task<IActionResult> Edit(int id, int page)
    {
        Staff staff = await _db.Staff.FindAsync(id);
        if (staff != null)
        {
            StaffViewModel model = new StaffViewModel();
            model.PageViewModel = new PageViewModel { CurrentPage = page };
            model.Entity = staff;

            return View(model);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(StaffViewModel model)
    {
        if (ModelState.IsValid & CheckUniqueValues(model.Entity))
        {
            Staff staff = _db.Staff.Find(model.Entity.StaffId);
            if (staff != null)
            {
                staff.Name = model.Entity.Name;

                _db.Staff.Update(staff);
                await _db.SaveChangesAsync();

                _cache.Clean();

                return RedirectToAction("Index", "Staff", new { page = model.PageViewModel.CurrentPage });
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
        Staff staff = await _db.Staff.FindAsync(id);
        if (staff == null)
            return NotFound();

        bool deleteFlag = false;
        string message = "Do you want to delete this entity";

      
        StaffViewModel model = new StaffViewModel();
        model.Entity = staff;
        model.PageViewModel = new PageViewModel { CurrentPage = page };
        model.DeleteViewModel = new DeleteViewModel { Message = message, IsDeleted = deleteFlag };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(StaffViewModel model)
    {
        Staff staff = await _db.Staff.FindAsync(model.Entity.StaffId);
        if (staff == null)
            return NotFound();

        _db.Staff.Remove(staff);
        await _db.SaveChangesAsync();

        _cache.Clean();

        model.DeleteViewModel = new DeleteViewModel { Message = "The entity was successfully deleted.", IsDeleted = true };

        return View(model);
    }

    private bool CheckUniqueValues(Staff staff)
    {
        bool firstFlag = true;

        Staff tempgenre = _db.Staff.FirstOrDefault(g => g.StaffId == staff.StaffId);
        if (tempgenre != null)
        {
            if (staff.StaffId != tempgenre.StaffId)
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

    private IQueryable<Staff> GetSortedEntities(SortState sortState, string name)
    {
        IQueryable<Staff> staff = _db.Staff.AsQueryable();

        switch (sortState)
        {
            case SortState.StaffNameAsc:
                staff = staff.OrderBy(g => g.Name);
                break;

            case SortState.StaffNameDesc:
                staff = staff.OrderByDescending(g => g.Name);
                break;
        }

        if (!string.IsNullOrEmpty(name))
            staff = staff.Where(g => g.Name.Contains(name)).AsQueryable();

        return staff;
    }
}