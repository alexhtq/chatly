namespace Chatly.Messages.Api;

public static class ConfigureApp
{
    public static async Task Configure(this WebApplication app)
    {
        app.UseExceptionHandler();
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseWebAssemblyDebugging();
            app.UseSwaggerUI(opts =>
                opts.SwaggerEndpoint("/openapi/v1.json", "Messenger"));
        }
        else
        {
            app.UseHttpsRedirection();
        }

        app.UseStaticFiles();
        app.UseBlazorFrameworkFiles();
        app.MapFallbackToFile("index.html");

        app.MapControllers();
    }
}