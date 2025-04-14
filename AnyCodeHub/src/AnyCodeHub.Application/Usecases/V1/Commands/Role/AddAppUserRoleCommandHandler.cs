using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Entities.Identity;
using AnyCodeHub.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static AnyCodeHub.Contract.Services.V1.Role.Command;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Role;
public class AddAppUserRoleCommandHandler : ICommandHandler<AddAppUserRoleCommand, bool>
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    public AddAppUserRoleCommandHandler(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<Result<bool>> Handle(AddAppUserRoleCommand request, CancellationToken cancellationToken)
    {
        var existUser = await _userManager.Users.Where(t => t.Email == request.Email).FirstOrDefaultAsync() ?? throw new UserException.UserNotFoundException(request.Email);
        var existRole = await _roleManager.Roles.Where(t => t.RoleCode == request.RoleCode).FirstOrDefaultAsync() ?? throw new RoleException.RoleNotFoundException(request.RoleCode);

        var existUserRole = await _userManager.GetRolesAsync(existUser);
        if (existUserRole?.Count > 0 && existUserRole.Any(t => t == request.RoleCode))
            throw new RoleException.UserExistsRoleAlready(request.Email, request.RoleCode);

        await _userManager.AddToRoleAsync(existUser, request.RoleCode);

        return Result.Success(true);
    }
}
