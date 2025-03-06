using AnyCodeHub.Contract.Abstractions.Message;

namespace AnyCodeHub.Contract.Services.V1.User;

public static class Query
{
    public record ConfirmEmailQuery(string Email, string Token) : IQuery<bool>;
}
