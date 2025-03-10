using AnyCodeHub.Domain.Entities;
using AnyCodeHub.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class CourseSectionConfiguration : IEntityTypeConfiguration<CourseSection>
    {
        public void Configure(EntityTypeBuilder<CourseSection> builder)
        {
            // Table configuration
            builder.ToTable(TableNames.CourseSection);

            // Primary key
            builder.HasKey(cs => cs.Id);

            // Foreign keys
            builder.HasOne<Course>()
                .WithMany(c => c.CourseSections)
                .HasForeignKey(cs => cs.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Required properties
            builder.Property(cs => cs.CourseId).IsRequired();
            builder.Property(cs => cs.Name).IsRequired();
            builder.Property(cs => cs.CreatedBy).IsRequired();

            // Optional properties with defaults
            builder.Property(cs => cs.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(cs => cs.IsDeleted).HasDefaultValue(false);
        }
    }
}