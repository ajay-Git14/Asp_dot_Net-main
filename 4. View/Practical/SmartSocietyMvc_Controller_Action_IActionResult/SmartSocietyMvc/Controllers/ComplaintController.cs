using Microsoft.AspNetCore.Mvc;
using SmartSocietyMvc.Models;

namespace SmartSocietyMvc.Controllers
{
    public class ComplaintController : Controller
    {
        private static readonly List<ComplaintViewModel> Complaints = new()
        {
            new ComplaintViewModel
            {
                Id = 1,
                Title = "Water Leakage",
                Description = "Water leakage near the parking area.",
                RaisedBy = "Amit Sharma",
                FlatNumber = "A-101",
                Priority = "High",
                Status = "Open",
                CreatedDate = DateTime.Now.AddDays(-2)
            },
            new ComplaintViewModel
            {
                Id = 2,
                Title = "Lift Not Working",
                Description = "Lift is not working in Tower B.",
                RaisedBy = "Neha Verma",
                FlatNumber = "B-204",
                Priority = "Critical",
                Status = "In Progress",
                CreatedDate = DateTime.Now.AddDays(-1)
            },
            new ComplaintViewModel
            {
                Id = 3,
                Title = "Security Light Issue",
                Description = "Security light near main gate is not working.",
                RaisedBy = "Rahul Mehta",
                FlatNumber = "C-303",
                Priority = "Medium",
                Status = "Resolved",
                CreatedDate = DateTime.Now
            }
        };

        // Example 1: Basic IActionResult returning View
        public IActionResult Index()
        {
            ViewData["PageTitle"] = "Complaint List";
            ViewBag.Message = "All complaints raised by society residents are shown below.";

            return View(Complaints);
        }

        // Example 2: Passing single strongly typed model to view
        public IActionResult Details(int id)
        {
            var complaint = Complaints.FirstOrDefault(c => c.Id == id);

            if (complaint == null)
            {
                return NotFound("Complaint not found.");
            }

            ViewData["PageTitle"] = "Complaint Details";
            ViewBag.Message = "Detailed information about selected complaint.";

            return View(complaint);
        }

        // Example 3: ViewData and ViewBag only
        public IActionResult Summary()
        {
            ViewData["PageTitle"] = "Complaint Summary";

            ViewBag.TotalComplaints = Complaints.Count;
            ViewBag.OpenComplaints = Complaints.Count(c => c.Status == "Open");
            ViewBag.InProgressComplaints = Complaints.Count(c => c.Status == "In Progress");
            ViewBag.ResolvedComplaints = Complaints.Count(c => c.Status == "Resolved");

            return View();
        }

        // Example 4: Returning plain text response
        public IActionResult PlainText()
        {
            return Content("This is a plain text response from IActionResult.");
        }

        // Example 5: Returning JSON response
        public IActionResult JsonData()
        {
            return Json(Complaints);
        }

        // Example 6: Redirecting to another action
        public IActionResult GoToSummary()
        {
            return RedirectToAction("Summary");
        }
    }
}
