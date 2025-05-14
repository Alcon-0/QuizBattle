using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApi.Middleware;

namespace MyApp.API.Configurations;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Exception handler
        services.AddTransient<ExceptionHandlingMiddleware>();
    
        // Database
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("QuizBattleDb"));
        });

        // Application Layer
        services.AddScoped<IQuizRepository>(provider => 
            new QuizRepository(provider.GetRequiredService<AppDbContext>()));
    
        services.AddScoped<IQuestionRepository>(provider => 
            new QuestionRepository(provider.GetRequiredService<AppDbContext>()));
    
        services.AddScoped<IQuizBattleRepository>(provider => 
            new QuizBattleRepository(provider.GetRequiredService<AppDbContext>()));
        
        services.AddScoped<IQuizService, QuizService>();

        // API
        services.AddControllers();
        
        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Quiz App API", Version = "v1" });
        });

        return services;
    }

    public static IServiceCollection AddSignalRServices(this IServiceCollection services)
    {
        services.AddSignalR();
        return services;
    }
}