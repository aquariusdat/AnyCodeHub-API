using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Entities.Identity;
using AnyCodeHub.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static AnyCodeHub.Contract.Services.V1.Role.Query;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Role;
public class GetAppUserRoleQueryHandler : IQueryHandler<GetAppUserRoleQuery, List<string>>
{
    private readonly UserManager<AppUser> _userManager;

    public GetAppUserRoleQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<List<string>>> Handle(GetAppUserRoleQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.Where(t => t.Email == request.Email).FirstOrDefaultAsync() ?? throw new UserException.UserNotFoundException(request.Email);
        var userRoles = await _userManager.GetRolesAsync(user);
        return userRoles.ToList();
    }
}
