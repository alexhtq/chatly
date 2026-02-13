using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Chatly.Messages.Api.Middleware;

public class GlobalExceptionHandler(ProblemDetailsFactory problemDetailsFactory) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exc,
        CancellationToken cancellationToken)
    {
        var problemDetails = problemDetailsFactory.CreateProblemDetails(
            httpContext: httpContext,
            statusCode: StatusCodes.Status500InternalServerError);

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }
}