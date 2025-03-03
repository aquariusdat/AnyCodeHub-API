using AnyCodeHub.Domain.Entities;
using AnyCodeHub.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnyCodeHub.Persistence
{
    public sealed class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder) => builder.ApplyConfigurationsFromAssembly(Persistence.AssemblyReferences.Assembly);
        public DbSet<AppUser> AppUses { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<ActionInFunction> ActionInFunctions { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
