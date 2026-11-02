# ASP.NET Core MVC: 1-Hour Beginner Friendly Practical
## Topic: CRUD Operations Using Entity Framework Core

> **Live Coding Duration:** 1 Hour  
> **Style:** Beginner friendly, practical-first, code in front of students.  
> **Assumption:** EF Core setup, `ApplicationDbContext`, database connection, and migration setup already exist from the previous EF Core practical.

---

# 1. Practical Goal

In this practical, students will learn how to perform CRUD operations using Entity Framework Core.

CRUD means:

```text
C = Create
R = Read
U = Update
D = Delete
```

In a real project, CRUD is used everywhere.

For our beginner practical, we will use a simple **Complaint Management** example.

By the end, students should understand:

```text
How to insert data using AddAsync
How to save data using SaveChangesAsync
How to read records using ToListAsync
How to read one record using FirstOrDefaultAsync
How to update an existing record safely
How to delete or archive a record
Why soft delete is safer than hard delete
```

---

# 2. What We Will Build In 1 Hour

We will build basic complaint CRUD:

```text
1. Complaint list
2. Create complaint
3. View complaint details
4. Edit complaint
5. Archive complaint using soft delete
```

We will not build advanced role-based workflows in this hour.

Keep it simple for students.

---

# 3. Is Data Folder Required?

Yes.

For this topic, the **Data** folder is required because EF Core needs `ApplicationDbContext`.

Expected file:

```text
Data/ApplicationDbContext.cs
```

This file connects our C# model with the SQL Server database.

Example:

```csharp
public DbSet<Complaint> Complaints { get; set; }
```

This line means EF Core will work with a database table named:

```text
Complaints
```

---

# 4. 1-Hour Live Coding Flow

| Time | Task |
|---|---|
| 0-5 min | Explain CRUD in simple language |
| 5-10 min | Check Complaint model and DbContext |
| 10-20 min | Create complaint form and POST action |
| 20-30 min | Read complaint list |
| 30-38 min | Details page using FirstOrDefaultAsync |
| 38-50 min | Edit complaint safely |
| 50-57 min | Archive complaint using soft delete |
| 57-60 min | Revision and testing |

---

# 5. Required Packages

These should already be installed from the EF Core setup session:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

If Identity is also used:

```bash
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```

---

# 6. Required Existing Setup

Before this practical, these should already exist:

```text
Data/ApplicationDbContext.cs
Models/Complaint.cs
Program.cs with AddDbContext
appsettings.json with DefaultConnection
Database migration already applied
```

---

# 7. Complaint Model

Open:

```text
Models/Complaint.cs
```

Use this beginner-friendly model:

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
    }
}
```

---

## Explain To Students

This class will become a table in the database.

Important fields:

| Property | Meaning |
|---|---|
| Id | Primary key |
| Title | Complaint title |
| Description | Complaint details |
| Category | Complaint category |
| Priority | Low, Medium, High |
| Status | New, In Progress, Resolved |
| CreatedDate | When complaint was created |
| IsArchived | Soft delete flag |

---

# 8. ApplicationDbContext

Open:

```text
Data/ApplicationDbContext.cs
```

Make sure `DbSet` exists:

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
    }
}
```

If your project is using Identity, then it may look like this:

```csharp
using LiveClassDemos.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LiveClassDemos.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Complaint> Complaints { get; set; }
    }
}
```

Both are fine depending on your project.

---

# 9. Add Migration If Model Changed

If `Complaint` model was newly created or changed, run:

```bash
dotnet ef migrations add AddComplaintCrudTable
dotnet ef database update
```

If migration already exists, do not create duplicate migration.

---

# 10. Create ComplaintsController

Create file:

```text
Controllers/ComplaintsController.cs
```

Add this code step by step during live coding.

```csharp
using LiveClassDemos.Data;
using LiveClassDemos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LiveClassDemos.Controllers
{
    public class ComplaintsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComplaintsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var complaints = await _context.Complaints
                .Where(c => !c.IsArchived)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

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

            complaint.Status = "New";
            complaint.CreatedDate = DateTime.Now;
            complaint.IsArchived = false;

            await _context.Complaints.AddAsync(complaint);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Complaint created successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var complaint = await _context.Complaints
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsArchived);

            if (complaint == null)
            {
                return NotFound();
            }

            return View(complaint);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var complaint = await _context.Complaints
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsArchived);

            if (complaint == null)
            {
                return NotFound();
            }

            return View(complaint);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Complaint formData)
        {
            if (id != formData.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            var complaintFromDb = await _context.Complaints
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsArchived);

            if (complaintFromDb == null)
            {
                return NotFound();
            }

            complaintFromDb.Title = formData.Title;
            complaintFromDb.Description = formData.Description;
            complaintFromDb.Category = formData.Category;
            complaintFromDb.Priority = formData.Priority;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Complaint updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Archive(int id)
        {
            var complaint = await _context.Complaints
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsArchived);

            if (complaint == null)
            {
                return NotFound();
            }

            complaint.IsArchived = true;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Complaint archived successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
```

---

# 11. Explain Controller Code

## Read All Complaints

```csharp
var complaints = await _context.Complaints
    .Where(c => !c.IsArchived)
    .OrderByDescending(c => c.CreatedDate)
    .ToListAsync();
```

Meaning:

```text
Get only active complaints
Show latest complaints first
Convert database records into a list
```

---

## Create Complaint

```csharp
await _context.Complaints.AddAsync(complaint);
await _context.SaveChangesAsync();
```

Meaning:

```text
AddAsync prepares INSERT
SaveChangesAsync saves it into database
```

---

## Details

```csharp
FirstOrDefaultAsync(c => c.Id == id && !c.IsArchived)
```

Meaning:

```text
Find one complaint by id
Ignore archived complaints
Return null if not found
```

---

## Safe Edit

We do not directly save posted object.

Better pattern:

```csharp
var complaintFromDb = await _context.Complaints.FirstOrDefaultAsync(...);

complaintFromDb.Title = formData.Title;
complaintFromDb.Description = formData.Description;
```

Meaning:

```text
Load real database record first
Update only allowed fields
Then save
```

This helps avoid overposting.

---

## Archive

```csharp
complaint.IsArchived = true;
await _context.SaveChangesAsync();
```

Meaning:

```text
Do not delete permanently
Just hide it from normal list
```

This is called soft delete.

---

# 12. Create Views Folder

Create folder:

```text
Views/Complaints
```

---

# 13. Create Index View

Create file:

```text
Views/Complaints/Index.cshtml
```

Add:

```cshtml
@model List<LiveClassDemos.Models.Complaint>

<h1>Complaints</h1>

@if (TempData["SuccessMessage"] != null)
{
    <div style="color:green; font-weight:bold;">
        @TempData["SuccessMessage"]
    </div>
}

<p>
    <a asp-action="Create">Create New Complaint</a>
</p>

@if (!Model.Any())
{
    <p>No complaints found.</p>
}
else
{
    <table border="1" cellpadding="8">
        <thead>
            <tr>
                <th>Id</th>
                <th>Title</th>
                <th>Category</th>
                <th>Priority</th>
                <th>Status</th>
                <th>Created Date</th>
                <th>Actions</th>
            </tr>
        </thead>

        <tbody>
            @foreach (var complaint in Model)
            {
                <tr>
                    <td>@complaint.Id</td>
                    <td>@complaint.Title</td>
                    <td>@complaint.Category</td>
                    <td>@complaint.Priority</td>
                    <td>@complaint.Status</td>
                    <td>@complaint.CreatedDate.ToString("dd-MMM-yyyy hh:mm tt")</td>
                    <td>
                        <a asp-action="Details" asp-route-id="@complaint.Id">Details</a> |
                        <a asp-action="Edit" asp-route-id="@complaint.Id">Edit</a> |

                        <form asp-action="Archive" asp-route-id="@complaint.Id" method="post" style="display:inline;">
                            <button type="submit">Archive</button>
                        </form>
                    </td>
                </tr>
            }
        </tbody>
    </table>
}
```

---

# 14. Create Create View

Create file:

```text
Views/Complaints/Create.cshtml
```

Add:

```cshtml
@model LiveClassDemos.Models.Complaint

<h1>Create Complaint</h1>

<form asp-action="Create" method="post">

    <div asp-validation-summary="All" style="color:red"></div>

    <div>
        <label asp-for="Title"></label>
        <input asp-for="Title" />
        <span asp-validation-for="Title" style="color:red"></span>
    </div>

    <br />

    <div>
        <label asp-for="Description"></label>
        <textarea asp-for="Description"></textarea>
        <span asp-validation-for="Description" style="color:red"></span>
    </div>

    <br />

    <div>
        <label asp-for="Category"></label>
        <select asp-for="Category">
            <option value="">-- Select Category --</option>
            <option value="Water">Water</option>
            <option value="Electricity">Electricity</option>
            <option value="Lift">Lift</option>
            <option value="Security">Security</option>
            <option value="Parking">Parking</option>
        </select>
        <span asp-validation-for="Category" style="color:red"></span>
    </div>

    <br />

    <div>
        <label asp-for="Priority"></label>
        <select asp-for="Priority">
            <option value="">-- Select Priority --</option>
            <option value="Low">Low</option>
            <option value="Medium">Medium</option>
            <option value="High">High</option>
        </select>
        <span asp-validation-for="Priority" style="color:red"></span>
    </div>

    <br />

    <button type="submit">Save Complaint</button>
    <a asp-action="Index">Back</a>

</form>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

---

# 15. Create Details View

Create file:

```text
Views/Complaints/Details.cshtml
```

Add:

```cshtml
@model LiveClassDemos.Models.Complaint

<h1>Complaint Details</h1>

<p><strong>Id:</strong> @Model.Id</p>
<p><strong>Title:</strong> @Model.Title</p>
<p><strong>Description:</strong> @Model.Description</p>
<p><strong>Category:</strong> @Model.Category</p>
<p><strong>Priority:</strong> @Model.Priority</p>
<p><strong>Status:</strong> @Model.Status</p>
<p><strong>Created Date:</strong> @Model.CreatedDate.ToString("dd-MMM-yyyy hh:mm tt")</p>

<a asp-action="Edit" asp-route-id="@Model.Id">Edit</a> |
<a asp-action="Index">Back</a>
```

---

# 16. Create Edit View

Create file:

```text
Views/Complaints/Edit.cshtml
```

Add:

```cshtml
@model LiveClassDemos.Models.Complaint

<h1>Edit Complaint</h1>

<form asp-action="Edit" asp-route-id="@Model.Id" method="post">

    <input type="hidden" asp-for="Id" />

    <div asp-validation-summary="All" style="color:red"></div>

    <div>
        <label asp-for="Title"></label>
        <input asp-for="Title" />
        <span asp-validation-for="Title" style="color:red"></span>
    </div>

    <br />

    <div>
        <label asp-for="Description"></label>
        <textarea asp-for="Description"></textarea>
        <span asp-validation-for="Description" style="color:red"></span>
    </div>

    <br />

    <div>
        <label asp-for="Category"></label>
        <select asp-for="Category">
            <option value="Water">Water</option>
            <option value="Electricity">Electricity</option>
            <option value="Lift">Lift</option>
            <option value="Security">Security</option>
            <option value="Parking">Parking</option>
        </select>
        <span asp-validation-for="Category" style="color:red"></span>
    </div>

    <br />

    <div>
        <label asp-for="Priority"></label>
        <select asp-for="Priority">
            <option value="Low">Low</option>
            <option value="Medium">Medium</option>
            <option value="High">High</option>
        </select>
        <span asp-validation-for="Priority" style="color:red"></span>
    </div>

    <br />

    <p>
        <strong>Status:</strong> @Model.Status
    </p>

    <button type="submit">Update Complaint</button>
    <a asp-action="Index">Back</a>

</form>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

---

# 17. Add Navigation Link

Open:

```text
Views/Shared/_Layout.cshtml
```

Inside navbar, add:

```cshtml
<li class="nav-item">
    <a class="nav-link text-dark" asp-controller="Complaints" asp-action="Index">
        Complaints
    </a>
</li>
```

---

# 18. Run And Test

Run:

```bash
dotnet run
```

Open:

```text
/Complaints
```

---

## Test 1: Create

Create a complaint:

```text
Title: Lift not working
Description: Lift is not working in Tower B since morning.
Category: Lift
Priority: High
```

Expected:

```text
Complaint appears in list
Success message appears
Record is saved in database
```

---

## Test 2: Read List

Open:

```text
/Complaints
```

Expected:

```text
Active complaints are shown
Archived complaints are hidden
Latest complaint appears first
```

---

## Test 3: Details

Click:

```text
Details
```

Expected:

```text
One complaint detail page opens
```

---

## Test 4: Edit

Click:

```text
Edit
```

Change:

```text
Priority: Medium
```

Expected:

```text
Complaint priority updates
Status remains unchanged
```

---

## Test 5: Archive

Click:

```text
Archive
```

Expected:

```text
Complaint disappears from list
Record is not physically deleted
IsArchived becomes true in database
```

---

# 19. Important Teaching Points

## AddAsync

```csharp
await _context.Complaints.AddAsync(complaint);
```

This prepares a new record for insertion.

---

## SaveChangesAsync

```csharp
await _context.SaveChangesAsync();
```

This actually saves changes into SQL Server.

---

## ToListAsync

```csharp
await _context.Complaints.ToListAsync();
```

This reads multiple records.

---

## FirstOrDefaultAsync

```csharp
await _context.Complaints.FirstOrDefaultAsync(c => c.Id == id);
```

This reads one record based on condition.

---

## Update

```csharp
complaintFromDb.Title = formData.Title;
await _context.SaveChangesAsync();
```

Load first, update allowed fields, save.

---

## Soft Delete

```csharp
complaint.IsArchived = true;
await _context.SaveChangesAsync();
```

Do not permanently delete important business records.

---

# 20. Common Mistakes

| Mistake | Problem | Fix |
|---|---|---|
| Forgetting SaveChangesAsync | Data not saved | Always call SaveChangesAsync |
| Missing `using Microsoft.EntityFrameworkCore;` | Async methods not found | Add namespace |
| No DbSet in DbContext | Table/query not available | Add `DbSet<Complaint>` |
| Migration not updated | Database table missing | Run migration and update database |
| Direct update from form | Overposting risk | Load entity from database first |
| Hard deleting business data | History lost | Use soft delete |
| Archive button using GET | Unsafe | Use POST form |
| Not checking null | Runtime error | Return NotFound |

---

# 21. One-Minute Revision

| EF Core Method | Use |
|---|---|
| `AddAsync` | Add new entity |
| `SaveChangesAsync` | Save tracked changes to database |
| `ToListAsync` | Get list of records |
| `FirstOrDefaultAsync` | Get one record with condition |
| `FindAsync` | Get one record by primary key |
| `Include` | Load related data |
| `Remove` | Hard delete record |
| Soft delete | Mark record archived instead of deleting |

---

# 22. Interview Lines

## CRUD

CRUD means Create, Read, Update, and Delete database operations.

## Create

Create operation inserts a new record using `AddAsync` and `SaveChangesAsync`.

## Read

Read operation retrieves records using EF Core LINQ queries like `ToListAsync` and `FirstOrDefaultAsync`.

## Update

Update operation loads an existing record, changes allowed properties, and calls `SaveChangesAsync`.

## Delete

Delete operation removes a record, but business applications often prefer soft delete.

## Soft Delete

Soft delete marks a record as archived instead of physically deleting it.

## SaveChangesAsync

`SaveChangesAsync` persists all tracked changes from DbContext to the database.

---

# 23. Mini Assignment

Ask students to complete these:

```text
1. Add Status dropdown in Edit view
2. Allow only New, In Progress, Resolved
3. Update Status safely in Edit POST
4. Add Archived list page
5. Show only archived complaints on Archived page
6. Add Restore button for archived complaint
```

---

# 24. Final Outcome

After this 1-hour practical, students should be able to say:

```text
I implemented CRUD operations using Entity Framework Core.

I created complaints using AddAsync and SaveChangesAsync. I displayed complaint lists using ToListAsync. I opened complaint details using FirstOrDefaultAsync. I updated existing complaints safely by loading the database record first and updating only allowed fields. I also implemented soft delete using IsArchived instead of permanently deleting records.
```
