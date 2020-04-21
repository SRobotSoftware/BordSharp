using Microsoft.EntityFrameworkCore;

namespace API.Domain.Models
{
    public class BordsContext : DbContext
    {
        public BordsContext(DbContextOptions<BordsContext> options) : base(options) { }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }
}
