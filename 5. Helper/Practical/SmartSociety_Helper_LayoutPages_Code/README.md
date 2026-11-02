# Smart Society - Helper Module: Layout Pages Code

This zip contains a complete ASP.NET Core MVC sample for the **Layout Pages** topic.

It demonstrates:

- `_Layout.cshtml`
- `_ViewStart.cshtml`
- `_ViewImports.cshtml`
- `RenderBody()`
- `RenderSectionAsync()`
- Shared top navigation
- Shared side navigation
- Shared footer
- Reusable partial views
- Page header partial
- Status badge partial
- Priority badge partial
- Complaint summary card partial
- Empty state partial
- Layout-based dashboard and complaint pages

## Project Structure

```text
SmartSociety.Helper.LayoutPages.slnx
src/SmartSociety.Web
├── Controllers
│   ├── ComplaintsController.cs
│   └── DashboardController.cs
├── ViewModels
│   ├── ComplaintDetailsViewModel.cs
│   ├── ComplaintIndexViewModel.cs
│   ├── ComplaintSummaryViewModel.cs
│   ├── DashboardIndexViewModel.cs
│   ├── DemoComplaintData.cs
│   ├── EmptyStateViewModel.cs
│   └── PageHeaderViewModel.cs
├── Views
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml
│   ├── Complaints
│   │   ├── Create.cshtml
│   │   ├── Details.cshtml
│   │   └── Index.cshtml
│   ├── Dashboard
│   │   └── Index.cshtml
│   └── Shared
│       ├── _ComplaintSummaryCard.cshtml
│       ├── _EmptyState.cshtml
│       ├── _Footer.cshtml
│       ├── _Layout.cshtml
│       ├── _PageHeader.cshtml
│       ├── _PriorityBadge.cshtml
│       ├── _SideNavigation.cshtml
│       ├── _StatusBadge.cshtml
│       ├── _TopNavigation.cshtml
│       └── _ValidationScriptsPartial.cshtml
└── wwwroot/css/site.css
```

## How To Run

From the extracted zip root:

```powershell
dotnet restore src\SmartSociety.Web\SmartSociety.Web.csproj
dotnet build src\SmartSociety.Web\SmartSociety.Web.csproj
dotnet run --project src\SmartSociety.Web\SmartSociety.Web.csproj
```

Open these URLs:

```text
/Dashboard
/Complaints
/Complaints/Details/1
/Complaints/Create
/Complaints?status=Open
/Complaints?status=Resolved
```

## Suggested Git Commit

```bash
git add .
git commit -m "feat: add mvc layout pages and reusable partial views"
```

## Note

This code uses demo in-memory data only. Database, EF Core, authentication, authorization, form POST, and validation are intentionally not added here because this topic focuses on layout pages and reusable UI composition.
