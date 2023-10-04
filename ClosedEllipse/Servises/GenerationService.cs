using ClosedEllipse.Models;
using Microsoft.AspNetCore.Http.Extensions;

namespace ClosedEllipse.Services;

public class GenerationService
{
    private readonly ILogger _logger;
    public GenerationService(ILogger<GenerationService> logger)
    {
        _logger = logger;
    }
    public IResult Generate(GenerationParamsDTO request, HttpContext ctx)
    {   
        try
        {
            var generator = new SpheroidGenerator(request, _logger);
            var result = generator.Generate()?.Select(x => new ResponseDTO(x)).ToArray();
            if (result is null)
                return Results.BadRequest("Items with this properties cannot be created");
        
            return Results.Created(ctx.Request.GetDisplayUrl(), result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError("{ex.Message}", ex.Message);
            return Results.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError("{ex.StackTrace}", ex.StackTrace);
            return Results.Problem("Something went wrong...");
        }
    }
}