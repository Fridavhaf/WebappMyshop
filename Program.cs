var builder = WebApplication.CreateBuilder(args);

/*
This line registers the services needed for controllers and views 
in the ASP.NET Core Dependency Injection (DI) container, enabling 
the MVC pattern to handle HTTP requests.
Services are components (e.g. database contexts, authentication, logging) 
that provide functionality and can be injected and used across the application.
*/
builder.Services.AddControllersWithViews();

var app = builder.Build();

// This block checks if the app is running in the development environment. 
// If so, it enables the Developer Exception Page, which shows detailed 
// error information to help with debugging.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles(); // Enables serving static files like CSS, JS, and images from wwwroot

// Maps the default controller route using the pattern 
// "{controller=Home}/{action=Index}/{id?}". 
// For example, /Item/Table maps to the Table action in ItemController.
app.MapDefaultControllerRoute();

app.Run();
