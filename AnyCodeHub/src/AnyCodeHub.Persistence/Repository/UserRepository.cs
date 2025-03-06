using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities.Identity;
using AnyCodeHub.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace AnyCodeHub.Persistence.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserManager<AppUser> _userManager;

    public UserRepository(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }


    public async Task<AppUser> GetByEmail(string Email)
    {
        var user = await _userManager.FindByEmailAsync(Email) ?? throw new UserException.UserNotFoundException(Email);

        if (user.IsDeleted)
            throw new UserException.UserHasBeenDeletedException(Email);

        if (!user.IsActive)
            throw new UserException.DeactiveUserException(Email);

        if (user.LockoutEnabled)
        {
            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.Now)
            {
                throw new UserException.UserHasBeenLockedException(user.UserName, user.LockoutEnd.Value);
            }

            user.LockoutEnd = null;
            user.AccessFailedCount = 0;
            user.LockoutEnabled = false;
        }

        return user;
    }

    public async Task<IEnumerable<string>> GetRoleByEmail(string Email)
    {
        AppUser findedUser = await GetByEmail(Email);
        return await _userManager.GetRolesAsync(findedUser);
    }
}
