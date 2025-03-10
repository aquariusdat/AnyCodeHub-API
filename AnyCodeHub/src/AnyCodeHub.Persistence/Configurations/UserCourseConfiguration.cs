using AnyCodeHub.Domain.Entities;
using AnyCodeHub.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class UserCourseConfiguration : IEntityTypeConfiguration<UserCourse>
    {
        public void Configure(EntityTypeBuilder<UserCourse> builder)
        {
            // Table configuration
            builder.ToTable("UserCourses");

            // Primary key
            builder.HasKey(uc => uc.Id);

            // Foreign keys
            builder.HasOne<Course>()
                .WithMany(c => c.UserCourses)
                .HasForeignKey(uc => uc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Required properties
            builder.Property(uc => uc.CourseId).IsRequired();
            builder.Property(uc => uc.UserId).IsRequired();
            builder.Property(uc => uc.CreatedBy).IsRequired();

            // Optional properties with defaults
            builder.Property(uc => uc.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(uc => uc.IsDeleted).HasDefaultValue(false);
        }
    }
}