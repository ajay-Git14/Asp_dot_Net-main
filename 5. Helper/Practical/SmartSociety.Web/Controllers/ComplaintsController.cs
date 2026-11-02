using Microsoft.AspNetCore.Mvc;
using SmartSociety.Web.ViewModels;

namespace SmartSociety.Web.Controllers;

public class ComplaintsController : Controller
{
    [HttpGet]
    public IActionResult Index(string? status)
    {
        var complaints = GetDemoComplaints();

        if (!string.IsNullOrWhiteSpace(status))
        {
            complaints = complaints
                .Where(x => x.Status.Equals(
                    status,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        var model = new ComplaintIndexViewModel
        {
            PageTitle = "Smart Society Complaints",
            SelectedStatus = status,
            Complaints = complaints,
            TotalComplaints = complaints.Count,
            OpenComplaints = complaints.Count(x => x.Status == "Open"),
            ResolvedComplaints = complaints.Count(x => x.Status == "Resolved")
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var complaint = GetDemoComplaints()
            .FirstOrDefault(x => x.Id == id);

        if (complaint is null)
        {
            return NotFound();
        }

        var model = new ComplaintDetailsViewModel
        {
            Id = complaint.Id,
            ReferenceNumber = complaint.ReferenceNumber,
            Title = complaint.Title,
            Description =
                "Resident reported this issue from the Smart Society portal.",
            Category = complaint.Category,
            Priority = complaint.Priority,
            Status = complaint.Status,
            CreatedBy = "Resident User",
            CreatedOn = complaint.CreatedOn,
            CanEdit = complaint.Status != "Resolved"
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    private static List<ComplaintSummaryViewModel> GetDemoComplaints()
    {
        return
        [
            new ComplaintSummaryViewModel
            {
                Id = 1,
                ReferenceNumber = "CMP-2026-0001",
                Title = "Lift not working",
                Category = "Maintenance",
                Priority = "High",
                Status = "Open",
                CreatedOn = DateTime.Today.AddDays(-2)
            },
            new ComplaintSummaryViewModel
            {
                Id = 2,
                ReferenceNumber = "CMP-2026-0002",
                Title = "Water leakage near parking",
                Category = "Plumbing",
                Priority = "Medium",
                Status = "Assigned",
                CreatedOn = DateTime.Today.AddDays(-1)
            },
            new ComplaintSummaryViewModel
            {
                Id = 3,
                ReferenceNumber = "CMP-2026-0003",
                Title = "Street light repaired",
                Category = "Electrical",
                Priority = "Low",
                Status = "Resolved",
                CreatedOn = DateTime.Today.AddDays(-5)
            }
        ];
    }
}
