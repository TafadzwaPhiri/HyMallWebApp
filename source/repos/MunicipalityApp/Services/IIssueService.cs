using MunicipalityApp.Models;

namespace MunicipalityApp.Services
{
    public interface IIssueService
    {
        Issue Add(Issue issue);
        Issue? GetById(int id);
        IEnumerable<Issue> GetAll();
        void LoadFromFile(); // optional persistence
        void SaveToFile();
        //void UpdateStatus(int id, string newStatus);
    }
}
