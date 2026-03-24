using System.ComponentModel.DataAnnotations;

namespace HyMall_App.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public string SubmittedBy { get; set; } = string.Empty; // e.g., tenant email

        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        public string? Reason { get; set; } // Optional rejection reason

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}