using ClosedEllipse.Models;

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
    
    return Results.Ok();
});

app.Run();