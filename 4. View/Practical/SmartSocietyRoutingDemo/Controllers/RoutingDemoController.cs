using Microsoft.AspNetCore.Mvc;
using SmartSocietyRoutingDemo.Models;
using SmartSocietyRoutingDemo.ViewModels;

namespace SmartSocietyRoutingDemo.Controllers;

// This is the ONLY MVC controller in the project.
// Some actions use conventional routing and others use attribute routing.
public class RoutingDemoController : Controller
{
    // In-memory data keeps the routing example simple and database-free.
    private static readonly List<Complaint> Complaints =
    [
        new()
        {
            Id = 1,
            ReferenceNumber = "CMP-2026-0001",
            Title = "Lift is not working",
            Status = "Open",
            Block = "A"
        },
        new()
        {
            Id = 2,
            ReferenceNumber = "CMP-2026-0002",
            Title = "Water leakage near parking",
            Status = "InProgress",
            Block = "B"
        },
        new()
        {
            Id = 3,
            ReferenceNumber = "CMP-2026-0003",
            Title = "Street light requires repair",
            Status = "Resolved",
            Block = "C"
        }
    ];

    // ============================================================
    // 1. CONVENTIONAL ROUTING + QUERY STRING VALUES
    // Examples:
    //   /
    //   /RoutingDemo
    //   /RoutingDemo/Index
    //   /RoutingDemo/Index?status=Open&search=lift&pageNumber=1
    // ============================================================
    [HttpGet]
    public IActionResult Index(
        string? status,
        string? search,
        int pageNumber = 1)
    {
        if (pageNumber < 1)
        {
            pageNumber = 1;
        }

        IEnumerable<Complaint> result = Complaints;

        if (!string.IsNullOrWhiteSpace(status))
        {
            result = result.Where(complaint =>
                complaint.Status.Equals(
                    status,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            result = result.Where(complaint =>
                complaint.Title.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase));
        }

        var model = new RoutingHomeViewModel
        {
            Complaints = result.ToList(),
            SelectedStatus = status,
            Search = search,
            PageNumber = pageNumber,

            // Url.Action generates a URL from controller/action route values.
            UrlFromUrlAction = Url.Action(
                nameof(Details),
                "RoutingDemo",
                new { id = 1 }) ?? string.Empty,

            // Url.RouteUrl generates a URL by a named route.
            UrlFromNamedRoute = Url.RouteUrl(
                "complaint-details",
                new { id = 1 }) ?? string.Empty
        };

        return View(model);
    }

    // ============================================================
    // 2. REQUIRED ROUTE PARAMETER + ROUTE CONSTRAINT
    // Friendly named conventional route from Program.cs:
    //   /complaints/1
    // The :int:min(1) constraints decide whether the route matches.
    // The checks inside the action validate application input/data.
    // ============================================================
    [HttpGet]
    public IActionResult Details(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Complaint id must be greater than zero.");
        }

        var complaint = Complaints.FirstOrDefault(item => item.Id == id);

        if (complaint is null)
        {
            return NotFound($"Complaint {id} was not found.");
        }

        return View(complaint);
    }

    // ============================================================
    // 3. ANOTHER NAMED CONVENTIONAL ROUTE
    //   /complaints/status/Open
    // The :alpha constraint allows alphabetic route values.
    // ============================================================
    [HttpGet]
    public IActionResult ByStatus(string status)
    {
        var matchingComplaints = Complaints
            .Where(complaint => complaint.Status.Equals(
                status,
                StringComparison.OrdinalIgnoreCase))
            .ToList();

        ViewBag.SelectedStatus = status;
        return View(matchingComplaints);
    }

    // ============================================================
    // 4. OPTIONAL ROUTE PARAMETER
    // Default conventional route examples:
    //   /RoutingDemo/Optional
    //   /RoutingDemo/Optional/2
    // id is nullable because the route value is optional.
    // ============================================================
    [HttpGet]
    public IActionResult Optional(int? id)
    {
        var messages = id.HasValue
            ? new List<string> { $"Optional id was supplied: {id.Value}" }
            : new List<string> { "No optional id was supplied." };

        return RouteResult(
            "Optional Route Parameter",
            "The {id?} segment in the default route is optional.",
            messages);
    }

    // ============================================================
    // 5. DEFAULT ACTION PARAMETER VALUE
    //   /RoutingDemo/DefaultValue
    //   /RoutingDemo/DefaultValue?pageNumber=3
    // ============================================================
    [HttpGet]
    public IActionResult DefaultValue(int pageNumber = 1)
    {
        return RouteResult(
            "Default Parameter Value",
            "When pageNumber is missing, C# supplies the default value 1.",
            [$"Current pageNumber: {pageNumber}"]);
    }

    // ============================================================
    // 6. GET AND POST ACTION SELECTION
    // Both actions use the same action name, but the HTTP method differs.
    // ============================================================
    [HttpGet]
    public IActionResult Create()
    {
        return View(new ComplaintInputModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ComplaintInputModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var nextId = Complaints.Max(item => item.Id) + 1;
        var complaint = new Complaint
        {
            Id = nextId,
            ReferenceNumber = $"CMP-2026-{nextId:0000}",
            Title = model.Title,
            Status = "Open",
            Block = model.Block
        };

        Complaints.Add(complaint);
        TempData["SuccessMessage"] = "Complaint created successfully.";

        // Redirect by named route. This demonstrates RedirectToRoute.
        return RedirectToRoute(
            "complaint-details",
            new { id = complaint.Id });
    }

    // ============================================================
    // 7. ATTRIBUTE ROUTING + NAMED ROUTE
    //   /track
    // ============================================================
    [HttpGet("track", Name = "track-home")]
    public IActionResult TrackHome()
    {
        return RouteResult(
            "Attribute Route",
            "The URL /track is written directly on the action with HttpGet.",
            ["Try /track/CMP-2026-0001 next."]);
    }

    // ============================================================
    // 8. REQUIRED ATTRIBUTE ROUTE PARAMETER + MULTIPLE CONSTRAINTS
    //   /track/CMP-2026-0001
    // Constraints control matching. The action still validates meaning.
    // ============================================================
    [HttpGet(
        "track/{referenceNumber:minlength(5):maxlength(30)}",
        Name = "track-complaint")]
    public IActionResult Track(string referenceNumber)
    {
        if (!referenceNumber.StartsWith(
                "CMP-",
                StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(
                "The route matched its length constraints, but business validation requires the CMP- prefix.");
        }

        var complaint = Complaints.FirstOrDefault(item =>
            item.ReferenceNumber.Equals(
                referenceNumber,
                StringComparison.OrdinalIgnoreCase));

        if (complaint is null)
        {
            return NotFound(
                $"No complaint exists for reference {referenceNumber}.");
        }

        return View("Details", complaint);
    }

    // ============================================================
    // 9. ROUTE TOKENS: [controller] AND [action]
    // Generated path:
    //   /RoutingDemo/TokenDemo
    // ============================================================
    [HttpGet(
        "[controller]/[action]/{id:int?}",
        Name = "token-demo")]
    public IActionResult TokenDemo(int? id)
    {
        return RouteResult(
            "Route Tokens",
            "[controller] becomes RoutingDemo and [action] becomes TokenDemo.",
            [$"Optional id: {id?.ToString() ?? "not supplied"}"]);
    }

    // ============================================================
    // 10. HTTP VERB ATTRIBUTE WITH A ROUTE TEMPLATE
    // GET /verb-demo/2
    // ============================================================
    [HttpGet(
        "verb-demo/{id:int:min(1)}",
        Name = "verb-get")]
    public IActionResult VerbGet(int id)
    {
        return RouteResult(
            "HttpGet With Route Template",
            "This action accepts only GET requests matching /verb-demo/{id}.",
            [$"Received id: {id}"]);
    }

    // POST /verb-demo
    [HttpPost("verb-demo", Name = "verb-post")]
    [ValidateAntiForgeryToken]
    public IActionResult VerbPost(string message)
    {
        return RouteResult(
            "HttpPost With Route Template",
            "This action accepts only POST requests sent to /verb-demo.",
            [$"Posted message: {message}"]);
    }

    // ============================================================
    // 11. DEFAULT VALUE INSIDE AN ATTRIBUTE ROUTE TEMPLATE
    //   /pages
    //   /pages/4
    // ============================================================
    [HttpGet(
        "pages/{pageNumber:int=1}",
        Name = "page-default")]
    public IActionResult PageWithRouteDefault(int pageNumber)
    {
        return RouteResult(
            "Default Route Value",
            "The route template supplies pageNumber = 1 when the segment is absent.",
            [$"Current page: {pageNumber}"]);
    }

    // ============================================================
    // 12. OPTIONAL ATTRIBUTE ROUTE SEGMENT
    //   /category
    //   /category/Maintenance
    // ============================================================
    [HttpGet(
        "category/{name?}",
        Name = "optional-category")]
    public IActionResult Category(string? name)
    {
        return RouteResult(
            "Optional Attribute Route Segment",
            "The question mark makes the category name optional.",
            [$"Category: {name ?? "All categories"}"]);
    }

    // ============================================================
    // 13. MULTIPLE ROUTE PARAMETERS AND CONSTRAINTS
    //   /building/A/floor/3
    // ============================================================
    [HttpGet(
        "building/{block:alpha}/floor/{floor:int:min(0)}",
        Name = "building-floor")]
    public IActionResult BuildingFloor(
        [FromRoute] string block,
        [FromRoute] int floor)
    {
        return RouteResult(
            "Multiple Route Parameters",
            "The URL supplies block and floor as separate route values.",
            [$"Block: {block}", $"Floor: {floor}"]);
    }

    // ============================================================
    // 14. WORKING RESULT OF A CONTROLLER PREFIX + ACTION TEMPLATE
    //   /society/complaints/2
    // In a multi-controller project this is commonly written as:
    //   controller: [Route("society")]
    //   action:     [HttpGet("complaints/{id:int}")]
    // The single-controller requirement uses the final combined template.
    // ============================================================
    [HttpGet(
        "society/complaints/{id:int:min(1)}",
        Name = "combined-route-equivalent")]
    public IActionResult CombinedRouteEquivalent(int id)
    {
        return RedirectToRoute("complaint-details", new { id });
    }

    // ============================================================
    // 15. ATTRIBUTE ROUTE PRECEDENCE
    // Literal segments are more specific than parameter segments.
    // Controller source-code order should not be used to resolve routes.
    // ============================================================
    [HttpGet("reports/latest", Name = "latest-report")]
    public IActionResult LatestReport()
    {
        return RouteResult(
            "Attribute Route Precedence — Literal",
            "The literal route /reports/latest is more specific than a parameter route.",
            ["Selected the latest report endpoint."]);
    }

    [HttpGet("reports/{year:int:min(2000):max(2100)}", Name = "report-year")]
    public IActionResult ReportByYear(int year)
    {
        return RouteResult(
            "Attribute Route Precedence — Constrained Parameter",
            "The year route uses an integer range constraint.",
            [$"Selected report year: {year}"]);
    }

    // ============================================================
    // 16. ROUTE VALUE VS QUERY-STRING VALUE
    // Route: /search
    // Query string: ?term=lift&page=2
    // ============================================================
    [HttpGet("search", Name = "search-complaints")]
    public IActionResult Search(string? term, int page = 1)
    {
        return RouteResult(
            "Route Values Versus Query String",
            "The path selects the action; term and page are supplied by the query string.",
            [
                $"Search term: {term ?? "not supplied"}",
                $"Page: {page}"
            ]);
    }

    // ============================================================
    // 17. ABSOLUTE ATTRIBUTE ROUTE
    // The ~/ prefix ignores any future controller-level route prefix.
    //   /routing/help
    // ============================================================
    [HttpGet("~/routing/help", Name = "routing-help")]
    public IActionResult Help()
    {
        return View();
    }

    // ============================================================
    // 18. AMBIGUITY PREVENTION
    // These routes are intentionally different and constrained.
    //   /find/by-id/2
    //   /find/by-status/Open
    // ============================================================
    [HttpGet("find/by-id/{id:int}", Name = "find-by-id")]
    public IActionResult FindById(int id)
    {
        return RedirectToRoute("complaint-details", new { id });
    }

    [HttpGet("find/by-status/{status:alpha}", Name = "find-by-status")]
    public IActionResult FindByStatus(string status)
    {
        return RedirectToRoute("complaint-status", new { status });
    }

    // ============================================================
    // 19. URL GENERATION AND REDIRECT HELPERS
    // RedirectToAction uses controller/action route values.
    // RedirectToRoute uses a route name.
    // ============================================================
    [HttpGet("go/list", Name = "go-list")]
    public IActionResult GoToList()
    {
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("go/complaint/{id:int:min(1)}", Name = "go-complaint")]
    public IActionResult GoToComplaint(int id)
    {
        return RedirectToRoute("complaint-details", new { id });
    }

    // ============================================================
    // 20. AREA ROUTING WITH THE SAME SINGLE CONTROLLER
    // Program.cs maps /Admin/Dashboard/{id?}.
    // [Area("Admin")] supplies the required area route value.
    // ============================================================
    [Area("Admin")]
    [HttpGet]
    public IActionResult AreaDashboard(int? id)
    {
        var model = BuildRouteResult(
            "Admin Area Route",
            "Areas add an area route value and are normally used to organize larger feature groups.",
            [$"Optional dashboard id: {id?.ToString() ?? "not supplied"}"]);

        // Explicit path keeps this beginner project to one shared Views folder.
        return View("~/Views/RoutingDemo/AreaDashboard.cshtml", model);
    }

    // ============================================================
    // CONTROLLER-LEVEL + ACTION-LEVEL ATTRIBUTE ROUTE CONCEPT
    // ------------------------------------------------------------
    // A controller-level prefix applies to every action in that
    // controller and combines with each action-level template:
    //
    // [Route("society")]
    // public class ExampleController : Controller
    // {
    //     [HttpGet("complaints/{id:int}")]
    //     public IActionResult Details(int id) { ... }
    // }
    //
    // Final URL: /society/complaints/5
    //
    // This live project keeps conventional and attribute-routed actions
    // together in ONE controller, so it deliberately avoids placing a
    // [Route] prefix on the whole controller. The concept is also
    // explained and tested in README.md.
    // ============================================================

    private IActionResult RouteResult(
        string heading,
        string explanation,
        List<string> messages)
    {
        var model = BuildRouteResult(heading, explanation, messages);
        return View("RouteResult", model);
    }

    private RouteResultViewModel BuildRouteResult(
        string heading,
        string explanation,
        List<string> messages)
    {
        var routeValues = RouteData.Values.ToDictionary(
            pair => pair.Key,
            pair => pair.Value?.ToString() ?? "(null)");

        var queryValues = Request.Query.ToDictionary(
            pair => pair.Key,
            pair => pair.Value.ToString());

        return new RouteResultViewModel
        {
            Heading = heading,
            Explanation = explanation,
            RequestPath = Request.Path,
            HttpMethod = Request.Method,
            EndpointName = HttpContext.GetEndpoint()?.DisplayName ?? "Unknown endpoint",
            RouteValues = routeValues,
            QueryValues = queryValues,
            Messages = messages
        };
    }
}
