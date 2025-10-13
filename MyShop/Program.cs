using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Myshop.DAL;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ItemDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ItemDbContextConnection' not found.");

/*
This line registers the services needed for controllers and views 
in the ASP.NET Core Dependency Injection (DI) container, enabling 
the MVC pattern to handle HTTP requests.
Services are components (e.g. database contexts, authentication, logging) 
that provide functionality and can be injected and used across the application.
*/
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ItemDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:ItemDbContextConnection"]);
});

builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ItemDbContext>();


// builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
// {
//     // Password settings
//     options.Password.RequireDigit = true;
//     options.Password.RequireLowercase = true;
//     options.Password.RequireUppercase = true;
//     options.Password.RequireNonAlphanumeric = true;
//     options.Password.RequiredLength = 6;

//     // Lockout after 5 failed login attempts
//     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
//     options.Lockout.MaxFailedAccessAttempts = 5;
//     options.Lockout.AllowedForNewUsers = true;

//     // User settings
//     options.User.RequireUniqueEmail = true;

//     // Disable email confirmation for testing
//     options.SignIn.RequireConfirmedEmail = false;
//     options.SignIn.RequireConfirmedAccount = false;
// })
// .AddEntityFrameworkStores<ItemDbContext>()
// .AddDefaultTokenProviders();

// builder.Services.ConfigureApplicationCookie(options =>
// {
//     options.LoginPath = "/Identity/Account/Login";
// });

builder.Services.AddScoped<IItemRepository, ItemRepository>();

builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".MyShop.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 30 minutes
    options.Cookie.IsEssential = true;
});

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File($"Logs/app_{DateTime.Now:yyyyMMdd_HHmmss}.log");

loggerConfiguration.Filter.ByExcluding(e => e.Properties.TryGetValue("SourceContext", out var value) &&
                            e.Level == LogEventLevel.Information &&
                            e.MessageTemplate.Text.Contains("Executed DbCommand"));

var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

// This block checks if the app is running in the development environment. 
// If so, it enables the Developer Exception Page, which shows detailed 
// error information to help with debugging.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    DBInit.Seed(app); // Seed the database with initial data in development mode
}

app.UseStaticFiles(); // Enables serving static files like CSS, JS, and images from wwwroot
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Maps the default controller route using the pattern 
// "{controller=Home}/{action=Index}/{id?}". 
// For example, /Item/Table maps to the Table action in ItemController.
app.MapDefaultControllerRoute();

app.MapRazorPages();

app.Run();
