using AnyCodeHub.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AnyCodeHub.Persistence.Constants;

namespace AnyCodeHub.Persistence.Configurations;

internal sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable(TableNames.AppUsers);

        builder.HasKey(x => x.Id);

        builder.HasIndex(t => new { t.Email, t.UserName }).IsUnique();

        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.Property(x => x.IsAdmin).HasDefaultValue(null);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        builder.Property(x => x.Email).IsRequired(true);
        builder.Property(x => x.IsAdmin).IsRequired(true).HasDefaultValue(false);
        builder.Property(x => x.PhoneNumberConfirmed).HasDefaultValue(false);
        builder.Property(x => x.EmailConfirmed).HasDefaultValue(false);
        builder.Property(x => x.TwoFactorEnabled).HasDefaultValue(false);
        builder.Property(x => x.LockoutEnabled).HasDefaultValue(false);
        builder.Property(x => x.AccessFailedCount).HasDefaultValue(0);

        // Each User can have many UserClaims
        builder.HasMany(e => e.Claims)
            .WithOne()
            .HasForeignKey(uc => uc.UserId)
            .IsRequired();

        // Each User can have many UserLogins
        builder.HasMany(e => e.Logins)
            .WithOne()
            .HasForeignKey(ul => ul.UserId)
            .IsRequired();

        // Each User can have many UserTokens
        builder.HasMany(e => e.Tokens)
            .WithOne()
            .HasForeignKey(ut => ut.UserId)
            .IsRequired();

        // Each User can have many entries in the UserRole join table
        builder.HasMany(e => e.UserRoles)
            .WithOne()
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();
    }
}
