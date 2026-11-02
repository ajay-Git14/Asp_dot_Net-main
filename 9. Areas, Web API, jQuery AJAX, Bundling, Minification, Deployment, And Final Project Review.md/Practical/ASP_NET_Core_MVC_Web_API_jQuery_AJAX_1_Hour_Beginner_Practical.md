# ASP.NET Core MVC: 1-Hour Beginner Friendly Practical
## Topic: Web API and jQuery AJAX

> **Live Coding Duration:** 1 Hour  
> **Style:** Beginner friendly, practical-first, code in front of students.  
> **Project Assumption:** MVC project already exists. EF Core `Complaint` model and `ApplicationDbContext` may already exist from previous practicals.

---

# 1. Practical Goal

In this practical, students will learn how to create a simple **Web API** in ASP.NET Core MVC and call it using **jQuery AJAX**.

By the end, students should understand:

```text
MVC Controller returns View
API Controller returns JSON
AJAX calls API without full page reload
DTO controls API input/output data
Network tab helps debug API calls
Postman can test APIs separately
```

---

# 2. What We Will Build In 1 Hour

We will build a small AJAX/API demo for complaints.

We will create:

```text
1. DTO classes
2. ComplaintsApiController
3. DashboardApiController
4. Simple Razor page for AJAX testing
5. AJAX call to load complaints
6. AJAX call to refresh dashboard summary
7. AJAX call to update complaint status
```

---

# 3. Is Data Folder Required?

For this topic, answer depends on implementation.

## Case 1: Simple API with static dummy data

```text
Data folder is not required.
```

## Case 2: Real CRUD API using EF Core

```text
Data folder is required.
```

Why?

Because EF Core needs:

```text
Data/ApplicationDbContext.cs
```

to read and write complaint data from SQL Server.

For this practical, we will assume EF Core is already available and use:

```text
ApplicationDbContext
Complaint model
```

---

# 4. 1-Hour Live Coding Flow

| Time | Task |
|---|---|
| 0-5 min | Explain MVC vs API and JSON |
| 5-12 min | Create DTO classes |
| 12-25 min | Create ComplaintsApiController |
| 25-35 min | Create DashboardApiController |
| 35-48 min | Create AJAX test page |
| 48-55 min | Test in Browser Network tab |
| 55-60 min | Postman testing and revision |

---

# 5. Beginner Explanation

## MVC Controller

MVC controller usually returns a full Razor page.

Example:

```csharp
return View();
```

## API Controller

API controller usually returns data as JSON.

Example:

```csharp
return Ok(data);
```

## AJAX

AJAX allows browser to call the server without refreshing the full page.

Example:

```text
Click button
↓
AJAX calls API
↓
API returns JSON
↓
Only one part of page updates
```

---

# 6. Required Existing Files

For EF Core version, these should already exist:

```text
Models/Complaint.cs
Data/ApplicationDbContext.cs
Program.cs with AddDbContext
Database migration already applied
```

Basic `Complaint` model example:

```csharp
using System.ComponentModel.DataAnnotations;

namespace LiveClassDemos.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public string Priority { get; set; } = string.Empty;

        public string Status { get; set; } = "New";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsArchived { get; set; }
    }
}
```

---

# 7. Create DTO Folder

Create folder:

```text
DTOs
```

DTO means:

```text
Data Transfer Object
```

Simple meaning:

```text
DTO decides what data API accepts and returns.
```

Do not expose full database entity directly in API.

---

# 8. Create ComplaintListDto

Create file:

```text
DTOs/ComplaintListDto.cs
```

Add:

```csharp
namespace LiveClassDemos.DTOs
{
    public class ComplaintListDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}
```

---

# 9. Create ComplaintDetailsDto

Create file:

```text
DTOs/ComplaintDetailsDto.cs
```

Add:

```csharp
namespace LiveClassDemos.DTOs
{
    public class ComplaintDetailsDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}
```

---

# 10. Create CreateComplaintDto

Create file:

```text
DTOs/CreateComplaintDto.cs
```

Add:

```csharp
using System.ComponentModel.DataAnnotations;

namespace LiveClassDemos.DTOs
{
    public class CreateComplaintDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public string Priority { get; set; } = string.Empty;
    }
}
```

---

# 11. Create UpdateStatusDto

Create file:

```text
DTOs/UpdateStatusDto.cs
```

Add:

```csharp
using System.ComponentModel.DataAnnotations;

namespace LiveClassDemos.DTOs
{
    public class UpdateStatusDto
    {
        [Required]
        public string NewStatus { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Remarks { get; set; }
    }
}
```

---

# 12. Create DashboardSummaryDto

Create file:

```text
DTOs/DashboardSummaryDto.cs
```

Add:

```csharp
namespace LiveClassDemos.DTOs
{
    public class DashboardSummaryDto
    {
        public int TotalComplaints { get; set; }

        public int NewComplaints { get; set; }

        public int InProgressComplaints { get; set; }

        public int ResolvedComplaints { get; set; }

        public int HighPriorityComplaints { get; set; }
    }
}
```

---

# 13. Create API Controllers Folder

Create folder:

```text
Controllers/Api
```

This keeps API controllers separate from normal MVC controllers.

---

# 14. Create ComplaintsApiController

Create file:

```text
Controllers/Api/ComplaintsApiController.cs
```

Add:

```csharp
using LiveClassDemos.Data;
using LiveClassDemos.DTOs;
using LiveClassDemos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LiveClassDemos.Controllers.Api
{
    [Route("api/complaints")]
    [ApiController]
    public class ComplaintsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ComplaintsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetComplaints(string? status)
        {
            var query = _context.Complaints
                .Where(c => !c.IsArchived);

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(c => c.Status == status);
            }

            var complaints = await query
                .OrderByDescending(c => c.CreatedDate)
                .Select(c => new ComplaintListDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Category = c.Category,
                    Priority = c.Priority,
                    Status = c.Status,
                    CreatedDate = c.CreatedDate
                })
                .ToListAsync();

            return Ok(complaints);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetComplaintById(int id)
        {
            var complaint = await _context.Complaints
                .Where(c => c.Id == id && !c.IsArchived)
                .Select(c => new ComplaintDetailsDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Category = c.Category,
                    Priority = c.Priority,
                    Status = c.Status,
                    CreatedDate = c.CreatedDate
                })
                .FirstOrDefaultAsync();

            if (complaint == null)
            {
                return NotFound(new
                {
                    message = "Complaint not found."
                });
            }

            return Ok(complaint);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComplaint(CreateComplaintDto dto)
        {
            var complaint = new Complaint
            {
                Title = dto.Title,
                Description = dto.Description,
                Category = dto.Category,
                Priority = dto.Priority,
                Status = "New",
                CreatedDate = DateTime.Now,
                IsArchived = false
            };

            await _context.Complaints.AddAsync(complaint);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created, new
            {
                message = "Complaint created successfully.",
                complaintId = complaint.Id
            });
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateStatusDto dto)
        {
            var complaint = await _context.Complaints
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsArchived);

            if (complaint == null)
            {
                return NotFound(new
                {
                    message = "Complaint not found."
                });
            }

            complaint.Status = dto.NewStatus;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Complaint status updated successfully.",
                complaintId = complaint.Id,
                newStatus = complaint.Status
            });
        }
    }
}
```

---

# 15. Explain API Controller Code

## `[ApiController]`

```csharp
[ApiController]
```

This makes controller API-friendly.

It helps with automatic validation error response.

---

## `ControllerBase`

```csharp
public class ComplaintsApiController : ControllerBase
```

API controller does not return Razor views.

It returns:

```text
Ok()
NotFound()
BadRequest()
StatusCode()
```

---

## Route

```csharp
[Route("api/complaints")]
```

This means base API URL is:

```text
/api/complaints
```

---

# 16. Create DashboardApiController

Create file:

```text
Controllers/Api/DashboardApiController.cs
```

Add:

```csharp
using LiveClassDemos.Data;
using LiveClassDemos.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LiveClassDemos.Controllers.Api
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = new DashboardSummaryDto
            {
                TotalComplaints = await _context.Complaints
                    .CountAsync(c => !c.IsArchived),

                NewComplaints = await _context.Complaints
                    .CountAsync(c => !c.IsArchived && c.Status == "New"),

                InProgressComplaints = await _context.Complaints
                    .CountAsync(c => !c.IsArchived && c.Status == "InProgress"),

                ResolvedComplaints = await _context.Complaints
                    .CountAsync(c => !c.IsArchived && c.Status == "Resolved"),

                HighPriorityComplaints = await _context.Complaints
                    .CountAsync(c => !c.IsArchived && c.Priority == "High")
            };

            return Ok(summary);
        }
    }
}
```

---

# 17. Create AJAX Demo MVC Controller

Create normal MVC controller:

```text
Controllers/AjaxDemoController.cs
```

Add:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace LiveClassDemos.Controllers
{
    public class AjaxDemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
```

---

# 18. Create AJAX Demo View

Create folder:

```text
Views/AjaxDemo
```

Create file:

```text
Views/AjaxDemo/Index.cshtml
```

Add:

```cshtml
@{
    ViewData["Title"] = "AJAX Demo";
}

<h1>Web API And jQuery AJAX Demo</h1>

<hr />

<h2>Dashboard Summary</h2>

<button id="refreshDashboard">Refresh Dashboard</button>

<div style="margin-top:15px;">
    <p>Total Complaints: <strong id="totalComplaints">0</strong></p>
    <p>New Complaints: <strong id="newComplaints">0</strong></p>
    <p>In Progress Complaints: <strong id="inProgressComplaints">0</strong></p>
    <p>Resolved Complaints: <strong id="resolvedComplaints">0</strong></p>
    <p>High Priority Complaints: <strong id="highPriorityComplaints">0</strong></p>
</div>

<hr />

<h2>Complaint Filter</h2>

<select id="statusFilter">
    <option value="">All</option>
    <option value="New">New</option>
    <option value="InProgress">InProgress</option>
    <option value="Resolved">Resolved</option>
</select>

<button id="loadComplaints">Load Complaints</button>

<table border="1" cellpadding="8" style="margin-top:15px; width:100%;">
    <thead>
        <tr>
            <th>Id</th>
            <th>Title</th>
            <th>Category</th>
            <th>Priority</th>
            <th>Status</th>
            <th>Action</th>
        </tr>
    </thead>
    <tbody id="complaintTableBody">
    </tbody>
</table>

<hr />

<h2>Update Complaint Status</h2>

<input type="number" id="complaintId" placeholder="Complaint Id" />

<select id="newStatus">
    <option value="New">New</option>
    <option value="InProgress">InProgress</option>
    <option value="Resolved">Resolved</option>
</select>

<textarea id="remarks" placeholder="Remarks"></textarea>

<button id="updateStatus">Update Status</button>

@section Scripts {
    <script>
        function loadDashboardSummary() {
            $.ajax({
                url: "/api/dashboard/summary",
                method: "GET",
                success: function (data) {
                    $("#totalComplaints").text(data.totalComplaints);
                    $("#newComplaints").text(data.newComplaints);
                    $("#inProgressComplaints").text(data.inProgressComplaints);
                    $("#resolvedComplaints").text(data.resolvedComplaints);
                    $("#highPriorityComplaints").text(data.highPriorityComplaints);
                },
                error: function (xhr) {
                    alert("Unable to load dashboard summary.");
                    console.log(xhr.responseText);
                }
            });
        }

        function loadComplaints() {
            var status = $("#statusFilter").val();
            var url = "/api/complaints";

            if (status) {
                url = url + "?status=" + encodeURIComponent(status);
            }

            $.ajax({
                url: url,
                method: "GET",
                success: function (complaints) {
                    var rows = "";

                    $.each(complaints, function (index, complaint) {
                        rows += "<tr>";
                        rows += "<td>" + complaint.id + "</td>";
                        rows += "<td>" + complaint.title + "</td>";
                        rows += "<td>" + complaint.category + "</td>";
                        rows += "<td>" + complaint.priority + "</td>";
                        rows += "<td>" + complaint.status + "</td>";
                        rows += "<td><button class='selectComplaint' data-id='" + complaint.id + "'>Select</button></td>";
                        rows += "</tr>";
                    });

                    $("#complaintTableBody").html(rows);
                },
                error: function (xhr) {
                    alert("Unable to load complaints.");
                    console.log(xhr.responseText);
                }
            });
        }

        function updateStatus() {
            var id = $("#complaintId").val();

            var request = {
                newStatus: $("#newStatus").val(),
                remarks: $("#remarks").val()
            };

            $.ajax({
                url: "/api/complaints/" + id + "/status",
                method: "PUT",
                contentType: "application/json",
                data: JSON.stringify(request),
                success: function (response) {
                    alert(response.message);
                    loadComplaints();
                    loadDashboardSummary();
                },
                error: function (xhr) {
                    alert("Unable to update status.");
                    console.log(xhr.responseText);
                }
            });
        }

        $(document).ready(function () {
            loadDashboardSummary();
            loadComplaints();

            $("#refreshDashboard").on("click", function () {
                loadDashboardSummary();
            });

            $("#loadComplaints").on("click", function () {
                loadComplaints();
            });

            $("#statusFilter").on("change", function () {
                loadComplaints();
            });

            $("#updateStatus").on("click", function () {
                updateStatus();
            });

            $(document).on("click", ".selectComplaint", function () {
                var id = $(this).data("id");
                $("#complaintId").val(id);
            });
        });
    </script>
}
```

---

# 19. Check jQuery Reference

Open:

```text
Views/Shared/_Layout.cshtml
```

Make sure jQuery is available before page scripts.

Usually default MVC template has:

```html
<script src="~/lib/jquery/dist/jquery.min.js"></script>
<script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
<script src="~/js/site.js" asp-append-version="true"></script>
@await RenderSectionAsync("Scripts", required: false)
```

Important:

```text
jQuery must load before AJAX script runs.
```

---

# 20. Add Navigation Link

In `_Layout.cshtml`, add:

```cshtml
<li class="nav-item">
    <a class="nav-link text-dark" asp-controller="AjaxDemo" asp-action="Index">
        AJAX Demo
    </a>
</li>
```

---

# 21. Run And Test

Run:

```bash
dotnet run
```

Open:

```text
/AjaxDemo
```

---

# 22. Test API Directly In Browser

Open:

```text
/api/complaints
```

Expected:

```text
JSON complaint list
```

Open:

```text
/api/dashboard/summary
```

Expected:

```text
JSON dashboard summary
```

---

# 23. Test AJAX Page

Open:

```text
/AjaxDemo
```

Expected:

```text
Dashboard counts load without full page reload
Complaint table loads without full page reload
Status filter updates table
Update status button updates selected complaint
```

---

# 24. Browser Network Tab Debugging

Open browser developer tools:

```text
Right click page
Inspect
Network tab
Click AJAX button
Click API request
```

Check:

```text
Request URL
HTTP method
Status code
Payload
Response JSON
```

Common status codes:

| Code | Meaning |
|---|---|
| 200 | Success |
| 201 | Created |
| 400 | Bad request / validation error |
| 401 | Not logged in |
| 403 | Logged in but not allowed |
| 404 | API or record not found |
| 500 | Server error |

---

# 25. Test In Postman

## GET Complaints

```text
GET https://localhost:xxxx/api/complaints
```

Expected:

```text
200 OK
JSON list
```

## GET Complaint By Id

```text
GET https://localhost:xxxx/api/complaints/1
```

Expected:

```text
200 OK or 404 Not Found
```

## POST Create Complaint

```text
POST https://localhost:xxxx/api/complaints
```

Header:

```text
Content-Type: application/json
```

Body:

```json
{
  "title": "Water leakage",
  "description": "Water is leaking near the parking area.",
  "category": "Water",
  "priority": "High"
}
```

Expected:

```text
201 Created
```

## PUT Update Status

```text
PUT https://localhost:xxxx/api/complaints/1/status
```

Header:

```text
Content-Type: application/json
```

Body:

```json
{
  "newStatus": "InProgress",
  "remarks": "Work started."
}
```

Expected:

```text
200 OK
```

---

# 26. Important Teaching Points

## Web API

```text
API returns data, usually JSON.
```

## jQuery AJAX

```text
AJAX calls API without full page reload.
```

## DTO

```text
DTO protects API input/output shape.
```

## Network Tab

```text
Network tab shows what request was sent and what response came back.
```

## Postman

```text
Postman tests API without UI.
```

---

# 27. Common Mistakes

| Mistake | Problem | Fix |
|---|---|---|
| Returning View from API controller | API should return data | Use `Ok()` |
| Missing `[ApiController]` | API behavior incomplete | Add `[ApiController]` |
| Wrong route URL | AJAX fails | Match API route exactly |
| jQuery not loaded | `$ is not defined` error | Add jQuery script |
| Missing `contentType` | JSON body may not bind | Use `application/json` |
| Missing `JSON.stringify` | Object not sent correctly | Use `JSON.stringify(request)` |
| No error callback | Hard to debug | Add error handler |
| Returning full entity | Exposes too much data | Use DTO |
| Using GET to update data | Wrong HTTP usage | Use PUT/POST |
| Not checking Network tab | Debugging guesswork | Inspect request/response |

---

# 28. One-Minute Revision

| Concept | Meaning |
|---|---|
| Web API | HTTP endpoint that returns data |
| MVC Controller | Returns Razor View |
| API Controller | Returns JSON and status code |
| `[ApiController]` | Enables API-specific behavior |
| `ControllerBase` | Base class for API-only controllers |
| JSON | Data format used by APIs |
| DTO | Data Transfer Object |
| AJAX | Calls server without full page reload |
| Network Tab | Debug API/AJAX requests |
| Postman | Test API manually |

---

# 29. Interview Lines

## Web API

A Web API exposes HTTP endpoints that return data, usually JSON.

## MVC Controller Vs API Controller

MVC controllers return Razor views, while API controllers return data and HTTP status codes.

## ApiController

`[ApiController]` enables API-specific behavior such as automatic validation responses.

## DTO

DTO controls what data is accepted and returned by API endpoints.

## AJAX

jQuery AJAX calls Web APIs in the background and updates part of the page without full reload.

## Network Tab

Browser Network tab helps debug API calls by showing URL, method, payload, status code, and response.

---

# 30. Mini Assignment

Ask students to complete:

```text
1. Add Delete/Archive API endpoint
2. Add AJAX Archive button in complaint table
3. Add filter by Priority
4. Add API endpoint GET /api/complaints?priority=High
5. Show error message on page instead of alert
6. Test all APIs in Postman
```

---

# 31. Final Outcome

After this practical, students should be able to say:

```text
I created Web API endpoints in ASP.NET Core MVC and called them using jQuery AJAX.

I used DTOs for API request and response data. I created APIs for complaint list, complaint details, complaint creation, status update, and dashboard summary.

I used AJAX to refresh dashboard counts, filter complaints, and update complaint status without full page reload.

I can debug API calls using Browser Network tab and test APIs separately using Postman.
```
