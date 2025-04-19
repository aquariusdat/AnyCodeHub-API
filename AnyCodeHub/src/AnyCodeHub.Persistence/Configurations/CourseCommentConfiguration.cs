using AnyCodeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class CourseCommentConfiguration : IEntityTypeConfiguration<CourseComment>
    {
        public void Configure(EntityTypeBuilder<CourseComment> builder)
        {
            // Table configuration
            builder.ToTable("CourseComments");

            // Primary key
            builder.HasKey(cc => cc.Id);

            // Foreign keys
            builder.HasOne<Course>()
                .WithMany(c => c.CourseComments)
                .HasForeignKey(cc => cc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.HasOne<Comment>()
            //    .WithMany()
            //    .HasForeignKey(cc => cc.CommentId)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Required properties
            builder.Property(cc => cc.CourseId).IsRequired();
            builder.Property(cc => cc.CommentId).IsRequired();
            builder.Property(cc => cc.CreatedBy).IsRequired();

            // Optional properties with defaults
            builder.Property(cc => cc.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(cc => cc.IsDeleted).HasDefaultValue(false);
        }
    }
}