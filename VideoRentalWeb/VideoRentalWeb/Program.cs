using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using VideoRentalWeb.DataModels;
using VideoRentalWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<VideoRentalContext>(options => options.UseSqlServer("Server=DESKTOP-I9HQNLA;Database=video_rental;Trusted_Connection=True;TrustServerCertificate=True"));

builder.Services.AddIdentity<User, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 5; // ����������� �����
    opts.Password.RequireNonAlphanumeric = false; // ��������� �� �� ���������-�������� �������
    opts.Password.RequireLowercase = false; // ��������� �� ������� � ������ ��������
    opts.Password.RequireUppercase = false; // ��������� �� ������� � ������� ��������
    opts.Password.RequireDigit = false; // ��������� �� �����
})
    .AddEntityFrameworkStores<VideoRentalContext>();

builder.Services.AddTransient<CacheProvider>();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddMvc();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath = "/Account/Login");
builder.Services.AddAuthorization();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "VideoRentalCookie";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddControllersWithViews(options =>
{
    options.CacheProfiles.Add("CacheProfile",
        new CacheProfile()
        {
            Duration = 262
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();

app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        );

        endpoints.MapControllerRoute(
            name: "Disks",
            pattern: "disks/{controller = Disks}/{action= Index}/{id?}"
        );
    }
);

app.Run();