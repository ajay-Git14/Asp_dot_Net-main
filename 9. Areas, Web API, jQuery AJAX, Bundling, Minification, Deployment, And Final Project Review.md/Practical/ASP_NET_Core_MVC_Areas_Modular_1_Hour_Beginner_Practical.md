# ASP.NET Core MVC: 1-Hour Beginner Friendly Practical
## Topic: Areas and Modular Application Development

> **Live Coding Duration:** 1 Hour  
> **Style:** Beginner friendly, practical-first, code in front of students.  
> **Project Example:** Smart Society / LiveClassDemos MVC project

---

# 1. Practical Goal

In this practical, students will learn how to organize a large ASP.NET Core MVC application using **Areas**.

By the end of this practical, students should understand:

```text
What Areas are
Why Areas are useful
How to create Admin, Resident, and Staff modules
How to create Area controllers
How to create Area views
How to configure Area routing
How to link between Areas using asp-area
Why Areas are for organization, not automatic security
```

---

# 2. Simple Explanation

When an MVC project is small, we can keep controllers like this:

```text
Controllers/HomeController.cs
Controllers/ComplaintsController.cs
Controllers/StudentController.cs
```

But when the project grows, we may have different modules:

```text
Admin module
Resident module
Staff module
Reports module
Dashboard module
```

If all controllers stay in one folder, the project becomes hard to manage.

**Areas** help us split the MVC project into modules.

Simple meaning:

```text
Area = Separate module inside MVC application
```

---

# 3. Real-Life Analogy

Think of a large society office.

It has different counters:

```text
Admin Office
Resident Help Desk
Maintenance Staff Room
```

Each counter has its own responsibility.

In ASP.NET Core MVC:

```text
Admin Area = Admin module
Resident Area = Resident module
Staff Area = Staff module
```

---

# 4. What We Will Build In 1 Hour

We will create three Areas:

```text
Admin
Resident
Staff
```

And three pages:

```text
/Admin/Dashboard
/Resident/MyComplaints
/Staff/AssignedComplaints
```

Each Area will have:

```text
Controllers folder
Views folder
One controller
One Index view
```

---

# 5. Is Data Folder Required?

For this basic **Areas practical**, `Data` folder is **not required**.

Why?

Because we are only learning:

```text
Folder structure
Area controller
Area view
Area routing
Area navigation
```

But later, when Area pages show real database data, then they will use:

```text
Data/ApplicationDbContext.cs
Services
Repositories
```

So simple rule:

```text
Only Area structure demo = Data folder not required
Area + real complaint data = Data folder required
```

---

# 6. 1-Hour Live Coding Flow

| Time | Task |
|---|---|
| 0-5 min | Explain why Areas are needed |
| 5-12 min | Create Areas folder structure |
| 12-25 min | Create Admin Area controller and view |
| 25-35 min | Create Resident Area controller and view |
| 35-45 min | Create Staff Area controller and view |
| 45-52 min | Configure Area route in Program.cs |
| 52-57 min | Add navigation links using asp-area |
| 57-60 min | Test routes and revise |

---

# 7. Final Folder Structure

After this practical, project should look like this:

```text
LiveClassDemos
│
├── Areas
│   ├── Admin
│   │   ├── Controllers
│   │   │   └── DashboardController.cs
│   │   └── Views
│   │       └── Dashboard
│   │           └── Index.cshtml
│   │
│   ├── Resident
│   │   ├── Controllers
│   │   │   └── MyComplaintsController.cs
│   │   └── Views
│   │       └── MyComplaints
│   │           └── Index.cshtml
│   │
│   └── Staff
│       ├── Controllers
│       │   └── AssignedComplaintsController.cs
│       └── Views
│           └── AssignedComplaints
│               └── Index.cshtml
│
├── Controllers
├── Models
├── Views
└── Program.cs
```

---

# 8. Step 1: Create Areas Folder

Create this folder:

```text
Areas
```

Inside `Areas`, create:

```text
Admin
Resident
Staff
```

Inside each Area, create:

```text
Controllers
Views
```

Final folders:

```text
Areas/Admin/Controllers
Areas/Admin/Views

Areas/Resident/Controllers
Areas/Resident/Views

Areas/Staff/Controllers
Areas/Staff/Views
```

---

# 9. Step 2: Create Admin Dashboard Controller

Create file:

```text
Areas/Admin/Controllers/DashboardController.cs
```

Add this code:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace LiveClassDemos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
```

---

## Explanation

This line is very important:

```csharp
[Area("Admin")]
```

It tells ASP.NET Core MVC:

```text
This controller belongs to Admin Area.
```

Without this, Area routing may not find the controller correctly.

---

# 10. Step 3: Create Admin Dashboard View

Create folder:

```text
Areas/Admin/Views/Dashboard
```

Create file:

```text
Areas/Admin/Views/Dashboard/Index.cshtml
```

Add:

```cshtml
@{
    ViewData["Title"] = "Admin Dashboard";
}

<h1>Admin Dashboard</h1>

<p>This page belongs to the Admin Area.</p>

<ul>
    <li>Admin can monitor complaints.</li>
    <li>Admin can assign complaints to staff.</li>
    <li>Admin can view reports.</li>
</ul>

<a asp-area="" asp-controller="Home" asp-action="Index">Back To Home</a>
```

---

# 11. Step 4: Create Resident Controller

Create file:

```text
Areas/Resident/Controllers/MyComplaintsController.cs
```

Add:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace LiveClassDemos.Areas.Resident.Controllers
{
    [Area("Resident")]
    public class MyComplaintsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
```

---

# 12. Step 5: Create Resident View

Create folder:

```text
Areas/Resident/Views/MyComplaints
```

Create file:

```text
Areas/Resident/Views/MyComplaints/Index.cshtml
```

Add:

```cshtml
@{
    ViewData["Title"] = "My Complaints";
}

<h1>My Complaints</h1>

<p>This page belongs to the Resident Area.</p>

<ul>
    <li>Resident can raise complaints.</li>
    <li>Resident can view own complaints.</li>
    <li>Resident can track complaint status.</li>
</ul>

<a asp-area="" asp-controller="Home" asp-action="Index">Back To Home</a>
```

---

# 13. Step 6: Create Staff Controller

Create file:

```text
Areas/Staff/Controllers/AssignedComplaintsController.cs
```

Add:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace LiveClassDemos.Areas.Staff.Controllers
{
    [Area("Staff")]
    public class AssignedComplaintsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
```

---

# 14. Step 7: Create Staff View

Create folder:

```text
Areas/Staff/Views/AssignedComplaints
```

Create file:

```text
Areas/Staff/Views/AssignedComplaints/Index.cshtml
```

Add:

```cshtml
@{
    ViewData["Title"] = "Assigned Complaints";
}

<h1>Assigned Complaints</h1>

<p>This page belongs to the Staff Area.</p>

<ul>
    <li>Staff can view assigned complaints.</li>
    <li>Staff can update complaint progress.</li>
    <li>Staff can upload resolution proof.</li>
</ul>

<a asp-area="" asp-controller="Home" asp-action="Index">Back To Home</a>
```

---

# 15. Step 8: Configure Area Route In Program.cs

Open:

```text
Program.cs
```

Find the default route:

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

Add Area route **before** default route:

```csharp
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

---

## Important

Area route should come before default route.

Correct order:

```text
Area route first
Default route second
```

---

# 16. Step 9: Full Program.cs Route Section

Your bottom route section should look like this:

```csharp
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

If your project uses Identity, it should be:

```csharp
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

---

# 17. Step 10: Add Navigation Links

Open:

```text
Views/Shared/_Layout.cshtml
```

Inside navbar, add:

```cshtml
<li class="nav-item">
    <a class="nav-link text-dark"
       asp-area="Admin"
       asp-controller="Dashboard"
       asp-action="Index">
        Admin Dashboard
    </a>
</li>

<li class="nav-item">
    <a class="nav-link text-dark"
       asp-area="Resident"
       asp-controller="MyComplaints"
       asp-action="Index">
        My Complaints
    </a>
</li>

<li class="nav-item">
    <a class="nav-link text-dark"
       asp-area="Staff"
       asp-controller="AssignedComplaints"
       asp-action="Index">
        Assigned Complaints
    </a>
</li>
```

---

# 18. Understanding asp-area

This link:

```cshtml
<a asp-area="Admin" asp-controller="Dashboard" asp-action="Index">
    Admin Dashboard
</a>
```

creates this URL:

```text
/Admin/Dashboard
```

This link:

```cshtml
<a asp-area="" asp-controller="Home" asp-action="Index">
    Home
</a>
```

means:

```text
Go to root HomeController, not inside any Area.
```

---

# 19. Optional: Add Role-Based Navigation

If Identity and roles are already added, show links by role.

```cshtml
@if (User.Identity != null && User.Identity.IsAuthenticated)
{
    if (User.IsInRole("Admin"))
    {
        <li class="nav-item">
            <a class="nav-link text-dark"
               asp-area="Admin"
               asp-controller="Dashboard"
               asp-action="Index">
                Admin Dashboard
            </a>
        </li>
    }

    if (User.IsInRole("Resident"))
    {
        <li class="nav-item">
            <a class="nav-link text-dark"
               asp-area="Resident"
               asp-controller="MyComplaints"
               asp-action="Index">
                My Complaints
            </a>
        </li>
    }

    if (User.IsInRole("Staff"))
    {
        <li class="nav-item">
            <a class="nav-link text-dark"
               asp-area="Staff"
               asp-controller="AssignedComplaints"
               asp-action="Index">
                Assigned Complaints
            </a>
        </li>
    }
}
```

---

# 20. Optional: Add Authorization To Area Controllers

If Identity and roles are already configured, protect Area controllers.

Admin:

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveClassDemos.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
```

Resident:

```csharp
[Area("Resident")]
[Authorize(Roles = "Resident")]
public class MyComplaintsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
```

Staff:

```csharp
[Area("Staff")]
[Authorize(Roles = "Staff")]
public class AssignedComplaintsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
```

---

# 21. Important Teaching Point

Areas organize code.

Areas do not automatically secure code.

This is wrong thinking:

```text
Admin controller is inside Admin Area, so only Admin can access it.
```

Correct thinking:

```text
Admin controller is inside Admin Area for organization.
[Authorize(Roles = "Admin")] secures it.
```

---

# 22. Run And Test

Run project:

```bash
dotnet run
```

Test these URLs:

```text
/Admin/Dashboard
/Resident/MyComplaints
/Staff/AssignedComplaints
```

Expected:

```text
Each URL should open its own Area page.
```

---

# 23. Testing Table

| URL | Expected Page |
|---|---|
| `/Admin/Dashboard` | Admin Dashboard |
| `/Resident/MyComplaints` | My Complaints |
| `/Staff/AssignedComplaints` | Assigned Complaints |

---

# 24. Common Errors And Fixes

| Error | Reason | Fix |
|---|---|---|
| 404 for `/Admin/Dashboard` | Area route missing | Add `{area:exists}` route |
| View not found | View folder path wrong | Use `Areas/Admin/Views/Dashboard/Index.cshtml` |
| Controller not found | Missing `[Area("Admin")]` | Add Area attribute |
| Link goes to root controller | Missing `asp-area` | Add `asp-area="Admin"` |
| Home link not working from Area | Area not cleared | Use `asp-area=""` |
| Tag Helpers not working | `_ViewImports.cshtml` issue | Add Tag Helper import |

---

# 25. Area ViewImports Fix

If Tag Helpers like `asp-area` or `asp-controller` do not work inside Area views, create:

```text
Areas/Admin/Views/_ViewImports.cshtml
Areas/Resident/Views/_ViewImports.cshtml
Areas/Staff/Views/_ViewImports.cshtml
```

Add:

```cshtml
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

Usually root `_ViewImports.cshtml` may already handle this, but this fix helps beginners.

---

# 26. One-Minute Revision

| Concept | Meaning |
|---|---|
| Area | Module inside MVC application |
| `[Area("Admin")]` | Marks controller as part of Admin Area |
| Area route | Enables URLs like `/Admin/Dashboard` |
| `asp-area` | Generates links to Area routes |
| `asp-area=""` | Links back to root controllers |
| Area views | Stored under `Areas/{Area}/Views/{Controller}/{Action}.cshtml` |
| Authorization | Still required for security |

---

# 27. Interview Lines

## Area

An Area is a modular section of an ASP.NET Core MVC application with its own controllers and views.

## Why Areas

Areas help organize large MVC applications into modules such as Admin, Resident, and Staff.

## Area Controller

An Area controller is a normal MVC controller decorated with `[Area("AreaName")]`.

## Area Route

Area route uses `{area:exists}` in Program.cs to support URLs like `/Admin/Dashboard`.

## asp-area

`asp-area` tells Razor Tag Helpers which Area should be used while generating links or forms.

## Security

Areas organize code but do not secure it automatically. Use `[Authorize]` for server-side security.

---

# 28. Mini Assignment

Ask students to complete these:

```text
1. Create Admin ReportsController inside Admin Area
2. Create Reports/Index.cshtml view
3. Create Resident ComplaintSubmissionController inside Resident Area
4. Create ComplaintSubmission/Create.cshtml view
5. Create Staff StatusUpdateController inside Staff Area
6. Create StatusUpdate/Index.cshtml view
7. Add navigation links using asp-area
8. Test all routes manually
```

---

# 29. Final Outcome

After this 1-hour practical, students should be able to say:

```text
I created Areas in ASP.NET Core MVC.

I created Admin, Resident, and Staff modules. Each Area has its own Controllers and Views folders. I created Area controllers using the [Area] attribute and configured Area routing in Program.cs using {area:exists}.

I also used asp-area to generate correct links between Areas and understood that Areas organize code but do not automatically secure it.
```

---

# 30. Suggested Commit

```bash
git add .
git commit -m "feat: add mvc areas for admin resident and staff modules"
```
