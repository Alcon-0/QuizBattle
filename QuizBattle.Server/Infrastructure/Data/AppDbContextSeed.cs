using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Domain.Entities;
using Infrastructure.Services;
using System.IO;
using System.Reflection;
using MongoDB.Bson;

namespace Infrastructure.Data;

public static class SeedData
{
    public static async Task EnsurePopulated(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        
        var context = services.GetRequiredService<AppDbContext>();
        var mongoImageService = services.GetRequiredService<MongoImageService>();

        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(mongoImageService);

        // Apply pending migrations first
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        // Check if seeding is needed (no quizzes exist)
        if (!context.Quizzes.Any())
        {
            // Seed cover images to MongoDB with descriptions
            var mathCoverId = await SeedCoverImage(mongoImageService, "math_cover.jpg", 
                "A chalkboard with complex mathematical equations");
                
            var geoCoverId = await SeedCoverImage(mongoImageService, "geo_cover.jpg",
                "A world map with highlighted countries");
                
            var scienceCoverId = await SeedCoverImage(mongoImageService, "science_cover.jpg",
                "A laboratory with beakers and test tubes");
            
            // Seed sample quizzes with questions and options
            var mathQuiz = new Quiz
            {
                Title = "Advanced Mathematics",
                Description = "Challenge your math skills with these problems",
                CreatedAt = DateTime.UtcNow,
                MongoCoverImageId = mathCoverId.ToString(),
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "What is the derivative of 3x² + 2x + 1?",
                        CorrectAnswerIndex = 1,
                        Options = new List<AnswerOption>
                        {
                            new AnswerOption { Text = "3x + 2", Id = 0 },
                            new AnswerOption { Text = "6x + 2", Id = 1 },
                            new AnswerOption { Text = "6x² + 2", Id = 2 },
                            new AnswerOption { Text = "x³ + x² + x", Id = 3 }
                        }
                    },
                    new Question
                    {
                        Text = "What is the solution to the equation eˣ = 10?",
                        CorrectAnswerIndex = 2,
                        Options = new List<AnswerOption>
                        {
                            new AnswerOption { Text = "x = 1", Id = 0 },
                            new AnswerOption { Text = "x = 2.302", Id = 1 },
                            new AnswerOption { Text = "x = ln(10)", Id = 2 },
                            new AnswerOption { Text = "x = log₁₀(e)", Id = 3 }
                        }
                    },
                    new Question
                    {
                        Text = "What is the integral of 1/x dx?",
                        CorrectAnswerIndex = 3,
                        Options = new List<AnswerOption>
                        {
                            new AnswerOption { Text = "x + C", Id = 0 },
                            new AnswerOption { Text = "1/x² + C", Id = 1 },
                            new AnswerOption { Text = "ln(x²) + C", Id = 2 },
                            new AnswerOption { Text = "ln|x| + C", Id = 3 }
                        }
                    }
                }
            };

            var geographyQuiz = new Quiz
            {
                Title = "World Geography Challenge",
                Description = "Test your advanced knowledge of world geography",
                CreatedAt = DateTime.UtcNow,
                MongoCoverImageId = geoCoverId.ToString(),
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "Which country has the longest coastline?",
                        CorrectAnswerIndex = 0,
                        Options = new List<AnswerOption>
                        {
                            new AnswerOption { Text = "Canada", Id = 0 },
                            new AnswerOption { Text = "Russia", Id = 1 },
                            new AnswerOption { Text = "Australia", Id = 2 },
                            new AnswerOption { Text = "United States", Id = 3 }
                        },
                        ImageId = await SeedQuestionImage(mongoImageService, "coastline_map.jpg",
                            "Map comparing coastline lengths of different countries")
                    },
                    new Question
                    {
                        Text = "What is the capital of Bhutan?",
                        CorrectAnswerIndex = 1,
                        Options = new List<AnswerOption>
                        {
                            new AnswerOption { Text = "Kathmandu", Id = 0 },
                            new AnswerOption { Text = "Thimphu", Id = 1 },
                            new AnswerOption { Text = "Paro", Id = 2 },
                            new AnswerOption { Text = "Punakha", Id = 3 }
                        }
                    }
                }
            };

            var scienceQuiz = new Quiz
            {
                Title = "Advanced Science",
                Description = "Difficult questions from various science fields",
                CreatedAt = DateTime.UtcNow,
                MongoCoverImageId = scienceCoverId.ToString(),
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "Which element has the highest melting point?",
                        CorrectAnswerIndex = 2,
                        Options = new List<AnswerOption>
                        {
                            new AnswerOption { Text = "Tungsten", Id = 0 },
                            new AnswerOption { Text = "Osmium", Id = 1 },
                            new AnswerOption { Text = "Carbon", Id = 2 },
                            new AnswerOption { Text = "Rhenium", Id = 3 }
                        }
                    },
                    new Question
                    {
                        Text = "What is the speed of light in a vacuum?",
                        CorrectAnswerIndex = 3,
                        Options = new List<AnswerOption>
                        {
                            new AnswerOption { Text = "300,000 km/h", Id = 0 },
                            new AnswerOption { Text = "3,000,000 m/s", Id = 1 },
                            new AnswerOption { Text = "186,000 m/s", Id = 2 },
                            new AnswerOption { Text = "299,792,458 m/s", Id = 3 }
                        }
                    }
                }
            };

            context.Quizzes.AddRange(mathQuiz, geographyQuiz, scienceQuiz);
            await context.SaveChangesAsync();
            Console.WriteLine("Database seeded with initial data.");
        }
        else
        {
            Console.WriteLine("Database already contains data - skipping seeding.");
        }
    }

    private static async Task<ObjectId> SeedCoverImage(
        MongoImageService mongoService, 
        string imageName,
        string description)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePath = $"Infrastructure.Data.SeedImages.{imageName}";

        await using var stream = assembly.GetManifestResourceStream(resourcePath);
        if (stream == null)
            throw new FileNotFoundException($"Embedded resource {resourcePath} not found");

        return await mongoService.UploadImageWithMetadataAsync(
            stream,
            imageName,
            "image/jpeg",
            new BsonDocument
            {
                { "description", description },
                { "isSeedData", true },
                { "seedDate", DateTime.UtcNow }
            });
    }

    private static async Task<string> SeedQuestionImage(
        MongoImageService mongoService,
        string imageName,
        string description)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePath = $"Infrastructure.Data.SeedImages.{imageName}";

        await using var stream = assembly.GetManifestResourceStream(resourcePath);
        if (stream == null)
            throw new FileNotFoundException($"Embedded resource {resourcePath} not found");

        var imageId = await mongoService.UploadImageWithMetadataAsync(
            stream,
            imageName,
            "image/jpeg",
            new BsonDocument
            {
                { "description", description },
                { "isSeedData", true },
                { "seedDate", DateTime.UtcNow },
                { "isQuestionImage", true }
            });

        return imageId.ToString();
    }
}