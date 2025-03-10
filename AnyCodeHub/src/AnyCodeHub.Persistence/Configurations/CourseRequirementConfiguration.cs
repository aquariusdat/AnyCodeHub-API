using AnyCodeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class CourseRequirementConfiguration : IEntityTypeConfiguration<CourseRequirement>
    {
        public void Configure(EntityTypeBuilder<CourseRequirement> builder)
        {
            // Table configuration
            builder.ToTable("CourseRequirements");

            // Primary key
            builder.HasKey(cr => cr.Id);

            // Foreign keys
            builder.HasOne<Course>()
                .WithMany(c => c.CourseRequirements)
                .HasForeignKey(cr => cr.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Required properties
            builder.Property(cr => cr.CourseId).IsRequired();
            builder.Property(cr => cr.RequirementContent).IsRequired();
            builder.Property(cr => cr.CreatedBy).IsRequired();

            // Optional properties with defaults
            builder.Property(cr => cr.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(cr => cr.IsDeleted).HasDefaultValue(false);
        }
    }
}