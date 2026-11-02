# ASP.NET Core MVC: 1-Hour Beginner Friendly Practical
## Topic: EF Core Setup, Models, DbContext, and Migrations

> **Live Coding Duration:** 1 Hour  
> **Style:** Beginner friendly, practical-first, code in front of students.  
> **Project Example:** Smart Society Complaint System / LiveClassDemos MVC project

---

# 1. Practical Goal

In this 1-hour practical, students will learn how to connect an ASP.NET Core MVC project with SQL Server using **Entity Framework Core**.

By the end, students should understand:

```text
EF Core = Database work using C# classes
Entity = C# class that becomes database table
DbContext = Bridge between application and database
DbSet = Represents one table
Migration = Database structure change file
Update-Database = Creates/updates actual database
```

---

# 2. What We Will Build

We will create database structure for a simple Smart Society complaint system.

We will create:

```text
1. Complaint entity
2. ComplaintComment entity
3. ComplaintAttachment entity
4. ComplaintStatusHistory entity
5. ApplicationDbContext
6. Connection string
7. DbContext registration in Program.cs
8. Migration
9. SQL Server database tables
```

---

# 3. Is Data Folder Required?

Yes, for this practical, **Data folder is required**.

Reason:

```text
DbContext is normally placed inside the Data folder.
DbContext connects C# models with database tables.
```

Use this folder:

```text
Data
|-- ApplicationDbContext.cs
```

If you already created `ApplicationDbContext` in the Identity practical, do not create a new one. Just update the existing file.

---

# 4. 1-Hour Live Coding Flow

| Time | Task |
|---|---|
| 0-5 min | Explain why database is required |
| 5-10 min | Install EF Core packages |
| 10-25 min | Create entity classes |
| 25-35 min | Create/update ApplicationDbContext |
| 35-42 min | Add connection string and Program.cs setup |
| 42-52 min | Run migration and update database |
| 52-58 min | Verify database tables |
| 58-60 min | Revision and common mistakes |

---

# 5. Beginner Explanation

## Problem Without Database

If we store complaints only in memory:

```text
Application starts
Resident creates complaint
Application stops
Complaint is lost
```

That is not acceptable in a real project.

## Solution With EF Core

EF Core helps us save data permanently in SQL Server.

```text
Resident creates complaint
EF Core saves complaint in database
Application restarts
Complaint is still available
```

---

# 6. Install EF Core Packages

Open terminal in project folder and run:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

If your project is also using Identity, this package may already exist:

```bash
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```

---

# 7. Create Complaint Entity

Create file:

```text
Models/Complaint.cs
```

Add this code:

```csharp
using System.ComponentModel.DataAnnotations;

namespace LiveClassDemos.Models
{
    public class Complaint
    {
        [Key]
        public int ComplaintId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Priority { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "New";

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<ComplaintComment> Comments { get; set; } = new List<ComplaintComment>();

        public ICollection<ComplaintAttachment> Attachments { get; set; } = new List<ComplaintAttachment>();

        public ICollection<ComplaintStatusHistory> StatusHistory { get; set; } = new List<ComplaintStatusHistory>();
    }
}
```

## Explain To Students

This class will become a database table:

```text
Complaint class → Complaints table
ComplaintId property → ComplaintId column
Title property → Title column
```

---

# 8. Create ComplaintComment Entity

Create file:

```text
Models/ComplaintComment.cs
```

Add this code:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveClassDemos.Models
{
    public class ComplaintComment
    {
        [Key]
        public int ComplaintCommentId { get; set; }

        public int ComplaintId { get; set; }

        [Required]
        [StringLength(1000)]
        public string CommentText { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(ComplaintId))]
        public Complaint? Complaint { get; set; }
    }
}
```

## Explain To Students

One complaint can have many comments.

Example:

```text
Complaint: Water leakage
Comment 1: Resident created complaint
Comment 2: Admin assigned staff
Comment 3: Staff started work
```

---

# 9. Create ComplaintAttachment Entity

Create file:

```text
Models/ComplaintAttachment.cs
```

Add this code:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveClassDemos.Models
{
    public class ComplaintAttachment
    {
        [Key]
        public int ComplaintAttachmentId { get; set; }

        public int ComplaintId { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ContentType { get; set; }

        public long FileSizeInBytes { get; set; }

        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        public string AttachmentType { get; set; } = "ComplaintImage";

        [ForeignKey(nameof(ComplaintId))]
        public Complaint? Complaint { get; set; }
    }
}
```

## Explain To Students

This table does not store the actual image binary.

It stores file information:

```text
File name
File path
File type
File size
Upload date
```

---

# 10. Create ComplaintStatusHistory Entity

Create file:

```text
Models/ComplaintStatusHistory.cs
```

Add this code:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveClassDemos.Models
{
    public class ComplaintStatusHistory
    {
        [Key]
        public int ComplaintStatusHistoryId { get; set; }

        public int ComplaintId { get; set; }

        [Required]
        [StringLength(30)]
        public string OldStatus { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string NewStatus { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Remarks { get; set; }

        public DateTime ChangedDate { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(ComplaintId))]
        public Complaint? Complaint { get; set; }
    }
}
```

## Explain To Students

This table tracks status changes.

Example:

```text
New → Assigned
Assigned → In Progress
In Progress → Resolved
```

This is useful for audit history.

---

# 11. Create Or Update ApplicationDbContext

Create folder:

```text
Data
```

Create file:

```text
Data/ApplicationDbContext.cs
```

## Option A: If Identity Is Not Used

Use this version:

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
        public DbSet<ComplaintAttachment> ComplaintAttachments { get; set; }
        public DbSet<ComplaintStatusHistory> ComplaintStatusHistories { get; set; }
    }
}
```

## Option B: If Identity Is Already Used

Use this version:

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
        public DbSet<ComplaintComment> ComplaintComments { get; set; }
        public DbSet<ComplaintAttachment> ComplaintAttachments { get; set; }
        public DbSet<ComplaintStatusHistory> ComplaintStatusHistories { get; set; }
    }
}
```

## Explain To Students

`DbContext` is the bridge between C# and database.

`DbSet` means table.

```text
DbSet<Complaint> Complaints → Complaints table
DbSet<ComplaintComment> ComplaintComments → ComplaintComments table
```

---

# 12. Add Connection String

Open:

```text
appsettings.json
```

Add this:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=LiveClassDemosEfCoreDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

If your `appsettings.json` already has content, do not replace everything blindly. Add only the `ConnectionStrings` section carefully.

---

# 13. Register DbContext In Program.cs

Open:

```text
Program.cs
```

Add namespaces:

```csharp
using LiveClassDemos.Data;
using Microsoft.EntityFrameworkCore;
```

Before:

```csharp
var app = builder.Build();
```

Add:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
```

## If Identity Is Already Configured

Make sure DbContext registration comes before Identity registration:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
```

---

# 14. Build The Project First

Before running migration, build the project.

```bash
dotnet build
```

If build fails, fix code errors first.

Important rule:

```text
Migration will not work if project does not build.
```

---

# 15. Install EF Tool If Needed

If this command fails:

```bash
dotnet ef
```

Install EF tool:

```bash
dotnet tool install --global dotnet-ef
```

If already installed, update it:

```bash
dotnet tool update --global dotnet-ef
```

---

# 16. Add Migration

Run:

```bash
dotnet ef migrations add InitialComplaintSchema
```

This creates a `Migrations` folder.

Explain:

```text
Migration file contains instructions to create database tables.
```

---

# 17. Update Database

Run:

```bash
dotnet ef database update
```

This creates/updates the database in SQL Server.

Expected database:

```text
LiveClassDemosEfCoreDb
```

Expected tables:

```text
Complaints
ComplaintComments
ComplaintAttachments
ComplaintStatusHistories
__EFMigrationsHistory
```

If Identity is used, also expect:

```text
AspNetUsers
AspNetRoles
AspNetUserRoles
AspNetUserClaims
AspNetRoleClaims
AspNetUserLogins
AspNetUserTokens
```

---

# 18. Verify In SQL Server

Open SQL Server Management Studio or SQL Server Object Explorer.

Check:

```text
Databases
↓
LiveClassDemosEfCoreDb
↓
Tables
```

You should see complaint-related tables.

---

# 19. Quick Controller Test Optional

If time remains, create a quick test action.

Open any controller or create:

```text
Controllers/DbTestController.cs
```

Code:

```csharp
using LiveClassDemos.Data;
using LiveClassDemos.Models;
using Microsoft.AspNetCore.Mvc;

namespace LiveClassDemos.Controllers
{
    public class DbTestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DbTestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AddSampleComplaint()
        {
            var complaint = new Complaint
            {
                Title = "Water leakage",
                Description = "Water is leaking near basement parking area.",
                Category = "Plumbing",
                Priority = "High",
                Status = "New"
            };

            _context.Complaints.Add(complaint);
            await _context.SaveChangesAsync();

            return Content("Sample complaint saved in database.");
        }
    }
}
```

Run and open:

```text
/DbTest/AddSampleComplaint
```

Then verify one record in the `Complaints` table.

---

# 20. Testing Checklist

| Test | Expected Result |
|---|---|
| `dotnet build` | Build successful |
| `dotnet ef migrations add InitialComplaintSchema` | Migration created |
| `dotnet ef database update` | Database updated |
| Check database | Tables created |
| Optional test action | Complaint inserted |

---

# 21. Common Errors And Fixes

| Error | Reason | Fix |
|---|---|---|
| `No DbContext was found` | DbContext missing or wrong project | Check `ApplicationDbContext` and build project |
| `Build failed` | Compile errors | Run `dotnet build` and fix errors |
| `UseSqlServer not found` | SQL Server package missing | Install `Microsoft.EntityFrameworkCore.SqlServer` |
| `dotnet ef not found` | EF tool missing | Install `dotnet-ef` |
| Login failed / connection failed | Wrong connection string | Check SQL Server name |
| Table not created | Migration not applied | Run `dotnet ef database update` |
| Entity requires primary key | Missing key property | Add `Id` or `[Key]` |

---

# 22. One-Minute Revision

| Concept | Simple Meaning |
|---|---|
| EF Core | Database work using C# |
| ORM | Maps C# classes to database tables |
| Entity | C# class that becomes table |
| DbContext | Bridge between app and database |
| DbSet | Represents a table |
| Connection string | Database address/configuration |
| Migration | Schema change file |
| Update Database | Applies migration to SQL Server |

---

# 23. Interview Lines

## EF Core

EF Core is an ORM that allows ASP.NET Core applications to work with databases using C# classes and LINQ.

## ORM

ORM maps C# objects to relational database tables.

## Code First

Code First means creating C# entity classes first and generating database tables from those classes using migrations.

## Entity

An entity class represents a database table.

## DbContext

DbContext is the bridge between the application and database.

## DbSet

DbSet represents a database table for a specific entity.

## Migration

Migration tracks database schema changes generated from entity classes.

## Update-Database

Update-Database applies migration changes to the actual SQL Server database.

---

# 24. Mini Assignment

Ask students to complete:

```text
1. Add one more property in Complaint: Location
2. Run dotnet ef migrations add AddComplaintLocation
3. Run dotnet ef database update
4. Verify Location column in Complaints table
5. Add one sample complaint with Location value
```

Property:

```csharp
[StringLength(100)]
public string? Location { get; set; }
```

---

# 25. Final Outcome

After this 1-hour practical, students should be able to say:

```text
I configured EF Core in an ASP.NET Core MVC project.

I created entity classes, added DbSet properties in ApplicationDbContext, configured SQL Server connection string, registered DbContext in Program.cs, created migration, and updated the database.

I understand that EF Core uses C# classes to create and manage database tables using Code First migrations.
```
