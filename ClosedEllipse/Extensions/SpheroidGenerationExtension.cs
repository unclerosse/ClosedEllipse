using ClosedEllipse.Models;
using ClosedEllipse.Services;
using ClosedEllipse.Servises;

namespace ClosedEllipse.Extensions;

public static class SpheroidGenerationExtension
{
    public static IServiceCollection AddSpheroidGenerationService(this IServiceCollection services)
    {
        services.AddScoped<IntersectionService>();
        services.AddScoped<GenerationService>();
        services.AddScoped<SpheroidSliceService>();

        return services;
    }

    public static IEndpointRouteBuilder UseSpheroidGenerationService(this IEndpointRouteBuilder app)
    {        
        app.MapPost("/generate", (GenerationService service, GenerationParamsDTO request) =>
            service.Generate(request));
        
        app.MapPut("/check-intersection", (IntersectionService service, SpheroidPairDto request) =>
            service.CheckIntersection(request));

        app.MapPost("/get-slices", (SpheroidSliceService service, SpheroidRequestDto request) =>
            service.GetSlices(request));

        return app;
    }
}