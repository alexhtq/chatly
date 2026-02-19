using Chatly.Messages.Api.Database;
using Chatly.Messages.Api.Database.Interceptors;
using Chatly.Messages.Api.Middleware;
using Chatly.Messages.Api.Services;
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
        builder.Services.AddValidatorsFromAssemblyContaining<CreateMessageValidator>();
        builder.Services.AddScoped<IMessageService, MessageService>();
        builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.AddProblemDetailsForFailedRequests();
        builder.AddDatabase();
        builder.AddCorsPolicy();
    }

    private static void AddProblemDetailsForFailedRequests(this WebApplicationBuilder builder)
    {
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
    }

    private static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<MessagesContext>(options =>
        {
            var dbConnectionString = builder.Configuration.GetConnectionString("Database")
                ?? throw new InvalidOperationException("ConnectionStrings:Database configuration is required");

            options
                .UseNpgsql(dbConnectionString)
                //.LogTo(Console.WriteLine)
                .AddInterceptors(
                    new TimestampInterceptor(
                        builder.Services.BuildServiceProvider().GetRequiredService<TimeProvider>()));
        });
    }

    private static void AddCorsPolicy(this WebApplicationBuilder builder)
    {
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