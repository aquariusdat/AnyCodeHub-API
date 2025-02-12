namespace AnyCodeHub.Domain.Exceptions;
public static class UserException
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string UserName) : base($"Username [{UserName}] was not found.")
        {

        }
    }

    public class DeactiveUserException : Exception
    {
        public DeactiveUserException(string UserName) : base($"Username [{UserName}] has been deactive")
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
        public UserExistsException(string UserName) : base($"Username [{UserName}] already exists")
        {

        }
    }

    public class UserHasBeenDeletedException : Exception
    {
        public UserHasBeenDeletedException(string UserName) : base($"Username [{UserName}] has been deleted.")
        {

        }
    }

    public class UserHasBeenLockedException : Exception
    {
        public UserHasBeenLockedException(string UserName, DateTimeOffset LockoutEnd) : base($"Username [{UserName}] has been locked until {LockoutEnd.ToString($"dd/MM/yyyy HH:mm:sss")}.")
        {

        }
    }
}
