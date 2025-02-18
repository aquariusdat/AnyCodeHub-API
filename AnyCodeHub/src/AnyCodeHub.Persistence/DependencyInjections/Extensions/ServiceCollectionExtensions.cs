using AnyCodeHub.Domain.Abstractions;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities.Identity;
using AnyCodeHub.Persistence.Repository;
using AnyCodeHub.Persistence.DependencyInjections.Extensions;
using AnyCodeHub.Persistence.DependencyInjections.Options;
using AnyCodeHub.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AnyCodeHub.Persistence.DependencyInjections.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddPostgreSql(this IServiceCollection services)
    {
        services.AddDbContextPool<DbContext, ApplicationDbContext>((provider, builder) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var options = provider.GetRequiredService<IOptionsMonitor<PostgreSqlRetryOptions>>();

            var ouboxInterceptor = provider.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();
            var auditableInterceptor = provider.GetService<UpdateAuditableEntitiesInterceptor>();

            #region ============== SQL-SERVER-STRATEGY-1 ==============
            //builder
            //    .EnableDetailedErrors(true)
            //    .EnableSensitiveDataLogging(true)
            //    .UseLazyLoadingProxies(true)
            //    .UseNpgsql(
            //        connectionString: configuration.GetConnectionString("AnyCodeHubConnectionString"),
            //        npgsqlOptionsAction: optionsBuilder
            //                    => optionsBuilder.ExecutionStrategy(
            //                            dependencies => new NpgsqlRetryingExecutionStrategy(
            //                                    dependencies: dependencies,
            //                                    maxRetryCount: options.CurrentValue.MaxRetryCount,
            //                                    maxRetryDelay: options.CurrentValue.MaxRetryDelay,
            //                                    errorCodesToAdd: options.CurrentValue.ErrorNumbersToAdd))
            //                            .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name));
            #endregion ============== SQL-SERVER-STRATEGY-1 ==============

            #region ============== SQL - SERVER - STRATEGY - 2 ==============
            builder
                .EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(true)
                .UseLazyLoadingProxies(true)
                .UseNpgsql(
                    connectionString: configuration.GetConnectionString("AnyCodeHubConnectionString"),
                    npgsqlOptionsAction: optionsBuilder
                                => optionsBuilder
                                        //.ExecutionStrategy(
                                        //dependencies => new NpgsqlRetryingExecutionStrategy(
                                        //        dependencies: dependencies,
                                        //        maxRetryCount: options.CurrentValue.MaxRetryCount,
                                        //        maxRetryDelay: options.CurrentValue.MaxRetryDelay,
                                        //        errorCodesToAdd: options.CurrentValue.ErrorNumbersToAdd))
                                        .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
                .AddInterceptors(auditableInterceptor, ouboxInterceptor);
            #endregion ============== SQL - SERVER - STRATEGY - 2 ==============
        });

        services.AddIdentityCore<AppUser>()
        .AddRoles<AppRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.AllowedForNewUsers = true; // Default true
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Default 5
            options.Lockout.MaxFailedAccessAttempts = 3; // Default 5

            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        });

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddRepositoryBaseConfigurations();
        services.AddRepositoryConfigurations();
    }

    public static OptionsBuilder<PostgreSqlRetryOptions> ConfigurePostgreSqlRetryOptions(this IServiceCollection services, IConfigurationSection section)
        => services.AddOptions<PostgreSqlRetryOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart()
            ;

    public static void AddRepositoryBaseConfigurations(this IServiceCollection services)
    {
        services.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork));
        services.AddTransient(typeof(IUnitOfWorkDbContext<>), typeof(EFDbContextUnitOfWork<>));
        services.AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        services.AddTransient(typeof(IRepositoryDbContextBase<,,>), typeof(RepositoryDbContextBase<,,>));
    }

    public static void AddRepositoryConfigurations(this IServiceCollection services)
    {
        //services.AddTransient<IUserRepository, UserRepository>();
    }

    public static void AddInterceptors(this IServiceCollection services)
    {
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
    }
}
