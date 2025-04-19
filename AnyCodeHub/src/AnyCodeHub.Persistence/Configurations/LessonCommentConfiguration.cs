using AnyCodeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyCodeHub.Persistence.Configurations
{
    public class LessonCommentConfiguration : IEntityTypeConfiguration<LessonComment>
    {
        public void Configure(EntityTypeBuilder<LessonComment> builder)
        {
            // Table configuration
            builder.ToTable("LessonComments");

            // Primary key
            builder.HasKey(lc => lc.Id);

            // Foreign keys
            builder.HasOne<Lesson>()
                .WithMany()
                .HasForeignKey(lc => lc.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.HasOne<Comment>()
            //    .WithMany()
            //    .HasForeignKey(lc => lc.CommentId)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Required properties
            builder.Property(lc => lc.LessonId).IsRequired();
            builder.Property(lc => lc.CommentId).IsRequired();
            builder.Property(lc => lc.CreatedBy).IsRequired();

            // Optional properties with defaults
            builder.Property(lc => lc.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(lc => lc.IsDeleted).HasDefaultValue(false);
        }
    }
}