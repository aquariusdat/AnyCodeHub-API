using Microsoft.AspNetCore.Identity;

namespace AnyCodeHub.Domain.Entities.Identity;

public class AppUser : IdentityUser<Guid>
{
    public AppUser()
    {

    }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public DateTime? BirthOfDate { get; set; }
    public bool IsActive { get; set; }
    public bool IsAdmin { get; set; }
    public string? Profile { get; set; }
    public string? Intro { get; set; }
    public string? Avatar { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Guid CreatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }

    public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }
    public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }
    public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; }
    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
}
