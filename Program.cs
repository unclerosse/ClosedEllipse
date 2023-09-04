using ClosedEllipse.Models;
using ClosedEllipse.Services;

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

        return Results.Created("/generate", gen.GetSpheroids().Select(x => new ResponseDTO(x)).ToArray());

    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();