using AnyCodeHub.Domain.Entities;
using AnyCodeHub.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            // Table configuration
            builder.ToTable("Ratings");

            // Primary key
            builder.HasKey(r => r.Id);

            // Foreign keys
            builder.HasOne<Course>()
                .WithMany(c => c.Ratings)
                .HasForeignKey(r => r.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Required properties
            builder.Property(r => r.CourseId).IsRequired();
            builder.Property(r => r.UserId).IsRequired();
            builder.Property(r => r.Rate).IsRequired();
            builder.Property(r => r.CreatedBy).IsRequired();

            // Optional properties with defaults
            builder.Property(r => r.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(r => r.IsDeleted).HasDefaultValue(false);
        }
    }
}