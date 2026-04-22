using AppCore.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WebApi.Exceptions;

public class ProblemDetailsExceptionHandler(
    ProblemDetailsFactory factory,
    ILogger<ProblemDetailsExceptionHandler> logger,
    IHostEnvironment env) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Always log the full exception so it appears in the console
        logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        int statusCode;
        string title;

        if (exception is ContactNotFoundException or NoteNotFoundException)
        {
            statusCode = StatusCodes.Status404NotFound;
            title      = "Resource not found";
        }
        else
        {
            statusCode = StatusCodes.Status500InternalServerError;
            title      = "An unexpected error occurred";
        }

        var problem = factory.CreateProblemDetails(
            context,
            statusCode,
            title,
            detail: env.IsDevelopment() ? exception.ToString() : exception.Message);

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}