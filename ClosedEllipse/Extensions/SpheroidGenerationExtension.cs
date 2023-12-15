using ClosedEllipse.Models;
using ClosedEllipse.Services;

namespace ClosedEllipse.Extensions;

public static class SpheroidGenerationExtension
{
    public static IServiceCollection AddSpheroidGenerationService(this IServiceCollection services)
    {
        services.AddSingleton<IntersectionService>();
        return services.AddSingleton<GenerationService>();
    }

    public static IEndpointRouteBuilder UseSpheroidGenerationService(this IEndpointRouteBuilder app)
    {        
        app.MapPost("/generate", (GenerationService service, GenerationParamsDTO request) =>
            service.Generate(request));
        
        app.MapPut("/check-intersection", (IntersectionService service, SpheroidPairDto request) =>
            service.CheckIntersection(request));

        return app;
    }
}