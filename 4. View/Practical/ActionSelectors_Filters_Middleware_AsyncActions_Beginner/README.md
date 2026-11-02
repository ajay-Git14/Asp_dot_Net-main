# ASP.NET Core MVC Beginner Practice Project

## Topic

**Action Selectors, Filters, Middleware, and Async Actions**

This project is a small, runnable ASP.NET Core MVC application created for beginners. It uses one simple **Product Catalog** feature so that you can see all four concepts working together without unnecessary complexity.

## Learning outcomes

After completing this project, you will understand:

1. How `[HttpGet]` and `[HttpPost]` select different controller actions.
2. How `[ActionName]`, `[AcceptVerbs]`, and `[NonAction]` affect action selection.
3. How a custom action filter runs before and after a controller action.
4. How middleware runs in the HTTP request pipeline.
5. How `async`, `await`, and `Task<T>` are used in MVC actions and services.
6. How model validation and anti-forgery protection work in a POST request.

## Project structure

```text
ActionSelectors_Filters_Middleware_AsyncActions_Beginner/
|-- ActionFlowDemo.sln
|-- ActionFlowDemo.http
|-- README.md
|-- run.bat
|-- docs/
|   `-- Request-Flow.md
`-- src/
    `-- ActionFlowDemo/
        |-- Controllers/
        |-- Filters/
        |-- Middleware/
        |-- Models/
        |-- Services/
        |-- Views/
        |-- wwwroot/
        |-- ActionFlowDemo.csproj
        `-- Program.cs
```

## Prerequisites

- .NET 10 SDK
- Visual Studio 2022/2026, Visual Studio Code, or JetBrains Rider
- A modern browser

Check your SDK:

```powershell
dotnet --version
```

## Run the project

### Option 1: Command line

Open a terminal in the extracted folder and run:

```powershell
dotnet restore
dotnet build
dotnet run --project src/ActionFlowDemo/ActionFlowDemo.csproj
```

Open one of the URLs printed in the terminal. The included launch profile normally uses:

```text
http://localhost:5180
https://localhost:7180
```

### Option 2: Windows helper file

Double-click `run.bat`.

If HTTPS gives a certificate warning, run:

```powershell
dotnet dev-certs https --trust
```

## URLs to practise

| URL | Concept |
|---|---|
| `/` | Home page |
| `/Products/Index` | Async GET action |
| `/Products/Create` | GET action that displays a form |
| `/Products/Create` | POST action that processes the form |
| `/Products/Featured` | `[ActionName]` example |
| `/Products/Search?term=laptop` | Query string and async search |
| `/Products/Ping` | `[AcceptVerbs("GET", "HEAD")]` example |
| `/Products/SlowDemo` | Visible async delay example |

## Concept 1: Action selectors

Two methods can have the same action name because the HTTP verb selects the correct one:

```csharp
[HttpGet]
public IActionResult Create()
{
    return View();
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(CreateProductViewModel model)
{
    // Save the product.
}
```

- A browser opening the page sends `GET`.
- Submitting the form sends `POST`.
- `[ValidateAntiForgeryToken]` protects the form from cross-site request forgery.

Other selectors demonstrated in `ProductsController`:

```csharp
[ActionName("Featured")]
[AcceptVerbs("GET", "HEAD")]
[NonAction]
```

## Concept 2: Action filter

`ExecutionTimeFilter` runs around every action in `ProductsController`.

```text
Before action -> Controller action executes -> After action
```

It logs the action name and adds an `X-Action-Time-ms` response header.

The filter is registered in `Program.cs`:

```csharp
builder.Services.AddScoped<ExecutionTimeFilter>();
```

It is applied to the controller:

```csharp
[ServiceFilter(typeof(ExecutionTimeFilter))]
public class ProductsController : Controller
```

## Concept 3: Middleware

`RequestTimingMiddleware` runs for each MVC request. It records the total pipeline time and adds an `X-Request-Time-ms` header.

It is added to the request pipeline in `Program.cs`:

```csharp
app.UseRouting();
app.UseMiddleware<RequestTimingMiddleware>();
app.UseAuthorization();
```

Middleware works at the application/pipeline level. A filter works inside MVC around actions or results.

## Concept 4: Async actions

The controller awaits the product service:

```csharp
public async Task<IActionResult> Index()
{
    var products = await _productService.GetAllAsync();
    return View(products);
}
```

The included service uses a small `Task.Delay` only to simulate a database or API call. In a real application, the awaited operation would normally be EF Core, HTTP, file, or cloud I/O.

## Request flow

```text
Browser
  -> Routing
  -> RequestTimingMiddleware
  -> Authorization
  -> MVC ExecutionTimeFilter (before)
  -> ProductsController action
  -> Async ProductService
  -> MVC ExecutionTimeFilter (after)
  -> Razor View
  -> Browser response
```

Read `docs/Request-Flow.md` for a more detailed explanation.

## How to observe middleware and filter output

1. Run the project.
2. Open `/Products/Index`.
3. Look at the terminal logs.
4. Open browser Developer Tools.
5. Select the Network tab.
6. Reload the page.
7. Inspect the response headers.

You should see:

```text
X-Action-Time-ms
X-Request-Time-ms
```

## Beginner practice tasks

1. Add a `[HttpGet]` action named `CheapProducts`.
2. Return products whose price is below 1000.
3. Create another middleware that adds an `X-Application-Name` header.
4. Change the artificial delay in `InMemoryProductService`.
5. Add a category property to the product model and form.
6. Apply the action filter to only one action instead of the entire controller.

## Common mistakes

### Both Create methods run or route incorrectly

Make sure one method has `[HttpGet]` and the other has `[HttpPost]`.

### POST returns 400

Do not remove the form tag helper or anti-forgery token while using `[ValidateAntiForgeryToken]`.

### Async warning appears

Do not mark a method `async` unless it contains an awaited asynchronous operation.

### Middleware does not run for CSS

`UseStaticFiles()` appears before the custom middleware. Static files can finish the request early. This is intentional so you can learn how middleware order changes behaviour.

### View not found

The view folder and action name must match, or the view name must be specified explicitly.

## Recommended learning order

1. Run the project without changing anything.
2. Study `ProductsController.cs`.
3. Study `ExecutionTimeFilter.cs`.
4. Study `RequestTimingMiddleware.cs`.
5. Study `Program.cs` and its middleware order.
6. Complete the beginner practice tasks.
