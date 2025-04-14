namespace AnyCodeHub.Domain.Exceptions;
public static class UserException
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string Email) : base($"Email [{Email}] was not found.")
        {

        }
    }

    public class DeactiveUserException : Exception
    {
        public DeactiveUserException(string Email) : base($"Email [{Email}] has been deactivated or verified yet.")
        {

        }
    }

    public class WrongPasswordException : Exception
    {
        public WrongPasswordException() : base($"Password is wrong.")
        {

        }
    }

    public class UserExistsException : Exception
    {
        public UserExistsException(string Email) : base($"Email [{Email}] already exists")
        {

        }
    }

    public class UserHasBeenDeletedException : Exception
    {
        public UserHasBeenDeletedException(string Email) : base($"Email [{Email}] has been deleted.")
        {

        }
    }

    public class UserHasBeenLockedException : Exception
    {
        public UserHasBeenLockedException(string Email, DateTimeOffset LockoutEnd) : base($"Email [{Email}] has been locked until {LockoutEnd.ToString($"dd/MM/yyyy HH:mm:sss")}.")
        {

        }
    }
}
