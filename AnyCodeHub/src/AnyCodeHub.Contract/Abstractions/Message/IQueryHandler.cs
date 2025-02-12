using AnyCodeHub.Contract.Abstractions.Shared;
using MediatR;

namespace AnyCodeHub.Contract.Abstractions.Message;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
