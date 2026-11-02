# Understanding the Request Flow

## The simple picture

When a user opens this URL:

```text
/Products/Details/1
```

ASP.NET Core processes the request in stages.

## Stage 1: Routing

Routing reads the URL and selects the matching controller and action.

```text
Controller: ProductsController
Action: Details
Parameter: id = 1
```

## Stage 2: Middleware

`RequestTimingMiddleware` surrounds the remaining pipeline.

```text
Middleware starts timer
    -> remaining pipeline executes
Middleware stops timer
```

Middleware is suitable for application-wide concerns such as:

- request logging
- exception handling
- authentication
- authorization
- CORS
- response compression
- request timing

## Stage 3: MVC filter

The `ExecutionTimeFilter` runs inside MVC around the selected action.

```text
Filter before
    -> controller action
Filter after
```

Filters are suitable for MVC-specific concerns such as:

- action logging
- action validation
- result modification
- exception handling around MVC actions
- authorization rules around controllers/actions

## Stage 4: Async controller action

The controller calls the service and awaits it:

```csharp
var product = await _productService.GetByIdAsync(id);
```

While the action is waiting for I/O, the request thread is not blocked doing useless work. The server can use its resources for other requests.

## Stage 5: Razor result

The controller passes the model to a Razor view:

```csharp
return View(product);
```

The view produces HTML and ASP.NET Core sends the response to the browser.

## Middleware versus filter

| Question | Middleware | Filter |
|---|---|---|
| Where does it run? | HTTP pipeline | MVC pipeline |
| Does it know the selected action? | It can after routing | Yes |
| Can it cover non-MVC endpoints? | Yes | No |
| Typical use | Logging, security, errors | Action/result concerns |
| Registration | `app.UseMiddleware<T>()` | Attribute or MVC options |

## Action selector decision

For `/Products/Create`, the URL is the same for both methods. The HTTP verb decides which method is selected.

```text
GET  /Products/Create -> Show the form
POST /Products/Create -> Process the form
```

This is one of the most common patterns in ASP.NET Core MVC.
