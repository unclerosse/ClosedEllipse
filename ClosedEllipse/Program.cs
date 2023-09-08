using ClosedEllipse.Models;
using ClosedEllipse.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => 
{
    x.AllowAnyOrigin();
    x.AllowAnyHeader();
});

app.MapPost("/generate", (RequestDTO request, HttpContext ctx) =>
{
    Console.WriteLine(request.ToString());

    try
    {
        var gen = new SpheroidGenerator(request);

        var result = gen.Generate()?.Select(x => new ResponseDTO(x)).ToArray();
        if (result is null)
            return Results.BadRequest("Items with this properties cannot be created");
        
        Console.WriteLine($"Total items generated: {result.Length}");
        
        var fileName = "spheroid-response.json";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

        File.WriteAllText(fileName, JsonSerializer.Serialize(result));

        return Results.Created("/generate", result);

    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();

public partial class Program { }