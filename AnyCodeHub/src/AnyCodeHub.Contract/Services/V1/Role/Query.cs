using AnyCodeHub.Contract.Abstractions.Message;

namespace AnyCodeHub.Contract.Services.V1.Role;
public static class Query
{
    public record GetAppUserRoleQuery(string Email) : IQuery<List<string>>;
    public record GetRoleQuery() : IQuery<List<Response.GetRoleResponse>>;
}