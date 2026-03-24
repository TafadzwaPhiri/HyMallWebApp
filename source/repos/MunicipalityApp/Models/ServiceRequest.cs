namespace MunicipalityApp.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Details { get; set; } = "";
        public string Status { get; set; } = "Open";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Priority { get; set; } = 0;
        public string Category { get; set; } = "";
        public string Description { get; set; } = "";
    }
}