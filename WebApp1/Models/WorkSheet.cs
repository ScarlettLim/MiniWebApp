using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace WebApp1.Models
{
    public class WorkSheet
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan TimeIn { get; set; }

        [Required]
        public TimeSpan TimeOut { get; set; }

        [Required]
        public double WorkHours => (TimeOut - TimeIn).TotalHours;
        
        [Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        [Required]
        public int ProjectTaskId { get; set; }
        public ProjectTask Task { get; set; }
    }
}
