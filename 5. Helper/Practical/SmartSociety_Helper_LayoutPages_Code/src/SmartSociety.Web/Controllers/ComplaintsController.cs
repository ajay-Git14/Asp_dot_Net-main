using Microsoft.AspNetCore.Mvc;
using SmartSociety.Web.ViewModels;

namespace SmartSociety.Web.Controllers;

public class ComplaintsController : Controller
{
    [HttpGet]
    public IActionResult Index(string? status)
    {
        var complaints = DemoComplaintData.GetComplaints();

        if (!string.IsNullOrWhiteSpace(status))
        {
            complaints = complaints
                .Where(x => x.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        var model = new ComplaintIndexViewModel
        {
            PageTitle = "Complaint List",
            PageSubtitle = "This page uses the main layout and reusable complaint card partial.",
            SelectedStatus = status,
            TotalComplaints = complaints.Count,
            OpenComplaints = complaints.Count(x => x.Status == "Open"),
            ResolvedComplaints = complaints.Count(x => x.Status == "Resolved"),
            Complaints = complaints
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var complaint = DemoComplaintData.GetComplaints()
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
            Description = complaint.Description,
            Category = complaint.Category,
            Priority = complaint.Priority,
            Status = complaint.Status,
            CreatedBy = complaint.CreatedBy,
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
}
