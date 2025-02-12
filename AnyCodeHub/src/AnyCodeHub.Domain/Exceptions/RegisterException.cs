namespace AnyCodeHub.Domain.Exceptions;
public static class RegisterException
{

    public class EmailExistsException : Exception
    {
        public EmailExistsException(string Email) : base($"Email [{Email}] already exists.")
        {

        }
    }

    public class UserNameExistsException : Exception
    {
        public UserNameExistsException(string UserName) : base($"UserName [{UserName}] already exists.")
        {

        }
    }

    public class PhoneNumberExistsException : Exception
    {
        public PhoneNumberExistsException(string PhoneNumber) : base($"PhoneNumber [{PhoneNumber}] already exists.")
        {

        }
    }
}
