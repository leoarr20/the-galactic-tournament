using Microsoft.EntityFrameworkCore;
using TheGalacticTournament.Api.Data;
using TheGalacticTournament.Api.Services;

// Creates the application builder.
// This object is used to configure services, dependency injection,
// configuration files, logging and the HTTP request pipeline.
var builder = WebApplication.CreateBuilder(args);

// Registers controller support.
// This allows the API to use controller classes such as SpeciesController,
// BattlesController and RankingController.
builder.Services.AddControllers();

// Registers API endpoint metadata.
// This is required by Swagger/OpenAPI to discover the available endpoints.
builder.Services.AddEndpointsApiExplorer();

// Registers Swagger services.
// Swagger is used to generate interactive API documentation in development.
builder.Services.AddSwaggerGen();

// Registers the Entity Framework Core database context.
// The SQL Server connection string is read from appsettings.json,
// appsettings.Development.json or appsettings.Production.json,
// depending on the current environment.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Reads the allowed frontend origins from configuration.
// Example:
// "Cors": {
//   "AllowedOrigins": [
//     "http://localhost:4200"
//   ]
// }
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

// Configures CORS policy for the frontend.
// This allows the Angular application to call the API from the configured origins.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins ?? Array.Empty<string>())
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Registers application services in the dependency injection container.
// Scoped means that one instance is created per HTTP request.
builder.Services.AddScoped<ISpeciesService, SpeciesService>();
builder.Services.AddScoped<IBattleService, BattleService>();

// Builds the application using all configured services.
var app = builder.Build();

// Enables Swagger only in the development environment.
// This prevents exposing API documentation publicly in production.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirects HTTP requests to HTTPS.
// 
// In local development, this can be disabled if the API is running only with HTTP,
// for example: http://localhost:5154.
// 
// In production, this should normally be enabled to force secure HTTPS traffic.
// app.UseHttpsRedirection();

// Applies the CORS policy configured above.
// This must be placed before authorization and before mapping controllers.
app.UseCors("AllowFrontend");

// Enables authorization middleware.
// Even if the project does not currently use authentication/authorization,
// this keeps the request pipeline ready for future security rules.
app.UseAuthorization();

// Maps controller endpoints.
// This enables routes such as:
// GET    /api/species
// POST   /api/species
// GET    /api/battles
// POST   /api/battles
// GET    /api/ranking
app.MapControllers();

// Starts the web application.
app.Run();