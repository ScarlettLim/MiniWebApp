using System.ComponentModel.DataAnnotations;

namespace WebApp1.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
