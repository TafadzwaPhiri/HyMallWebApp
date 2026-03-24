namespace MunicipalityApp.Models
{
    public class EventItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Category { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Description { get; set; } = "";
        public int Priority { get; set; } = 0; // higher value -> higher priority
        public List<string> Tags { get; set; } = new List<string>();
    }
}
