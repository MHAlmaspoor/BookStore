using BookStore.IdentityService.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
namespace BookStore.IdentityService.Api.ExceptionHandling;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger=logger;
    }

    public async ValueTask<bool> TryHandleAsync( HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);

        ProblemDetails problem;

        switch (exception)
        {
            case ValidationException validationException:

                problem = new ValidationProblemDetails(
                    validationException.Errors.GroupBy(x=>x.PropertyName).ToDictionary(
                        g=>g.Key,
                        g=>g.Select( e=>e.ErrorMessage).ToArray()))
                            {
                                Title="Validation Error",
                                Status= StatusCodes.Status400BadRequest
                            };
                break;
            case DomainException domainException:
                problem = new ProblemDetails
                {
                    Title="Domain Error.",
                    Detail=domainException.Message,
                    Status=StatusCodes.Status400BadRequest
                };
                break;
            default:
                problem =new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An unexpected error occurred.",
                    Status= StatusCodes.Status500InternalServerError
                };
                break;
        }
        httpContext.Response.StatusCode = problem.Status!.Value;

        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}
