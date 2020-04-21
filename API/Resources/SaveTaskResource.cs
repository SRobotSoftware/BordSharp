using API.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace API.Resources
{
    public class SaveTaskResource
    {
        [Required]
        [MaxLength(30)]
        public string Description { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
        [Required]
        public EPriority Priority { get; set; }
        public int BoardId { get; set; }
    }
}
