using API.Domain.Models;

namespace API.Resources
{
    public class TaskResource
    {
        public int TaskId { get; set; }
        public int BoardId { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public string Priority { get; set; }
        public BoardMinimalResource Board { get; set; }
    }
}
