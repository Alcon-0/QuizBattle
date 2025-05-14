using System;
using WebApi.Hubs;
using WebApi.Middleware;

namespace MyApp.API.Configurations;

public static class AppBuilderExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        app.MapControllers();
        app.MapHub<BattleHub>("/battleHub");

        return app;
    }
}