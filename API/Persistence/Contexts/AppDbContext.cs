using API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Board> Boards { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Board>().ToTable("Boards");
            builder.Entity<Board>().HasKey(p => p.BoardId);
            builder.Entity<Board>().Property(p => p.BoardId).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Board>().Property(p => p.Name).IsRequired().HasMaxLength(30);
            builder.Entity<Board>().HasMany(p => p.Tasks).WithOne(p => p.Board).HasForeignKey(prop => prop.BoardId);

            builder.Entity<Task>().ToTable("Tasks");
            builder.Entity<Task>().HasKey(p => p.TaskId);
            builder.Entity<Task>().Property(p => p.TaskId).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Task>().Property(p => p.Description).IsRequired().HasMaxLength(30);
            builder.Entity<Task>().Property(p => p.IsCompleted).IsRequired();
            builder.Entity<Task>().Property(p => p.Priority).IsRequired();

            builder.Entity<Board>().HasData(
                new Board { BoardId = 1, Name = "Tasks" }
                );
            builder.Entity<Task>().HasData(
                new Task { TaskId = 1, Description = "Hello World", Priority = EPriority.low, IsCompleted = false, BoardId = 1 }
                );
        }
    }
}
