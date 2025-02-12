using AnyCodeHub.Contract.Abstractions.Shared;
using MediatR;
namespace AnyCodeHub.Contract.Abstractions.Message;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
