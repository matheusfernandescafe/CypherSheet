using Microsoft.Extensions.DependencyInjection;

namespace CypherSheet.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICharacterAppService, CharacterAppService>();
        return services;
    }
}
