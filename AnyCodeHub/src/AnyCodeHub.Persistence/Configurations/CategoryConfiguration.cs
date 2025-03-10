using AnyCodeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Table configuration
            builder.ToTable("Categories");

            // Primary key
            builder.HasKey(c => c.Id);

            // Required properties
            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.CreatedBy).IsRequired();

            // Optional properties with defaults
            builder.Property(c => c.Description).IsRequired(false);
            builder.Property(c => c.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(c => c.IsDeleted).HasDefaultValue(false);
        }
    }
}