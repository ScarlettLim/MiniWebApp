using System.ComponentModel.DataAnnotations;

namespace WebApp1.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public List<ProjectTask> Tasks { get; set; } = new();
    }
}
