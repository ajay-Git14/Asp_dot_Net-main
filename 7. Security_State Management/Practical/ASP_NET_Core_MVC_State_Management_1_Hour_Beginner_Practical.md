# ASP.NET Core MVC: 1-Hour Beginner Friendly Practical
## Topic: State Management, Cookies, Session, ViewData, ViewBag, and TempData

> **Live Coding Style:** This practical is designed for **1 hour only**.  
> Keep theory short. Code in front of students step by step.

---

## 1. Session Goal

By the end of this practical, students will understand:

- Why HTTP does not remember previous requests
- What state management means
- How to use `ViewBag`
- How to use `ViewData`
- How to use `TempData`
- How to write and read cookies
- How to configure and use session
- When to use each technique in real projects

---

## 2. Simple Story For Students

Imagine a student registration website.

The student submits a form.

After submit, we want to show:

```text
Student registered successfully.
```

We also want to remember:

```text
Last selected course
Page title
Temporary dashboard filter
```

ASP.NET Core MVC gives us different state management options for these needs.

---

## 3. What We Will Build In 1 Hour

We will add these features to an existing MVC project:

```text
1. Show page title using ViewBag
2. Show module name using ViewData
3. Show success message using TempData
4. Save last selected course in Cookie
5. Save temporary student data in Session
6. Display all values on a Dashboard page
```

---

## 4. 1-Hour Live Coding Flow

| Time | Task |
|---|---|
| 0-5 min | Explain HTTP is stateless |
| 5-12 min | Add ViewBag and ViewData |
| 12-25 min | Add TempData success message |
| 25-38 min | Add Cookie for last selected course |
| 38-50 min | Configure and use Session |
| 50-58 min | Test complete flow |
| 58-60 min | Revision and interview lines |

---

# Part A: Quick Concept

## What Is Stateless HTTP?

HTTP does not automatically remember old requests.

Example:

```text
Request 1: Open student form
Request 2: Submit student form
Request 3: Open dashboard
```

By default, each request is separate.

So if we want to remember something, we need **state management**.

---

## State Management Options

| Technique | Use Case |
|---|---|
| ViewBag | Send small data from controller to view in same request |
| ViewData | Send small data from controller to view in same request |
| TempData | Send data after redirect, usually success/error message |
| Cookie | Store small non-sensitive data in browser |
| Session | Store temporary user-specific data on server |

---

# Part B: Configure Session

Open:

```text
Program.cs
```

Add session services before `builder.Build()`:

```csharp
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
```

Now add middleware after `app.UseRouting();`:

```csharp
app.UseSession();
```

Your middleware order should look like this:

```csharp
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

---

## Explain To Students

Session will not work unless we configure:

```text
AddDistributedMemoryCache()
AddSession()
UseSession()
```

---

# Part C: Update StudentController

Open:

```text
Controllers/StudentController.cs
```

Use this beginner-friendly version.

```csharp
using LiveClassDemos.Models;
using Microsoft.AspNetCore.Mvc;

namespace LiveClassDemos.Controllers
{
    public class StudentController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            // ViewBag works only for current request
            ViewBag.PageTitle = "Student Registration Form";

            // ViewData also works only for current request
            ViewData["ModuleName"] = "ASP.NET Core MVC State Management";

            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PageTitle = "Student Registration Form";
                ViewData["ModuleName"] = "ASP.NET Core MVC State Management";
                return View(student);
            }

            // TempData survives redirect
            TempData["SuccessMessage"] = "Student registered successfully.";

            // Cookie stores small non-sensitive data in browser
            Response.Cookies.Append(
                "LastSelectedCourse",
                student.Course ?? "",
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict
                });

            // Session stores temporary server-side user-specific data
            HttpContext.Session.SetString("StudentName", student.Name ?? "");
            HttpContext.Session.SetString("SelectedCourse", student.Course ?? "");

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            // Read cookie
            string lastSelectedCourse = Request.Cookies["LastSelectedCourse"] ?? "No course cookie found";

            // Read session
            string studentName = HttpContext.Session.GetString("StudentName") ?? "No student session found";
            string selectedCourse = HttpContext.Session.GetString("SelectedCourse") ?? "No course session found";

            ViewBag.LastSelectedCourse = lastSelectedCourse;
            ViewBag.StudentName = studentName;
            ViewBag.SelectedCourse = selectedCourse;

            return View();
        }

        [HttpGet]
        public IActionResult ClearState()
        {
            // Delete cookie
            Response.Cookies.Delete("LastSelectedCourse");

            // Clear session values
            HttpContext.Session.Remove("StudentName");
            HttpContext.Session.Remove("SelectedCourse");

            TempData["SuccessMessage"] = "Cookie and session data cleared.";

            return RedirectToAction("Dashboard");
        }
    }
}
```

---

# Part D: Update Create View

Open:

```text
Views/Student/Create.cshtml
```

Use this view.

```cshtml
@model LiveClassDemos.Models.Student

<h1>@ViewBag.PageTitle</h1>

<p>
    <strong>Module:</strong> @ViewData["ModuleName"]
</p>

<form asp-action="Create" method="post">

    <div>
        <label asp-for="Name"></label>
        <input asp-for="Name" />
        <span asp-validation-for="Name" style="color:red"></span>
    </div>

    <br />

    <div>
        <label asp-for="Email"></label>
        <input asp-for="Email" />
        <span asp-validation-for="Email" style="color:red"></span>
    </div>

    <br />

    <div>
        <label asp-for="Course"></label>
        <select asp-for="Course">
            <option value="">-- Select Course --</option>
            <option value="ASP.NET Core MVC">ASP.NET Core MVC</option>
            <option value="React">React</option>
            <option value="PHP">PHP</option>
        </select>
        <span asp-validation-for="Course" style="color:red"></span>
    </div>

    <br />

    <div>
        <label asp-for="Age"></label>
        <input asp-for="Age" />
        <span asp-validation-for="Age" style="color:red"></span>
    </div>

    <br />

    <button type="submit">Submit</button>

</form>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

---

## Explain To Students

These two lines come from controller:

```cshtml
@ViewBag.PageTitle
@ViewData["ModuleName"]
```

They work only in the current request.

They are not good for redirect messages.

---

# Part E: Create Dashboard View

Create folder if it does not exist:

```text
Views/Student
```

Create file:

```text
Views/Student/Dashboard.cshtml
```

Add this code:

```cshtml
<h1>Student Dashboard</h1>

@if (TempData["SuccessMessage"] != null)
{
    <div style="color:green; font-weight:bold;">
        @TempData["SuccessMessage"]
    </div>
}

<hr />

<h3>Cookie Value</h3>

<p>
    <strong>Last Selected Course From Cookie:</strong>
    @ViewBag.LastSelectedCourse
</p>

<hr />

<h3>Session Values</h3>

<p>
    <strong>Student Name From Session:</strong>
    @ViewBag.StudentName
</p>

<p>
    <strong>Selected Course From Session:</strong>
    @ViewBag.SelectedCourse
</p>

<hr />

<a asp-action="Create">Register Another Student</a>
<br />
<a asp-action="ClearState">Clear Cookie And Session</a>
```

---

# Part F: Test The Practical

## Test 1: Open Create Page

Open:

```text
/Student/Create
```

Expected:

```text
Page title should come from ViewBag.
Module name should come from ViewData.
```

---

## Test 2: Submit Form

Enter:

```text
Name: Deepak Kumar
Email: deepak@test.com
Course: ASP.NET Core MVC
Age: 29
```

Click Submit.

Expected:

```text
Redirect to Dashboard page
Show success message using TempData
Show selected course from Cookie
Show student name and course from Session
```

---

## Test 3: Refresh Dashboard

Refresh the Dashboard page.

Expected:

```text
TempData success message may disappear after first read.
Cookie value should still show.
Session value should still show.
```

---

## Test 4: Clear State

Click:

```text
Clear Cookie And Session
```

Expected:

```text
Cookie value removed
Session values removed
Success message shown using TempData
```

---

# Part G: What To Explain While Coding

## ViewBag

Use this line in controller:

```csharp
ViewBag.PageTitle = "Student Registration Form";
```

Use this line in view:

```cshtml
@ViewBag.PageTitle
```

Explain:

```text
ViewBag passes data from controller to view in the same request.
```

---

## ViewData

Use this line in controller:

```csharp
ViewData["ModuleName"] = "ASP.NET Core MVC State Management";
```

Use this line in view:

```cshtml
@ViewData["ModuleName"]
```

Explain:

```text
ViewData also passes data from controller to view in the same request.
It uses dictionary syntax.
```

---

## TempData

Use this line before redirect:

```csharp
TempData["SuccessMessage"] = "Student registered successfully.";
```

Use this in Dashboard view:

```cshtml
@TempData["SuccessMessage"]
```

Explain:

```text
TempData is used when we want to show a message after redirect.
```

---

## Cookie

Write cookie:

```csharp
Response.Cookies.Append("LastSelectedCourse", student.Course);
```

Read cookie:

```csharp
Request.Cookies["LastSelectedCourse"];
```

Explain:

```text
Cookie is stored in browser.
Use it only for small non-sensitive data.
```

---

## Session

Write session:

```csharp
HttpContext.Session.SetString("StudentName", student.Name);
```

Read session:

```csharp
HttpContext.Session.GetString("StudentName");
```

Explain:

```text
Session stores temporary user-specific data on server.
Browser only stores session id.
```

---

# Part H: Common Mistakes

| Mistake | Reason | Fix |
|---|---|---|
| Session not working | Session not configured | Add `AddSession()` and `UseSession()` |
| ViewBag message lost after redirect | ViewBag works only same request | Use TempData |
| Cookie value not updating | Wrong key or browser cache confusion | Check cookie name carefully |
| Storing password in cookie | Security risk | Never store sensitive data in cookie |
| Using session as database | Bad design | Store permanent data in database |
| Forgetting `return View(student)` when invalid | User input lost | Return same model |

---

# Part I: One-Minute Revision

Use this table at the end:

| Concept | Lifetime | Best Use |
|---|---|---|
| ViewBag | Same request | Page title, small labels |
| ViewData | Same request | Page title, small labels |
| TempData | Next request / after redirect | Success or error message |
| Cookie | Until expiry/browser delete | Small non-sensitive preference |
| Session | Until timeout | Temporary user-specific data |

---

# Part J: Interview Lines

## HTTP Stateless

HTTP is stateless because each request is independent and the server does not automatically remember previous requests.

## ViewBag

ViewBag is used to pass small data from controller to view in the same request.

## ViewData

ViewData is a dictionary used to pass small data from controller to view in the same request.

## TempData

TempData is used to pass short-lived data between requests, especially after redirects.

## Cookie

Cookie stores small key-value data in the user's browser and is sent back to the server with future requests.

## Session

Session stores temporary user-specific data on the server, while the browser stores only a session identifier.

---

# Part K: Mini Assignment

Ask students to add:

```text
1. Store Age in Session
2. Show Age on Dashboard
3. Store LastVisitedPage in Cookie
4. Show LastVisitedPage on Dashboard
5. Add ClearState button to remove both
```

---

# Final Outcome

After this 1-hour practical, students should be able to say:

```text
I implemented state management in ASP.NET Core MVC.

I used ViewBag and ViewData for current-request data, TempData for success messages after redirect, Cookie for small non-sensitive browser data, and Session for temporary server-side user data.

I also configured session in Program.cs and tested cookie/session values on a dashboard page.
```
