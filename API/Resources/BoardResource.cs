using System.Collections.Generic;

namespace API.Resources
{
    public class BoardResource
    {
        public int BoardId { get; set; }
        public string Name { get; set; }
        public List<TaskMinimalResource> Tasks { get; set; }
    }
}
