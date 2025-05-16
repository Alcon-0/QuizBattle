using System.Reflection;
using System.Text;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            options.UseSqlServer(
                configuration.GetConnectionString("QuizApp"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
        });

        services.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 1;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidIssuer = configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? throw new ArgumentNullException("JWT:Secret", "JWT secret key is not configured")))
            };
        });


        // Application Layer
        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();

        services.AddScoped<IQuizService, QuizService>();

        services.Configure<MongoDBSettings>(
            configuration.GetSection("MongoDB"));

        // Add MongoDB services
        services.AddSingleton<MongoImageService>();
        // Cors
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", builder =>
            {
                builder.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); ;
            });
        });

        // API
        services.AddControllers();

        // Swagger
        services.AddEndpointsApiExplorer();
        // Swagger configuration
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Quiz App API",
                Version = "v1",
                Description = "API for creating and managing quizzes and questions"
            });

            // Add XML comments for better documentation
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            // JWT Bearer Authentication definition
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "JWT Authorization header using the Bearer scheme.\n\n" +
                             "Enter your token in the text input below.\n\n" +
                             "Example: \"Bearer {token}\"",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    jwtSecurityScheme,
                    Array.Empty<string>()
                }
            });
        });


        return services;
    }
}