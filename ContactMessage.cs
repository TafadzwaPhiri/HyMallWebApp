namespace HyMall_App.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        // Add this for "Mark as Read"
        public bool IsRead { get; set; } = false;
    }

    // Form binding model (separate from DB entity)
    public class ContactForm
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Subject { get; set; } = "General Inquiry";
        public string Message { get; set; } = string.Empty;
    }
}