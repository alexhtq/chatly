namespace Chatly.Messages.Api;

public static class ConfigureApp
{
    public static async Task Configure(this WebApplication app)
    {            
        if (app.Environment.IsDevelopment() is true)
        {
            app.MapOpenApi();
            app.UseSwaggerUI(opts =>
                opts.SwaggerEndpoint("/openapi/v1.json", "Messenger"));
        }
        
        app.UseExceptionHandler();
        app.UseCors("AllowFrontend");
        
        var hostBlazorApp = app.Configuration.GetValue("HostBlazorApp", true);
        
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
}