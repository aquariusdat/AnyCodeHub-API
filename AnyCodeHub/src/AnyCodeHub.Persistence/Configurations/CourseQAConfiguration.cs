using AnyCodeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class CourseQAConfiguration : IEntityTypeConfiguration<CourseQA>
    {
        public void Configure(EntityTypeBuilder<CourseQA> builder)
        {
            // Table configuration
            builder.ToTable("CourseQAs");

            // Primary key
            builder.HasKey(cqa => cqa.Id);

            // Foreign keys
            builder.HasOne<Course>()
                .WithMany()
                .HasForeignKey(cqa => cqa.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Required properties
            builder.Property(cqa => cqa.Question).IsRequired();
            builder.Property(cqa => cqa.Answer).IsRequired();
            builder.Property(cqa => cqa.CourseId).IsRequired();

            // Optional properties with defaults
            builder.Property(cqa => cqa.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(cqa => cqa.IsDeleted).HasDefaultValue(false);
        }
    }
}