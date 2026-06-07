using Microsoft.EntityFrameworkCore;
using ExpertOS.API.Data;
using ExpertOS.API.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Services ──────────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// In-memory database
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("ExpertOSDb"));

// AI Service: OpenRouterService if key present, else MockAiService
var openRouterKey = builder.Configuration["OpenRouter:ApiKey"];
if (!string.IsNullOrWhiteSpace(openRouterKey))
{
    builder.Services.AddSingleton<IAiService, OpenRouterService>();
    Console.WriteLine("✅ Using OpenRouter AI Service");
}
else
{
    builder.Services.AddSingleton<IAiService, MockAiService>();
    Console.WriteLine("ℹ️  No OpenRouter API key found — using MockAiService");
}

// CORS — allow frontend dev servers
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins(
                "http://localhost:5173",
                "http://localhost:3000",
                "http://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// ── App ───────────────────────────────────────────────────────────────────────
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.SeedData();
}

app.Run();
