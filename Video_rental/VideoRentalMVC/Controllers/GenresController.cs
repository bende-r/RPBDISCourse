using Microsoft.AspNetCore.Mvc;

using VideoRentalModels;

namespace VideoRentalMVC.Controllers;

public class GenresController : Controller
{
    private VideoRentalContext db;

    public GenresController(VideoRentalContext context)
    {
        db = context;
    }

    [ResponseCache(CacheProfileName = "CacheProfile")]
    public IActionResult Index()
    {
        return View(db.Genres.Take(20).ToList());
    }
}