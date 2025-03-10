using AnyCodeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            // Table configuration
            builder.ToTable("Lessons");

            // Primary key
            builder.HasKey(l => l.Id);

            // Foreign keys
            builder.HasOne<Section>()
                .WithMany()
                .HasForeignKey(l => l.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Required properties
            builder.Property(l => l.Title).IsRequired();
            builder.Property(l => l.SectionId).IsRequired();
            builder.Property(l => l.CreatedBy).IsRequired();

            // Optional properties with defaults
            builder.Property(l => l.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(l => l.IsDeleted).HasDefaultValue(false);
        }
    }
}