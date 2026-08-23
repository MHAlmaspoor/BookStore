using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

[ApiController]
[Route("api/health")]
public sealed class HealthController : ControllerBase
{
    private readonly HealthCheckService _healthCheckService;

    public HealthController(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    [HttpGet("live")]
    public IActionResult Live()
    {
        return Ok(new
        {
            status = "Healthy"
        });
    }

    [HttpGet("ready")]
    public async Task<IActionResult> Ready(CancellationToken cancellationToken)
    {
        var result = await _healthCheckService.CheckHealthAsync(check=> check.Tags.Contains("ready"), cancellationToken);

        var response = new
        {
            status = result.Status.ToString(),
            checks = result.Entries.ToDictionary( x=>x.Key, x => new
            {
                status = x.Value.Status.ToString(),
                description = x.Value.Description
            })
        };

        return result.Status == HealthStatus.Healthy ? Ok(response) : StatusCode(StatusCodes.Status503ServiceUnavailable, response);
    }

    [HttpGet("redis")]
    public async Task<IActionResult> Redis(CancellationToken cancellationToken)
    {
        var result = await _healthCheckService.CheckHealthAsync(check => check.Tags.Contains("redis"), cancellationToken);

        var response = new
        {
            status = result.Status.ToString(),
            checks = result.Entries.ToDictionary(x => x.Key, x=>new{
                status = x.Value.Status.ToString(),
                description = x.Value.Description 
            })
        };

        return result.Status == HealthStatus.Healthy ? Ok(response) : StatusCode(StatusCodes.Status503ServiceUnavailable, response);
    }
}

    /*
    app.MapHealthChecks(
        "/health/live",
        new HealthCheckOptions
        {
            Predicate = _ => false
        });

    app.MapHealthChecks(
        "/health/ready",
        new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready")
        });
    */


