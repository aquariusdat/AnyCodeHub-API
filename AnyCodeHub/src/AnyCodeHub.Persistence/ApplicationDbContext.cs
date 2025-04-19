using AnyCodeHub.Domain.Entities;
using AnyCodeHub.Domain.Entities.Identity;
using AnyCodeHub.Persistence.Outbox;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnyCodeHub.Persistence
{
    public sealed class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
            => builder.ApplyConfigurationsFromAssembly(Persistence.AssemblyReferences.Assembly);

        public DbSet<AppUser> AppUses { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<ActionInFunction> ActionInFunctions { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        //public DbSet<Comment> Comments { get; set; }
        public DbSet<CourseBenefit> CourseBenefits { get; set; }
        public DbSet<CourseRequirement> CourseRequirements { get; set; }
        public DbSet<CourseSection> CourseSections { get; set; }
        public DbSet<CourseQA> CourseQAs { get; set; }
        public DbSet<CourseTechnology> CourseTechnologies { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonComment> LessonComments { get; set; }
        public DbSet<SectionLesson> SectionLessons { get; set; }

    }
}
