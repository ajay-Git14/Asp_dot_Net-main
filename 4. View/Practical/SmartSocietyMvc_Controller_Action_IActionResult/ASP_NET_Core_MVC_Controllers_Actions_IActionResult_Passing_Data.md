# ASP.NET Core MVC - Controllers, Actions, IActionResult, And Passing Data

## Project Name

Smart Society Issue Management System

## Topic Goal

This practical teaches how ASP.NET Core MVC works using:

- Controllers
- Action Methods
- IActionResult
- ViewData
- ViewBag
- Strongly Typed ViewModel
- Passing List Data To View
- Passing Single Object Data To View
- Returning View, JSON, Text, NotFound, And Redirect Responses

---

## 1. Create Project

```bash
dotnet new mvc -n SmartSocietyMvc
cd SmartSocietyMvc
```

---

## 2. Folder Structure

```text
SmartSocietyMvc
│
├── Controllers
│   └── ComplaintController.cs
│
├── Models
│   └── ComplaintViewModel.cs
│
├── Views
│   └── Complaint
│       ├── Index.cshtml
│       ├── Details.cshtml
│       └── Summary.cshtml
│
├── Program.cs
└── SmartSocietyMvc.csproj
```

---

## 3. MVC Request Flow

```text
Browser Request
     ↓
Routing
     ↓
Controller
     ↓
Action Method
     ↓
IActionResult
     ↓
View / JSON / Text / Redirect / Error
     ↓
Browser Response
```

---

## 4. Controller To View Mapping

```text
Controller: ComplaintController
Action: Index()
View Folder: Views/Complaint
View File: Index.cshtml
Final View Path: Views/Complaint/Index.cshtml
```

---

## 5. Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add MVC services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Complaint}/{action=Index}/{id?}");

app.Run();
```

---

## 6. Models/ComplaintViewModel.cs

```csharp
namespace SmartSocietyMvc.Models
{
    public class ComplaintViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string RaisedBy { get; set; } = string.Empty;

        public string FlatNumber { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}
```

---

## 7. Controllers/ComplaintController.cs

```csharp
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

        public IActionResult Index()
        {
            ViewData["PageTitle"] = "Complaint List";
            ViewBag.Message = "All complaints raised by society residents are shown below.";

            return View(Complaints);
        }

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

        public IActionResult Summary()
        {
            ViewData["PageTitle"] = "Complaint Summary";

            ViewBag.TotalComplaints = Complaints.Count;
            ViewBag.OpenComplaints = Complaints.Count(c => c.Status == "Open");
            ViewBag.InProgressComplaints = Complaints.Count(c => c.Status == "In Progress");
            ViewBag.ResolvedComplaints = Complaints.Count(c => c.Status == "Resolved");

            return View();
        }

        public IActionResult PlainText()
        {
            return Content("This is a plain text response from IActionResult.");
        }

        public IActionResult JsonData()
        {
            return Json(Complaints);
        }

        public IActionResult GoToSummary()
        {
            return RedirectToAction("Summary");
        }
    }
}
```

---

## 8. Views/Complaint/Index.cshtml

```cshtml
@model List<SmartSocietyMvc.Models.ComplaintViewModel>

@{
    ViewData["Title"] = ViewData["PageTitle"];
}

<h1>@ViewData["PageTitle"]</h1>

<p>@ViewBag.Message</p>

<p>
    <a href="/Complaint/Summary">View Complaint Summary</a>
</p>

<table border="1" cellpadding="10" cellspacing="0">
    <thead>
        <tr>
            <th>Complaint Id</th>
            <th>Title</th>
            <th>Raised By</th>
            <th>Flat Number</th>
            <th>Priority</th>
            <th>Status</th>
            <th>Created Date</th>
            <th>Action</th>
        </tr>
    </thead>

    <tbody>
        @foreach (var complaint in Model)
        {
            <tr>
                <td>@complaint.Id</td>
                <td>@complaint.Title</td>
                <td>@complaint.RaisedBy</td>
                <td>@complaint.FlatNumber</td>
                <td>@complaint.Priority</td>
                <td>@complaint.Status</td>
                <td>@complaint.CreatedDate.ToString("dd-MMM-yyyy")</td>
                <td>
                    <a href="/Complaint/Details/@complaint.Id">View Details</a>
                </td>
            </tr>
        }
    </tbody>
</table>
```

---

## 9. Views/Complaint/Details.cshtml

```cshtml
@model SmartSocietyMvc.Models.ComplaintViewModel

@{
    ViewData["Title"] = ViewData["PageTitle"];
}

<h1>@ViewData["PageTitle"]</h1>

<p>@ViewBag.Message</p>

<table border="1" cellpadding="10" cellspacing="0">
    <tr>
        <th>Complaint Id</th>
        <td>@Model.Id</td>
    </tr>

    <tr>
        <th>Title</th>
        <td>@Model.Title</td>
    </tr>

    <tr>
        <th>Description</th>
        <td>@Model.Description</td>
    </tr>

    <tr>
        <th>Raised By</th>
        <td>@Model.RaisedBy</td>
    </tr>

    <tr>
        <th>Flat Number</th>
        <td>@Model.FlatNumber</td>
    </tr>

    <tr>
        <th>Priority</th>
        <td>@Model.Priority</td>
    </tr>

    <tr>
        <th>Status</th>
        <td>@Model.Status</td>
    </tr>

    <tr>
        <th>Created Date</th>
        <td>@Model.CreatedDate.ToString("dd-MMM-yyyy hh:mm tt")</td>
    </tr>
</table>

<br />

<a href="/Complaint/Index">Back To Complaint List</a>
```

---

## 10. Views/Complaint/Summary.cshtml

```cshtml
@{
    ViewData["Title"] = ViewData["PageTitle"];
}

<h1>@ViewData["PageTitle"]</h1>

<p>This page shows data passed using ViewBag.</p>

<table border="1" cellpadding="10" cellspacing="0">
    <tr>
        <th>Total Complaints</th>
        <td>@ViewBag.TotalComplaints</td>
    </tr>

    <tr>
        <th>Open Complaints</th>
        <td>@ViewBag.OpenComplaints</td>
    </tr>

    <tr>
        <th>In Progress Complaints</th>
        <td>@ViewBag.InProgressComplaints</td>
    </tr>

    <tr>
        <th>Resolved Complaints</th>
        <td>@ViewBag.ResolvedComplaints</td>
    </tr>
</table>

<br />

<a href="/Complaint/Index">Back To Complaint List</a>
```

---

## 11. Run The Project

```bash
dotnet run
```

Open the localhost URL shown in the terminal.

---

## 12. Test URLs

```text
/Complaint/Index
/Complaint/Details/1
/Complaint/Summary
/Complaint/PlainText
/Complaint/JsonData
/Complaint/GoToSummary
```

---

## 13. Concepts Covered

| Concept | File / Method |
|---|---|
| Controller | ComplaintController |
| Action Method | Index, Details, Summary |
| IActionResult | All action methods |
| ViewData | PageTitle |
| ViewBag | Message and summary counts |
| Strongly Typed Model | ComplaintViewModel |
| Passing List To View | Index action |
| Passing Single Object To View | Details action |
| return View() | Index, Details, Summary |
| return NotFound() | Details |
| return Content() | PlainText |
| return Json() | JsonData |
| RedirectToAction() | GoToSummary |

---

## 14. Common Mistakes

1. View folder name does not match controller name.
2. View file name does not match action name.
3. Model namespace is missing in `.cshtml`.
4. Using ViewBag everywhere instead of ViewModel.
5. Returning `View()` but not creating the `.cshtml` file.
6. Forgetting to add MVC route in `Program.cs`.

---

## 15. Assignment

Add one more action:

```text
/Complaint/Open
```

It should show only complaints where status is `Open`.

Expected output:

```text
Only open complaints should be visible on the page.
```

---

## 16. Recap

In this practical, we created a working ASP.NET Core MVC example and learned how the browser request reaches a controller, how an action method returns an IActionResult, and how data is passed from controller to view using ViewData, ViewBag, and strongly typed ViewModel.
