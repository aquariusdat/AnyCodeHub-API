namespace AnyCodeHub.Domain.Exceptions;
public abstract class ExistsException : DomainException
{
    protected ExistsException(string message)
       : base("Exists data", message)
    {
    }
}
