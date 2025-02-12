namespace AnyCodeHub.Domain.Exceptions;

public static class RoleException
{
    public class RoleExistException : Exception
    {
        public RoleExistException(string RoleCode) : base($"Role {RoleCode} already exists.")
        {

        }
    }
    public class RoleNotFoundException : NotFoundException
    {
        public RoleNotFoundException(string RoleCode) : base($"Role {RoleCode} wasn't found.")
        {

        }
    }

    public class UserExistsRoleAlready : ExistsException
    {
        public UserExistsRoleAlready(string Email, string RoleCode) : base($"Email [{Email}] has role [{RoleCode}] already.")
        {

        }
    }
}
