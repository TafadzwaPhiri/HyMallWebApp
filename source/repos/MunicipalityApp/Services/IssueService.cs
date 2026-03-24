using MunicipalityApp.Models;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace MunicipalityApp.Services
{
    public class IssueService : IIssueService
    {
        private readonly ConcurrentDictionary<int, Issue> _store = new();
        private int _counter = 0;
        private readonly string _dataFile = Path.Combine(Directory.GetCurrentDirectory(), "Data", "issues.json");

        public IssueService()
        {
            // ensure folder
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Data"));
            LoadFromFile();
        }

        public Issue Add(Issue issue)
        {
            issue.Id = System.Threading.Interlocked.Increment(ref _counter);
            _store.TryAdd(issue.Id, issue);
            SaveToFile();
            return issue;
        }

        public Issue? GetById(int id) => _store.TryGetValue(id, out var i) ? i : null;

        public IEnumerable<Issue> GetAll() => _store.Values.OrderByDescending(i => i.CreatedAt);

        public void LoadFromFile()
        {
            if (!File.Exists(_dataFile)) return;
            try
            {
                var json = File.ReadAllText(_dataFile);
                var list = JsonSerializer.Deserialize<List<Issue>>(json);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        _store[item.Id] = item;
                        if (item.Id > _counter) _counter = item.Id;
                    }
                }
            }
            catch { /* ignore parsing errors for now */ }
        }

        

        public void SaveToFile()
        {
            var list = _store.Values.OrderBy(i => i.Id).ToList();
            var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_dataFile, json);
        }
    }
}
