namespace Chatly.Messages.Api;

public static class ConfigureApp
{
    public static async Task Configure(this WebApplication app)
    {            
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(opts =>
                opts.SwaggerEndpoint("/openapi/v1.json", "Messenger"));
        }
        
        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseCors("AllowFrontend");
        
        var hostBlazorApp = app.Configuration.GetValue("HostBlazorApp", true);
        
        if (hostBlazorApp)
        {
            app.UseWebAssemblyDebugging();
            app.UseStaticFiles();
            app.UseBlazorFrameworkFiles();
        }

        app.MapControllers();

        if (hostBlazorApp)
        {
            app.MapFallbackToFile("index.html");
        }
    }
}