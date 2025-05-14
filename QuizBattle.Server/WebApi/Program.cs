using MyApp.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSignalRServices();

var app = builder.Build();

// Configure the HTTP request pipeline
app.ConfigureApplication();

await app.RunAsync();