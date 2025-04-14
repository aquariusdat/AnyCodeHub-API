
using AnyCodeHub.Contract.Abstractions.Message;

namespace AnyCodeHub.Contract.Services.V1.Role;

public static class Command
{
    public record AddAppUserRoleCommand(string Email, string RoleCode) : ICommand<bool>;
    public record AddRoleCommand(string RoleCode, string Description) : ICommand<Response.AddRoleRepsonse>;
}
