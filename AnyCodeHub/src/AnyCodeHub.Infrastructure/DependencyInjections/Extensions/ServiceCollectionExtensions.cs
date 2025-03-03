using AnyCodeHub.Application.Abstractions;
using AnyCodeHub.Infrastructure.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnyCodeHub.Infrastructure.DependencyInjections.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddRedisInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options => options.Configuration = $"{configuration.GetSection("RedisConfigurations:Host").Value}:{configuration.GetSection("RedisConfigurations:Port").Value}");
        services.AddTransient<ICachingService, CachingService>();
    }


}
