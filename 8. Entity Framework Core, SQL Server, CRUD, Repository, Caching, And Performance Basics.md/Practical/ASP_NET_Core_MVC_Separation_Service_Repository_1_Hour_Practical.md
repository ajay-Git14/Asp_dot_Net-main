# ASP.NET Core MVC: 1-Hour Beginner Friendly Practical
## Topic: Separation of Work, Service / BO Classes, Repository, and Relationships

> **Live Coding Duration:** 1 Hour  
> **Style:** Beginner friendly, practical-first, code in front of students.  
> **Assumption:** EF Core setup and basic Complaint CRUD already exist.

---

# 1. Practical Goal

In the previous practical, students wrote CRUD code using EF Core.

Now we will improve the structure.

Instead of putting all code inside controller, we will separate work into proper layers:

```text
Controller
↓
Service / BO Layer
↓
Repository Layer
↓
ApplicationDbContext
↓
Database
```

By the end of this practical, students should understand:

```text
Why controller should stay clean
What service / BO class does
What repository does
How DbContext works as Unit of Work
How one Complaint can have many Comments
How foreign key works
How navigation property works
How to register service and repository in Program.cs
```

---

# 2. What We Will Build In 1 Hour

We will refactor complaint code into a clean structure.

We will create:

```text
1. IComplaintRepository
2. ComplaintRepository
3. IComplaintService
4. ComplaintService
5. Simple one-to-many relationship: Complaint → ComplaintComments
6. Controller using service instead of DbContext directly
7. DI registration in Program.cs
```

We will keep this practical simple and realistic for one live coding hour.

---

# 3. Is Data Folder Required?

Yes.

For this topic, the **Data** folder is required because repository uses `ApplicationDbContext`.

Expected file:

```text
Data/ApplicationDbContext.cs
```

Repository will use this DbContext to talk to database.

Example:

```csharp
private readonly ApplicationDbContext _context;
```

---

# 4. 1-Hour Live Coding Flow

| Time | Task |
|---|---|
| 0-5 min | Explain separation of work |
| 5-10 min | Show bad controller-heavy code |
| 10-20 min | Create repository interface and class |
| 20-35 min | Create service interface and class |
| 35-45 min | Refactor controller to use service |
| 45-52 min | Add ComplaintComment relationship |
| 52-57 min | Register DI and test |
| 57-60 min | Revision and interview lines |

---

# 5. Simple Explanation For Students

## Controller

Controller handles browser request and response.

```text
Receive form
Check ModelState
Call service
Return view or redirect
```

Controller should not contain heavy business logic.

---

## Service / BO Class

Service contains business logic.

Example:

```text
New complaint should start with New status
Archived complaint should not appear in normal list
Comment should belong to an existing complaint
```

In some companies, this is called:

```text
BO class
Business Operation class
Business Service class
Service class
```

For this practical, we will call it `ComplaintService`.

---

## Repository

Repository contains database queries.

Example:

```text
Get all active complaints
Get complaint by id
Add complaint
Add comment
Save changes
```

Repository talks to `ApplicationDbContext`.

---

# 6. Required Existing Files

Before starting, these should already exist:

```text
Models/Complaint.cs
Data/ApplicationDbContext.cs
Controllers/ComplaintsController.cs
Views/Complaints/Index.cshtml
Views/Complaints/Create.cshtml
Views/Complaints/Details.cshtml
Program.cs
```

---

# 7. Recommended Folder Structure

Create these folders if they do not exist:

```text
Interfaces
Repositories
Services
```

Final structure:

```text
LiveClassDemos
│
├── Controllers
│   └── ComplaintsController.cs
│
├── Data
│   └── ApplicationDbContext.cs
│
├── Interfaces
│   └── IComplaintService.cs
│
├── Models
│   ├── Complaint.cs
│   └── ComplaintComment.cs
│
├── Repositories
│   ├── IComplaintRepository.cs
│   └── ComplaintRepository.cs
│
├── Services
│   └── ComplaintService.cs
│
└── Program.cs
```

---

# 8. Step 1: Update Complaint Model

Open:

```text
Models/Complaint.cs
```

Use this model:

```csharp
using System.ComponentModel.DataAnnotations;

namespace LiveClassDemos.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public string Priority { get; set; } = string.Empty;

        public string Status { get; set; } = "New";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsArchived { get; set; }

        public List<ComplaintComment> Comments { get; set; } = new List<ComplaintComment>();
    }
}
```

---

## Teaching Point

This line creates a one-to-many relationship:

```csharp
public List<ComplaintComment> Comments { get; set; } = new List<ComplaintComment>();
```

Meaning:

```text
One Complaint can have many Comments.
```

---

# 9. Step 2: Create ComplaintComment Model

Create file:

```text
Models/ComplaintComment.cs
```

Add:

```csharp
using System.ComponentModel.DataAnnotations;

namespace LiveClassDemos.Models
{
    public class ComplaintComment
    {
        public int Id { get; set; }

        [Required]
        public int ComplaintId { get; set; }

        [Required]
        [StringLength(500)]
        public string CommentText { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Complaint? Complaint { get; set; }
    }
}
```

---

## Teaching Point

This is foreign key:

```csharp
public int ComplaintId { get; set; }
```

It connects comment to complaint.

This is navigation property:

```csharp
public Complaint? Complaint { get; set; }
```

It allows EF Core to navigate from comment to complaint.

---

# 10. Step 3: Update ApplicationDbContext

Open:

```text
Data/ApplicationDbContext.cs
```

Add both DbSets:

```csharp
using LiveClassDemos.Models;
using Microsoft.EntityFrameworkCore;

namespace LiveClassDemos.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Complaint> Complaints { get; set; }

        public DbSet<ComplaintComment> ComplaintComments { get; set; }
    }
}
```

If your project uses Identity, keep `IdentityDbContext<ApplicationUser>` and only add the DbSets:

```csharp
public DbSet<Complaint> Complaints { get; set; }

public DbSet<ComplaintComment> ComplaintComments { get; set; }
```

---

# 11. Step 4: Add Migration For Relationship

Run:

```bash
dotnet ef migrations add AddComplaintComments

dotnet ef database update
```

If these tables already exist, skip migration.

---

# 12. Step 5: Create IComplaintRepository

Create file:

```text
Repositories/IComplaintRepository.cs
```

Add:

```csharp
using LiveClassDemos.Models;

namespace LiveClassDemos.Repositories
{
    public interface IComplaintRepository
    {
        Task<List<Complaint>> GetAllActiveAsync();

        Task<Complaint?> GetByIdAsync(int id);

        Task<Complaint?> GetDetailsAsync(int id);

        Task AddAsync(Complaint complaint);

        Task AddCommentAsync(ComplaintComment comment);

        Task SaveChangesAsync();
    }
}
```

---

## Teaching Point

Interface is a contract.

It says:

```text
Any complaint repository must provide these methods.
```

---

# 13. Step 6: Create ComplaintRepository

Create file:

```text
Repositories/ComplaintRepository.cs
```

Add:

```csharp
using LiveClassDemos.Data;
using LiveClassDemos.Models;
using Microsoft.EntityFrameworkCore;

namespace LiveClassDemos.Repositories
{
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly ApplicationDbContext _context;

        public ComplaintRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Complaint>> GetAllActiveAsync()
        {
            return await _context.Complaints
                .Where(c => !c.IsArchived)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<Complaint?> GetByIdAsync(int id)
        {
            return await _context.Complaints
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Complaint?> GetDetailsAsync(int id)
        {
            return await _context.Complaints
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsArchived);
        }

        public async Task AddAsync(Complaint complaint)
        {
            await _context.Complaints.AddAsync(complaint);
        }

        public async Task AddCommentAsync(ComplaintComment comment)
        {
            await _context.ComplaintComments.AddAsync(comment);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
```

---

## Teaching Point

Repository contains EF Core queries:

```csharp
_context.Complaints.Where(...)
_context.Complaints.AddAsync(...)
_context.SaveChangesAsync()
```

Service will not directly write EF Core queries now.

---

# 14. Step 7: Create IComplaintService

Create file:

```text
Interfaces/IComplaintService.cs
```

Add:

```csharp
using LiveClassDemos.Models;

namespace LiveClassDemos.Interfaces
{
    public interface IComplaintService
    {
        Task<List<Complaint>> GetAllComplaintsAsync();

        Task<Complaint?> GetComplaintDetailsAsync(int id);

        Task CreateComplaintAsync(Complaint complaint);

        Task AddCommentAsync(int complaintId, string commentText);

        Task ArchiveComplaintAsync(int id);
    }
}
```

---

# 15. Step 8: Create ComplaintService

Create file:

```text
Services/ComplaintService.cs
```

Add:

```csharp
using LiveClassDemos.Interfaces;
using LiveClassDemos.Models;
using LiveClassDemos.Repositories;

namespace LiveClassDemos.Services
{
    public class ComplaintService : IComplaintService
    {
        private readonly IComplaintRepository _complaintRepository;

        public ComplaintService(IComplaintRepository complaintRepository)
        {
            _complaintRepository = complaintRepository;
        }

        public async Task<List<Complaint>> GetAllComplaintsAsync()
        {
            return await _complaintRepository.GetAllActiveAsync();
        }

        public async Task<Complaint?> GetComplaintDetailsAsync(int id)
        {
            return await _complaintRepository.GetDetailsAsync(id);
        }

        public async Task CreateComplaintAsync(Complaint complaint)
        {
            complaint.Status = "New";
            complaint.CreatedDate = DateTime.Now;
            complaint.IsArchived = false;

            await _complaintRepository.AddAsync(complaint);
            await _complaintRepository.SaveChangesAsync();
        }

        public async Task AddCommentAsync(int complaintId, string commentText)
        {
            var complaint = await _complaintRepository.GetByIdAsync(complaintId);

            if (complaint == null || complaint.IsArchived)
            {
                throw new InvalidOperationException("Complaint not found.");
            }

            var comment = new ComplaintComment
            {
                ComplaintId = complaintId,
                CommentText = commentText,
                CreatedDate = DateTime.Now
            };

            await _complaintRepository.AddCommentAsync(comment);
            await _complaintRepository.SaveChangesAsync();
        }

        public async Task ArchiveComplaintAsync(int id)
        {
            var complaint = await _complaintRepository.GetByIdAsync(id);

            if (complaint == null || complaint.IsArchived)
            {
                throw new InvalidOperationException("Complaint not found.");
            }

            complaint.IsArchived = true;

            await _complaintRepository.SaveChangesAsync();
        }
    }
}
```

---

## Teaching Point

Service contains business rules:

```csharp
complaint.Status = "New";
complaint.IsArchived = false;
```

Repository only saves and reads data.

---

# 16. Step 9: Register DI In Program.cs

Open:

```text
Program.cs
```

Add namespaces:

```csharp
using LiveClassDemos.Interfaces;
using LiveClassDemos.Repositories;
using LiveClassDemos.Services;
```

Add before `var app = builder.Build();`:

```csharp
builder.Services.AddScoped<IComplaintRepository, ComplaintRepository>();

builder.Services.AddScoped<IComplaintService, ComplaintService>();
```

---

## Teaching Point

`AddScoped` means:

```text
Create one object per HTTP request.
```

This is correct because `ApplicationDbContext` is also scoped.

---

# 17. Step 10: Refactor ComplaintsController

Open:

```text
Controllers/ComplaintsController.cs
```

Replace direct `ApplicationDbContext` usage with service.

```csharp
using LiveClassDemos.Interfaces;
using LiveClassDemos.Models;
using Microsoft.AspNetCore.Mvc;

namespace LiveClassDemos.Controllers
{
    public class ComplaintsController : Controller
    {
        private readonly IComplaintService _complaintService;

        public ComplaintsController(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        public async Task<IActionResult> Index()
        {
            var complaints = await _complaintService.GetAllComplaintsAsync();

            return View(complaints);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Complaint complaint)
        {
            if (!ModelState.IsValid)
            {
                return View(complaint);
            }

            await _complaintService.CreateComplaintAsync(complaint);

            TempData["SuccessMessage"] = "Complaint created successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var complaint = await _complaintService.GetComplaintDetailsAsync(id);

            if (complaint == null)
            {
                return NotFound();
            }

            return View(complaint);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int complaintId, string commentText)
        {
            if (string.IsNullOrWhiteSpace(commentText))
            {
                TempData["ErrorMessage"] = "Comment cannot be empty.";
                return RedirectToAction(nameof(Details), new { id = complaintId });
            }

            await _complaintService.AddCommentAsync(complaintId, commentText);

            TempData["SuccessMessage"] = "Comment added successfully.";

            return RedirectToAction(nameof(Details), new { id = complaintId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Archive(int id)
        {
            await _complaintService.ArchiveComplaintAsync(id);

            TempData["SuccessMessage"] = "Complaint archived successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
```

---

## Teaching Point

Now controller is clean.

Controller only:

```text
Receives request
Checks ModelState
Calls service
Returns view / redirect
```

No EF Core query is inside controller.

---

# 18. Step 11: Update Details View To Show Comments

Open:

```text
Views/Complaints/Details.cshtml
```

Use this simple version:

```cshtml
@model LiveClassDemos.Models.Complaint

<h1>Complaint Details</h1>

@if (TempData["SuccessMessage"] != null)
{
    <div style="color:green; font-weight:bold;">
        @TempData["SuccessMessage"]
    </div>
}

@if (TempData["ErrorMessage"] != null)
{
    <div style="color:red; font-weight:bold;">
        @TempData["ErrorMessage"]
    </div>
}

<p><strong>Id:</strong> @Model.Id</p>
<p><strong>Title:</strong> @Model.Title</p>
<p><strong>Description:</strong> @Model.Description</p>
<p><strong>Category:</strong> @Model.Category</p>
<p><strong>Priority:</strong> @Model.Priority</p>
<p><strong>Status:</strong> @Model.Status</p>
<p><strong>Created Date:</strong> @Model.CreatedDate.ToString("dd-MMM-yyyy hh:mm tt")</p>

<hr />

<h2>Comments</h2>

@if (Model.Comments == null || !Model.Comments.Any())
{
    <p>No comments yet.</p>
}
else
{
    <ul>
        @foreach (var comment in Model.Comments.OrderByDescending(c => c.CreatedDate))
        {
            <li>
                @comment.CommentText
                <br />
                <small>@comment.CreatedDate.ToString("dd-MMM-yyyy hh:mm tt")</small>
            </li>
        }
    </ul>
}

<h3>Add Comment</h3>

<form asp-action="AddComment" method="post">
    <input type="hidden" name="complaintId" value="@Model.Id" />

    <textarea name="commentText"></textarea>

    <br /><br />

    <button type="submit">Add Comment</button>
</form>

<br />

<a asp-action="Index">Back</a>
```

---

# 19. Run And Test

Run:

```bash
dotnet run
```

Open:

```text
/Complaints
```

---

## Test 1: Complaint List

Expected:

```text
Complaints should load using service and repository.
```

---

## Test 2: Create Complaint

Create complaint:

```text
Title: Water leakage
Description: Water is leaking near Tower A parking.
Category: Water
Priority: High
```

Expected:

```text
Complaint created successfully.
Status should be New.
```

---

## Test 3: Details With Comments

Open details page and add comment:

```text
Please check this urgently.
```

Expected:

```text
Comment saved in ComplaintComments table.
Details page shows comment.
```

---

## Test 4: Archive Complaint

Click archive from Index page if your Index has archive form.

Expected:

```text
Complaint disappears from active list.
IsArchived becomes true.
```

---

# 20. Optional: Index Archive Button

In `Views/Complaints/Index.cshtml`, add this inside Actions column:

```cshtml
<form asp-action="Archive" asp-route-id="@complaint.Id" method="post" style="display:inline;">
    <button type="submit">Archive</button>
</form>
```

---

# 21. Important Concepts To Explain

## Separation Of Work

```text
Controller = HTTP request/response
Service = Business rules
Repository = Database queries
DbContext = EF Core database session
```

---

## Repository Pattern

Repository hides EF Core query details.

Controller should not write:

```csharp
_context.Complaints.Where(...)
```

Controller should call:

```csharp
_complaintService.GetAllComplaintsAsync()
```

---

## DbContext As Unit Of Work

DbContext tracks changes.

Example:

```text
Add complaint comment
Update complaint status
Save once
```

`SaveChangesAsync` saves all tracked changes together.

---

## One-To-Many Relationship

```text
One Complaint → Many ComplaintComments
```

In parent:

```csharp
public List<ComplaintComment> Comments { get; set; }
```

In child:

```csharp
public int ComplaintId { get; set; }
public Complaint? Complaint { get; set; }
```

---

# 22. Common Mistakes

| Mistake | Problem | Fix |
|---|---|---|
| Controller directly uses DbContext everywhere | Controller becomes messy | Use service/repository |
| Business rules inside repository | Wrong responsibility | Put business rules in service |
| Repository returns IActionResult | Repository should not know MVC | Return data only |
| Forgetting DI registration | Runtime service error | Register in Program.cs |
| Forgetting migration after adding comment model | Table missing | Add migration and update database |
| Forgetting Include | Comments do not load | Use `.Include(c => c.Comments)` |
| Calling SaveChanges in every repository method | Hard to save related changes together | Let service decide when to save |
| No foreign key | Relationship unclear | Add `ComplaintId` |

---

# 23. One-Minute Revision

| Concept | Meaning |
|---|---|
| Controller | Handles request and response |
| Service / BO | Handles business rules |
| Repository | Handles database access |
| DbContext | Tracks and saves database changes |
| Unit of Work | Save multiple related changes together |
| One-to-many | One parent has many child records |
| Foreign key | Connects child record to parent |
| Navigation property | Allows EF Core to move between related objects |
| Include | Loads related data |

---

# 24. Interview Lines

## Separation Of Work

Separation of work means dividing responsibilities across controllers, services, repositories, and DbContext so the application remains clean and maintainable.

## Service / BO Class

A service or BO class contains business rules and workflow logic.

## Repository

A repository contains data access logic and hides EF Core queries from the service layer.

## DbContext Unit Of Work

DbContext acts as Unit of Work because it tracks multiple changes and saves them together using SaveChangesAsync.

## One-To-Many Relationship

One-to-many means one parent record can have many child records, such as one Complaint having many Comments.

## Foreign Key

A foreign key connects a child table to a parent table.

## Navigation Property

A navigation property allows EF Core to navigate between related entities.

---

# 25. Mini Assignment

Ask students to complete these:

```text
1. Create ComplaintAttachment model
2. Add ComplaintId foreign key
3. Add Attachments collection in Complaint model
4. Add DbSet<ComplaintAttachment> in ApplicationDbContext
5. Add migration and update database
6. Add repository method AddAttachmentAsync
7. Add service method AddAttachmentAsync
8. Show attachments on complaint details page
```

---

# 26. Final Outcome

After this 1-hour practical, students should be able to say:

```text
I refactored complaint code using proper separation of work.

The controller now calls the service. The service contains business rules. The repository contains EF Core queries. DbContext tracks and saves data.

I also created a one-to-many relationship between Complaint and ComplaintComment using ComplaintId foreign key and navigation properties.
```
