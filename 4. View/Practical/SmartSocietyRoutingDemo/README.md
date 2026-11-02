# Smart Society Routing Demo — ASP.NET Core MVC (.NET 10)

A beginner-friendly, database-free routing project with **exactly one MVC controller**:

```text
Controllers/RoutingDemoController.cs
```

The project intentionally keeps all routing examples together so a learner can compare them easily.

## Requirements

- .NET 10 SDK
- Visual Studio 2026, Visual Studio Code, or another C# editor

## Run The Project

Open a terminal inside the project folder:

```bash
dotnet restore
dotnet build
dotnet run
```

Then open the HTTP address printed by the terminal. The included launch profile uses:

```text
http://localhost:5091
```

## Project Structure

```text
SmartSocietyRoutingDemo/
├── Controllers/
│   └── RoutingDemoController.cs   # The only controller
├── Models/
│   ├── Complaint.cs
│   └── ComplaintInputModel.cs
├── ViewModels/
├── Views/
├── wwwroot/css/site.css
├── Program.cs                     # Conventional, Area, and attribute mapping
└── SmartSocietyRoutingDemo.csproj
```

## Complete Concept Coverage

| Concept | File/Example |
|---|---|
| Routing meaning and endpoint routing | `Program.cs` pipeline |
| Conventional routing | `/RoutingDemo/Optional/2` |
| Default MVC route | `{controller=RoutingDemo}/{action=Index}/{id?}` |
| Route template structure | All routes in `Program.cs` and controller attributes |
| controller, action, id values | Default route and RouteResult page |
| Required route parameter | `/complaints/1` |
| Optional route parameter | `/RoutingDemo/Optional` |
| Default parameter value | `/RoutingDemo/DefaultValue` |
| Default value in route template | `/pages` |
| Attribute routing | `/track` |
| Controller/action route combination | `/society/complaints/2` plus the exact two-level syntax in controller comments |
| `[controller]` and `[action]` tokens | `/RoutingDemo/TokenDemo/7` |
| HTTP verb route templates | `GET /verb-demo/2`, `POST /verb-demo` |
| Route constraints | `int`, `min`, `alpha`, `minlength`, `maxlength` |
| Constraint vs validation | `Track` action |
| Named routes | `complaint-details`, `track-complaint`, and others |
| Razor Tag Helpers | `Views/RoutingDemo/Index.cshtml` |
| `RedirectToAction` | `/go/list` |
| `RedirectToRoute` | Create POST and `/go/complaint/2` |
| `Url.Action` | Home page URL-generation table |
| `Url.RouteUrl` | Home page URL-generation table |
| Conventional route order | Specific routes before default route in `Program.cs` |
| Attribute route precedence | `/reports/latest` and `/reports/2026` |
| Ambiguous route prevention | `/find/by-id/2`, `/find/by-status/Open` |
| Route values vs query strings | `/search?term=lift&page=2` |
| Areas | `/Admin/Dashboard` |
| Mixed routing strategy | Conventional and attribute actions in one controller |

## Important Single-Controller Design Note

A controller-level `[Route("prefix")]` changes all actions in that controller into attribute-routed actions. This project must also demonstrate live conventional actions in the same single controller, so it does not place `[Route]` on the whole controller.

The controller-level plus action-level combination concept is still included clearly as a commented code example:

```csharp
[Route("society")]
public class ExampleController : Controller
{
    [HttpGet("complaints/{id:int}")]
    public IActionResult Details(int id)
    {
        // Final URL: /society/complaints/5
    }
}
```

This avoids adding a second live controller while ensuring the concept is not missed.

## Suggested Test URLs

```text
/
/RoutingDemo/Index?status=Open&search=lift&pageNumber=1
/complaints/1
/complaints/status/Open
/RoutingDemo/Optional
/RoutingDemo/Optional/2
/RoutingDemo/DefaultValue?pageNumber=3
/RoutingDemo/Create
/track
/track/CMP-2026-0001
/RoutingDemo/TokenDemo/7
/verb-demo/2
/pages
/pages/4
/category
/category/Maintenance
/building/A/floor/3
/society/complaints/2
/reports/latest
/reports/2026
/search?term=lift&page=2
/find/by-id/2
/find/by-status/Open
/go/list
/go/complaint/3
/Admin/Dashboard
/routing/help
```

## Expected Failure Tests

These failures are useful evidence that constraints and validation are working:

```text
/complaints/abc          -> route does not match the integer constraint
/complaints/0            -> route does not match min(1)
/building/A/floor/-1     -> route does not match min(0)
/track/ABCDE             -> route matches length, then business validation returns Bad Request
```

## Beginner Learning Order

1. Start with `Program.cs`.
2. Read the `Index`, `Details`, and `Optional` actions.
3. Run each link on the home page.
4. Observe route values on the RouteResult page.
5. Read the attribute-routed actions.
6. Compare route values with query-string values.
7. Test redirects, POST routing, and Area routing.
8. Open `/routing/help` for the full checklist.
