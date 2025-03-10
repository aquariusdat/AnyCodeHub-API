using AnyCodeHub.Domain.Entities;
using AnyCodeHub.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class CourseConfigruration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable(nameof(TableNames.Course));

            builder.HasKey(t => t.Id);
            builder.HasIndex(t => t.Slug).IsUnique();

            builder.Property(t => t.Id).IsRequired();
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(150);
            builder.Property(t => t.AuthorId).IsRequired();
            builder.Property(t => t.Level).HasConversion<int>();
            builder.Property(t => t.TotalViews).HasDefaultValue(0);
            builder.Property(t => t.Rating).HasDefaultValue(0);
        }
    }
}
