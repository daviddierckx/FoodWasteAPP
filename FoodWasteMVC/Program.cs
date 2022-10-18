using FoodWaste.Application.Data;
using FoodWaste.Application.Helpers;
using FoodWaste.Application.Interfaces;
using FoodWaste.Application.Services;
using FoodWaste.Domain;
using FoodWaste.Infrastructure.Data;
using FoodWaste.Infrastructure.Repository;
using FoodWasteMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Adding Repositories TODO
builder.Services.AddScoped<IPakketRepo, PakketRepo>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IStudentRepo, StudentRepo>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

builder.Services.AddScoped<IUserRepo, UserRepo>();

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

//DI for DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection"),
    OptionsBuilder => OptionsBuilder.MigrationsAssembly("FoodWasteMVC")
    ));
builder.Services.AddIdentity<AppUser, IdentityRole>()
       .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)//Adds Cookies
    .AddCookie();
var app = builder.Build();


if(args.Length == 1 && args[0].ToLower() == "seeddata")
{
    await Seed.SeedUsersAndRolesAsync(app);

}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
  
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
