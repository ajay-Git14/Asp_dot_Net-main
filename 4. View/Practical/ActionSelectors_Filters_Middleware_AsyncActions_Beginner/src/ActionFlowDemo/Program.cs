using ActionFlowDemo.Filters;
using ActionFlowDemo.Middleware;
using ActionFlowDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Adds MVC controllers and Razor views to the dependency-injection container.
builder.Services.AddControllersWithViews();

// The in-memory service lives for the entire application so newly added products remain available.
builder.Services.AddSingleton<IProductService, InMemoryProductService>();

// ServiceFilter resolves this filter from dependency injection.
builder.Services.AddScoped<ExecutionTimeFilter>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Static files are handled before the custom middleware.
// Therefore, CSS requests normally do not pass through RequestTimingMiddleware.
app.UseStaticFiles();

// Routing selects the endpoint before our middleware executes.
app.UseRouting();

// Custom middleware measures the total time spent in the remaining pipeline.
app.UseMiddleware<RequestTimingMiddleware>();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
