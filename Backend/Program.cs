using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);
Env.Load();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//Add controllers
builder.Services.AddControllers();
// Add http client for Google API
builder.Services.AddHttpClient();

// Add Google Sheets API-key
builder.Configuration.AddEnvironmentVariables();

// Read Google Sheets API-key
var googleSheetsApiKey = Environment.GetEnvironmentVariable("GOOGLE_SHEETS_API_KEY");

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.AllowAnyOrigin()
            // policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
}

// To serve static files (like images) from wwwroot
app.UseStaticFiles();

// Use CORS policy
app.UseCors("AllowFrontend");

// Map controllers
app.MapControllers();

// app.UseHttpsRedirection();
app.Run();


