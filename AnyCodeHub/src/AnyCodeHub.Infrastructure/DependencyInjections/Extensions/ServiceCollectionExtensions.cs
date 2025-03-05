using AnyCodeHub.Application.Abstractions;
using AnyCodeHub.Contract.Abstractions.Services;
using AnyCodeHub.Infrastructure.Caching;
using AnyCodeHub.Infrastructure.DependencyInjections.Options;
using AnyCodeHub.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnyCodeHub.Infrastructure.DependencyInjections.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddRedisInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options => options.Configuration = $"{configuration.GetSection("RedisConfigurations:Host").Value}:{configuration.GetSection("RedisConfigurations:Port").Value}");
        services.AddTransient<ICachingService, CachingService>();

        AddMailConfiguration(services, configuration);
        AddServices(services);
    }

    public static void AddMailConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var mailSettings = configuration.GetSection("MailSettings").Get<MailSettings>();
        services.AddSingleton(mailSettings);
        services.AddTransient<IEmailSenderService, EmailSenderService>();
    }

    public static void AddServices(IServiceCollection services) => services.AddTransient<IUrlHelperService, UrlHelperService>();

}
