using AnyCodeHub.Application.Abstractions;
using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Services;
using AnyCodeHub.Infrastructure.Caching;
using AnyCodeHub.Infrastructure.DependencyInjections.Options;
using AnyCodeHub.Infrastructure.Services;
using CorrelationId.Abstractions;
using MassTransit;
using MassTransit.Configuration;
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

    public static void AddProducerInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var masstransitConfigurations = configuration.GetSection(nameof(MasstransitConfiguration)).Get<MasstransitConfiguration>();
        services.AddSingleton(masstransitConfigurations);

        var messageBusOptions = configuration.GetSection(nameof(MessageBusOptions)).Get<MessageBusOptions>();
        services.AddSingleton(messageBusOptions);

        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();

            cfg.UsingRabbitMq((context, bus) =>
            {

                bus.Host(masstransitConfigurations.Host, masstransitConfigurations.VHost, h =>
                {
                    h.Username(masstransitConfigurations.UserName);
                    h.Password(masstransitConfigurations.Password);

                });

                bus.MessageTopology.SetEntityNameFormatter(new KebabCaseEntityNameFormatter());

                #region Topic exchange
                // Publish event
                //bus.Publish<Command.Notification>(e =>
                //{
                //    e.Durable = true;
                //    e.AutoDelete = false;
                //    e.ExchangeType = ExchangeType.Topic;
                //});

                //// Send command
                //bus.Send<Command.Notification>(e =>
                //{
                //    e.UseRoutingKeyFormatter(context => context.Message.Type.ToString());
                //});
                #endregion

                // Using Newtonsoft serialization
                bus.UseNewtonsoftJsonSerializer();
                //bus.ConfigureNewtonsoftJsonSerializer(settings =>
                //{
                //    settings.Converters.Add(new TypeNameHandlingConverter(TypeNameHandling.Objects));
                //    settings.Converters.Add(new DateOnlyJsonConverter());
                //    settings.Converters.Add(new ExpirationDateOnlyJsonConverter());

                //    return settings;
                //});
                //bus.ConfigureNewtonsoftJsonDeserializer(settings =>
                //{
                //    settings.Converters.Add(new TypeNameHandlingConverter(TypeNameHandling.Objects));
                //    settings.Converters.Add(new DateOnlyJsonConverter());
                //    settings.Converters.Add(new ExpirationDateOnlyJsonConverter());

                //    return settings;
                //});

                //bus.ConnectReceiveObserver(new LoggingReceiveObserver());
                //bus.ConnectConsumeObserver(new LoggingConsumeObserver());
                //bus.ConnectPublishObserver(new LoggingPublishObserver());
                //bus.ConnectSendObserver(new LoggingSendObserver());

                // Configure CorrelationId for send commands
                bus.ConfigureSend(pipe => pipe.AddPipeSpecification(
                    new DelegatePipeSpecification<SendContext<IDomainEvent>>(ctx =>
                    {
                        var accessor = context.GetRequiredService<ICorrelationContextAccessor>();
                        ctx.CorrelationId = new(accessor.CorrelationContext.CorrelationId);
                    })));

                // Configure CorrelationId for BEGIN publish event
                //bus.ConfigurePublish(pipe => pipe.AddPipeSpecification(
                //    new DelegatePipeSpecification<PublishContext<IDomainEvent>>(ctx =>
                //    {
                //        var accessor = context.GetRequiredService<ICorrelationContextAccessor>();
                //        ctx.CorrelationId = new(accessor.CorrelationContext.CorrelationId);
                //    })));

                // Configure CorrelationId for publish events forward
                bus.ConfigurePublish(pipe => pipe.AddPipeSpecification(
                       new DelegatePipeSpecification<PublishContext<IDomainEvent>>(ctx
                           => ctx.CorrelationId = ctx.InitiatorId)));


                bus.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(3)));
            });

        });

        //services.AddTransient<IPostCreatedProducer, PostCreatedProducerHandler>();
        //services.AddTransient<ISendNotification, SendNotificationProducer>();

        //services.AddDefaultCorrelationId(options =>
        //{
        //    options.RequestHeader = options.ResponseHeader = options.LoggingScopeKey = "CorrelationId";
        //    options.UpdateTraceIdentifier = true;
        //    options.AddToLoggingScope = true;
        //});
    }

}
