using AnyCodeHub.Contract.Abstractions.Shared;
using MassTransit;
using MediatR;

namespace AnyCodeHub.Contract.Abstractions.Message;

[ExcludeFromTopology]
public interface IDomainEvent : IRequest
{
    public Guid EventId { get; init; }
    public Guid Id { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
}
[ExcludeFromTopology]
public interface IDomainEvent<TResponse> : IRequest<Result<TResponse>>
{
    public Guid EventId { get; init; }
    public Guid Id { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
}

