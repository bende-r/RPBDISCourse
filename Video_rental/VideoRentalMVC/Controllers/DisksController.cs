using Microsoft.AspNetCore.Mvc;

using VideoRentalModels;

namespace VideoRentalMVC.Controllers;

public class DisksController : Controller
{
    private VideoRentalContext db;

    public DisksController(VideoRentalContext context)
    {
        db = context;
    }

    [ResponseCache(CacheProfileName = "CacheProfile")]
    public IActionResult Index()
    {
        return View(db.Disks.Take(20).ToList());
    }
}