using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.Role;
using AnyCodeHub.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static AnyCodeHub.Contract.Services.V1.Role.Command;
using static AnyCodeHub.Domain.Exceptions.RoleException;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Role;
public class AddRoleCommandHandler : ICommandHandler<AddRoleCommand, Response.AddRoleRepsonse>
{
    private readonly RoleManager<AppRole> _roleManager;
    public AddRoleCommandHandler(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Result<Response.AddRoleRepsonse>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var existRole = await _roleManager.Roles.Where(t => t.RoleCode.ToLower() == request.RoleCode.ToLower()).FirstOrDefaultAsync();

        if (existRole != null)
            throw new RoleExistException(request.RoleCode);

        var role = new AppRole() { Name = request.RoleCode, Description = request.Description, RoleCode = request.RoleCode };

        await _roleManager.CreateAsync(role);

        return Result.Success(new Response.AddRoleRepsonse(role.Id, role.RoleCode));
    }
}
