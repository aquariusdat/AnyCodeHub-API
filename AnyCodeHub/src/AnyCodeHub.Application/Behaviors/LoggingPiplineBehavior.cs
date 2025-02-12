using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AnyCodeHub.Application.Behaviors;
public sealed class LoggingPiplineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;
    public LoggingPiplineBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Begin logging request:::{typeof(TRequest).Name} - With params: {JsonConvert.SerializeObject(request)}");
        var response = await next();
        _logger.LogInformation($"End logging request:::{typeof(TRequest).Name}");
        return response;
    }
}
