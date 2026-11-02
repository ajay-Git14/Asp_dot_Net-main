using Microsoft.AspNetCore.Mvc;
using SmartSociety.Web.ViewModels;

namespace SmartSociety.Web.Controllers;

public class DashboardController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var complaints = DemoComplaintData.GetComplaints();

        var model = new DashboardIndexViewModel
        {
            PageTitle = "Smart Society Dashboard",
            PageSubtitle = "Layout page, shared navigation, footer, sections, and reusable partial views demo.",
            TotalComplaints = complaints.Count,
            OpenComplaints = complaints.Count(x => x.Status == "Open"),
            AssignedComplaints = complaints.Count(x => x.Status == "Assigned"),
            ResolvedComplaints = complaints.Count(x => x.Status == "Resolved"),
            RecentComplaints = complaints
                .OrderByDescending(x => x.CreatedOn)
                .Take(3)
                .ToList()
        };

        return View(model);
    }
}
