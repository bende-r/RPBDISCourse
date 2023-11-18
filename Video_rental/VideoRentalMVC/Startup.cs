using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Services;

using VideoRentalModels;

using VideoRentalMVC.Data;
using VideoRentalMVC.Middleware;

namespace VideoRentalMVC;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        string connectionString = Configuration.GetConnectionString("SQLConnection");
        services.AddDbContext<VideoRentalContext>(options => options.UseSqlServer(connectionString));

        string sqlConnectionIdentityString = Configuration.GetConnectionString("SQLConnection");
        services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(sqlConnectionIdentityString));
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationContext>();

        services.AddTransient<CacheProvider>();
        services.AddMemoryCache();
        services.AddDistributedMemoryCache();
        services.AddSession();

        services.AddControllersWithViews(options =>
        {
            options.CacheProfiles.Add("CacheProfile",
                new CacheProfile()
                {
                    Duration = 262
                });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseSession();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseRoleInitializer();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}