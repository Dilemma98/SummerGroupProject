using Microsoft.EntityFrameworkCore;
using Backend.Data;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from the .env file
DotNetEnv.Env.Load();

// Read the database connection string from environment variables
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

// Add services to the container
builder.Services.AddOpenApi(); // Swagger/OpenAPI support
builder.Services.AddControllers(); // Enable MVC controllers
builder.Services.AddHttpClient(); // HTTP client support for external APIs

// Configure Entity Framework with PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.CommandTimeout(60); // Optional: Set command timeout
    })
);

// Configure CORS to allow frontend access (e.g., Vite/Vue app)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // Frontend origin
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Enable Swagger UI only in development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Middleware pipeline
app.UseCors("AllowFrontend");       // Enable CORS
app.UseStaticFiles();               // Serve static files from wwwroot
app.MapControllers();               // Enable API routes
// app.UseHttpsRedirection();       // Optional: enable HTTPS redirection

app.Run();
