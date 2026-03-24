namespace MunicipalityApp.Models
{
    public class EventSearchModel
    {
        public string Query { get; set; } = "";
        public List<EventItem> Results { get; set; } = new();
        public List<EventItem> Recommendations { get; set; } = new();
    }
}
