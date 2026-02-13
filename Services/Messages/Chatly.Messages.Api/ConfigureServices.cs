using Chatly.Messages.Api.Database;
using Chatly.Messages.Api.Middleware;
using Chatly.Shared.Messages.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chatly.Messages.Api;

public static class ConfigureServices
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        
        builder.Services.AddOpenApi();
        
        builder.Services.AddDbContext<MessagesContext>(opts =>
            opts.UseInMemoryDatabase("MessagesInMemoryDB"));
        
        builder.Services.AddValidatorsFromAssemblyContaining<AddMessageValidator>();
        
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();   
        
        builder.Services.AddProblemDetails(opts =>
            opts.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                context.ProblemDetails.Extensions.Add(
                    "requestId", context.HttpContext.TraceIdentifier);
            });

        builder.Services.AddCors(options =>
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy
                    .WithOrigins("http://localhost:4000")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }));
    }
}