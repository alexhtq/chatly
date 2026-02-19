using Chatly.Messages.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace Chatly.Messages.Api;

public static class ConfigureApp
{
    public static async Task Configure(this WebApplication app)
    {            
        app.MapOpenApi();
        app.UseSwaggerUI(opts =>
            opts.SwaggerEndpoint("/openapi/v1.json", "Messenger"));

        await app.ApplyMigrationsAsync();
        
        app.UseExceptionHandler();
        app.UseCors("AllowFrontend");
        
        // Serving the Blazor app from the API is for faster development and testing.
        // In production, the frontend is deployed in a separate container.
        var hostBlazorApp = app.Configuration.GetValue<bool?>("HostBlazorApp") 
            ?? throw new InvalidOperationException("HostBlazorApp configuration is required");
        
        if (hostBlazorApp is true)
        {
            app.UseWebAssemblyDebugging();
            app.UseStaticFiles();
            app.UseBlazorFrameworkFiles();
        }

        app.MapControllers();

        if (hostBlazorApp is true)
        {
            app.MapFallbackToFile("index.html");
        }
    }

    private static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await using var messagesContext = scope.ServiceProvider.GetRequiredService<MessagesContext>();

        try
        {
            await messagesContext.Database.MigrateAsync();
            app.Logger.LogInformation("Database migrations applied successfully.");
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "An error occurred while applying database migrations.");
            throw;
        }
    }
}