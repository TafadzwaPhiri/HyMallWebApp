namespace MunicipalityApp.Models
{
    public class BSTNode
    {
        public ServiceRequest Data { get; set; }
        public BSTNode? Left { get; set; }
        public BSTNode? Right { get; set; }
        public int Height { get; set; }

        public BSTNode(ServiceRequest req)
        {
            Data = req;
            Height = 1;
        }
    }
}
