using AnyCodeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class CourseCategoryConfiguration : IEntityTypeConfiguration<CourseCategory>
    {
        public void Configure(EntityTypeBuilder<CourseCategory> builder)
        {
            // Table configuration
            builder.ToTable("CourseCategories");

            // Primary key
            builder.HasKey(cc => cc.Id);

            // Foreign keys
            builder.HasOne<Course>()
                .WithMany(c => c.CourseCategories)
                .HasForeignKey(cc => cc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(cc => cc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Required properties
            builder.Property(cc => cc.CourseId).IsRequired();
            builder.Property(cc => cc.CategoryId).IsRequired();
            builder.Property(cc => cc.CreatedBy).IsRequired();

            // Optional properties with defaults
            builder.Property(cc => cc.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(cc => cc.IsDeleted).HasDefaultValue(false);
        }
    }
}