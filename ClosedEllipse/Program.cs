using ClosedEllipse.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(loggingBuilder => {
	loggingBuilder.AddFile($"logs/ce-{DateTime.Now:yyyy-MM-dd}.log", append:true);
});

builder.Services.AddSpheroidGenerationService();

builder.Services.AddCors(options => options
    .AddDefaultPolicy(policy => 
        policy.WithOrigins("http://localhost:5070/")
            .AllowAnyHeader()
            .AllowAnyHeader()
    ));

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

app.UseSpheroidGenerationService();

app.MapFallbackToFile("index.html");

app.Run();
