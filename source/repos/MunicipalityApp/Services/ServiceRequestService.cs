using MunicipalityApp.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc;

namespace MunicipalityApp.Services
{
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly ConcurrentDictionary<int, ServiceRequest> _store = new();
        private int _counter = 0;

        // BST root
        private BSTNode? _root;

        // Graph nodes
        private readonly Dictionary<int, GraphNode> _graph = new();

        // Simple heap via .NET PriorityQueue for priority operations
        private readonly System.Collections.Generic.PriorityQueue<ServiceRequest, int> _heap = new();

        public ServiceRequestService()
        {
            // seed sample requests (expanded)
            Seed();
        }

        public ServiceRequest Add(ServiceRequest request)
        {
            request.Id = System.Threading.Interlocked.Increment(ref _counter);
            // ensure createdAt if not set
            if (request.CreatedAt == default) request.CreatedAt = DateTime.UtcNow;

            _store[request.Id] = request;
            _heap.Enqueue(request, -request.Priority);

            // add to graph node (empty neighbor list initially)
            _graph[request.Id] = new GraphNode(request) { Id = request.Id };

            // insert into BST so the tree is kept up-to-date for every Add
            InsertIntoBST(request);

            return request;
        }

        public ServiceRequest? GetById(int id) => _store.TryGetValue(id, out var r) ? r : null;

        public IEnumerable<ServiceRequest> GetAll() => _store.Values.OrderByDescending(r => r.CreatedAt);

        public void UpdateStatus(int id, string newStatus)
        {
            if (_store.TryGetValue(id, out var req))
            {
                req.Status = newStatus;
                // also update graph node data if present
                if (_graph.TryGetValue(id, out var gnode))
                {
                    gnode.Data.Status = newStatus;
                }
            }
        }
        

        public void InsertIntoBST(ServiceRequest req)
        {
            var node = new BSTNode(req);
            _root = InsertAVL(_root, node);
        }

        public IEnumerable<ServiceRequest> InOrderBST()
        {
            var list = new List<ServiceRequest>();
            void Rec(BSTNode? n)
            {
                if (n == null) return;
                Rec(n.Left);
                list.Add(n.Data);
                Rec(n.Right);
            }
            Rec(_root);
            return list;
        }

        // basic AVL insert (balanced BST) by request.Id
        private BSTNode? InsertAVL(BSTNode? root, BSTNode node)
        {
            if (root == null) return node;
            if (node.Data.Id < root.Data.Id) root.Left = InsertAVL(root.Left, node);
            else root.Right = InsertAVL(root.Right, node);

            root.Height = 1 + Math.Max(GetHeight(root.Left), GetHeight(root.Right));
            int balance = GetBalance(root);

            // left left
            if (balance > 1 && node.Data.Id < root.Left!.Data.Id)
                return RightRotate(root);
            // right right
            if (balance < -1 && node.Data.Id > root.Right!.Data.Id)
                return LeftRotate(root);
            // left right
            if (balance > 1 && node.Data.Id > root.Left!.Data.Id)
            {
                root.Left = LeftRotate(root.Left!);
                return RightRotate(root);
            }
            // right left
            if (balance < -1 && node.Data.Id < root.Right!.Data.Id)
            {
                root.Right = RightRotate(root.Right!);
                return LeftRotate(root);
            }
            return root;
        }

        private int GetHeight(BSTNode? n) => n?.Height ?? 0;
        private int GetBalance(BSTNode? n) => n == null ? 0 : GetHeight(n.Left) - GetHeight(n.Right);

        private BSTNode RightRotate(BSTNode y)
        {
            var x = y.Left!;
            var T2 = x.Right;
            x.Right = y;
            y.Left = T2;
            y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));
            x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));
            return x;
        }

        private BSTNode LeftRotate(BSTNode x)
        {
            var y = x.Right!;
            var T2 = y.Left;
            y.Left = x;
            x.Right = T2;
            x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));
            y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));
            return y;
        }

        public void AddGraphEdge(int idA, int idB, int weight)
        {
            if (!_graph.ContainsKey(idA) || !_graph.ContainsKey(idB)) return;
            var a = _graph[idA];
            var b = _graph[idB];
            // prevent duplicate edges
            if (!a.Neighbors.Any(n => n.node.Id == b.Id))
                a.Neighbors.Add((b, weight));
            if (!b.Neighbors.Any(n => n.node.Id == a.Id))
                b.Neighbors.Add((a, weight));
        }

        public IEnumerable<ServiceRequest> BFSTraverse(int startId)
        {
            var results = new List<ServiceRequest>();
            if (!_graph.ContainsKey(startId)) return results;
            var visited = new HashSet<int>();
            var q = new Queue<GraphNode>();
            q.Enqueue(_graph[startId]);
            visited.Add(startId);
            while (q.Any())
            {
                var n = q.Dequeue();
                results.Add(n.Data);
                foreach (var (neighbor, w) in n.Neighbors)
                {
                    if (!visited.Contains(neighbor.Id))
                    {
                        visited.Add(neighbor.Id);
                        q.Enqueue(neighbor);
                    }
                }
            }
            return results;
        }

        // Minimum Spanning Tree (Prim's) returning edges of MST as pairs
        public IEnumerable<(ServiceRequest, ServiceRequest)> MinimumSpanningTree()
        {
            var result = new List<(ServiceRequest, ServiceRequest)>();
            if (!_graph.Any()) return result;
            var start = _graph.Values.First();
            var visited = new HashSet<int> { start.Id };
            var edges = new List<(int weight, int from, int to)>();
            edges.AddRange(start.Neighbors.Select(n => (n.weight, start.Id, n.node.Id)));

            while (edges.Any())
            {
                var e = edges.OrderBy(x => x.weight).First();
                edges.Remove(e);
                if (visited.Contains(e.to)) continue;
                visited.Add(e.to);
                result.Add((_graph[e.from].Data, _graph[e.to].Data));
                foreach (var next in _graph[e.to].Neighbors)
                {
                    if (!visited.Contains(next.node.Id))
                        edges.Add((next.weight, e.to, next.node.Id));
                }
            }
            return result;
        }

        private void Seed()
        {
            // Create 20 sample requests with varied categories, priorities, and statuses
            var samples = new List<ServiceRequest>
            {
                new ServiceRequest { Title = "Pothole on Main St", Details = "Large pothole by bus stop", Priority = 6, Status = "Pending" },
                new ServiceRequest { Title = "Streetlight out", Details = "Streetlight not working on 3rd Ave", Priority = 4, Status = "Pending" },
                new ServiceRequest { Title = "Overflowing bin", Details = "Garbage bin overflowing at Market Square", Priority = 3, Status = "Pending" },
                new ServiceRequest { Title = "Broken traffic light", Details = "Traffic light stuck on red at Oak & 5th", Priority = 8, Status = "In Progress" },
                new ServiceRequest { Title = "Water leak", Details = "Burst water pipe near Pine Road", Priority = 9, Status = "In Progress" },
                new ServiceRequest { Title = "Fallen tree", Details = "Tree blocking pavement on River Lane", Priority = 5, Status = "Pending" },
                new ServiceRequest { Title = "Graffiti cleanup", Details = "Graffiti on community center wall", Priority = 2, Status = "Completed" },
                new ServiceRequest { Title = "Broken bench", Details = "Park bench damaged in Riverside Park", Priority = 2, Status = "Pending" },
                new ServiceRequest { Title = "Blocked drain", Details = "Storm drain clogged after rain", Priority = 7, Status = "In Progress" },
                new ServiceRequest { Title = "Noisy transformer", Details = "Transformer humming outside 12 Elm", Priority = 4, Status = "Pending" },
                new ServiceRequest { Title = "Illegal dumping", Details = "Construction waste dumped near bridge", Priority = 6, Status = "Pending" },
                new ServiceRequest { Title = "Bus shelter damaged", Details = "Glass panel broken at bus stop 9", Priority = 3, Status = "Pending" },
                new ServiceRequest { Title = "Sidewalk crack", Details = "Trip hazard on Maple Ave", Priority = 5, Status = "Pending" },
                new ServiceRequest { Title = "Water discoloration", Details = "Brown water reported in North Hill", Priority = 7, Status = "Pending" },
                new ServiceRequest { Title = "Noise complaint", Details = "Loud construction after hours on 7th", Priority = 2, Status = "Completed" },
                new ServiceRequest { Title = "Traffic sign down", Details = "Stop sign knocked down near school", Priority = 8, Status = "Pending" },
                new ServiceRequest { Title = "Park lights flicker", Details = "Lights in Queen's Park flickering", Priority = 3, Status = "Pending" },
                new ServiceRequest { Title = "Abandoned vehicle", Details = "Car abandoned on Elm Street for two weeks", Priority = 4, Status = "Pending" },
                new ServiceRequest { Title = "Water pressure low", Details = "Low water pressure in Willow Estate", Priority = 6, Status = "In Progress" },
                new ServiceRequest { Title = "Tree roots lifting road", Details = "Roots causing bumps on Cedar Rd", Priority = 5, Status = "Pending" }
            };

            // Add all samples (Add will populate _store, _graph, heap and insert into BST)
            foreach (var s in samples)
            {
                Add(s);
            }

            // Create some graph connections to make traversals and MST meaningful
            // Connect nearby or related requests (use existing keys in _graph)
            var ids = _graph.Keys.ToList();
            if (ids.Count >= 6)
            {
                AddGraphEdge(ids[0], ids[1], 3);
                AddGraphEdge(ids[1], ids[2], 4);
                AddGraphEdge(ids[2], ids[3], 6);
                AddGraphEdge(ids[3], ids[4], 2);
                AddGraphEdge(ids[4], ids[5], 5);
                AddGraphEdge(ids[0], ids[5], 8);
            }

            // Optionally connect a few more random edges for richer graph
            var rnd = new Random();
            for (int i = 0; i < Math.Min(10, ids.Count - 1); i++)
            {
                var a = ids[rnd.Next(ids.Count)];
                var b = ids[rnd.Next(ids.Count)];
                if (a != b) AddGraphEdge(a, b, rnd.Next(1, 10));
            }
        }
    }
}



