# ASP.NET Core MVC: Validation and Data Annotations
## Beginner-Friendly 1-Hour Live Coding Practical

> **Session Type:** Live coding in front of students  
> **Duration:** 1 hour  
> **Project:** Smart Society Complaint System  
> **Important:** Model, controller, views, and project files already exist. In this session, we are only improving validation.

---

# 1. What Students Will Learn

By the end of this 1-hour practical, students should understand:

- What validation means in ASP.NET Core MVC
- Why validation is needed before saving form data
- How to use Data Annotations like `[Required]`, `[StringLength]`, and `[Display]`
- How `ModelState.IsValid` works in the controller
- How to show validation errors in Razor views
- How client-side validation works using `_ValidationScriptsPartial`
- How to add one simple business rule using `IValidatableObject`

---

# 2. What We Will Code in This 1 Hour

We will update the existing complaint creation form.

We will code only these parts:

```text
1. Add validation attributes in Complaint model
2. Add one new property: EmergencyDetails
3. Add model-level validation for Critical priority
4. Update Create.cshtml to show validation messages
5. Update controller POST Create action to check ModelState
6. Test invalid and valid form submissions
```

We will not create a new project.

We will not add database code.

We will not add custom validation attribute.

We will not implement full remote validation in this 1-hour session.

---

# 3. Real Project Story

In the Smart Society project, a resident can raise a complaint.

Bad complaint example:

```text
Title: 
Description: fix it
Category: 
Priority: Critical
Emergency Details: 
```

This is not useful for admin or maintenance staff.

A good complaint should have proper information:

```text
Title: Water leakage in basement
Description: Water is continuously leaking near parking area B2 since morning.
Category: Plumbing
Priority: Critical
Emergency Details: Water is near the electrical panel.
```

So validation helps us stop incomplete and poor-quality complaints.

---

# 4. 1-Hour Live Coding Plan

| Time | Activity |
|---|---|
| 0-5 min | Explain validation with Smart Society example |
| 5-15 min | Add Data Annotations in existing Complaint model |
| 15-25 min | Update Create view with validation messages |
| 25-35 min | Update controller with ModelState check |
| 35-45 min | Add Critical priority business validation using IValidatableObject |
| 45-55 min | Run and test validation cases |
| 55-60 min | Revision, mistakes, and assignment |

---

# 5. Files We Will Touch

Only update these existing files:

```text
Models/Complaint.cs
Controllers/ComplaintsController.cs
Views/Complaints/Create.cshtml
```

Optional file to check:

```text
Views/Shared/_ValidationScriptsPartial.cshtml
```

---

# 6. Step 1: Open Existing Complaint Model

Open this file:

```text
Models/Complaint.cs
```

At the top, make sure this namespace is imported:

```csharp
using System.ComponentModel.DataAnnotations;
```

Now update your model like this.

## Final Complaint Model Code

```csharp
using System.ComponentModel.DataAnnotations;

namespace SmartSocietyLiveCoding.Models
{
    public class Complaint : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Complaint title is required.")]
        [StringLength(100, ErrorMessage = "Complaint title cannot exceed 100 characters.")]
        [Display(Name = "Complaint Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Complaint description is required.")]
        [StringLength(1000, MinimumLength = 20, ErrorMessage = "Description must be between 20 and 1000 characters.")]
        [Display(Name = "Complaint Description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Complaint category is required.")]
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters.")]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Complaint priority is required.")]
        [StringLength(20, ErrorMessage = "Priority cannot exceed 20 characters.")]
        [Display(Name = "Priority")]
        public string Priority { get; set; } = string.Empty;

        [Display(Name = "Emergency Details")]
        [StringLength(500, ErrorMessage = "Emergency details cannot exceed 500 characters.")]
        public string? EmergencyDetails { get; set; }

        public string? ImagePath { get; set; }

        public string? CompletionProofPath { get; set; }

        public string Status { get; set; } = "Pending";

        public string AssignedStaff { get; set; } = "Not Assigned";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Priority == "Critical" && string.IsNullOrWhiteSpace(EmergencyDetails))
            {
                yield return new ValidationResult(
                    "Emergency details are required for critical complaints.",
                    new[] { nameof(EmergencyDetails) }
                );
            }
        }
    }
}
```

## Important Note for Trainer

Your namespace may be different.

For example:

```csharp
namespace SmartSocietyDemo.Models
```

or:

```csharp
namespace SmartSocietyLiveCoding.Models
```

Do not blindly copy the namespace. Keep the namespace from your existing project.

---

# 7. Explain Data Annotations Simply

Explain while coding:

```csharp
[Required]
```

Means the field cannot be empty.

```csharp
[StringLength(100)]
```

Means the text cannot be longer than 100 characters.

```csharp
[StringLength(1000, MinimumLength = 20)]
```

Means the description must be between 20 and 1000 characters.

```csharp
[Display(Name = "Complaint Title")]
```

Means the UI label will show `Complaint Title` instead of only `Title`.

```csharp
IValidatableObject
```

Is used when validation depends on more than one field.

Example:

```text
If Priority is Critical, Emergency Details must be filled.
```

This depends on:

```text
Priority
EmergencyDetails
```

So we use model-level validation.

---

# 8. Step 2: Update Create View

Open this file:

```text
Views/Complaints/Create.cshtml
```

Replace or update the form with this code.

## Final Create.cshtml Code

```cshtml
@model SmartSocietyLiveCoding.Models.Complaint

@{
    ViewData["Title"] = "Create Complaint";
}

<h2>Create Complaint</h2>

<form asp-action="Create" method="post" enctype="multipart/form-data">

    <div asp-validation-summary="ModelOnly" class="text-danger mb-3"></div>

    <div class="mb-3">
        <label asp-for="Title" class="form-label"></label>
        <input asp-for="Title" class="form-control" />
        <span asp-validation-for="Title" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label asp-for="Description" class="form-label"></label>
        <textarea asp-for="Description" class="form-control"></textarea>
        <span asp-validation-for="Description" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label asp-for="Category" class="form-label"></label>
        <select asp-for="Category" class="form-control">
            <option value="">-- Select Category --</option>
            <option value="Water">Water</option>
            <option value="Electricity">Electricity</option>
            <option value="Security">Security</option>
            <option value="Parking">Parking</option>
            <option value="General">General</option>
        </select>
        <span asp-validation-for="Category" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label asp-for="Priority" class="form-label"></label>
        <select asp-for="Priority" class="form-control">
            <option value="">-- Select Priority --</option>
            <option value="Low">Low</option>
            <option value="Medium">Medium</option>
            <option value="High">High</option>
            <option value="Critical">Critical</option>
        </select>
        <span asp-validation-for="Priority" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label asp-for="EmergencyDetails" class="form-label"></label>
        <textarea asp-for="EmergencyDetails" class="form-control"></textarea>
        <span asp-validation-for="EmergencyDetails" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label class="form-label">Complaint Image</label>
        <input type="file" name="imageFile" class="form-control" />
    </div>

    <button type="submit" class="btn btn-primary">Submit Complaint</button>
    <a asp-action="Index" class="btn btn-secondary">Back</a>

</form>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

## Important Note for Trainer

Your model namespace may be different.

If your project uses this:

```csharp
namespace SmartSocietyDemo.Models
```

Then your view should use:

```cshtml
@model SmartSocietyDemo.Models.Complaint
```

Keep the existing namespace from your project.

---

# 9. Explain Razor Validation Tags

Explain while coding:

```cshtml
<div asp-validation-summary="ModelOnly" class="text-danger"></div>
```

This shows form-level or model-level validation errors.

Example:

```text
Emergency details are required for critical complaints.
```

```cshtml
<span asp-validation-for="Title" class="text-danger"></span>
```

This shows validation error near the Title field.

Example:

```text
Complaint title is required.
```

```cshtml
@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

This enables client-side validation in the browser.

But remind students:

```text
Client-side validation is for better user experience.
Server-side validation is still mandatory.
```

---

# 10. Step 3: Update Controller POST Create Action

Open this file:

```text
Controllers/ComplaintsController.cs
```

Find your POST Create action.

It may look similar to this:

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(
    [Bind("Title,Description,Category,Priority")] Complaint complaint,
    IFormFile? imageFile)
{
    // existing code
}
```

Update it like this.

## Final POST Create Action

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(
    [Bind("Title,Description,Category,Priority,EmergencyDetails")] Complaint complaint,
    IFormFile? imageFile)
{
    if (!ModelState.IsValid)
    {
        return View(complaint);
    }

    complaint.Id = _nextId++;
    complaint.Status = "Pending";
    complaint.AssignedStaff = "Not Assigned";
    complaint.CreatedAt = DateTime.Now;
    complaint.ImagePath = await SaveFileAsync(imageFile);

    _complaints.Add(complaint);

    return RedirectToAction(nameof(Index));
}
```

## Explain Like This

This line is very important:

```csharp
if (!ModelState.IsValid)
{
    return View(complaint);
}
```

It means:

```text
If validation fails, do not save the complaint.
Show the same form again with validation errors.
```

This line is also important:

```csharp
[Bind("Title,Description,Category,Priority,EmergencyDetails")]
```

It means the form is allowed to send only these safe fields.

The form cannot set protected fields like:

```text
Status
AssignedStaff
CreatedAt
ImagePath
CompletionProofPath
```

Those values are controlled by the server.

---

# 11. Step 4: Update Index View Only If Needed

If you want to show Emergency Details on the table, open:

```text
Views/Complaints/Index.cshtml
```

Add one table header:

```cshtml
<th>Emergency Details</th>
```

And add one table data column inside the loop:

```cshtml
<td>@complaint.EmergencyDetails</td>
```

This is optional for the 1-hour session.

If time is short, skip this step.

---

# 12. Step 5: Run the Application

Open terminal in project folder.

Run:

```bash
dotnet build
```

If build succeeds, run:

```bash
dotnet run
```

Open the application URL shown in terminal.

Example:

```text
https://localhost:5001/Complaints/Create
```

or:

```text
http://localhost:5000/Complaints/Create
```

Your port number may be different.

---

# 13. Step 6: Test Validation Cases Live

## Test Case 1: Submit Empty Form

Do not enter anything.

Click Submit.

Expected result:

```text
Complaint title is required.
Complaint description is required.
Complaint category is required.
Complaint priority is required.
```

---

## Test Case 2: Short Description

Enter:

```text
Title: Water issue
Description: fix it
Category: Water
Priority: Low
```

Expected result:

```text
Description must be between 20 and 1000 characters.
```

---

## Test Case 3: Critical Priority Without Emergency Details

Enter:

```text
Title: Lift stopped suddenly
Description: Lift stopped suddenly with people inside near Tower A.
Category: General
Priority: Critical
Emergency Details: empty
```

Expected result:

```text
Emergency details are required for critical complaints.
```

---

## Test Case 4: Valid Complaint

Enter:

```text
Title: Water leakage in basement
Description: Water is continuously leaking near parking area B2 since morning.
Category: Water
Priority: Critical
Emergency Details: Water is near electrical wiring.
```

Expected result:

```text
Complaint is saved successfully.
User is redirected to Index page.
```

---

# 14. Final Learning Flow

Explain the complete flow:

```text
User fills complaint form
↓
Form submits to controller
↓
ASP.NET Core binds form data to Complaint model
↓
Data Annotation validation runs
↓
IValidatableObject validation runs
↓
ModelState stores errors
↓
If invalid, same form is returned
↓
If valid, complaint is saved
```

---

# 15. Common Mistakes During Live Coding

## Mistake 1: Namespace mismatch

Problem:

```text
View cannot find model type
```

Fix:

Make sure this line matches your actual project namespace:

```cshtml
@model SmartSocietyLiveCoding.Models.Complaint
```

---

## Mistake 2: Forgetting validation scripts

Problem:

```text
Client-side validation does not work
```

Fix:

Add this at the bottom of the view:

```cshtml
@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

---

## Mistake 3: Not checking ModelState

Problem:

```text
Invalid data still gets saved
```

Fix:

Add this in POST action before saving:

```csharp
if (!ModelState.IsValid)
{
    return View(complaint);
}
```

---

## Mistake 4: Dropdown default value is not empty

Problem:

Required validation does not trigger for dropdown.

Fix:

Use an empty option:

```cshtml
<option value="">-- Select Priority --</option>
```

---

## Mistake 5: Forgetting EmergencyDetails in Bind

Problem:

EmergencyDetails value does not come to controller.

Fix:

Add it in Bind:

```csharp
[Bind("Title,Description,Category,Priority,EmergencyDetails")]
```

---

# 16. What Not To Cover in This 1 Hour

Do not go deep into these topics during this live session:

```text
Custom validation attributes
Remote validation full implementation
Database duplicate checking
Service layer validation
Client-side custom adapter
Unobtrusive validation internals
FluentValidation
```

You can mention them as future topics only.

---

# 17. Quick Revision Questions

Ask students these questions at the end:

1. What is validation?
2. What is `[Required]` used for?
3. What is `[StringLength]` used for?
4. What is `[Display]` used for?
5. What does `ModelState.IsValid` check?
6. Why do we return the same view when validation fails?
7. What is the use of `asp-validation-for`?
8. What is the use of `asp-validation-summary`?
9. Why is client-side validation not enough?
10. Why did we use `IValidatableObject`?

---

# 18. Short Answers for Trainer

1. Validation checks whether user input is correct, complete, and meaningful.
2. `[Required]` makes a field mandatory.
3. `[StringLength]` controls minimum and maximum text length.
4. `[Display]` gives a user-friendly label.
5. `ModelState.IsValid` checks whether model validation passed or failed.
6. We return the same view so the user can fix the errors.
7. `asp-validation-for` shows field-level validation errors.
8. `asp-validation-summary` shows form-level or model-level errors.
9. Client-side validation can be bypassed, so server-side validation is mandatory.
10. `IValidatableObject` is used when validation depends on multiple fields.

---

# 19. Mini Assignment for Students

Add validation to the Edit complaint form also.

Rules:

```text
Title is required
Title max length is 100
Description is required
Description minimum length is 20
Description max length is 1000
Category is required
Priority is required
```

Also test:

```text
Empty title
Short description
Empty category
Empty priority
```

---

# 20. Final Interview Line

Students should be able to say:

```text
In ASP.NET Core MVC, I used Data Annotations like Required, StringLength, and Display to validate form input. I checked ModelState.IsValid in the POST action before saving data. I used asp-validation-for and asp-validation-summary in the Razor view to show errors. I also used IValidatableObject for model-level validation where Critical complaints require Emergency Details. Client-side validation improves user experience, but server-side validation is mandatory because browser validation can be bypassed.
```
