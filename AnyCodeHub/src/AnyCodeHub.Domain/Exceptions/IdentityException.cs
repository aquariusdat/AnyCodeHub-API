namespace AnyCodeHub.Domain.Exceptions;
public static class IdentityException
{
    public class TokenException : DomainException
    {
        public TokenException(string Message) : base("Token exception", Message)
        {

        }
    }
}
