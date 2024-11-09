using UpDownCryptorollBackend.MapperConfiguration;

namespace UpDownCryptorollBackend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPl(this IServiceCollection services)
    {
        services.AddScoped<CustomMapper>();
        
        return services;
    }
}