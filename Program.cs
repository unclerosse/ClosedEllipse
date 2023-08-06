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

app.UseCors(x => x.AllowAnyOrigin());

app.MapGet("/generate", (int Rgl, int a, int b) => 
{
    if (a <= 0 || b <= 0 || Rgl <= 0)
        return Results.BadRequest("Wrong data has been provided");
    
    double Vgl = 4.0/3.0 * 3.14 * Rgl * Rgl * Rgl;
    double V = 4.0/3.0 * 3.14 * a * a * b;
    
    if (Vgl <= V)
        return Results.BadRequest("Global volume are lesser than local");
    
    return Results.Ok(V);
});

app.Run();