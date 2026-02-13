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
        
        builder.Services.AddDbContext<MessagesContext>(options =>
        {
            options.UseInMemoryDatabase("MessagesInMemoryDB");    
        });
        
        builder.Services.AddValidatorsFromAssemblyContaining<AddMessageValidator>();
        
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();   
        
        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                context.ProblemDetails.Extensions.Add(
                    "requestId", context.HttpContext.TraceIdentifier);
            };      
        });

        var allowedOrigin = builder.Configuration.GetValue<string>("Cors:AllowedOrigin") ?? string.Empty;
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy
                    .WithOrigins(allowedOrigin)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}