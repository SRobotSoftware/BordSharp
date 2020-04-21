using System.ComponentModel.DataAnnotations;

namespace API.Resources
{
    public class SaveBoardResource
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
