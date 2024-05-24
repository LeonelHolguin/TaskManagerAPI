using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Domain.Common;
using TaskManager.Core.Domain.Entities;
using Task = TaskManager.Core.Domain.Entities.Task;

namespace TaskManager.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationContext(DbContextOptions<ApplicationContext> options, IHttpContextAccessor httpContextAccessor) : base(options) 
        { 
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var currentUserName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch(entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.UtcNow;
                        entry.Entity.CreatedBy = currentUserName ?? "UserRegistration";
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = currentUserName;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region tables
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Task>().ToTable("Tasks");
            #endregion

            #region primary keys
            modelBuilder.Entity<User>().HasKey(user => user.Id);
            modelBuilder.Entity<Task>().HasKey(task => task.Id);
            #endregion

            #region relationships
            modelBuilder.Entity<User>()
                .HasMany<Task>(user => user.Tasks)
                .WithOne(task => task.User)
                .HasForeignKey(task => task.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region property configurations

            #region User
            modelBuilder.Entity<User>().Property(p => p.Name)
                .IsRequired().HasMaxLength(30);

            modelBuilder.Entity<User>().Property(p => p.LastName)
                .IsRequired().HasMaxLength(30);

            modelBuilder.Entity<User>().Property(p => p.Email)
                .IsRequired().HasMaxLength(100);

            modelBuilder.Entity<User>().Property(p => p.UserName)
                .IsRequired().HasMaxLength(25);

            modelBuilder.Entity<User>().Property(p => p.Password)
                .IsRequired();

            modelBuilder.Entity<User>().Property(p => p.Created)
                .IsRequired();

            modelBuilder.Entity<User>().Property(p => p.CreatedBy)
                .IsRequired().HasMaxLength(60);

            modelBuilder.Entity<User>().Property(p => p.LastModified);

            modelBuilder.Entity<User>().Property(p => p.LastModifiedBy)
                .HasMaxLength(60);
            #endregion

            #region Task
            modelBuilder.Entity<Task>().Property(p => p.Name)
                .IsRequired().HasMaxLength(30);

            modelBuilder.Entity<Task>().Property(p => p.SerialNumber)
                .IsRequired();

            modelBuilder.Entity<Task>().Property(p => p.Description)
                .IsRequired();

            modelBuilder.Entity<Task>().Property(p => p.Created)
                .IsRequired();

            modelBuilder.Entity<Task>().Property(p => p.CreatedBy)
                .IsRequired().HasMaxLength(60);

            modelBuilder.Entity<Task>().Property(p => p.LastModified);

            modelBuilder.Entity<Task>().Property(p => p.LastModifiedBy)
                .HasMaxLength(60);
            #endregion

            #endregion
        }
    }
}
