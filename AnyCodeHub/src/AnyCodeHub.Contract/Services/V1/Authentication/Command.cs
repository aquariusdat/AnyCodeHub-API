using AnyCodeHub.Contract.Abstractions.Message;

namespace AnyCodeHub.Contract.Services.V1.Authentication
{
    public static class Command
    {
        public record RegisterCommand(string FirstName, string LastName, DateTime? BirthOfDate, string Email, string Password, string PasswordConfirmed, string PhoneNumber) : ICommand<bool>;

        public record RevokeTokenCommand : ICommand<bool>
        {
            public string AccessToken { get; set; }
        }
    }
}
