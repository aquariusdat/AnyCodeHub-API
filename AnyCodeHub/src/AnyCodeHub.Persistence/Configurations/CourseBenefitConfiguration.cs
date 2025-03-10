using AnyCodeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class CourseBenefitConfiguration : IEntityTypeConfiguration<CourseBenefit>
    {
        public void Configure(EntityTypeBuilder<CourseBenefit> builder)
        {
            // Table configuration
            builder.ToTable("CourseBenefits");

            // Primary key
            builder.HasKey(cb => cb.Id);

            // Foreign keys
            builder.HasOne<Course>()
                .WithMany(c => c.CourseBenefits)
                .HasForeignKey(cb => cb.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Required properties
            builder.Property(cb => cb.CourseId).IsRequired();
            builder.Property(cb => cb.Description).IsRequired();
            builder.Property(cb => cb.CreatedBy).IsRequired();

            // Optional properties with defaults
            builder.Property(cb => cb.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(cb => cb.IsDeleted).HasDefaultValue(false);
        }
    }
}