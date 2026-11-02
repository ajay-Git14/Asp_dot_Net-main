ASP.NET Core MVC Project: Form and Model Binding

Goal

Create a simple MVC project where the user fills a form, submits it, and the controller receives the form data using model binding.

Example project:

Student Registration Form

No database.
No API.
Only MVC project.


---

1. Create MVC Project

dotnet new mvc -n ModelBindingDemo
cd ModelBindingDemo
dotnet run

Open:

https://localhost:xxxx


---

2. Final Project Structure

ModelBindingDemo
│
├── Controllers
│   └── StudentController.cs
│
├── Models
│   └── Student.cs
│
├── Views
│   └── Student
│       ├── Create.cshtml
│       └── Success.cshtml
│
├── Program.cs
└── ...


---

3. Model

Create this file:

Models/Student.cs

using System.ComponentModel.DataAnnotations;

namespace ModelBindingDemo.Models
{
    public class Student
    {
        [Required(ErrorMessage = "Student name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course is required")]
        public string Course { get; set; } = string.Empty;

        [Range(18, 60, ErrorMessage = "Age must be between 18 and 60")]
        public int Age { get; set; }
    }
}

Beginner Understanding

This class represents form fields.

Name   → textbox
Email  → textbox
Course → dropdown
Age    → number input

Validation attributes like [Required], [EmailAddress], and [Range] allow MVC to validate user input on the server side, and MVC can also support client-side validation when validation scripts are used. 


---

4. Controller

Create this file:

Controllers/StudentController.cs

using Microsoft.AspNetCore.Mvc;
using ModelBindingDemo.Models;

namespace ModelBindingDemo.Controllers
{
    public class StudentController : Controller
    {
        // This action shows the empty form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // This action receives submitted form data
        [HttpPost]
        public IActionResult Create(Student student)
        {
            // ModelState checks validation rules from Student model
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            // Passing submitted data to Success view
            return View("Success", student);
        }
    }
}

Beginner Understanding

This method shows the form:

[HttpGet]
public IActionResult Create()

This method receives form data:

[HttpPost]
public IActionResult Create(Student student)

The important part is this:

Student student

MVC automatically fills this object from the form fields.

This is called model binding.


---

5. Create Form View

Create folder:

Views/Student

Create file:

Views/Student/Create.cshtml

@model ModelBindingDemo.Models.Student

@{
    ViewData["Title"] = "Student Registration";
}

<h2>Student Registration Form</h2>

<form asp-controller="Student" asp-action="Create" method="post">

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
            <option value="C#">C#</option>
            <option value="SQL Server">SQL Server</option>
        </select>
        <span asp-validation-for="Course" style="color:red"></span>
    </div>

    <br />

    <div>
        <label asp-for="Age"></label>
        <input asp-for="Age" type="number" />
        <span asp-validation-for="Age" style="color:red"></span>
    </div>

    <br />

    <button type="submit">Submit</button>

</form>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}

Beginner Understanding

This line connects the view with the model:

@model ModelBindingDemo.Models.Student

This input:

<input asp-for="Name" />

generates an HTML input connected with:

public string Name { get; set; }

So when the form is submitted:

Name textbox value → Student.Name
Email textbox value → Student.Email
Course dropdown value → Student.Course
Age number value → Student.Age


---

6. Success View

Create file:

Views/Student/Success.cshtml

@model ModelBindingDemo.Models.Student

@{
    ViewData["Title"] = "Registration Success";
}

<h2>Registration Successful</h2>

<p>Below data came from the submitted form using model binding.</p>

<table border="1" cellpadding="10">
    <tr>
        <th>Name</th>
        <td>@Model.Name</td>
    </tr>

    <tr>
        <th>Email</th>
        <td>@Model.Email</td>
    </tr>

    <tr>
        <th>Course</th>
        <td>@Model.Course</td>
    </tr>

    <tr>
        <th>Age</th>
        <td>@Model.Age</td>
    </tr>
</table>

<br />

<a asp-controller="Student" asp-action="Create">Register Another Student</a>


---

7. Run the Project

dotnet run

Open this URL:

https://localhost:xxxx/Student/Create

Fill the form and submit.


---

8. What Happens Internally?

User fills form
        ↓
Clicks Submit
        ↓
Browser sends form data using POST
        ↓
StudentController Create(Student student) receives data
        ↓
MVC maps form fields to Student object
        ↓
Validation is checked using ModelState
        ↓
If valid → Success page
If invalid → Same form with error messages


---

9. Main Learning Point

Model binding means:

HTML form data automatically becomes C# object data.

Example:

<input name="Name" value="Rahul" />

becomes:

student.Name = "Rahul";

You do not need to manually read form data like this:

Request.Form["Name"]

MVC does this automatically.


---

10. Interview Line

Model Binding in ASP.NET Core MVC is a process where MVC automatically maps incoming request data such as form values, route values, and query strings to action method parameters or model objects. MVC model binding supports values from form data, route data, query strings, headers, and other request sources. 


---

11. Common Beginner Mistakes

Mistake 1: Form field name does not match model property

Wrong:

<input name="StudentName" />

Model:

public string Name { get; set; }

Correct:

<input asp-for="Name" />


---

Mistake 2: Missing [HttpPost]

Wrong:

public IActionResult Create(Student student)

Correct:

[HttpPost]
public IActionResult Create(Student student)


---

Mistake 3: Forgetting validation check

Wrong:

return View("Success", student);

Correct:

if (!ModelState.IsValid)
{
    return View(student);
}

return View("Success", student);


---

Simple Summary

Model       → Student.cs
View        → Create.cshtml form
Controller  → StudentController receives data
Binding     → MVC fills Student object automatically
Validation  → ModelState checks rules
Result      → Success page shows submitted data
