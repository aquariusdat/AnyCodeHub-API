using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.Role;
using AnyCodeHub.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static AnyCodeHub.Contract.Services.V1.Role.Query;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Role;
public class GetRoleQueryHandler : IQueryHandler<GetRoleQuery, List<Response.GetRoleResponse>>
{
    private readonly RoleManager<AppRole> _roleManager;

    public GetRoleQueryHandler(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Result<List<Response.GetRoleResponse>>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
    {
        var lstRole = await _roleManager.Roles.Select(t => new Response.GetRoleResponse(t.Id, t.RoleCode)).ToListAsync();
        return lstRole;
    }
}
