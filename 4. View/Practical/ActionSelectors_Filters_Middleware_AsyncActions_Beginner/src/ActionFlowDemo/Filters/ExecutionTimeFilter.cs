using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ActionFlowDemo.Filters;

// An action filter runs inside MVC, before and after a controller action.
public sealed class ExecutionTimeFilter : IAsyncActionFilter
{
    private readonly ILogger<ExecutionTimeFilter> _logger;

    public ExecutionTimeFilter(ILogger<ExecutionTimeFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var controllerName = context.RouteData.Values["controller"]?.ToString();
        var actionName = context.RouteData.Values["action"]?.ToString();
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "FILTER BEFORE: {Controller}/{Action}",
            controllerName,
            actionName);

        // Calling next() allows the controller action to execute.
        var executedContext = await next();

        stopwatch.Stop();

        // At this point the action has completed, but the MVC result normally has not been written yet.
        context.HttpContext.Response.Headers["X-Action-Time-ms"] =
            stopwatch.ElapsedMilliseconds.ToString();

        _logger.LogInformation(
            "FILTER AFTER: {Controller}/{Action} completed in {ElapsedMilliseconds} ms. Exception: {ExceptionType}",
            controllerName,
            actionName,
            stopwatch.ElapsedMilliseconds,
            executedContext.Exception?.GetType().Name ?? "None");
    }
}
