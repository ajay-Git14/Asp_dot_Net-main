# Learning Notes - Layout Pages In ASP.NET Core MVC

## Goal

Understand how ASP.NET Core MVC uses layout pages and partial views to create reusable UI structure.

## Main Files

| File | Purpose |
| --- | --- |
| `Views/Shared/_Layout.cshtml` | Main page shell used by normal Views |
| `Views/_ViewStart.cshtml` | Sets the default layout for Views |
| `Views/_ViewImports.cshtml` | Adds common namespaces and Tag Helpers |
| `Views/Shared/_TopNavigation.cshtml` | Reusable top navigation partial |
| `Views/Shared/_SideNavigation.cshtml` | Reusable sidebar partial |
| `Views/Shared/_Footer.cshtml` | Reusable footer partial |
| `Views/Shared/_PageHeader.cshtml` | Reusable page title/action area |
| `Views/Shared/_ComplaintSummaryCard.cshtml` | Reusable complaint card partial |
| `Views/Shared/_StatusBadge.cshtml` | Reusable status badge partial |
| `Views/Shared/_PriorityBadge.cshtml` | Reusable priority badge partial |
| `Views/Shared/_EmptyState.cshtml` | Reusable no-data UI partial |

## Key Concepts

### Layout Page

`_Layout.cshtml` is the common HTML shell for multiple Views.

It usually contains:

```text
HTML head
CSS references
Navigation
Sidebar
Footer
Script references
RenderBody()
RenderSectionAsync()
```

### RenderBody

`@RenderBody()` marks the place where the child View content will be inserted.

Example:

```cshtml
<main>
    @RenderBody()
</main>
```

### RenderSectionAsync

Sections allow a child View to inject optional content into a layout.

Layout:

```cshtml
@await RenderSectionAsync("Scripts", required: false)
```

View:

```cshtml
@section Scripts {
    <script src="~/js/page.js"></script>
}
```

### Partial Views

Partial Views are reusable UI blocks.

Examples:

```text
Status badge
Priority badge
Complaint card
Page header
Empty state
Footer
Navigation
```

### Recommended Rule

Use layout pages for page-level structure.
Use partial views for repeated UI fragments.
Use ViewModels to pass typed data into Views and partial Views.

## Interview Explanation

In ASP.NET Core MVC, `_Layout.cshtml` provides a shared page structure for multiple Razor Views. It normally contains common UI such as navigation, sidebar, footer, CSS, JavaScript, and the `@RenderBody()` placeholder where each page renders its unique content. `_ViewStart.cshtml` is commonly used to assign the layout automatically so that every View does not need to repeat `Layout = "_Layout"`.

Partial Views are used for reusable UI sections such as complaint cards, badges, headers, and empty states. They help avoid duplicate Razor code and keep pages clean. In an enterprise MVC application, layout pages and partial views improve maintainability, consistency, and team productivity.
