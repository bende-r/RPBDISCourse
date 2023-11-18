using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Services;

using VideoRentalWeb.DataModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services
    .AddDbContext<VideoRentalContext>(options => options.UseSqlServer("Server=DESKTOP-I9HQNLA;Database=video_rental;Trusted_Connection=True;TrustServerCertificate=True"));


//builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer("Server=DESKTOP-I9HQNLA;Database=video_rental;Trusted_Connection=True;TrustServerCertificate=True"));
//builder.Services.AddIdentity<User, IdentityRole>()
//   .AddEntityFrameworkStores<ApplicationContext>();
builder.Services.AddIdentity<User, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 5; // минимальная длина
    opts.Password.RequireNonAlphanumeric = false; // требуются ли не алфавитно-цифровые символы
    opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
    opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
    opts.Password.RequireDigit = false; // требуются ли цифры
})
    .AddEntityFrameworkStores<VideoRentalContext>();

builder.Services.AddTransient<CacheProvider>();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/login");

builder.Services.AddControllersWithViews(options =>
{
    options.CacheProfiles.Add("CacheProfile",
        new CacheProfile()
        {
            Duration = 262
        });
}); var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"

    );

app.MapControllerRoute(
    name: "disks",
    pattern: "{controller=Disks}/"
);


app.Run();
