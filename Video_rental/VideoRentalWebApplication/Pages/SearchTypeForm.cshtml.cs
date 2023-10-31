using Microsoft.AspNetCore.Mvc.RazorPages;

using VideoRentalModels;

using Type = VideoRentalModels.Type;

namespace VideoRentalWebApplication.Pages
{
    public class SearchTypeFormModel : PageModel
    {
        public string Message { get; set; }
        public List<Type> Types = new List<Type>();
        private readonly VideoRentalContext _context;

        public int Position { get; set; }

        public SearchTypeFormModel(VideoRentalContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Message = "Введите индефикатор Типа";
            if (HttpContext.Session.Get<int>("TypeId") != 0)
                Position = HttpContext.Session.Get<int>("TypeId");
            else Position = 1;
        }

        public void OnPost(int TypeId)
        {
            HttpContext.Session.Set<int>("TypeId", TypeId);
            var employe = from type in _context.Types
                          where type.TypeId == TypeId
                          select type;
            Types = employe.ToList();
            Position = HttpContext.Session.Get<int>("TypeId");
        }
    }
}
