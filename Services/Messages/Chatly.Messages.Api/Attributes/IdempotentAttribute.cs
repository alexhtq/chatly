using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Chatly.Messages.Api.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class IdempotentAttribute : Attribute, IAsyncActionFilter
{
    private const string IdempotencyKeyHeader = "Idempotency-Key";
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Validate header.
        if (!context.HttpContext.Request.Headers.TryGetValue(
                IdempotencyKeyHeader, out StringValues idempotencyKeyValue) ||
            !Guid.TryParse(idempotencyKeyValue, out Guid idempotencyKey))
        {
            var problemDetailsFactory = context.HttpContext.RequestServices
                .GetRequiredService<ProblemDetailsFactory>();
            
            var problemDetails = problemDetailsFactory.CreateProblemDetails(
                httpContext: context.HttpContext,
                statusCode: StatusCodes.Status400BadRequest,
                detail: $"Invalid or missing {IdempotencyKeyHeader} header."
            );
            
            context.Result = new BadRequestObjectResult(problemDetails);
            
            return;
        }

        var cache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
        var cacheKey = $"idempotency:{idempotencyKey}";
        var cachedResult = cache.Get<IActionResult>(cacheKey);
        
        // Cache hit.
        if (cachedResult is not null)
        {
            context.Result = cachedResult; 
            return;
        }

        // Cache miss.
        var executedContext = await next(); 
        if (executedContext.Result is CreatedAtActionResult createdResult)
        {
            cache.Set(cacheKey, createdResult, _cacheDuration);
        }
    }
}
