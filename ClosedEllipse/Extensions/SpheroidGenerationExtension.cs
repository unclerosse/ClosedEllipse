using ClosedEllipse.Services;

namespace ClosedEllipse.Extensions;

public static class SpheroidGenerationExtension
{
    public static IServiceCollection AddSpheroidGenerationService(this IServiceCollection services)
    {
        return services.AddSingleton<GenerationService>();
    }
}