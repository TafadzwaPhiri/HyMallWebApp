namespace MunicipalityApp.Models
{
    public class GraphNode
    {
        public int Id { get; set; }
        public ServiceRequest Data { get; set; }
        public List<(GraphNode node, int weight)> Neighbors { get; set; } = new();
        public GraphNode(ServiceRequest data)
        {
            Data = data;
        }
    }
}
