

namespace AnyCodeHub.Contract.Services.V1.Role;
public static class Response
{
    public record GetAppUserRoleResponse(string RoleCode);
    public record GetRoleResponse(Guid Id, string RoleCode);

    public record AddRoleRepsonse(Guid Id, string RoleCode);
}
