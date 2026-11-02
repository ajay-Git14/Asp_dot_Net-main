using ActionFlowDemo.Filters;
using ActionFlowDemo.Models;
using ActionFlowDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace ActionFlowDemo.Controllers;

// Route tokens create URLs such as /Products/Index and /Products/Create.
[Route("[controller]/[action]")]

// This custom action filter runs before and after every action in this controller.
[ServiceFilter(typeof(ExecutionTimeFilter))]
public sealed class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        IProductService productService,
        ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    // GET /Products/Index
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllAsync();
        return View(products);
    }

    // GET /Products/Details?id=1
    // GET /Products/Details/1 also works through the conventional route only when
    // no controller-level attribute route is used, so this project uses the query-string form.
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product is null)
        {
            return NotFound($"No product was found with ID {id}.");
        }

        return View(product);
    }

    // GET /Products/Create
    // The GET version only displays the empty form.
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateProductViewModel());
    }

    // POST /Products/Create
    // The POST version receives and processes submitted form data.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var product = await _productService.AddAsync(model);
        TempData["SuccessMessage"] = CreateStatusMessage(product);

        return RedirectToAction(nameof(Details), new { id = product.Id });
    }

    // The public method name is ShowFeaturedProducts, but its action name is Featured.
    // GET /Products/Featured
    [HttpGet]
    [ActionName("Featured")]
    public async Task<IActionResult> ShowFeaturedProducts()
    {
        var products = await _productService.GetFeaturedAsync();
        return View("Featured", products);
    }

    // GET /Products/Search?term=laptop
    [HttpGet]
    public async Task<IActionResult> Search(string? term)
    {
        ViewBag.Term = term;
        var products = await _productService.SearchAsync(term);
        return View(products);
    }

    // Both GET and HEAD requests can select this action.
    [AcceptVerbs("GET", "HEAD")]
    public IActionResult Ping()
    {
        Response.Headers["X-Ping-Message"] = "Action selector matched successfully";
        return Content("ProductsController is available.");
    }

    // This action makes the async wait easy to observe in browser developer tools.
    [HttpGet]
    public async Task<IActionResult> SlowDemo()
    {
        _logger.LogInformation("SlowDemo started.");
        await Task.Delay(1200);
        _logger.LogInformation("SlowDemo completed.");

        return Content("The async action completed after approximately 1.2 seconds.");
    }

    // NonAction tells MVC that this public helper method must never become an endpoint.
    [NonAction]
    public string CreateStatusMessage(Product product)
    {
        return $"Product '{product.Name}' was created successfully.";
    }
}
