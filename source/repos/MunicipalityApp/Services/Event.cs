using MunicipalityApp.Models;
using System.Collections.Concurrent;
using System.Text.Json;

namespace MunicipalityApp.Services
{
    public class EventService : IEventService
    {
        // Uses several data structures:
        // - dictionary by id
        // - priority queue for high priority events (we'll simulate with SortedSet)
        // - stacks/queues for recent and upcoming flows
        // - set for unique categories/tags

        private readonly ConcurrentDictionary<int, EventItem> _byId = new();
        private readonly Queue<EventItem> _recentQueue = new();
        private readonly Stack<EventItem> _recentStack = new();
        private readonly SortedSet<(int priority, int id)> _priorityIndex = new(new PriorityComparer());
        private readonly HashSet<string> _categories = new(StringComparer.OrdinalIgnoreCase);
        private int _counter = 0;
        private readonly string _dataFile = Path.Combine(Directory.GetCurrentDirectory(), "Data", "events.json");

        public EventService()
        {
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Data"));
            LoadFromFile();
            if (!_byId.Any())
                SeedSampleEvents();
        }

        public void SeedSampleEvents()
        {
            Add(new EventItem { Title = "Community Clean-up", Category = "Sanitation", Date = DateTime.UtcNow.AddDays(3), Description = "Volunteers needed to clean the park.", Priority = 5, Tags = new List<string> { "clean-up", "park" } });
            Add(new EventItem { Title = "Roadworks Notice", Category = "Roads", Date = DateTime.UtcNow.AddDays(7), Description = "Scheduled pothole repairs on Main St.", Priority = 7, Tags = new List<string> { "roads", "repairs" } });
            Add(new EventItem { Title = "Electricity Safety Workshop", Category = "Utilities", Date = DateTime.UtcNow.AddDays(10), Description = "Tips on safe electricity use.", Priority = 4, Tags = new List<string> { "safety", "workshop" } });
        }

        public IEnumerable<EventItem> GetAll()
        {
            return _byId.Values.OrderBy(e => e.Date);
        }

        public EventItem? GetById(int id) => _byId.TryGetValue(id, out var e) ? e : null;

        public void Add(EventItem item)
        {
            item.Id = System.Threading.Interlocked.Increment(ref _counter);
            _byId[item.Id] = item;
            _categories.Add(item.Category ?? "");
            _recentQueue.Enqueue(item);
            _recentStack.Push(item);
            _priorityIndex.Add((-item.Priority, item.Id)); // negative because SortedSet sorts ascending
            SaveToFile();
        }

        public IEnumerable<EventItem> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return GetAll();
            query = query.Trim().ToLowerInvariant();
            var results = _byId.Values.Where(e =>
                e.Title.ToLowerInvariant().Contains(query) ||
                e.Category.ToLowerInvariant().Contains(query) ||
                e.Description.ToLowerInvariant().Contains(query) ||
                e.Tags.Any(t => t.ToLowerInvariant().Contains(query)))
                .OrderBy(e => e.Date);
            return results;
        }

        public IEnumerable<EventItem> RecommendBasedOnSearch(string query, int take = 5)
        {
            // Simple recommendation:
            // - tokenize query
            // - look for tag/category matches and priority
            if (string.IsNullOrWhiteSpace(query)) return _byId.Values.OrderByDescending(e => e.Priority).Take(take);
            var tokens = query.ToLowerInvariant().Split(new[] { ' ', ',', '.' }, StringSplitOptions.RemoveEmptyEntries);
            var scores = new Dictionary<int, int>();
            foreach (var e in _byId.Values)
            {
                int score = 0;
                foreach (var t in tokens)
                {
                    if (e.Title?.ToLowerInvariant().Contains(t) == true) score += 5;
                    if (e.Description?.ToLowerInvariant().Contains(t) == true) score += 3;
                    if (e.Category?.ToLowerInvariant().Contains(t) == true) score += 4;
                    if (e.Tags.Any(tag => tag.ToLowerInvariant().Contains(t))) score += 6;
                }
                score += e.Priority * 2;
                if (score > 0) scores[e.Id] = score;
            }

            return scores.OrderByDescending(kv => kv.Value).Select(kv => _byId[kv.Key]).Take(take);
        }

        private void LoadFromFile()
        {
            if (!File.Exists(_dataFile)) return;
            try
            {
                var json = File.ReadAllText(_dataFile);
                var arr = JsonSerializer.Deserialize<List<EventItem>>(json);
                if (arr != null)
                {
                    foreach (var e in arr)
                    {
                        _byId[e.Id] = e;
                        if (e.Id > _counter) _counter = e.Id;
                        _categories.Add(e.Category ?? "");
                        _recentQueue.Enqueue(e);
                        _recentStack.Push(e);
                        _priorityIndex.Add((-e.Priority, e.Id));
                    }
                }
            }
            catch { }
        }

        private void SaveToFile()
        {
            var list = _byId.Values.OrderBy(e => e.Id).ToList();
            var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_dataFile, json);
        }

        // helper sortedset comparer
        private class PriorityComparer : IComparer<(int priority, int id)>
        {
            public int Compare((int priority, int id) x, (int priority, int id) y)
            {
                var r = x.priority.CompareTo(y.priority);
                if (r != 0) return r;
                return x.id.CompareTo(y.id);
            }
        }
    }
}
