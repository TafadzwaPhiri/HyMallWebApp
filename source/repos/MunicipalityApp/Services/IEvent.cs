using MunicipalityApp.Models;

namespace MunicipalityApp.Services
{
    public interface IEventService
    {
        void SeedSampleEvents();
        IEnumerable<EventItem> GetAll();
        EventItem? GetById(int id);
        void Add(EventItem item);
        IEnumerable<EventItem> Search(string query);
        IEnumerable<EventItem> RecommendBasedOnSearch(string query, int take = 5);
    }
}
