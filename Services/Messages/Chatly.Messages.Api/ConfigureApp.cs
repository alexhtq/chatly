using Chatly.Messages.Api.Database;

namespace Chatly.Messages.Api;

public static class ConfigureApp
{
    public static async Task Configure(this WebApplication app)
    {            
        app.MapOpenApi();
        app.UseSwaggerUI(opts =>
            opts.SwaggerEndpoint("/openapi/v1.json", "Messenger"));

        await app.HandleDatabaseSchemaAsync();
        
        app.UseExceptionHandler();
        app.UseCors("AllowFrontend");
        
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

    private static async Task HandleDatabaseSchemaAsync(this WebApplication app)
    {
        // Temporary solution that drops and creates the database schema on each run.
        // Will be replaced with proper migrations in the future.
        var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MessagesContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }
}