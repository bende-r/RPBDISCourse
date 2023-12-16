using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

public class StaffController : Controller
{
    private readonly VideoRentalContext _db;
    private readonly CacheProvider _cache;

    private const string FilterKey = "staff";

    private readonly UserManager<User> _userManager;

    public StaffController(UserManager<User> userManager, VideoRentalContext context, CacheProvider cacheProvider)
    {
        _db = context;
        _userManager = userManager;
        _cache = cacheProvider;
    }

    public IActionResult Index(SortState sortState = SortState.StaffNameAsc, int page = 1)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Проверка наличия роли Admin у текущего пользователя
            if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
            {
                StaffFilterViewModel filter = HttpContext.Session.Get<StaffFilterViewModel>(FilterKey);
                if (filter == null)
                {
                    filter = new StaffFilterViewModel() { Surname = string.Empty };
                    HttpContext.Session.Set(FilterKey, filter);
                }

                string modelKey = $"{typeof(Staff).Name}-{page}-{sortState}-{filter.Surname}";
                if (!_cache.TryGetValue(modelKey, out StaffViewModel model))
                {
                    model = new StaffViewModel();

                    IQueryable<Staff> carMarks = GetSortedEntities(sortState, filter.Surname);

                    int count = carMarks.Count();
                    int pageSize = 10;
                    model.PageViewModel = new PageViewModel(page, count, pageSize);

                    model.Entities = count == 0 ? new List<Staff>() : carMarks.Skip((model.PageViewModel.CurrentPage - 1) * pageSize).Take(pageSize).ToList();
                    foreach (var VARIABLE in model.Entities)
                    {
                        VARIABLE.Position = _db.Positions.Find(VARIABLE.PositionId);
                    }
                    model.SortViewModel = new SortViewModel(sortState);
                    model.StaffFilterViewModel = filter;

                    _cache.Set(modelKey, model);
                }

                return View(model);
            }

            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public IActionResult Index(StaffFilterViewModel filterModel, int page)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Проверка наличия роли Admin у текущего пользователя
            if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
            {
                StaffFilterViewModel filter = HttpContext.Session.Get<StaffFilterViewModel>(FilterKey);
                if (filter != null)
                {
                    filter.Surname = filterModel.Surname;

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
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public IActionResult Create(int page)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Проверка наличия роли Admin у текущего пользователя
            if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
            {
                StaffViewModel model = new StaffViewModel(_db.Positions.ToList())
                {
                    PageViewModel = new PageViewModel { CurrentPage = page }
                };

                return View(model);
            }

            else
            {
                return RedirectToAction("Index", "Staff");
            }
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(StaffViewModel model)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Проверка наличия роли Admin у текущего пользователя
            if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
            {

                if (ModelState.IsValid)
                {
                    Staff staff = new Staff()
                    {
                        Surname = model.Surname,
                        Name = model.Name,
                        Middlename = model.Middlename,
                        DateOfEmployment = model.DateOfEmployment,
                        PositionId = model.PositionId,
                    };


                    await _db.Staff.AddAsync(staff);
                    await _db.SaveChangesAsync();

                    _cache.Clean();

                    return RedirectToAction("Index", "Staff");
                }

                return View(model);
            }

            else
            {
                return RedirectToAction("Index", "Staff");
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
                Staff staff = await _db.Staff.FindAsync(id);
                if (staff != null)
                {
                    StaffViewModel model = new StaffViewModel()
                    {
                        Positions = new SelectList(_db.Positions.ToList(), "PositionId", "Title", staff.Position.Title),
                    };
                    model.PageViewModel = new PageViewModel { CurrentPage = page };
                    model.Entity = staff;

                    return View(model);
                }

                return NotFound();
            }

            else
            {
                return RedirectToAction("Index", "Staff");
            }
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(StaffViewModel model)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Проверка наличия роли Admin у текущего пользователя
            if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
            {

                if (ModelState.IsValid)
                {
                    Staff staff = _db.Staff.Find(model.Entity.StaffId);

                    if (staff != null)
                    {
                        staff.Name = model.Entity.Name;
                        staff.Surname = model.Entity.Surname;
                        staff.Middlename = model.Middlename;
                        staff.DateOfEmployment = model.DateOfEmployment;
                        staff.PositionId = model.PositionId;

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

            else
            {
                return RedirectToAction("Index", "Staff");
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

            else
            {
                return RedirectToAction("Index", "Staff");
            }
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(StaffViewModel model)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Проверка наличия роли Admin у текущего пользователя
            if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
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

            else
            {
                return RedirectToAction("Index", "Staff");
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
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Проверка наличия роли Admin у текущего пользователя
            if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
            {
                Staff staff = await _db.Staff.FindAsync(id);
                if (staff != null)
                {
                    Staff staff1 = staff;
                    staff1.Position = _db.Positions.FirstOrDefault(position => position.PositionId == staff1.PositionId);

                    StaffViewModel model = new StaffViewModel()
                    {
                        Entity = staff1,

                        PageViewModel = new PageViewModel()
                    };

                    return View(model);
                }

                return NotFound();
            }

            else
            {
                return RedirectToAction("Index", "Staff");
            }
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
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
            case SortState.StaffSurnameAsc:
                staff = staff.OrderBy(g => g.Surname);
                break;

            case SortState.StaffSurnameDesc:
                staff = staff.OrderByDescending(g => g.Surname);
                break;
            case SortState.StaffPositionAsc:
                staff = staff.OrderBy(g => g.Position.Title);
                break;

            case SortState.StaffPositionDesc:
                staff = staff.OrderByDescending(g => g.Position.Title);
                break;
        }

        if (!string.IsNullOrEmpty(name))
            staff = staff.Where(g => g.Name.Contains(name)).AsQueryable();

        return staff;
    }
}