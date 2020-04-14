using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace bord
{
    public class BordsContext : DbContext
    {
        public BordsContext(DbContextOptions<BordsContext> options) : base(options) { }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }

    public class Board
    {
        public int BoardId { get; set; }
        public string Name { get; set; }
        public List<Task> Tasks { get; } = new List<Task>();
    }

    public class Task
    {
        public int TaskId { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public int Priority { get; set; }
        public int BoardId { get; set; }
        public Board Board { get; set; }
    }
}
