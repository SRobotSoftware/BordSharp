using API.Domain.Models;

namespace API.Resources
{
    public class TaskMinimalResource
    {
        public int TaskId { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public string Priority { get; set; }
    }
}
