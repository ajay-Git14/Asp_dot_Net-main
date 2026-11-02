# ASP.NET Core MVC: 1-Hour Beginner Friendly Practical
## Topic: Bundling, Minification, Deployment, Documentation, and Final Review

> **Live Coding Duration:** 1 Hour  
> **Style:** Beginner friendly, practical-first, final project preparation.  
> **Project Example:** Smart Society Issue Management System / LiveClassDemos MVC Project

---

# 1. Practical Goal

In this practical, students will learn how to make an ASP.NET Core MVC project more professional and presentation-ready.

By the end of this practical, students should understand:

```text
What bundling means
What minification means
Why wwwroot is important
How static files are served
How to prepare project for deployment
How to create README documentation
How to prepare setup guide
How to prepare deployment checklist
How to prepare final review checklist
How to explain project in interview/demo
```

---

# 2. What We Will Prepare In 1 Hour

We will prepare final project evidence:

```text
1. Organize CSS and JavaScript files
2. Create bundleconfig.json
3. Understand bundling and minification
4. Use environment tag helper in layout
5. Prepare README.md
6. Prepare docs folder
7. Prepare database folder
8. Prepare deployment checklist
9. Prepare demo script
10. Prepare final review checklist
```

---

# 3. Is Data Folder Required?

For this topic, **Data folder is not required directly**.

This practical is mainly about:

```text
wwwroot
static files
bundling
minification
documentation
deployment checklist
README
final review
```

But if your project uses EF Core, then your project may already have:

```text
Data/ApplicationDbContext.cs
```

That is fine, but this final review topic does not create new EF Core data code.

---

# 4. 1-Hour Live Practical Flow

| Time | Task |
|---|---|
| 0-5 min | Explain final project readiness |
| 5-15 min | Explain wwwroot and static files |
| 15-25 min | Create bundleconfig.json |
| 25-35 min | Update layout with environment tag helper |
| 35-45 min | Create README.md |
| 45-52 min | Create docs and database folder plan |
| 52-58 min | Create final review checklist |
| 58-60 min | Interview/demo explanation |

---

# 5. Simple Explanation

A project is not professional only because code is working.

A professional project should be:

```text
Working
Organized
Optimized
Documented
Deployable
Demo-ready
Interview-ready
```

For interviews or client demo, we need:

```text
Clean GitHub repository
README file
Setup guide
Deployment guide
Database script
Screenshots
Demo script
Final review checklist
```

---

# 6. What Is Bundling?

Bundling means combining many CSS or JavaScript files into one file.

Example:

```text
site.css
dashboard.css
forms.css
```

becomes:

```text
app.min.css
```

JavaScript example:

```text
site.js
dashboard.js
complaint-status.js
```

becomes:

```text
app.min.js
```

---

## Why Bundling?

Bundling reduces browser requests.

Before bundling:

```text
Browser downloads 6 files
```

After bundling:

```text
Browser downloads 2 files
```

This improves page loading speed.

---

# 7. What Is Minification?

Minification means reducing file size by removing:

```text
Extra spaces
Line breaks
Comments
Unnecessary formatting
```

Example:

```css
body {
    background-color: white;
    color: black;
}
```

Minified:

```css
body{background-color:white;color:black}
```

---

# 8. wwwroot Folder

`wwwroot` is the public static file folder in ASP.NET Core MVC.

Common structure:

```text
wwwroot
├── css
├── js
├── images
└── uploads
```

Files inside `wwwroot` can be accessed by the browser.

Example:

```text
wwwroot/css/site.css
```

can be used as:

```html
<link rel="stylesheet" href="~/css/site.css" />
```

---

## Important Security Rule

Anything inside `wwwroot` can become public.

Do not store sensitive files inside `wwwroot`.

Avoid storing:

```text
Private documents
Passwords
Secrets
Production connection strings
Sensitive complaint files
```

---

# 9. Check Program.cs For Static Files

Open:

```text
Program.cs
```

Make sure this line exists:

```csharp
app.UseStaticFiles();
```

Example:

```csharp
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
```

Without `UseStaticFiles()`, CSS, JavaScript, and images will not load.

---

# 10. Create CSS And JS Files

Create these files if not already available:

```text
wwwroot/css/site.css
wwwroot/css/dashboard.css
wwwroot/css/forms.css
```

Create these JS files if needed:

```text
wwwroot/js/site.js
wwwroot/js/dashboard.js
wwwroot/js/complaint-status.js
```

---

## Sample dashboard.css

```css
.dashboard-card {
    border: 1px solid #ddd;
    padding: 15px;
    border-radius: 8px;
    margin-bottom: 15px;
}
```

---

## Sample forms.css

```css
.form-section {
    margin-bottom: 20px;
}

.field-validation-error {
    color: red;
}
```

---

## Sample dashboard.js

```javascript
console.log("Dashboard script loaded.");
```

---

## Sample complaint-status.js

```javascript
console.log("Complaint status script loaded.");
```

---

# 11. Create bundleconfig.json

Create file in project root:

```text
bundleconfig.json
```

Add:

```json
[
  {
    "outputFileName": "wwwroot/css/app.min.css",
    "inputFiles": [
      "wwwroot/css/site.css",
      "wwwroot/css/dashboard.css",
      "wwwroot/css/forms.css"
    ],
    "minify": {
      "enabled": true
    }
  },
  {
    "outputFileName": "wwwroot/js/app.min.js",
    "inputFiles": [
      "wwwroot/js/site.js",
      "wwwroot/js/dashboard.js",
      "wwwroot/js/complaint-status.js"
    ],
    "minify": {
      "enabled": true
    }
  }
]
```

---

## Explain To Students

This file tells bundling tool:

```text
Which files to combine
Where to create output file
Whether minification is enabled
```

Output files:

```text
wwwroot/css/app.min.css
wwwroot/js/app.min.js
```

---

# 12. Update _Layout.cshtml

Open:

```text
Views/Shared/_Layout.cshtml
```

Use environment tag helper.

For CSS:

```html
<environment include="Development">
    <link rel="stylesheet" href="~/css/site.css" />
    <link rel="stylesheet" href="~/css/dashboard.css" />
    <link rel="stylesheet" href="~/css/forms.css" />
</environment>

<environment exclude="Development">
    <link rel="stylesheet" href="~/css/app.min.css" asp-append-version="true" />
</environment>
```

For JavaScript:

```html
<environment include="Development">
    <script src="~/js/site.js"></script>
    <script src="~/js/dashboard.js"></script>
    <script src="~/js/complaint-status.js"></script>
</environment>

<environment exclude="Development">
    <script src="~/js/app.min.js" asp-append-version="true"></script>
</environment>
```

---

## Explain asp-append-version

```html
asp-append-version="true"
```

This helps browser load latest CSS/JS when file changes.

It avoids old cached file problem.

---

# 13. Development Vs Production

## Development

Used while coding and debugging.

```text
Readable CSS and JS files
Detailed errors
Local database
Debugging enabled
```

## Production

Used by real users.

```text
Minified CSS and JS
Friendly error page
Secure connection string
HTTPS
Optimized settings
```

---

# 14. Deployment Checklist

Create file:

```text
docs/deployment-checklist.md
```

Add:

```markdown
# Deployment Checklist

## Build

- [ ] Project builds successfully
- [ ] No compile errors
- [ ] No unused broken files
- [ ] Release build tested

## Configuration

- [ ] Production connection string configured securely
- [ ] No password committed in GitHub
- [ ] appsettings checked
- [ ] Environment set to Production

## Database

- [ ] Migrations available
- [ ] Database script prepared
- [ ] Sample data available
- [ ] Identity roles seeded
- [ ] Admin user setup process ready

## Security

- [ ] Login working
- [ ] Logout working
- [ ] Role-based access working
- [ ] Access denied page working
- [ ] Anti-forgery used on POST forms
- [ ] File upload validation applied

## Static Files

- [ ] CSS files loading
- [ ] JS files loading
- [ ] Images loading
- [ ] Bundled files generated
- [ ] Minified files tested

## Final

- [ ] README updated
- [ ] Screenshots captured
- [ ] Demo script prepared
- [ ] GitHub repository cleaned
```

---

# 15. Create docs Folder

Create folder:

```text
docs
```

Inside it, create:

```text
product-abstract.md
requirements.md
setup-guide.md
deployment-guide.md
demo-script.md
resume-explanation.md
final-review-checklist.md
```

---

# 16. Product Abstract Template

Create:

```text
docs/product-abstract.md
```

Add:

```markdown
# Product Abstract

## Project Name

Smart Society Issue Management System

## Problem Statement

Residential societies often manage complaints through phone calls, WhatsApp messages, or paper registers. This creates poor tracking, delayed resolution, and lack of accountability.

## Proposed Solution

Smart Society Issue Management System is an ASP.NET Core MVC web application that allows residents to raise complaints, admins to assign and monitor complaints, and staff to update progress and resolution status.

## User Roles

- Resident
- Admin
- Staff

## Core Features

- User registration and login
- Role-based access
- Resident complaint creation
- Admin dashboard
- Complaint assignment
- Staff assigned complaint workflow
- Status update and history
- File upload support
- Web API and AJAX support
- EF Core database integration

## Technology Stack

- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- Razor Views
- Bootstrap
- jQuery AJAX
- Web API
```

---

# 17. Setup Guide Template

Create:

```text
docs/setup-guide.md
```

Add:

```markdown
# Development Setup Guide

## Required Software

- Visual Studio 2022
- .NET SDK
- SQL Server LocalDB or SQL Server Express
- SQL Server Management Studio
- Git
- Browser
- Postman

## Steps To Run Project

1. Clone the repository

```bash
git clone <repository-url>
```

2. Open solution in Visual Studio

3. Restore NuGet packages

```bash
dotnet restore
```

4. Update connection string in `appsettings.json`

5. Run migration

```bash
dotnet ef database update
```

6. Run project

```bash
dotnet run
```

7. Open browser and test application

```text
https://localhost:xxxx
```

## Default Demo Users

Add demo users here if available.

## Common Issues

| Issue | Solution |
|---|---|
| Database not created | Run `dotnet ef database update` |
| CSS not loading | Check `app.UseStaticFiles()` |
| Login not working | Check Identity configuration |
| Role access failing | Check role seeding |
```

---

# 18. README.md Template

Create file in project root:

```text
README.md
```

Add:

```markdown
# Smart Society Issue Management System

## Overview

Smart Society Issue Management System is an ASP.NET Core MVC application for managing residential society complaints.

Residents can raise complaints, admins can assign and monitor complaints, and staff can update progress and resolution status.

## Problem Statement

Residential societies often manage complaints through informal methods like phone calls or WhatsApp. This creates tracking issues and poor accountability.

## Solution

This system provides a structured complaint workflow with authentication, role-based authorization, complaint tracking, assignment, comments, attachments, status history, Web API, and AJAX interactions.

## User Roles

- Resident
- Admin
- Staff

## Features

- Authentication and registration
- Role-based authorization
- Complaint creation
- Complaint listing and details
- Admin dashboard
- Staff assignment workflow
- Status update and history
- File upload support
- EF Core database integration
- Web API endpoints
- jQuery AJAX integration
- Caching and pagination awareness
- Modular Areas structure

## Technology Stack

- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- Razor Views
- Bootstrap
- jQuery AJAX
- Web API

## Architecture

```text
Controller
↓
Service Layer
↓
Repository Layer
↓
ApplicationDbContext
↓
SQL Server
```

## Setup Instructions

See:

```text
docs/setup-guide.md
```

## Deployment Instructions

See:

```text
docs/deployment-guide.md
```

## Screenshots

Add screenshots in:

```text
docs/screenshots
```

## Future Enhancements

- Email notifications
- SMS alerts
- Advanced reports
- Azure Blob Storage
- Background jobs
- Mobile app integration
```

---

# 19. Create database Folder

Create folder:

```text
database
```

Inside it, create:

```text
schema.sql
sample-data.sql
README.md
```

---

## database/README.md

```markdown
# Database Setup

This folder contains database-related scripts.

## Files

| File | Purpose |
|---|---|
| schema.sql | Database schema script |
| sample-data.sql | Sample data for demo |
| README.md | Database setup instructions |

## EF Core Migration Command

```bash
dotnet ef database update
```

## Generate SQL Script

```bash
dotnet ef migrations script -o database/schema.sql
```
```

---

# 20. Demo Script

Create:

```text
docs/demo-script.md
```

Add:

```markdown
# Demo Video Script

Hello, this is the Smart Society Issue Management System.

This project solves a common problem in residential societies where complaints are managed through WhatsApp, phone calls, or paper registers.

The system has three roles:

- Resident
- Admin
- Staff

First, I will login as a Resident. A resident can raise a complaint by entering title, description, category, and priority.

After submission, the complaint is saved in SQL Server using Entity Framework Core.

Now I will login as Admin. The admin can view dashboard, monitor complaints, and assign complaints to staff.

Now I will login as Staff. Staff can view assigned complaints and update complaint status.

The project uses ASP.NET Core MVC, EF Core, SQL Server, ASP.NET Core Identity, Razor Views, Web API, and jQuery AJAX.

The application follows layered architecture:

Controller → Service → Repository → DbContext → SQL Server

The project also includes README, setup guide, deployment checklist, database script, screenshots, and final review checklist.

Thank you.
```

---

# 21. Resume Explanation

Create:

```text
docs/resume-explanation.md
```

Add:

```markdown
# Resume Project Explanation

## Project Name

Smart Society Issue Management System

## Summary

Designed and developed an ASP.NET Core MVC-based complaint management system for residential societies. The application enables residents to raise complaints, admins to assign and monitor complaints, and maintenance staff to update complaint progress.

## Key Contributions

- Implemented ASP.NET Core MVC application structure
- Added authentication and role-based authorization using ASP.NET Core Identity
- Created EF Core Code First database schema
- Implemented complaint CRUD operations
- Used service and repository layers
- Added status history and soft delete/archive workflow
- Created Web API endpoints
- Integrated jQuery AJAX for interactive updates
- Prepared documentation and deployment checklist

## Technology Stack

ASP.NET Core MVC, C#, EF Core, SQL Server, Identity, Razor Views, Bootstrap, jQuery AJAX, Web API
```

---

# 22. Final Review Checklist

Create:

```text
docs/final-review-checklist.md
```

Add:

```markdown
# Final Project Review Checklist

## Functional Review

- [ ] Resident can register
- [ ] Resident can login
- [ ] Resident can create complaint
- [ ] Resident can view own complaints
- [ ] Admin can login
- [ ] Admin can access dashboard
- [ ] Admin can assign complaints
- [ ] Staff can login
- [ ] Staff can view assigned complaints
- [ ] Staff can update status

## Security Review

- [ ] Protected pages require login
- [ ] Admin pages require Admin role
- [ ] Resident pages require Resident role
- [ ] Staff pages require Staff role
- [ ] AccessDenied page works
- [ ] POST actions use anti-forgery
- [ ] Overposting protection checked

## Database Review

- [ ] Migrations exist
- [ ] Database creates successfully
- [ ] Identity tables exist
- [ ] Complaint tables exist
- [ ] Sample data available

## UI Review

- [ ] CSS loading
- [ ] JS loading
- [ ] Navigation working
- [ ] Validation messages visible
- [ ] Screenshots captured

## API/AJAX Review

- [ ] API endpoints tested
- [ ] AJAX requests working
- [ ] Browser Network tab checked
- [ ] Postman testing done

## Documentation Review

- [ ] README completed
- [ ] Setup guide completed
- [ ] Deployment guide completed
- [ ] Product abstract completed
- [ ] Demo script completed
- [ ] Resume explanation completed
```

---

# 23. Deployment Guide Template

Create:

```text
docs/deployment-guide.md
```

Add:

```markdown
# Deployment Guide

## High-Level Deployment Steps

1. Build project in Release mode
2. Prepare production database
3. Configure production connection string
4. Apply database migrations or SQL script
5. Publish ASP.NET Core MVC application
6. Configure environment as Production
7. Verify static files
8. Verify login and roles
9. Verify complaint workflow
10. Verify logs and error handling

## Publish Command

```bash
dotnet publish -c Release
```

## Azure Awareness

For Azure App Service:

- Create Azure App Service
- Create Azure SQL Database
- Configure connection string
- Set environment to Production
- Publish application
- Test application after deployment

## Production Checklist

- [ ] HTTPS enabled
- [ ] Production connection string secured
- [ ] Developer exception page disabled
- [ ] Swagger restricted or disabled
- [ ] Logs enabled
- [ ] File upload storage planned
```

---

# 24. GitHub Cleanup Checklist

Before pushing code:

```text
Remove bin folder
Remove obj folder
Remove .vs folder
Remove secrets
Remove production passwords
Add .gitignore
Check README
Check docs folder
Check database folder
Commit clean code
```

---

## Suggested .gitignore

```text
bin/
obj/
.vs/
*.user
*.suo
appsettings.Production.json
logs/
```

---

# 25. Run Final Build

Run:

```bash
dotnet build
```

If successful, run:

```bash
dotnet publish -c Release
```

---

# 26. Final Testing URLs

Test main flows:

```text
/
/Account/Login
/Account/Register
/Resident/MyComplaints
/Admin/Dashboard
/Staff/AssignedComplaints
/api/dashboard/summary
/api/complaints
```

---

# 27. Common Mistakes

| Mistake | Problem | Fix |
|---|---|---|
| Only code, no documentation | Project looks incomplete | Add README and docs |
| No setup guide | Others cannot run project | Add setup-guide.md |
| No database script | Demo setup fails | Add database scripts |
| Secrets committed | Security risk | Remove and rotate secrets |
| CSS/JS not loading | Static files issue | Check UseStaticFiles and paths |
| Production uses dev errors | Security risk | Configure production error handling |
| No screenshots | No visual proof | Capture screenshots |
| No demo script | Demo becomes weak | Prepare script |

---

# 28. One-Minute Revision

| Concept | Meaning |
|---|---|
| Bundling | Combines multiple CSS/JS files |
| Minification | Reduces file size |
| wwwroot | Public static files folder |
| UseStaticFiles | Enables CSS/JS/images |
| Development | Debug environment |
| Production | Real user environment |
| README | Main project documentation |
| Setup guide | Local run instructions |
| Deployment guide | Hosting instructions |
| Final review | Professional project checklist |

---

# 29. Interview Lines

## Bundling

Bundling combines multiple CSS or JavaScript files into fewer files to reduce browser requests.

## Minification

Minification reduces file size by removing unnecessary spaces, comments, and formatting.

## wwwroot

wwwroot is the public static file folder in ASP.NET Core.

## Deployment Checklist

A deployment checklist verifies build, configuration, database, security, static files, and documentation before hosting.

## README

README explains the project, features, tech stack, setup steps, and usage.

## Final Review

Final review ensures the project is functional, secure, documented, deployable, and demo-ready.

---

# 30. Mini Assignment

Ask students to complete:

```text
1. Create README.md
2. Create docs folder
3. Create setup-guide.md
4. Create deployment-guide.md
5. Create demo-script.md
6. Create final-review-checklist.md
7. Create database folder
8. Create bundleconfig.json
9. Add environment tag helper in _Layout.cshtml
10. Run dotnet build
11. Capture 5 screenshots
12. Push code to GitHub
```

---

# 31. Final Outcome

After this practical, students should be able to say:

```text
I prepared my ASP.NET Core MVC project for professional presentation.

I understand bundling, minification, wwwroot, static files, development vs production, deployment checklist, README documentation, setup guide, demo script, and final project review.

My project is now more organized, documented, deployable, and interview-ready.
```
