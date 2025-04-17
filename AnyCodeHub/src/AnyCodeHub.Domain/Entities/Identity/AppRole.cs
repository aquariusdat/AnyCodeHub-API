using Microsoft.AspNetCore.Identity;

namespace AnyCodeHub.Domain.Entities.Identity;

public class AppRole : IdentityRole<Guid>
{
    private readonly static List<AppRole> _listDefaultRoles = new List<AppRole>() {
            new AppRole()
            {
                Id = Guid.NewGuid(),
                Name = Contract.Enumerations.UserRole.ADMIN,
                Description = Contract.Enumerations.UserRole.ADMIN,
                RoleCode = Contract.Enumerations.UserRole.ADMIN,
                NormalizedName = Contract.Enumerations.UserRole.ADMIN,
            },
             new AppRole()
            {
                Id = Guid.NewGuid(),
                Name = Contract.Enumerations.UserRole.MODERATOR,
                Description = Contract.Enumerations.UserRole.MODERATOR,
                RoleCode = Contract.Enumerations.UserRole.MODERATOR,
                NormalizedName = Contract.Enumerations.UserRole.MODERATOR,
            },
              new AppRole()
            {
                Id = Guid.NewGuid(),
                Name = Contract.Enumerations.UserRole.GUEST,
                Description = Contract.Enumerations.UserRole.GUEST,
                RoleCode = Contract.Enumerations.UserRole.GUEST,
                NormalizedName = Contract.Enumerations.UserRole.GUEST,
            },
               new AppRole()
            {
                Id = Guid.NewGuid(),
                Name = Contract.Enumerations.UserRole.USER,
                Description = Contract.Enumerations.UserRole.USER,
                RoleCode = Contract.Enumerations.UserRole.USER,
                NormalizedName = Contract.Enumerations.UserRole.USER,
            },
        };


    public static List<AppRole> GetListDefaultValues()
    {
        return _listDefaultRoles;
    }

    public string Description { get; set; }
    public string RoleCode { get; set; }

    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
    public virtual ICollection<IdentityRoleClaim<Guid>> Claims { get; set; }
    public virtual ICollection<Permission> Permissions { get; set; }
}
