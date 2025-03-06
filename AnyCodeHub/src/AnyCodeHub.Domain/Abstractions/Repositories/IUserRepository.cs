using AnyCodeHub.Domain.Entities.Identity;

namespace AnyCodeHub.Domain.Abstractions.Repositories;

public interface IUserRepository
{
    Task<AppUser> GetByEmail(string Email);
    Task<IEnumerable<string>> GetRoleByEmail(string Email);
}
