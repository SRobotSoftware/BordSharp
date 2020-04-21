using System.Collections.Generic;

namespace API.Domain.Models
{
    public class Board
    {
        public int BoardId { get; set; }
        public string Name { get; set; }
        public List<Task> Tasks { get; } = new List<Task>();
    }
}
