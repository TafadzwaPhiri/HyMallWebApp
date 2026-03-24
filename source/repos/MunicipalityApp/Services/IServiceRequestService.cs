using MunicipalityApp.Models;

namespace MunicipalityApp.Services
{
    public interface IServiceRequestService
    {
        ServiceRequest Add(ServiceRequest request);
        ServiceRequest? GetById(int id);
        IEnumerable<ServiceRequest> GetAll();
        // BST operations
        void InsertIntoBST(ServiceRequest req);
        IEnumerable<ServiceRequest> InOrderBST();
        // Graph operations
        void AddGraphEdge(int idA, int idB, int weight);
        IEnumerable<ServiceRequest> BFSTraverse(int startId);
        IEnumerable<(ServiceRequest, ServiceRequest)> MinimumSpanningTree();
    }
}
