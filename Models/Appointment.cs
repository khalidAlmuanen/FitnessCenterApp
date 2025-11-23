using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    public enum AppointmentStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }

    public class Appointment
    {
        public int Id { get; set; }

        // FK
        [Required]
        public string MemberId { get; set; }

        [Required]
        public int TrainerId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        // Appointment Time
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        // Approval Mechanism
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public string? Notes { get; set; }

        // Navigation
        public ApplicationUser Member { get; set; }
        public Trainer Trainer { get; set; }
        public Service Service { get; set; }
    }
}
