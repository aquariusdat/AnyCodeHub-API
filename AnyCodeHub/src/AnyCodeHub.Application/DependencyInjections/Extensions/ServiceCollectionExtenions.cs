using AnyCodeHub.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using AnyCodeHub.Infrastructure.Auth.DependencyInjections.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AnyCodeHub.Application.Attributes;
using AnyCodeHub.Infrastructure.Auth.Abstractions.Auth;
using AnyCodeHub.Infrastructure.Auth.Repositories;
using AnyCodeHub.Infrastructure.Auth.Services;
using AnyCodeHub.Infrastructure.Auth.Abstractions;
using Microsoft.AspNetCore.Builder;
using AnyCodeHub.Application.Mappers;
using System.Text;

namespace AnyCodeHub.Application.DependencyInjections.Extensions;
public static class ServiceCollectionExtenions
{
    private static readonly string _corsName = "AnyCodeHubCORSName";
    public static void AddMediatRApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AssemblyReference.Assembly);
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPiplineBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformancePiplineBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>));
        services.AddValidatorsFromAssembly(AnyCodeHub.Contract.AssemblyReference.Assembly, includeInternalTypes: true);

    }

    public static void AddCorsApplication(this WebApplication app)
    {
        app.UseCors(_corsName);
    }



    public static void AddAutoMapperApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServiceProfile));
    }


    #region Authentication
    public static void AddJwtInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();

        var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
        services.AddSingleton(jwtOptions);

        var encryptOptions = configuration.GetSection("EncryptOptions").Get<EncryptOptions>();
        services.AddSingleton(encryptOptions);

        //var googleApiOptions = configuration.GetSection("GoogleApi").Get<GoogleApiOptions>();
        //services.AddSingleton(googleApiOptions);

        services.AddAuthenServices();
        //services.AddAuthenRepository();

        #region Authentication
        services
                    //.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                                                                                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                                                                                {
                                                                                    options.SaveToken = true;
                                                                                    //var signingKey = Encoding.UTF8.GetBytes(configuration.GetSection("JwtOptions:SecretKey").Value);
                                                                                    var signingKey = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

                                                                                    //var rsaKey = RSA.Create();
                                                                                    //string privateKey = File.ReadAllText(jwtOptions.PrivateKeyPath);
                                                                                    //rsaKey.FromXmlString(privateKey);
                                                                                    //var rsaSecurityKey = new RsaSecurityKey(rsaKey);

                                                                                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                                                                                    {
                                                                                        ValidateLifetime = true,
                                                                                        ValidateIssuerSigningKey = true,
                                                                                        ValidateIssuer = false, // on production make it true
                                                                                        ValidateAudience = false, // on production make it true
                                                                                        //ValidIssuer = configuration.GetSection("JwtOptions:Issuer").Value,
                                                                                        //ValidAudience = configuration.GetSection("JwtOptions:Audience").Value,
                                                                                        ValidIssuer = jwtOptions.Issuer,
                                                                                        ValidAudience = jwtOptions.Audience,
                                                                                        IssuerSigningKey = new SymmetricSecurityKey(signingKey),
                                                                                        //IssuerSigningKey = rsaSecurityKey,
                                                                                        ClockSkew = TimeSpan.Zero
                                                                                    };

                                                                                    //var signingKey = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

                                                                                    //options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                                                                                    //{
                                                                                    //    ValidateLifetime = true,
                                                                                    //    ValidateIssuerSigningKey = true,
                                                                                    //    ValidIssuer = jwtOptions.Issuer,
                                                                                    //    ValidAudience = jwtOptions.Audience,
                                                                                    //    IssuerSigningKey = new SymmetricSecurityKey(signingKey),
                                                                                    //    ClockSkew = TimeSpan.Zero
                                                                                    //};

                                                                                    options.Events = new JwtBearerEvents
                                                                                    {
                                                                                        OnMessageReceived = (context) =>
                                                                                        {
                                                                                            if (context.Request.Cookies.ContainsKey("X-Access-Token"))
                                                                                            {
                                                                                                context.Token = context.Request.Cookies["X-Access-Token"];
                                                                                            }

                                                                                            return Task.CompletedTask;
                                                                                        },
                                                                                        OnAuthenticationFailed = (context) =>
                                                                                        {
                                                                                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                                                                            {
                                                                                                context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                                                                                            }

                                                                                            return Task.CompletedTask;
                                                                                        }
                                                                                    };

                                                                                    options.EventsType = typeof(CustomJwtBearerEvents);
                                                                                })
                                                                                //.AddJwtBearer($"{JwtBearerDefaults.AuthenticationScheme}WithoutValidExpireTime", options =>
                                                                                //{
                                                                                //    options.SaveToken = true;
                                                                                //    var signingKey = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);
                                                                                //    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                                                                                //    {
                                                                                //        ValidateLifetime = false,
                                                                                //        ValidateIssuerSigningKey = true,
                                                                                //        ValidateIssuer = false, // on production make it true
                                                                                //        ValidateAudience = false, // on production make it true
                                                                                //        //ValidIssuer = configuration.GetSection("JwtOptions:Issuer").Value,
                                                                                //        //ValidAudience = configuration.GetSection("JwtOptions:Audience").Value,
                                                                                //        ValidIssuer = jwtOptions.Issuer,
                                                                                //        ValidAudience = jwtOptions.Audience,
                                                                                //        IssuerSigningKey = new SymmetricSecurityKey(signingKey),
                                                                                //        ClockSkew = TimeSpan.Zero
                                                                                //    };

                                                                                //    options.Events = new JwtBearerEvents
                                                                                //    {
                                                                                //        OnMessageReceived = (context) =>
                                                                                //        {
                                                                                //            if (context.Request.Cookies.ContainsKey("X-Access-Token"))
                                                                                //            {
                                                                                //                context.Token = context.Request.Cookies["X-Access-Token"];
                                                                                //            }

                                                                                //            return Task.CompletedTask;
                                                                                //        },
                                                                                //        OnAuthenticationFailed = (context) =>
                                                                                //        {
                                                                                //            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                                                                //            {
                                                                                //                context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                                                                                //            }

                                                                                //            return Task.CompletedTask;
                                                                                //        }
                                                                                //    };
                                                                                //})

                                                                                ;
        #endregion Authentication

        #region Authorization
        services.AddAuthorization();
        #endregion Authorization

    }

    public static void AddAuthenRepository(this IServiceCollection services)
    {
        services.AddTransient<IAuthenticationService, AuthenticationRepository>();
    }

    public static void AddAuthenServices(this IServiceCollection services)
    {
        services.AddTransient<ITokenGeneratorService, TokenGeneratorService>();
        //services.AddTransient<IOAuthGoogleService, OAuthGoogleService>();
        services.AddScoped<CustomJwtBearerEvents>();
        services.AddHttpClient();
    }
    #endregion
}
