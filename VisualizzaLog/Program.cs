using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VisualizzaLog.Areas.Identity.Data;
using VisualizzaLog.Data;
using VisualizzaLog.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<VisualizzaLogContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VisualizzaLogContext") ?? throw new InvalidOperationException("Connection string 'VisualizzaLogContext' not found.")));

builder.Services.AddDefaultIdentity<VisualizzaLogUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<IdentityVisualizzaLogContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<IdentityVisualizzaLogContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityVisualizzaLogContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityContext' not found.")));


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 12;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
    //CheckViolations.Initialize(services);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
