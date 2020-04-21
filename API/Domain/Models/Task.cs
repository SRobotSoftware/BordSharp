namespace API.Domain.Models
{
    public class Task
    {
        public int TaskId { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public EPriority Priority { get; set; }
        public int BoardId { get; set; }
        public Board Board { get; set; }
    }
}
