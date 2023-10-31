using Microsoft.AspNetCore.Mvc.RazorPages;

using VideoRentalModels;

namespace VideoRentalWebApplication.Pages
{
    public class SearchGenreFormModel : PageModel
    {
        public string Message { get; set; }
        public List<Genre> genres = new List<Genre>();
        private readonly VideoRentalContext _context;

        public string Position { get; set; }

        public SearchGenreFormModel(VideoRentalContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Message = "Введите название жанра";
            Position = HttpContext.Request.Cookies["senderButton"];
        }

        public void OnPost(string post)
        {
            if (!string.IsNullOrEmpty(post))
            {
                HttpContext.Response.Cookies.Append("senderButton", post, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddMinutes(30) // Устанавливаем время жизни куки
                });
            }
            var genre = from Genre in _context.Genres
                        where Genre.Title == post
                        select Genre;
            genres = genre.ToList();
        }
    }
}
