namespace AnyCodeHub.Domain.Exceptions;
public abstract class AlreadyDeletedException : DomainException
{
    protected AlreadyDeletedException(string message)
        : base("Already deleted", message)
    {
    }
}
