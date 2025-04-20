using ConfigurationLibrary.Data;
using ConfigurationLibrary.UI.Data.Context;
using ConfigurationLibrary.UI.Entities.Identity;
using ConfigurationLibrary.UI.Middlewares;
using ConfigurationLibrary.UI.Services.Auth;
using ConfigurationLibrary.UI.Services.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<ConfigurationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConfigurationConnection")));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConfigurationAppConnection")));

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseMiddleware<UserInfoMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();