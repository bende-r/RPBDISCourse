using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
    private readonly UserManager<User> _userManager;
    public TakingsController(UserManager<User> userManager, VideoRentalContext context, CacheProvider cacheProvider)
    {
        _db = context;
        _userManager = userManager;
        _cache = cacheProvider;
    }

    public IActionResult Index(SortState sortState = SortState.DiskTitleAsc, int page = 1)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
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

                foreach (var VARIABLE in model.Entities)
                {
                    VARIABLE.Disk = _db.Disks.Find(VARIABLE.DiskId);
                    VARIABLE.Client = _db.Clienteles.Find(VARIABLE.ClientId);
                    VARIABLE.Staff = _db.Staff.Find(VARIABLE.StaffId);
                }
                model.SortViewModel = new SortViewModel(sortState);
                model.TakingsFilterViewModel = filter;

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
    public IActionResult Index(TakingsFilterViewModel filterModel, int page)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
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
            TakingViewModel model = new TakingViewModel(_db.Clienteles.ToList(), _db.Disks.ToList(), _db.Staff.ToList())
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
    public async Task<IActionResult> Create(TakingViewModel model)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            if (ModelState.IsValid)
            {
                Taking taking = new Taking()
                {
                    ClientId = model.ClientId,
                    DiskId = model.DiskId,
                    DateOfCapture = model.DateOfCapture,
                    ReturnDate = model.ReturnDate,
                    PaymentMark = model.PaymentMark,
                    RefundMark = model.RefundMark,
                    StaffId = model.StaffId,
                };

                await _db.Takings.AddAsync(taking);
                await _db.SaveChangesAsync();

                _cache.Clean();

                return RedirectToAction("Index", "Takings");
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
            Taking taking = await _db.Takings.FindAsync(id);
            if (taking != null)
            {
                TakingViewModel model = new TakingViewModel()
                {
                    Clientele = new SelectList(_db.Clienteles.ToList(), "ClientId", "Surname", taking.Client.Surname),
                    Staff = new SelectList(_db.Staff.ToList(), "StaffId", "Surname", taking.Staff.Surname),
                    Disks = new SelectList(_db.Disks.ToList(), "DiskId", "Title", taking.Disk.Title),
                };
                model.PageViewModel = new PageViewModel { CurrentPage = page };
                model.Entity = taking;

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
    public async Task<IActionResult> Edit(TakingViewModel model)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            Taking taking = _db.Takings.Find(model.Entity.TakeId);
            if (taking != null)
            {
                taking.ClientId = model.ClientId;
                taking.DiskId = model.DiskId;
                taking.DateOfCapture = model.DateOfCapture;
                taking.ReturnDate = model.ReturnDate;
                taking.PaymentMark = model.PaymentMark;
                taking.RefundMark = model.RefundMark;
                taking.StaffId = model.StaffId;

                _db.Takings.Update(taking);
                await _db.SaveChangesAsync();

                _cache.Clean();

                return RedirectToAction("Index", "Takings", new { page = model.PageViewModel.CurrentPage });
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

        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(TakingViewModel model)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
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

        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            Taking taking = await _db.Takings.FindAsync(id);
            if (taking != null)
            {
                Taking taking1 = taking;
                taking1.Client = _db.Clienteles.FirstOrDefault(clientele => clientele.ClientId == taking1.ClientId);
                taking1.Staff = _db.Staff.FirstOrDefault(staff => staff.StaffId == taking1.StaffId);
                taking1.Disk = _db.Disks.FirstOrDefault(disk => disk.DiskId == taking1.DiskId);

                TakingViewModel model = new TakingViewModel()
                {
                    Entity = taking1,

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


    public async Task<IActionResult> Overdue()
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            DateTime currentDate = DateTime.Now;

            var overdueTakes = _db.Takings
                .Where(t => t.ReturnDate < currentDate)
                .ToList();

            foreach (var VARIABLE in overdueTakes)
            {
                VARIABLE.Disk = _db.Disks.Find(VARIABLE.DiskId);
                VARIABLE.Client = _db.Clienteles.Find(VARIABLE.ClientId);
                VARIABLE.Staff = _db.Staff.Find(VARIABLE.StaffId);
            }

            TakingViewModel model = new TakingViewModel()
            {
                Entities = overdueTakes,

                PageViewModel = new PageViewModel()
            };

            return View(model);


            return NotFound();
        }

        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    private bool CheckUniqueValues(Taking taking)
    {
        bool firstFlag = true;

        Taking tempgenre = _db.Takings.FirstOrDefault(g => g.DiskId == taking.DiskId);
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