using ClosedEllipse.Models;
using ClosedEllipse.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options
    .AddDefaultPolicy(policy => 
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyHeader();
    }));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
}

app.UseCors();

app.MapPost("/generate", (RequestDTO request, HttpContext ctx, ILogger<SpheroidGenerator> logger) =>
{
    try
    {
        var gen = new SpheroidGenerator(request, logger);

        var result = gen.Generate()?.Select(x => new ResponseDTO(x)).ToArray();
        if (result is null)
            return Results.BadRequest("Items with this properties cannot be created");
            
        var fileName = "spheroid-response.json";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

        // File.WriteAllText(fileName, JsonSerializer.Serialize(result));

        return Results.Created("/generate", result);

    }
    catch (ArgumentException ex)
    {
        app.Logger.LogError("{ex.Message}", ex.Message);
        return Results.BadRequest(ex.Message);
    }
    catch(Exception ex)
    {
        app.Logger.LogError("{ex.StackTrace}", ex.StackTrace);
        return Results.Problem("Something went wrong...");
    }
});
app.MapFallbackToFile("index.html");

app.Run();
