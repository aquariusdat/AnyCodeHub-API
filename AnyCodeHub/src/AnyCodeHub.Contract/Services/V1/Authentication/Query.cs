using AnyCodeHub.Contract.Abstractions.Message;

namespace AnyCodeHub.Contract.Services.V1.Authentication;

public static class Query
{
    public record Login(string Email, string Password) : IQuery<Response.AuthenticatedResponse>;
    public record SignInGoogleOAuthQuery(string state) : IQuery<string>;
    public record Token() : IQuery<Response.AuthenticatedResponse>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public record GetUserData(string? AccessToken) : IQuery<Response.UserInformation>;
}
