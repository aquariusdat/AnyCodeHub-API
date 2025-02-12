using AnyCodeHub.Contract.Abstractions.Shared;
using MediatR;

namespace AnyCodeHub.Contract.Abstractions.Message;

public interface ICommand : IRequest<Result>
{
}
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{

}
