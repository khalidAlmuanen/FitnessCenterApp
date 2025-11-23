using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string FullName { get; set; }

        [MaxLength(100)]
        public string? Specialization { get; set; } // Uzmanlık Alanı

        [Range(0, 50)]
        public int ExperienceYears { get; set; }

        [Range(0, 5)]
        public double Rating { get; set; }

        // FK
        public int GymId { get; set; }
        public int? ServiceId { get; set; } // Optional main service

        // Navigation
        public Gym Gym { get; set; }
        public Service? Service { get; set; }

        public ICollection<TrainerAvailability>? Availabilities { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
