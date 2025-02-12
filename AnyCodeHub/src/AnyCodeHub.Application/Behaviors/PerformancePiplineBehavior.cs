using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AnyCodeHub.Application.Behaviors;
public sealed class PerformancePiplineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;
    private static Stopwatch _stopwatch;
    public PerformancePiplineBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
        _stopwatch = new Stopwatch();
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _stopwatch.Start();
        var response = await next();
        _stopwatch.Stop();

        var elapsedMiliseconds = _stopwatch.ElapsedMilliseconds;

        if (elapsedMiliseconds <= 5000)
            return response;

        var requestName = typeof(TRequest).Name;
        _logger.LogWarning($"Long Time Running - Request Details: {requestName} ({elapsedMiliseconds} miliseconds) {@request}");

        return response;
    }
}
