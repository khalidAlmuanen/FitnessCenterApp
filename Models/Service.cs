using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }  // Cardio, Yoga, Pilates...

        [Range(1, 600)]
        public int DurationMinutes { get; set; } // Hizmet Süresi (dk)

        [Range(0, 100000)]
        public decimal Price { get; set; }       // Ücret

        public string? Description { get; set; }

        // FK
        public int GymId { get; set; }

        // Navigation
        public Gym Gym { get; set; }
        public ICollection<Trainer>? Trainers { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
