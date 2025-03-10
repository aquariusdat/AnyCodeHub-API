using AnyCodeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class SectionLessonConfiguration : IEntityTypeConfiguration<SectionLesson>
    {
        public void Configure(EntityTypeBuilder<SectionLesson> builder)
        {
            // Table configuration
            builder.ToTable("SectionLessons");

            // Primary key
            builder.HasKey(sl => sl.Id);

            // Foreign keys
            builder.HasOne<Section>()
                .WithMany()
                .HasForeignKey(sl => sl.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Lesson>()
                .WithMany()
                .HasForeignKey(sl => sl.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            // Required properties
            builder.Property(sl => sl.SectionId).IsRequired();
            builder.Property(sl => sl.LessonId).IsRequired();
            builder.Property(sl => sl.CreatedBy).IsRequired();

            // Optional properties with defaults
            builder.Property(sl => sl.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(sl => sl.IsDeleted).HasDefaultValue(false);
        }
    }
}