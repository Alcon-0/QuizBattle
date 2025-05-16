using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using WebApi.Middleware;

namespace MyApp.API.Configurations;

public static class AppBuilderExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quiz App API V1");
                c.RoutePrefix = "swagger";
            });
        }
        
        app.UseCors("AllowFrontend");
        //app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        app.MapControllers();

        _ = SeedData.EnsurePopulated(app);

        return app;
    }
}