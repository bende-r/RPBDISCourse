using VideoRentalModels;

using VideoRentalMVC.Data;

namespace VideoRentalMVC.Middleware;

public class DbInitializerMiddleware
{
    private readonly RequestDelegate _next;

    public DbInitializerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context, IServiceProvider serviceProvider, VideoRentalContext db)
    {
        if (!(context.Session.Keys.Contains("starting")))
        {
            DbDataInitializer.Initialize(db);
            context.Session.SetString("starting", "Yes");
        }

        return _next.Invoke(context);
    }
}

public static class DbInitializerExtensions
{
    public static IApplicationBuilder UseDbInitializer(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DbInitializerMiddleware>();
    }
}