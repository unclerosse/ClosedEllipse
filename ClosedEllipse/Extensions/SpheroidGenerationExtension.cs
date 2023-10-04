using ClosedEllipse.Models;
using ClosedEllipse.Services;

namespace ClosedEllipse.Extensions;

public static class SpheroidGenerationExtension
{
    public static IServiceCollection AddSpheroidGenerationService(this IServiceCollection services)
    {
        return services.AddSingleton<GenerationService>();
    }

    public static RouteHandlerBuilder UseSpheroidGenerationService(this IEndpointRouteBuilder app)
    {        
        return app.MapPost("/generate", (GenerationService service, GenerationParamsDTO request) =>
            service.Generate(request));
    }
}