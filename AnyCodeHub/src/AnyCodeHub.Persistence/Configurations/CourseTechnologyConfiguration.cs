using AnyCodeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class CourseTechnologyConfiguration : IEntityTypeConfiguration<CourseTechnology>
    {
        public void Configure(EntityTypeBuilder<CourseTechnology> builder)
        {
            // Table configuration
            builder.ToTable("CourseTechnologies");

            // Primary key
            builder.HasKey(ct => ct.Id);

            // Foreign keys
            builder.HasOne<Course>()
                .WithMany(c => c.CourseTechnologies)
                .HasForeignKey(ct => ct.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Technology>()
                .WithMany()
                .HasForeignKey(ct => ct.TechnologyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Required properties
            builder.Property(ct => ct.CourseId).IsRequired();
            builder.Property(ct => ct.TechnologyId).IsRequired();
            builder.Property(ct => ct.CreatedBy).IsRequired();

            // Optional properties with defaults
            builder.Property(ct => ct.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(ct => ct.IsDeleted).HasDefaultValue(false);
        }
    }
}