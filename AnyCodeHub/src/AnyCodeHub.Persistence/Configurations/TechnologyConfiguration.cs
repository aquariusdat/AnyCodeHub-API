using AnyCodeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class TechnologyConfiguration : IEntityTypeConfiguration<Technology>
    {
        public void Configure(EntityTypeBuilder<Technology> builder)
        {
            // Table configuration
            builder.ToTable("Technologies");

            // Primary key
            builder.HasKey(t => t.Id);

            // Required properties
            builder.Property(t => t.Name).IsRequired();

            // Optional properties with defaults
            builder.Property(t => t.Description).IsRequired(false);
        }
    }
}