var builder = WebApplication.CreateBuilder(args);

// Add MVC controllers and Razor views to dependency injection.
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/routing/help");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Endpoint routing selects an endpoint before authorization runs.
app.UseRouting();
app.UseAuthorization();

// ------------------------------------------------------------
// ROUTE ORDER MATTERS
// Put specific routes before the general default route.
// ------------------------------------------------------------

// Area route: demonstrates the extra {area} route value.
// The single controller contains one action marked [Area("Admin")].
app.MapAreaControllerRoute(
    name: "admin-area",
    areaName: "Admin",
    pattern: "Admin/Dashboard/{id?}",
    defaults: new
    {
        controller = "RoutingDemo",
        action = "AreaDashboard"
    });

// More specific conventional route for a complaint status.
app.MapControllerRoute(
    name: "complaint-status",
    pattern: "complaints/status/{status:alpha}",
    defaults: new
    {
        controller = "RoutingDemo",
        action = "ByStatus"
    });

// Friendly conventional route using an integer constraint.
app.MapControllerRoute(
    name: "complaint-details",
    pattern: "complaints/{id:int:min(1)}",
    defaults: new
    {
        controller = "RoutingDemo",
        action = "Details"
    });

// The default conventional MVC route.
// controller defaults to RoutingDemo, action defaults to Index,
// and id is optional because of the ? symbol.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=RoutingDemo}/{action=Index}/{id?}");

// Maps actions that use [Route], [HttpGet("template")],
// [HttpPost("template")], and similar attribute routes.
app.MapControllers();

app.Run();
