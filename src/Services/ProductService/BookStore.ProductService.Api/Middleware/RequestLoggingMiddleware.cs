using System.Diagnostics;

namespace BookStore.ProductService.Api.Middleware;

public sealed class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        //TraceId with Context.TraceIdenifier
        //var traceId = context.TraceIdentifier;

        //TraceId with Activity.TraceId
        // var traceId = Activity.Current?.TraceId.ToString();

        // using var scope = _logger.BeginScope(new Dictionary<string, object>
        // {
        //     ["TraceId"] = traceId
        // });

        _logger.LogInformation("HTTP {Method} {Path} started.", context.Request.Method, context.Request.Path);

        await _next(context);

        _logger.LogInformation("HTTP {Method} {Path} completed with status code {StausCode}.",
            context.Request.Method, context.Request.Path, context.Response.StatusCode);
    }
}
