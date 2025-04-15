using AnyCodeHub.Contract.Abstractions.Message;
using static AnyCodeHub.Contract.Services.V1.Authentication.Response;

namespace AnyCodeHub.Contract.Services.V1.Authentication
{
    public static class Command
    {
        public record RegisterCommand(string FirstName, string LastName, DateTime? BirthOfDate, string Email, string Password, string PasswordConfirmed, string PhoneNumber) : ICommand<bool>;
        public record CallbackGoogleOAuthCommand(string code, string scope, string state) : ICommand<AuthenticatedResponse>;
        public record RevokeTokenCommand : ICommand<bool>
        {
            public string AccessToken { get; set; }
        }
    }
}
