

using AnyCodeHub.Contract.Abstractions.Message;

namespace AnyCodeHub.Infrastructure.Abstractions;

public interface ISendMessage<TCommand, TResult> 
    where TCommand : IDomainEvent 
    where TResult : class
{
    Task<TResult> Send(TCommand command);
}
