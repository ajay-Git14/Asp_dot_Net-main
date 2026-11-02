# ASP.NET Core MVC: 1-Hour Beginner Friendly Practical
## Topic: Roles, Claims, Policy Thinking, and Security Hardening

> **Live Coding Duration:** 1 Hour  
> **Important:** This practical assumes your previous **Authentication + Identity** setup already exists.

---

# 1. Practical Goal

In the previous session, students learned:

```text
Register
Login
Logout
ASP.NET Core Identity
ApplicationUser
ApplicationDbContext
UserManager
SignInManager
```

In this session, students will learn what happens **after login**.

The application must decide:

```text
Who is logged in?
What is this user allowed to access?
```

This is handled by:

```text
Roles
Authorize attribute
Claims awareness
Policy awareness
Security hardening
```

---

# 2. Is Data Folder Required?

Yes.

For this practical, the `Data` folder is required because roles are stored in Identity database tables.

You need:

```text
Data/ApplicationDbContext.cs
Data/IdentitySeedData.cs
```

Why?

Because Identity roles are saved in tables like:

```text
AspNetRoles
AspNetUserRoles
AspNetUsers
```

So this practical depends on the previous Identity database setup.

---

# 3. What We Will Build In 1 Hour

We will add role-based access to an existing Identity project.

We will create:

```text
1. AppRoles constants file
2. IdentitySeedData class
3. Seed Resident, Admin, and Staff roles
4. Seed one default Admin user
5. Assign Resident role during registration
6. Create Admin-only page
7. Create Resident-only page
8. Create Staff-only page
9. Configure Access Denied page
10. Explain why UI hiding is not enough
```

---

# 4. 1-Hour Live Coding Flow

| Time | Task |
|---|---|
| 0-5 min | Explain login vs role access |
| 5-10 min | Create AppRoles constants |
| 10-25 min | Create IdentitySeedData |
| 25-32 min | Call seed method in Program.cs |
| 32-38 min | Assign Resident role during registration |
| 38-50 min | Create role-protected pages |
| 50-55 min | Add AccessDenied page |
| 55-60 min | Test with Admin, Resident, and anonymous user |

---

# 5. Beginner Explanation

## Authentication

Authentication means:

```text
Who are you?
```

Example:

```text
User logs in using email and password.
```

## Authorization

Authorization means:

```text
What can you access?
```

Example:

```text
Admin can open Admin page.
Resident cannot open Admin page.
```

## Role

A role is a user group.

Examples:

```text
Admin
Resident
Staff
```

---

# 6. Required Existing Files

Before starting this practical, these files should already exist:

```text
Models/ApplicationUser.cs
Data/ApplicationDbContext.cs
ViewModels/RegisterViewModel.cs
ViewModels/LoginViewModel.cs
Controllers/AccountController.cs
Views/Account/Register.cshtml
Views/Account/Login.cshtml
```

`Program.cs` should already have Identity configured.

Important line:

```csharp
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
```

---

# 7. Create AppRoles Constants

Create folder:

```text
Constants
```

Create file:

```text
Constants/AppRoles.cs
```

Add this code:

```csharp
namespace LiveClassDemos.Constants
{
    public static class AppRoles
    {
        public const string Resident = "Resident";
        public const string Admin = "Admin";
        public const string Staff = "Staff";
    }
}
```

## Explain To Students

Do not write role names manually everywhere.

Bad:

```csharp
[Authorize(Roles = "admin")]
```

Good:

```csharp
[Authorize(Roles = AppRoles.Admin)]
```

This avoids spelling mistakes.

---

# 8. Create IdentitySeedData

Create file:

```text
Data/IdentitySeedData.cs
```

Add this code:

```csharp
using LiveClassDemos.Constants;
using LiveClassDemos.Models;
using Microsoft.AspNetCore.Identity;

namespace LiveClassDemos.Data
{
    public static class IdentitySeedData
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles =
            {
                AppRoles.Resident,
                AppRoles.Admin,
                AppRoles.Staff
            };

            foreach (var role in roles)
            {
                bool roleExists = await roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            await SeedDefaultAdminAsync(userManager);
        }

        private static async Task SeedDefaultAdminAsync(UserManager<ApplicationUser> userManager)
        {
            string adminEmail = "admin@test.com";
            string adminPassword = "Admin@123";

            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin != null)
            {
                return;
            }

            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Default Admin",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, AppRoles.Admin);
            }
        }
    }
}
```

## Explain To Students

This code does two things:

```text
1. Creates roles if they do not exist
2. Creates one default admin user
```

Admin login:

```text
Email: admin@test.com
Password: Admin@123
```

This is only for classroom/demo. For production, never hardcode admin password in code.

---

# 9. Call Seed Method In Program.cs

Open:

```text
Program.cs
```

Add namespace:

```csharp
using LiveClassDemos.Data;
```

Before:

```csharp
app.Run();
```

Add:

```csharp
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await IdentitySeedData.SeedRolesAndAdminAsync(services);
}
```

Final bottom part should look like this:

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await IdentitySeedData.SeedRolesAndAdminAsync(services);
}

app.Run();
```

---

# 10. Assign Resident Role During Registration

Open:

```text
Controllers/AccountController.cs
```

Add namespace:

```csharp
using LiveClassDemos.Constants;
```

Find this code in Register POST:

```csharp
if (result.Succeeded)
{
    TempData["SuccessMessage"] = "Registration successful. Please login.";
    return RedirectToAction(nameof(Login));
}
```

Replace it with:

```csharp
if (result.Succeeded)
{
    await _userManager.AddToRoleAsync(user, AppRoles.Resident);

    TempData["SuccessMessage"] = "Registration successful. Please login.";
    return RedirectToAction(nameof(Login));
}
```

## Explain To Students

Any normal public registration should become:

```text
Resident
```

Never allow the user to select:

```text
Admin
```

from a public registration form.

---

# 11. Create SecurityDemoController

Create file:

```text
Controllers/SecurityDemoController.cs
```

Add this code:

```csharp
using LiveClassDemos.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveClassDemos.Controllers
{
    public class SecurityDemoController : Controller
    {
        [Authorize]
        public IActionResult LoggedInOnly()
        {
            return View();
        }

        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult AdminOnly()
        {
            return View();
        }

        [Authorize(Roles = AppRoles.Resident)]
        public IActionResult ResidentOnly()
        {
            return View();
        }

        [Authorize(Roles = AppRoles.Staff)]
        public IActionResult StaffOnly()
        {
            return View();
        }

        [Authorize(Roles = AppRoles.Admin + "," + AppRoles.Staff)]
        public IActionResult AdminOrStaff()
        {
            return View();
        }
    }
}
```

## Explain To Students

```csharp
[Authorize]
```

means:

```text
Only logged-in user can access.
```

```csharp
[Authorize(Roles = AppRoles.Admin)]
```

means:

```text
Only Admin can access.
```

---

# 12. Create Views For SecurityDemo

Create folder:

```text
Views/SecurityDemo
```

## File 1: LoggedInOnly.cshtml

```text
Views/SecurityDemo/LoggedInOnly.cshtml
```

```cshtml
<h1>Logged In Only Page</h1>

<p>If you can see this page, you are logged in.</p>

<p>
    <strong>User:</strong> @User.Identity?.Name
</p>
```

## File 2: AdminOnly.cshtml

```text
Views/SecurityDemo/AdminOnly.cshtml
```

```cshtml
<h1>Admin Only Page</h1>

<p>Only users with Admin role can see this page.</p>

<p>
    <strong>User:</strong> @User.Identity?.Name
</p>
```

## File 3: ResidentOnly.cshtml

```text
Views/SecurityDemo/ResidentOnly.cshtml
```

```cshtml
<h1>Resident Only Page</h1>

<p>Only users with Resident role can see this page.</p>

<p>
    <strong>User:</strong> @User.Identity?.Name
</p>
```

## File 4: StaffOnly.cshtml

```text
Views/SecurityDemo/StaffOnly.cshtml
```

```cshtml
<h1>Staff Only Page</h1>

<p>Only users with Staff role can see this page.</p>

<p>
    <strong>User:</strong> @User.Identity?.Name
</p>
```

## File 5: AdminOrStaff.cshtml

```text
Views/SecurityDemo/AdminOrStaff.cshtml
```

```cshtml
<h1>Admin Or Staff Page</h1>

<p>Admin and Staff users can see this page.</p>

<p>
    <strong>User:</strong> @User.Identity?.Name
</p>
```

---

# 13. Configure Access Denied Page

Open:

```text
Program.cs
```

After Identity configuration, add:

```csharp
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});
```

Example placement:

```csharp
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});
```

---

# 14. Add AccessDenied Action

Open:

```text
Controllers/AccountController.cs
```

Add this GET action:

```csharp
[HttpGet]
public IActionResult AccessDenied()
{
    return View();
}
```

---

# 15. Create AccessDenied View

Create file:

```text
Views/Account/AccessDenied.cshtml
```

Add this code:

```cshtml
<h1 style="color:red;">Access Denied</h1>

<p>You do not have permission to access this page.</p>

<a asp-controller="Home" asp-action="Index">Go To Home</a>
```

---

# 16. Add Menu Links In Layout

Open:

```text
Views/Shared/_Layout.cshtml
```

Inside navbar, add:

```cshtml
<li class="nav-item">
    <a class="nav-link text-dark" asp-controller="SecurityDemo" asp-action="LoggedInOnly">
        Logged In Only
    </a>
</li>

<li class="nav-item">
    <a class="nav-link text-dark" asp-controller="SecurityDemo" asp-action="AdminOnly">
        Admin Only
    </a>
</li>

<li class="nav-item">
    <a class="nav-link text-dark" asp-controller="SecurityDemo" asp-action="ResidentOnly">
        Resident Only
    </a>
</li>
```

Optional UI hiding:

```cshtml
@if (User.IsInRole("Admin"))
{
    <li class="nav-item">
        <a class="nav-link text-dark" asp-controller="SecurityDemo" asp-action="AdminOnly">
            Admin Only
        </a>
    </li>
}
```

## Important Teaching Point

UI hiding is good for user experience.

But real security is here:

```csharp
[Authorize(Roles = AppRoles.Admin)]
```

Never depend only on hiding links.

---

# 17. Run Migration If Needed

If Identity tables are not created yet, run:

```bash
dotnet ef migrations add AddIdentityAndRoles
dotnet ef database update
```

If migration already exists, only run:

```bash
dotnet ef database update
```

---

# 18. Run The Application

Run:

```bash
dotnet run
```

---

# 19. Testing Steps

## Test 1: Open Admin Page Without Login

Open:

```text
/SecurityDemo/AdminOnly
```

Expected:

```text
Redirect to Login page
```

## Test 2: Login As Default Admin

Login with:

```text
Email: admin@test.com
Password: Admin@123
```

Open:

```text
/SecurityDemo/AdminOnly
```

Expected:

```text
Admin page opens successfully
```

## Test 3: Register Normal User

Register:

```text
Full Name: Deepak Resident
Email: resident@test.com
Password: Test@123
Confirm Password: Test@123
```

This user should automatically get:

```text
Resident role
```

Login as:

```text
resident@test.com
Test@123
```

Open:

```text
/SecurityDemo/ResidentOnly
```

Expected:

```text
Resident page opens successfully
```

Open:

```text
/SecurityDemo/AdminOnly
```

Expected:

```text
Access Denied
```

## Test 4: Logged-In Only Page

Login as any user and open:

```text
/SecurityDemo/LoggedInOnly
```

Expected:

```text
Page opens for any logged-in user
```

Logout and try again.

Expected:

```text
Redirect to Login
```

---

# 20. Claims Awareness Only

Do not code claims deeply in this 1-hour practical.

Just explain:

```text
Role = user group
Claim = information about user
```

Examples:

```text
Role: Admin
Claim: Department = Electrical
Claim: FlatNumber = A-101
```

Small demo line:

```csharp
var email = User.Identity?.Name;
```

You can say:

```text
Later, claims can store small user information like FlatNumber or Department.
```

---

# 21. Policy Awareness Only

Do not implement complex policy in this 1-hour beginner practical.

Explain:

```text
Policy is a named authorization rule.
```

Example:

```text
Policy name: CanAssignComplaint
Rule: User must be Admin
```

Awareness code:

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanAssignComplaint", policy =>
        policy.RequireRole(AppRoles.Admin));
});
```

Usage:

```csharp
[Authorize(Policy = "CanAssignComplaint")]
```

For beginners, role-based authorization is enough for live coding.

---

# 22. Security Hardening Concepts To Explain

## 1. UI Hiding Is Not Security

Bad thinking:

```text
I hide Admin menu, so Resident cannot access Admin page.
```

Correct thinking:

```text
Hide menu for UX, but protect controller with [Authorize].
```

## 2. Use POST For Data Changes

Actions like logout, delete, update, assign should use POST.

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Logout()
{
    await _signInManager.SignOutAsync();
    return RedirectToAction("Index", "Home");
}
```

## 3. Use Anti-Forgery Token

For POST actions:

```csharp
[ValidateAntiForgeryToken]
```

## 4. Avoid Overposting

Do not allow user to post sensitive fields like:

```text
Role
Status
AssignedStaffId
IsAdmin
```

Server should decide these values.

## 5. Secure File Upload

For file upload:

```text
Check extension
Check size
Generate new file name
Do not trust original file name
```

---

# 23. Common Errors And Fixes

| Error | Reason | Fix |
|---|---|---|
| RoleManager not injected | Identity role support missing | Use `AddIdentity<ApplicationUser, IdentityRole>()` |
| User cannot access Admin page | User does not have Admin role | Seed admin and assign role |
| AccessDenied page not opening | AccessDeniedPath not configured | Configure cookie path |
| Resident can access admin URL | Controller not protected | Add `[Authorize(Roles = AppRoles.Admin)]` |
| Role spelling mismatch | Hardcoded role typo | Use AppRoles constants |
| Seed code runs before DB exists | Migration not applied | Run database update |
| Login works but role check fails | Role not assigned | Use `AddToRoleAsync` |

---

# 24. One-Minute Revision

| Concept | Meaning |
|---|---|
| Role | User group such as Admin, Resident, Staff |
| RoleManager | Creates and manages roles |
| UserManager.AddToRoleAsync | Assigns user to role |
| `[Authorize]` | Protects page/action |
| `[Authorize(Roles = "...")]` | Allows only selected role |
| Claim | Small information about user |
| Policy | Named authorization rule |
| Access Denied | Page shown when logged-in user lacks permission |
| Anti-forgery | Protects POST forms |
| Overposting | User submits extra fields that should not update |

---

# 25. Interview Lines

## Role

A role is a named permission group such as Admin, Resident, or Staff.

## RoleManager

RoleManager is an Identity service used to create and manage roles.

## Role Seeding

Role seeding creates required roles automatically when the application starts.

## Authorize Attribute

The Authorize attribute protects controllers and actions by requiring login, roles, or policies.

## Claims

A claim is a small piece of information about a user, such as FlatNumber or Department.

## Policy

A policy is a named authorization rule that can use roles, claims, or custom logic.

## UI Hiding

Hiding a button or menu is not security. Server-side authorization is required.

## Overposting

Overposting happens when a user submits extra fields that should not be updated.

---

# 26. Mini Assignment

Ask students to complete these tasks:

```text
1. Create a Staff user manually in seed code
2. Assign Staff role to that user
3. Login as Staff
4. Test /SecurityDemo/StaffOnly
5. Test that Staff cannot open /SecurityDemo/AdminOnly
6. Add one policy named CanViewAdminReports
7. Apply policy on one action
8. Create a short note: why UI hiding is not enough
```

---

# 27. Final Outcome

After this 1-hour practical, students should be able to say:

```text
I added roles to ASP.NET Core Identity.

I created AppRoles constants, seeded Resident, Admin, and Staff roles, created a default Admin user, and assigned Resident role during public registration.

I protected pages using [Authorize] and role-based authorization. I also configured Access Denied handling and understood why security must be enforced on the server, not only in the UI.
```
