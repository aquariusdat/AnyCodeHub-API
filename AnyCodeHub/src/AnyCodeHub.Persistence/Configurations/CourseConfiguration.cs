using AnyCodeHub.Domain.Entities;
using AnyCodeHub.Domain.Entities.Identity;
using AnyCodeHub.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            // Table configuration
            builder.ToTable(nameof(TableNames.Course));

            // Primary key
            builder.HasKey(t => t.Id);

            // Unique constraints
            builder.HasIndex(t => t.Slug).IsUnique();

            // Required properties
            builder.Property(t => t.Id).IsRequired();
            builder.Property(t => t.Name).IsRequired().HasMaxLength(150);
            builder.Property(t => t.AuthorId).IsRequired();
            builder.Property(t => t.Status).IsRequired().HasMaxLength(50);
            builder.Property(t => t.Price).IsRequired().HasPrecision(18, 2);

            // Optional properties with defaults
            builder.Property(t => t.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(t => t.IsDeleted).HasDefaultValue(false);
            builder.Property(t => t.Level).HasConversion<int>();
            builder.Property(t => t.TotalViews).HasDefaultValue(0);
            builder.Property(t => t.Rating).HasDefaultValue(0);
            builder.Property(t => t.TotalDuration).HasDefaultValue(0);

            // Optional properties
            builder.Property(t => t.Description).HasMaxLength(1000);
            builder.Property(t => t.SalePrice).HasPrecision(18, 2);
            builder.Property(t => t.ImageUrl).HasMaxLength(500);
            builder.Property(t => t.VideoUrl).HasMaxLength(500);

            // Relationships - One-to-Many
            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationships - One-to-Many (Course has many Sections)
            builder.HasMany<Section>()
                .WithOne()
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationships - One-to-Many (Course has many UserCourses) 
            builder.HasMany<UserCourse>()
                .WithOne()
                .HasForeignKey(uc => uc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationships - One-to-Many (Course has many Ratings)
            builder.HasMany<Rating>()
                .WithOne()
                .HasForeignKey(r => r.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationships - Many-to-Many (Course to Categories through CourseCategory)
            builder.HasMany<CourseCategory>()
                .WithOne()
                .HasForeignKey(cc => cc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationships - Many-to-Many (Course to Comments through CourseComment)
            builder.HasMany<CourseComment>()
                .WithOne()
                .HasForeignKey(cc => cc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationships - Many-to-Many (Course to Technologies through CourseTechnology)
            builder.HasMany<CourseTechnology>()
                .WithOne()
                .HasForeignKey(ct => ct.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationships - One-to-Many (Course has many CourseQAs)
            builder.HasMany<CourseQA>()
                .WithOne()
                .HasForeignKey(cqa => cqa.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationships - One-to-Many (Course has many CourseBenefits)
            builder.HasMany<CourseBenefit>()
                .WithOne()
                .HasForeignKey(cb => cb.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationships - One-to-Many (Course has many CourseRequirements)
            builder.HasMany<CourseRequirement>()
                .WithOne()
                .HasForeignKey(cr => cr.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationships - One-to-Many (Course has many CourseSections)
            builder.HasMany<CourseSection>()
                .WithOne()
                .HasForeignKey(cs => cs.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
