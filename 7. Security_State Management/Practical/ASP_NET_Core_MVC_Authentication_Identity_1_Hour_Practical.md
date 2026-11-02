# ASP.NET Core MVC: 1-Hour Beginner Friendly Practical
## Topic: Authentication, Authorization, and ASP.NET Core Identity

> **Live Coding Duration:** 1 Hour  
> **Style:** Beginner friendly, practical-first, less theory, code in front of students.

---

# 1. Practical Goal

In this 1-hour practical, students will learn how login security works in ASP.NET Core MVC using **ASP.NET Core Identity**.

By the end, students should understand:

```text
Authentication = Who are you?
Authorization = What are you allowed to access?
Identity = ASP.NET Core built-in user management system
UserManager = Creates and manages users
SignInManager = Login and logout
Authentication Cookie = Keeps user logged in after login
```

---

# 2. What We Will Build

We will add a basic login system to an existing ASP.NET Core MVC project.

We will create:

```text
1. ApplicationUser
2. ApplicationDbContext
3. RegisterViewModel
4. LoginViewModel
5. AccountController
6. Register view
7. Login view
8. Logout button
9. Secure page using [Authorize]
```

---

# 3. Important Note For 1-Hour Session

Do not try to cover all Identity features in one hour.

For this session, focus only on:

```text
Register
Login
Logout
Protect page with [Authorize]
```

Do not cover roles, claims, password reset, email confirmation, or admin/staff role management in this hour.

---

# 4. 1-Hour Live Coding Flow

| Time | Task |
|---|---|
| 0-5 min | Explain authentication vs authorization |
| 5-10 min | Install required packages |
| 10-18 min | Create ApplicationUser and ApplicationDbContext |
| 18-25 min | Configure Identity in Program.cs |
| 25-35 min | Create Register and Login ViewModels |
| 35-48 min | Create AccountController |
| 48-55 min | Create Register/Login views |
| 55-60 min | Run, test, and protect one page with `[Authorize]` |

---

# 5. Beginner Explanation

## Authentication

Authentication means:

```text
Who are you?
```

Example:

```text
User enters email and password.
System verifies the user.
```

## Authorization

Authorization means:

```text
What are you allowed to access?
```

Example:

```text
Only logged-in users can open Student Dashboard.
Only Admin can open Admin Dashboard.
```

## Identity

ASP.NET Core Identity is a ready-made system for:

```text
Register
Login
Logout
Password hashing
Authentication cookie
User tables
Roles and claims later
```

---

# 6. Install Required Packages

Open terminal in project folder.

Run these commands:

```bash
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

If you already installed these packages, skip this step.

---

# 7. Add Connection String

Open:

```text
appsettings.json
```

Add this connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=LiveClassDemosIdentityDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

If your file already has other settings, only add the `ConnectionStrings` part carefully.

---

# 8. Create ApplicationUser

Create folder:

```text
Models
```

Create file:

```text
Models/ApplicationUser.cs
```

Add this code:

```csharp
using Microsoft.AspNetCore.Identity;

namespace LiveClassDemos.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
```

## Explain To Students

`IdentityUser` already has:

```text
Id
UserName
Email
PasswordHash
PhoneNumber
```

We added:

```text
FullName
```

because our application needs student/user name.

---

# 9. Create ApplicationDbContext

Create folder:

```text
Data
```

Create file:

```text
Data/ApplicationDbContext.cs
```

Add this code:

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
    }
}
```

## Explain To Students

Normal `DbContext` is for application tables.

`IdentityDbContext` is for Identity tables like:

```text
AspNetUsers
AspNetRoles
AspNetUserRoles
AspNetUserClaims
AspNetUserTokens
```

---

# 10. Configure Identity In Program.cs

Open:

```text
Program.cs
```

Add these namespaces at the top:

```csharp
using LiveClassDemos.Data;
using LiveClassDemos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
```

Now add this before:

```csharp
var app = builder.Build();
```

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
```

Now make sure middleware order is like this:

```csharp
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
```

Full important part:

```csharp
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

## Explain To Students

This order is very important:

```text
UseAuthentication first
UseAuthorization second
```

Authentication loads the logged-in user.

Authorization checks what the user can access.

---

# 11. Create RegisterViewModel

Create folder:

```text
ViewModels
```

Create file:

```text
ViewModels/RegisterViewModel.cs
```

Add this code:

```csharp
using System.ComponentModel.DataAnnotations;

namespace LiveClassDemos.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full name is required.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
```

---

# 12. Create LoginViewModel

Create file:

```text
ViewModels/LoginViewModel.cs
```

Add this code:

```csharp
using System.ComponentModel.DataAnnotations;

namespace LiveClassDemos.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
```

---

# 13. Create AccountController

Create file:

```text
Controllers/AccountController.cs
```

Add this code:

```csharp
using LiveClassDemos.Models;
using LiveClassDemos.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LiveClassDemos.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Registration successful. Please login.";
                return RedirectToAction(nameof(Login));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(model.ReturnUrl) &&
                    Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("SecurePage", "Student");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            TempData["SuccessMessage"] = "You have logged out successfully.";

            return RedirectToAction("Index", "Home");
        }
    }
}
```

---

## Explain To Students

`UserManager` is used for:

```text
Creating user
Finding user
Managing user
```

`SignInManager` is used for:

```text
Login
Logout
Remember me
Authentication cookie
```

---

# 14. Create Register View

Create folder:

```text
Views/Account
```

Create file:

```text
Views/Account/Register.cshtml
```

Add this code:

```cshtml
@model LiveClassDemos.ViewModels.RegisterViewModel

<h1>Register</h1>

<form asp-action="Register" method="post">

    <div asp-validation-summary="All" style="color:red"></div>

    <div>
        <label asp-for="FullName"></label>
        <input asp-for="FullName" />
        <span asp-validation-for="FullName" style="color:red"></span>
    </div>

    <br />

    <div>
        <label asp-for="Email"></label>
        <input asp-for="Email" />
        <span asp-validation-for="Email" style="color:red"></span>
    </div>

    <br />

    <div>
        <label asp-for="Password"></label>
        <input asp-for="Password" />
        <span asp-validation-for="Password" style="color:red"></span>
    </div>

    <br />

    <div>
        <label asp-for="ConfirmPassword"></label>
        <input asp-for="ConfirmPassword" />
        <span asp-validation-for="ConfirmPassword" style="color:red"></span>
    </div>

    <br />

    <button type="submit">Register</button>

</form>

<p>
    Already registered?
    <a asp-action="Login">Login here</a>
</p>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

---

# 15. Create Login View

Create file:

```text
Views/Account/Login.cshtml
```

Add this code:

```cshtml
@model LiveClassDemos.ViewModels.LoginViewModel

<h1>Login</h1>

@if (TempData["SuccessMessage"] != null)
{
    <div style="color:green; font-weight:bold;">
        @TempData["SuccessMessage"]
    </div>
}

<form asp-action="Login" method="post">

    <input type="hidden" asp-for="ReturnUrl" />

    <div asp-validation-summary="All" style="color:red"></div>

    <div>
        <label asp-for="Email"></label>
        <input asp-for="Email" />
        <span asp-validation-for="Email" style="color:red"></span>
    </div>

    <br />

    <div>
        <label asp-for="Password"></label>
        <input asp-for="Password" />
        <span asp-validation-for="Password" style="color:red"></span>
    </div>

    <br />

    <div>
        <input asp-for="RememberMe" />
        <label asp-for="RememberMe"></label>
    </div>

    <br />

    <button type="submit">Login</button>

</form>

<p>
    New user?
    <a asp-action="Register">Register here</a>
</p>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

---

# 16. Add Secure Page In StudentController

Open:

```text
Controllers/StudentController.cs
```

Add namespace:

```csharp
using Microsoft.AspNetCore.Authorization;
```

Add this action:

```csharp
[Authorize]
public IActionResult SecurePage()
{
    return View();
}
```

## Explain To Students

`[Authorize]` means:

```text
Only logged-in users can access this page.
```

If user is not logged in, ASP.NET Core redirects to login page.

---

# 17. Create SecurePage View

Create file:

```text
Views/Student/SecurePage.cshtml
```

Add this code:

```cshtml
<h1>Secure Student Page</h1>

<p>You are logged in successfully.</p>

<p>
    <strong>User Email:</strong> @User.Identity?.Name
</p>

<form asp-controller="Account" asp-action="Logout" method="post">
    <button type="submit">Logout</button>
</form>
```

---

# 18. Add Login And Register Links In Layout

Open:

```text
Views/Shared/_Layout.cshtml
```

Inside navbar, add simple links:

```cshtml
<li class="nav-item">
    <a class="nav-link text-dark" asp-controller="Account" asp-action="Register">Register</a>
</li>

<li class="nav-item">
    <a class="nav-link text-dark" asp-controller="Account" asp-action="Login">Login</a>
</li>

<li class="nav-item">
    <a class="nav-link text-dark" asp-controller="Student" asp-action="SecurePage">Secure Page</a>
</li>
```

For logout, use a form because logout should be POST:

```cshtml
@if (User.Identity != null && User.Identity.IsAuthenticated)
{
    <li class="nav-item">
        <form asp-controller="Account" asp-action="Logout" method="post">
            <button type="submit" class="btn btn-link nav-link text-dark">
                Logout
            </button>
        </form>
    </li>
}
```

---

# 19. Create Database Migration

Run these commands:

```bash
dotnet ef migrations add AddIdentityTables
dotnet ef database update
```

If `dotnet ef` command is not found, install EF tool:

```bash
dotnet tool install --global dotnet-ef
```

Then run again:

```bash
dotnet ef migrations add AddIdentityTables
dotnet ef database update
```

---

# 20. Run The Project

Run:

```bash
dotnet run
```

Open:

```text
/Account/Register
```

Register a user:

```text
Full Name: Deepak Kumar
Email: deepak@test.com
Password: Test@123
Confirm Password: Test@123
```

Then login with the same email and password.

---

# 21. Test The Flow

## Test 1: Register

Open:

```text
/Account/Register
```

Expected:

```text
User registration successful.
Redirect to Login page.
```

---

## Test 2: Login

Open:

```text
/Account/Login
```

Expected:

```text
Login successful.
Redirect to Secure Student Page.
```

---

## Test 3: Secure Page Without Login

Logout first.

Then open:

```text
/Student/SecurePage
```

Expected:

```text
Application redirects to Login page.
```

This proves `[Authorize]` is working.

---

## Test 4: Logout

Click Logout.

Expected:

```text
User is logged out.
Secure page is no longer accessible.
```

---

# 22. Password Rule Note

ASP.NET Core Identity has default password rules.

For testing, use a password like:

```text
Test@123
```

This usually satisfies:

```text
Uppercase letter
Lowercase letter
Number
Special character
Minimum length
```

---

# 23. Common Mistakes And Fixes

| Mistake | Problem | Fix |
|---|---|---|
| `UseAuthentication()` missing | Login does not behave correctly | Add before `UseAuthorization()` |
| Wrong middleware order | Authorization cannot find user | Use Authentication before Authorization |
| Migration not created | Identity tables missing | Run migration and database update |
| Password too simple | Identity rejects registration | Use `Test@123` |
| Not checking `result.Succeeded` | Identity errors ignored | Add errors to ModelState |
| Logout using GET link | Not recommended | Use POST form |
| Manually storing password | Security risk | Let Identity hash password |

---

# 24. One-Minute Revision

| Concept | Meaning |
|---|---|
| Authentication | Verifies who the user is |
| Authorization | Checks what user can access |
| Identity | ASP.NET Core user management system |
| ApplicationUser | Custom user class |
| IdentityDbContext | DbContext with Identity tables |
| UserManager | Creates/manages users |
| SignInManager | Handles login/logout |
| Authentication Cookie | Keeps user logged in |
| `[Authorize]` | Protects controller/action |

---

# 25. Interview Lines

## Authentication

Authentication verifies who the user is.

## Authorization

Authorization checks what the authenticated user is allowed to access.

## ASP.NET Core Identity

ASP.NET Core Identity is the built-in system for registration, login, logout, password hashing, roles, claims, and authentication cookie management.

## ApplicationUser

ApplicationUser extends IdentityUser and allows us to add project-specific user fields.

## UserManager

UserManager creates and manages users.

## SignInManager

SignInManager handles login, logout, and authentication cookie creation.

## Authentication Cookie

An authentication cookie is created after successful login and helps ASP.NET Core recognize the logged-in user on future requests.

---

# 26. Mini Assignment

Ask students to do these small changes:

```text
1. Add PhoneNumber field in RegisterViewModel
2. Save PhoneNumber in ApplicationUser
3. Show logged-in email on SecurePage
4. Add [Authorize] on Student Create action
5. Test that anonymous user cannot open Student Create page
```

---

# 27. Final Outcome

After this practical, students should be able to say:

```text
I added ASP.NET Core Identity authentication to an MVC project.

I created ApplicationUser and ApplicationDbContext, configured Identity in Program.cs, created Register and Login ViewModels, and implemented AccountController with Register, Login, and Logout actions.

I also protected one page using [Authorize] and tested that only logged-in users can access it.
```
