using AnyCodeHub.Domain.Entities.Identity;
using AnyCodeHub.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Action = AnyCodeHub.Domain.Entities.Identity.Action;

namespace AnyCodeHub.Persistence.Configurations;
internal class ActionConfiguration : IEntityTypeConfiguration<Action>
{
    public void Configure(EntityTypeBuilder<Action> builder)
    {
        builder.ToTable(TableNames.Actions);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasMaxLength(50);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired(true);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.SortOrder).HasDefaultValue(null);

        // Each User can have many Permission
        builder.HasMany(e => e.Permissions)
            .WithOne()
            .HasForeignKey(p => p.ActionId)
            .IsRequired();

        // Each User can have many ActionInFunction
        builder.HasMany(e => e.ActionInFunctions)
            .WithOne()
            .HasForeignKey(aif => aif.ActionId)
            .IsRequired();


        builder.HasData(new List<Action>()
        {
            new Action()
            {
                Id = "CREATE",
                Name = "CREATE",
                IsActive = true,
                SortOrder = 0
            },
            new Action()
            {
                Id = "UPDATE",
                Name = "UPDATE",
                IsActive = true,
                SortOrder = 0
            },
            new Action()
            {
                Id = "READ",
                Name = "READ",
                IsActive = true,
                SortOrder = 0
            },
            new Action()
            {
                Id = "DELETE",
                Name = "DELETE",
                IsActive = true,
                SortOrder = 0
            },
        });
    }
}
