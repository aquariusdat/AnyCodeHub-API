using AnyCodeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            // Table configuration
            builder.ToTable("Sections");

            // Primary key
            builder.HasKey(s => s.Id);

            // Foreign keys
            builder.HasOne<Course>()
                .WithMany(c => c.Sections)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Required properties
            builder.Property(s => s.Name).IsRequired();
            builder.Property(s => s.CourseId).IsRequired();
            builder.Property(s => s.CreatedBy).IsRequired();

            // Optional properties with defaults
            builder.Property(s => s.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(s => s.IsDeleted).HasDefaultValue(false);
        }
    }
}