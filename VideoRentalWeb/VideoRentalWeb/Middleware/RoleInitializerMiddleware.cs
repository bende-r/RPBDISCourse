using Microsoft.AspNetCore.Identity;

using VideoRentalWeb.Data;
using VideoRentalWeb.DataModels;

namespace VideoRentalWeb.Middleware
{
    public class RoleInitializerMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleInitializerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            await RoleInitializer.InitializeAsync(userManager, roleManager);

            await _next(context);
        }
    }

    public static class DbInitializerExtensions
    {
        public static IApplicationBuilder UseRoleInitializer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RoleInitializerMiddleware>();
        }
    }
}