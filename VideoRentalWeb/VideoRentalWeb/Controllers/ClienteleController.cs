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

public class ClienteleController : Controller
{
    private readonly VideoRentalContext _db;
    private readonly CacheProvider _cache;

    private readonly UserManager<User> _userManager;

    private const string FilterKey = "clientele";

    public ClienteleController( UserManager<User> userManager, VideoRentalContext context, CacheProvider cacheProvider)
    {
        _db = context;
        _cache = cacheProvider;
        //_roleManager = roleManager;
        _userManager = userManager;
    }

    public IActionResult Index(SortState sortState = SortState.ClientNameAsc, int page = 1)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            ClienteleFilterViewModel filter = HttpContext.Session.Get<ClienteleFilterViewModel>(FilterKey);
            if (filter == null)
            {
                filter = new ClienteleFilterViewModel() { Name = string.Empty };
                HttpContext.Session.Set(FilterKey, filter);
            }

            string modelKey = $"{typeof(Clientele).Name}-{page}-{sortState}-{filter.Name}";
            if (!_cache.TryGetValue(modelKey, out ClienteleViewModel model))
            {
                model = new ClienteleViewModel();

                IQueryable<Clientele> clienteles = GetSortedEntities(sortState, filter.Name);

                int count = clienteles.Count();
                int pageSize = 10;
                model.PageViewModel = new PageViewModel(page, count, pageSize);

                model.Entities = count == 0 ? new List<Clientele>() : clienteles.Skip((model.PageViewModel.CurrentPage - 1) * pageSize).Take(pageSize).ToList();
                model.SortViewModel = new SortViewModel(sortState);
                model.ClienteleFilterViewModel = filter;

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
    public IActionResult Index(ClienteleFilterViewModel filterModel, int page)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            ClienteleFilterViewModel filter = HttpContext.Session.Get<ClienteleFilterViewModel>(FilterKey);
            if (filter != null)
            {
                filter.Name = filterModel.Name;

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
            ClienteleViewModel model = new ClienteleViewModel()
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
    public async Task<IActionResult> Create(ClienteleViewModel model)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            foreach (var entry in ModelState)
            {
                var key = entry.Key; // Название свойства
                var errors = entry.Value.Errors.Select(e => e.ErrorMessage).ToList(); // Список ошибок для свойства

                // Далее можно использовать key и errors в соответствии с вашими потребностями
                Console.WriteLine($"Property: {key}, Errors: {string.Join(", ", errors)}");
            }

            if (ModelState.IsValid)
            {
                await _db.Clienteles.AddAsync(model.Entity);
                await _db.SaveChangesAsync();

                _cache.Clean();

                return RedirectToAction("Index", "Clientele");
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
            Clientele clientele = await _db.Clienteles.FindAsync(id);
            if (clientele != null)
            {
                ClienteleViewModel model = new ClienteleViewModel();
                model.PageViewModel = new PageViewModel { CurrentPage = page };
                model.Entity = clientele;

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
    public async Task<IActionResult> Edit(ClienteleViewModel model)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            if (ModelState.IsValid)
            {
                Clientele clientele = _db.Clienteles.Find(model.Entity.ClientId);
                if (clientele != null)
                {
                    clientele.Name = model.Entity.Name;
                    clientele.Surname = model.Entity.Surname;
                    clientele.Middlename = model.Entity.Middlename;
                    clientele.Phone = model.Entity.Phone;
                    clientele.Passport = model.Entity.Passport;
                    clientele.Addres = model.Entity.Addres;

                    _db.Clienteles.Update(clientele);
                    await _db.SaveChangesAsync();

                    _cache.Clean();

                    return RedirectToAction("Index", "Clientele", new { page = model.PageViewModel.CurrentPage });
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
            Clientele clientele = await _db.Clienteles.FindAsync(id);
            if (clientele == null)
                return NotFound();

            bool deleteFlag = false;
            string message = "Do you want to delete this entity";

            ClienteleViewModel model = new ClienteleViewModel();
            model.Entity = clientele;
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
    public async Task<IActionResult> Delete(ClienteleViewModel model)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        // Проверка наличия роли Admin у текущего пользователя
        if (_userManager.IsInRoleAsync(currentUser, "Admin").Result || _userManager.IsInRoleAsync(currentUser, "Manager").Result)
        {
            Clientele clientele = await _db.Clienteles.FindAsync(model.Entity.ClientId);
            if (clientele == null)
                return NotFound();

            _db.Clienteles.Remove(clientele);
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
            Clientele clientele = await _db.Clienteles.FindAsync(id);
            if (clientele != null)
            {
                ClienteleViewModel model = new ClienteleViewModel()
                {
                    Entity = clientele,

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

    private IQueryable<Clientele> GetSortedEntities(SortState sortState, string clientName)
    {
        IQueryable<Clientele> clienteles = _db.Clienteles.AsQueryable();

        switch (sortState)
        {
            case SortState.ClientNameAsc:
                clienteles = clienteles.OrderBy(g => g.Name);
                break;

            case SortState.ClientNameDesc:
                clienteles = clienteles.OrderByDescending(g => g.Name);
                break;
            case SortState.ClientSurnameAsc:
                clienteles = clienteles.OrderBy(g => g.Surname);
                break;

            case SortState.ClientSurnameDesc:
                clienteles = clienteles.OrderByDescending(g => g.Surname);
                break;
        }

        if (!string.IsNullOrEmpty(clientName))
            clienteles = clienteles.Where(g => g.Name.Contains(clientName)).AsQueryable();

        return clienteles;
    }
}