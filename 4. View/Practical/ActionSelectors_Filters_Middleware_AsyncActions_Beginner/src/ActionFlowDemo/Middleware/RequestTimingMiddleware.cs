using System.Diagnostics;

namespace ActionFlowDemo.Middleware;

// Middleware runs in the ASP.NET Core HTTP request pipeline.
public sealed class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(
        RequestDelegate next,
        ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "MIDDLEWARE START: {Method} {Path}",
            context.Request.Method,
            context.Request.Path);

        // OnStarting runs immediately before ASP.NET Core sends response headers.
        context.Response.OnStarting(() =>
        {
            context.Response.Headers["X-Request-Time-ms"] =
                stopwatch.ElapsedMilliseconds.ToString();

            return Task.CompletedTask;
        });

        try
        {
            // Pass control to the next middleware/component in the pipeline.
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            _logger.LogInformation(
                "MIDDLEWARE END: {Method} {Path} returned {StatusCode} in {ElapsedMilliseconds} ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}
