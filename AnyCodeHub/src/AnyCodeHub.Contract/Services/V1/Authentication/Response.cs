namespace AnyCodeHub.Contract.Services.V1.Authentication;

public static class Response
{
    public class AuthenticatedResponse()
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpirationTime { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationTime { get; set; }
    }

    public class UserInformation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthOfDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
