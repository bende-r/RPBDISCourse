using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using VideoRentalModels;

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
        string connectionString = Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<VideoRentalContext>(options => options.UseSqlServer(connectionString));

        services.AddDistributedMemoryCache();
        services.AddSession();

        services.AddControllersWithViews(options =>
        {
            options.CacheProfiles.Add("CacheProfile",
                new CacheProfile()
                {
                    Duration = 284
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
        app.UseDbInitializer();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}