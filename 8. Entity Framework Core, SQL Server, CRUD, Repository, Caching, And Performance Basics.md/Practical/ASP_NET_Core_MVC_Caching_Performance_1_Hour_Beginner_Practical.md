# ASP.NET Core MVC: 1-Hour Beginner Friendly Practical
## Topic: Caching and Performance Basics

> **Live Coding Duration:** 1 Hour  
> **Style:** Beginner friendly, practical-first, less theory, more code.  
> **Assumption:** EF Core CRUD project already exists with `Complaint`, `ApplicationDbContext`, and complaint list/create/update flow.

---

# 1. Practical Goal

In this 1-hour practical, students will learn how to improve basic performance in ASP.NET Core MVC using caching and better queries.

By the end, students should understand:

```text
What caching means
Why caching improves performance
How to use IMemoryCache
What cache keys are
What absolute expiration means
What sliding expiration means
Where caching is safe
Where caching is dangerous
How to cache complaint categories
How to cache dashboard summary
Why pagination is important
Why query optimization matters
```

---

# 2. Simple Story For Students

In our complaint system, some data is used again and again.

Example:

```text
Complaint category list
Priority list
Status list
Dashboard summary counts
```

If we go to the database every time, the application may become slow.

Caching helps us store safe reusable data temporarily.

But we must be careful.

Do not cache private complaint details incorrectly.

---

# 3. What We Will Build In 1 Hour

We will add:

```text
1. Memory cache setup
2. CacheKeys class
3. ComplaintLookupService for category list cache
4. DashboardSummaryViewModel
5. DashboardService with cached summary counts
6. Cache removal after complaint changes
7. Beginner pagination example
8. Basic query optimization awareness
```

---

# 4. Is Data Folder Required?

Yes, in this practical the **Data** folder is required because dashboard summary reads complaint counts from database using:

```text
Data/ApplicationDbContext.cs
```

But simple lookup caching like category list can work without database.

---

# 5. 1-Hour Live Coding Flow

| Time | Task |
|---|---|
| 0-5 min | Explain caching with simple example |
| 5-10 min | Add memory cache in Program.cs |
| 10-18 min | Create CacheKeys class |
| 18-30 min | Create ComplaintLookupService and cache categories |
| 30-42 min | Create DashboardSummaryViewModel and DashboardService |
| 42-50 min | Remove dashboard cache after complaint changes |
| 50-57 min | Add pagination/query optimization example |
| 57-60 min | Revision and interview lines |

---

# 6. Beginner Explanation

## What Is Caching?

Caching means storing frequently used data temporarily.

Simple meaning:

```text
Do not calculate or load the same data again and again.
Store it for some time and reuse it.
```

---

## Real Example

Without cache:

```text
Request 1 → Load categories from code/database
Request 2 → Load categories again
Request 3 → Load categories again
```

With cache:

```text
Request 1 → Load categories and store in cache
Request 2 → Use cache
Request 3 → Use cache
```

---

## Good Data For Cache

```text
Complaint categories
Priority list
Status list
Dashboard summary for short time
Public page content
```

---

## Bad Data For Cache

```text
Private complaint details
Resident personal information
Complaint attachments
Authorization-sensitive data
Password/token data
```

---

# 7. Step 1: Add Memory Cache

Open:

```text
Program.cs
```

Add this line before `var app = builder.Build();`:

```csharp
builder.Services.AddMemoryCache();
```

Example:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();

var app = builder.Build();
```

---

## Explain To Students

This line enables ASP.NET Core memory caching.

Now we can inject:

```csharp
IMemoryCache
```

inside services.

---

# 8. Step 2: Create CacheKeys Class

Create folder:

```text
Constants
```

Create file:

```text
Constants/CacheKeys.cs
```

Add this code:

```csharp
namespace LiveClassDemos.Constants
{
    public static class CacheKeys
    {
        public const string ComplaintCategories = "complaint_categories";

        public const string ComplaintPriorities = "complaint_priorities";

        public const string ComplaintStatuses = "complaint_statuses";

        public const string AdminDashboardSummary = "admin_dashboard_summary";
    }
}
```

---

## Explain To Students

Cache key is the name used to store and read cached data.

Bad:

```csharp
"data"
```

Good:

```csharp
CacheKeys.ComplaintCategories
```

Clear cache keys prevent confusion.

---

# 9. Step 3: Create ComplaintLookupService

Create folder:

```text
Services
```

Create file:

```text
Services/ComplaintLookupService.cs
```

Add this code:

```csharp
using LiveClassDemos.Constants;
using Microsoft.Extensions.Caching.Memory;

namespace LiveClassDemos.Services
{
    public class ComplaintLookupService
    {
        private readonly IMemoryCache _memoryCache;

        public ComplaintLookupService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public List<string> GetComplaintCategories()
        {
            if (_memoryCache.TryGetValue(
                CacheKeys.ComplaintCategories,
                out List<string>? categories))
            {
                return categories!;
            }

            categories = new List<string>
            {
                "Water",
                "Electricity",
                "Lift",
                "Security",
                "Parking",
                "Cleaning"
            };

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                .SetAbsoluteExpiration(TimeSpan.FromHours(2));

            _memoryCache.Set(
                CacheKeys.ComplaintCategories,
                categories,
                cacheOptions);

            return categories;
        }

        public List<string> GetPriorities()
        {
            if (_memoryCache.TryGetValue(
                CacheKeys.ComplaintPriorities,
                out List<string>? priorities))
            {
                return priorities!;
            }

            priorities = new List<string>
            {
                "Low",
                "Medium",
                "High",
                "Critical"
            };

            _memoryCache.Set(
                CacheKeys.ComplaintPriorities,
                priorities,
                TimeSpan.FromHours(2));

            return priorities;
        }
    }
}
```

---

# 10. Step 4: Register ComplaintLookupService

Open:

```text
Program.cs
```

Add namespace:

```csharp
using LiveClassDemos.Services;
```

Register service:

```csharp
builder.Services.AddScoped<ComplaintLookupService>();
```

Example:

```csharp
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ComplaintLookupService>();
```

---

# 11. Step 5: Use Cached Categories In Controller

Open:

```text
Controllers/ComplaintsController.cs
```

Inject `ComplaintLookupService`:

```csharp
private readonly ComplaintLookupService _lookupService;

public ComplaintsController(
    ApplicationDbContext context,
    ComplaintLookupService lookupService)
{
    _context = context;
    _lookupService = lookupService;
}
```

Update GET Create action:

```csharp
[HttpGet]
public IActionResult Create()
{
    ViewBag.Categories = _lookupService.GetComplaintCategories();
    ViewBag.Priorities = _lookupService.GetPriorities();

    return View();
}
```

Also update invalid POST return:

```csharp
if (!ModelState.IsValid)
{
    ViewBag.Categories = _lookupService.GetComplaintCategories();
    ViewBag.Priorities = _lookupService.GetPriorities();
    return View(complaint);
}
```

---

# 12. Step 6: Use Cached Values In Create View

Open:

```text
Views/Complaints/Create.cshtml
```

Use category dropdown:

```cshtml
<div>
    <label asp-for="Category"></label>
    <select asp-for="Category">
        <option value="">-- Select Category --</option>

        @foreach (var category in ViewBag.Categories)
        {
            <option value="@category">@category</option>
        }
    </select>
    <span asp-validation-for="Category" style="color:red"></span>
</div>
```

Use priority dropdown:

```cshtml
<div>
    <label asp-for="Priority"></label>
    <select asp-for="Priority">
        <option value="">-- Select Priority --</option>

        @foreach (var priority in ViewBag.Priorities)
        {
            <option value="@priority">@priority</option>
        }
    </select>
    <span asp-validation-for="Priority" style="color:red"></span>
</div>
```

---

## Explain To Students

The dropdown data is now coming from a service.

The service uses memory cache.

So the category list is not recreated again and again unnecessarily.

---

# 13. Step 7: Create DashboardSummaryViewModel

Create folder if not already available:

```text
ViewModels
```

Create file:

```text
ViewModels/DashboardSummaryViewModel.cs
```

Add:

```csharp
namespace LiveClassDemos.ViewModels
{
    public class DashboardSummaryViewModel
    {
        public int TotalComplaints { get; set; }

        public int NewComplaints { get; set; }

        public int InProgressComplaints { get; set; }

        public int ResolvedComplaints { get; set; }

        public int CriticalComplaints { get; set; }
    }
}
```

---

# 14. Step 8: Create DashboardService

Create file:

```text
Services/DashboardService.cs
```

Add this code:

```csharp
using LiveClassDemos.Constants;
using LiveClassDemos.Data;
using LiveClassDemos.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LiveClassDemos.Services
{
    public class DashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public DashboardService(
            ApplicationDbContext context,
            IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<DashboardSummaryViewModel> GetDashboardSummaryAsync()
        {
            if (_memoryCache.TryGetValue(
                CacheKeys.AdminDashboardSummary,
                out DashboardSummaryViewModel? cachedSummary))
            {
                return cachedSummary!;
            }

            var summary = new DashboardSummaryViewModel
            {
                TotalComplaints = await _context.Complaints
                    .CountAsync(c => !c.IsArchived),

                NewComplaints = await _context.Complaints
                    .CountAsync(c => !c.IsArchived && c.Status == "New"),

                InProgressComplaints = await _context.Complaints
                    .CountAsync(c => !c.IsArchived && c.Status == "In Progress"),

                ResolvedComplaints = await _context.Complaints
                    .CountAsync(c => !c.IsArchived && c.Status == "Resolved"),

                CriticalComplaints = await _context.Complaints
                    .CountAsync(c => !c.IsArchived && c.Priority == "Critical")
            };

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

            _memoryCache.Set(
                CacheKeys.AdminDashboardSummary,
                summary,
                cacheOptions);

            return summary;
        }

        public void ClearDashboardSummaryCache()
        {
            _memoryCache.Remove(CacheKeys.AdminDashboardSummary);
        }
    }
}
```

---

# 15. Step 9: Register DashboardService

Open:

```text
Program.cs
```

Add:

```csharp
builder.Services.AddScoped<DashboardService>();
```

Final service registration can look like:

```csharp
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ComplaintLookupService>();
builder.Services.AddScoped<DashboardService>();
```

---

# 16. Step 10: Create DashboardController

Create file:

```text
Controllers/DashboardController.cs
```

Add:

```csharp
using LiveClassDemos.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiveClassDemos.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var summary = await _dashboardService.GetDashboardSummaryAsync();

            return View(summary);
        }
    }
}
```

---

# 17. Step 11: Create Dashboard View

Create folder:

```text
Views/Dashboard
```

Create file:

```text
Views/Dashboard/Index.cshtml
```

Add:

```cshtml
@model LiveClassDemos.ViewModels.DashboardSummaryViewModel

<h1>Dashboard Summary</h1>

<table border="1" cellpadding="8">
    <tr>
        <th>Total Complaints</th>
        <td>@Model.TotalComplaints</td>
    </tr>
    <tr>
        <th>New Complaints</th>
        <td>@Model.NewComplaints</td>
    </tr>
    <tr>
        <th>In Progress Complaints</th>
        <td>@Model.InProgressComplaints</td>
    </tr>
    <tr>
        <th>Resolved Complaints</th>
        <td>@Model.ResolvedComplaints</td>
    </tr>
    <tr>
        <th>Critical Complaints</th>
        <td>@Model.CriticalComplaints</td>
    </tr>
</table>

<br />

<a asp-controller="Complaints" asp-action="Index">Back To Complaints</a>
```

---

# 18. Step 12: Clear Dashboard Cache After Complaint Changes

Whenever complaint data changes, dashboard count may become stale.

So remove dashboard cache after:

```text
Create
Edit
Archive
Status update
Priority update
```

In `ComplaintsController`, inject `DashboardService`:

```csharp
private readonly DashboardService _dashboardService;
```

Constructor:

```csharp
public ComplaintsController(
    ApplicationDbContext context,
    ComplaintLookupService lookupService,
    DashboardService dashboardService)
{
    _context = context;
    _lookupService = lookupService;
    _dashboardService = dashboardService;
}
```

After create save:

```csharp
await _context.SaveChangesAsync();
_dashboardService.ClearDashboardSummaryCache();
```

After edit save:

```csharp
await _context.SaveChangesAsync();
_dashboardService.ClearDashboardSummaryCache();
```

After archive save:

```csharp
await _context.SaveChangesAsync();
_dashboardService.ClearDashboardSummaryCache();
```

---

## Explain To Students

If we do not remove cache after changes, dashboard may show old numbers.

This is called stale cache.

---

# 19. Step 13: Basic Pagination Example

Open `ComplaintsController` and update Index action:

```csharp
public async Task<IActionResult> Index(int pageNumber = 1)
{
    int pageSize = 5;

    if (pageNumber < 1)
    {
        pageNumber = 1;
    }

    var complaints = await _context.Complaints
        .Where(c => !c.IsArchived)
        .OrderByDescending(c => c.CreatedDate)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    ViewBag.PageNumber = pageNumber;

    return View(complaints);
}
```

Add pagination links in `Views/Complaints/Index.cshtml`:

```cshtml
<br />

<a asp-action="Index" asp-route-pageNumber="@(ViewBag.PageNumber - 1)">Previous</a>
|
<a asp-action="Index" asp-route-pageNumber="@(ViewBag.PageNumber + 1)">Next</a>
```

---

## Explain To Students

Pagination means:

```text
Do not load all records at once.
Load small number of records page by page.
```

This improves performance.

---

# 20. Step 14: Query Optimization Awareness

Bad list query:

```csharp
var complaints = await _context.Complaints
    .Include(c => c.Comments)
    .Include(c => c.Attachments)
    .ToListAsync();
```

Why bad?

```text
List page does not need all comments and attachments.
It loads too much data.
```

Better list query:

```csharp
var complaints = await _context.Complaints
    .Where(c => !c.IsArchived)
    .OrderByDescending(c => c.CreatedDate)
    .ToListAsync();
```

Even better later:

```text
Use ViewModel projection and pagination.
```

---

# 21. Optional: Response Cache Awareness

For public pages only, we can use:

```csharp
[ResponseCache(Duration = 60)]
public IActionResult About()
{
    return View();
}
```

Do not response-cache private pages like:

```text
Complaint details
Resident dashboard
Admin dashboard
Staff assigned complaints
```

---

# 22. Testing Steps

## Test 1: Category Cache

Open:

```text
/Complaints/Create
```

Expected:

```text
Category dropdown loads from ComplaintLookupService.
Service uses cache after first request.
```

---

## Test 2: Dashboard Cache

Open:

```text
/Dashboard
```

Expected:

```text
Dashboard summary loads.
Next request within 2 minutes uses cache.
```

---

## Test 3: Cache Clear After Create

Create a new complaint.

Then open:

```text
/Dashboard
```

Expected:

```text
Dashboard count updates because cache was removed after complaint create.
```

---

## Test 4: Pagination

Create more than 5 complaints.

Open:

```text
/Complaints
```

Expected:

```text
Only 5 complaints are shown per page.
Next link loads next page.
```

---

# 23. Common Mistakes

| Mistake | Problem | Fix |
|---|---|---|
| Forgetting `AddMemoryCache()` | IMemoryCache injection fails | Add it in Program.cs |
| Caching private complaint details | Privacy risk | Avoid sensitive caching |
| No cache expiration | Data can become stale | Add expiration |
| Dashboard cache too long | Wrong counts shown | Use short expiration |
| Not removing cache after update | Stale dashboard | Remove cache key |
| Same cache key for all users | Data leakage risk | Use safe keys or avoid |
| Loading all complaints | Slow page | Use pagination |
| Using Include everywhere | Heavy query | Include only when needed |

---

# 24. One-Minute Revision

| Concept | Meaning |
|---|---|
| Cache | Temporary storage for frequently used data |
| IMemoryCache | ASP.NET Core memory cache service |
| Cache Key | Unique name for cached data |
| Absolute Expiration | Cache expires after fixed time |
| Sliding Expiration | Cache expires after inactivity |
| Dashboard Cache | Cache aggregate counts for short time |
| Cache Invalidation | Remove cache when data changes |
| Response Cache | Cache full HTTP response |
| Distributed Cache | Shared cache for multiple servers |
| Pagination | Load records page by page |
| Query Optimization | Load only required data |

---

# 25. Interview Lines

## Caching

Caching stores frequently used data temporarily to improve performance and reduce repeated database work.

## IMemoryCache

IMemoryCache is the ASP.NET Core service used to store and retrieve cached data from server memory.

## Cache Key

A cache key uniquely identifies cached data.

## Absolute Expiration

Absolute expiration removes cached data after a fixed duration.

## Sliding Expiration

Sliding expiration removes cached data after a period of inactivity.

## Cache Invalidation

Cache invalidation means removing cached data when the original data changes.

## Pagination

Pagination improves performance by loading a small number of records at a time using Skip and Take.

## Query Optimization

Query optimization means filtering, ordering, projecting, and loading only the data required by the screen.

---

# 26. Mini Assignment

Ask students to complete:

```text
1. Cache complaint status list
2. Cache priority list
3. Add dashboard cache clear after archive
4. Add pageSize parameter in complaint list
5. Prevent pageSize greater than 50
6. Write a table: which data should be cached and which should not
```

---

# 27. Final Outcome

After this 1-hour practical, students should be able to say:

```text
I added basic caching and performance improvements in ASP.NET Core MVC.

I used IMemoryCache to cache safe data like complaint categories and dashboard summary. I learned cache keys, absolute expiration, sliding expiration, and cache invalidation. I also learned why sensitive user-specific complaint data should not be cached carelessly.

I also improved complaint list performance using pagination and understood why queries should load only required data.
```
