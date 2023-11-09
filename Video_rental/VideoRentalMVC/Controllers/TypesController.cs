using Microsoft.AspNetCore.Mvc;

using VideoRentalModels;

namespace VideoRentalMVC.Controllers;

public class TypesController : Controller
{
    private VideoRentalContext db;

    public TypesController(VideoRentalContext context)
    {
        db = context;
    }

    [ResponseCache(CacheProfileName = "CacheProfile")]
    public IActionResult Index()
    {
        return View(db.Types.Take(20).ToList());
    }
}