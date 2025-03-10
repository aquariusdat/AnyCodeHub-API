using AnyCodeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            // Table configuration
            builder.ToTable("Comments");

            // Primary key
            builder.HasKey(c => c.Id);

            // Required properties
            builder.Property(c => c.Content).IsRequired();
            builder.Property(c => c.CreatedBy).IsRequired();

            // Optional properties with defaults
            builder.Property(c => c.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(c => c.IsDeleted).HasDefaultValue(false);
        }
    }
}