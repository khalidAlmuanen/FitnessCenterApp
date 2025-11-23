using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    public class TrainerAvailability
    {
        public int Id { get; set; }

        public int TrainerId { get; set; }

        [Required]
        public DayOfWeek Day { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation
        public Trainer Trainer { get; set; }
    }
}
