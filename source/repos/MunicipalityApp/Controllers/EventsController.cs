using Microsoft.AspNetCore.Mvc;
using MunicipalityApp.Models;

namespace MunicipalityApp.Controllers
{
    public class EventsController : Controller
    {
        private static readonly List<EventItem> _events = new()
        {
            new EventItem { Id = 1, Title = "Community Market", Category = "Market", Date = DateTime.Now.AddDays(5), Description = "Shop local produce, handmade crafts, and baked goods." },
            new EventItem { Id = 2, Title = "Farmers' Market", Category = "Market", Date = DateTime.Now.AddDays(2), Description = "Fresh produce from local farmers and artisans." },
            new EventItem { Id = 3, Title = "Youth Coding Workshop", Category = "Education", Date = DateTime.Now.AddDays(7), Description = "Learn programming basics and create fun projects." },
            new EventItem { Id = 4, Title = "Local Art Exhibition", Category = "Art", Date = DateTime.Now.AddDays(10), Description = "Showcasing local art talent with live performances." },
            new EventItem { Id = 5, Title = "Music in the Park", Category = "Music", Date = DateTime.Now.AddDays(12), Description = "Enjoy live performances in the park." },
            new EventItem { Id = 6, Title = "Charity Run", Category = "Sports", Date = DateTime.Now.AddDays(3), Description = "Join the community fun run for a good cause." },
            new EventItem { Id = 7, Title = "Pet Adoption Day", Category = "Community", Date = DateTime.Now.AddDays(8), Description = "Find your new furry friend from local shelters." },
            new EventItem { Id = 8, Title = "Art in the Park", Category = "Art", Date = DateTime.Now.AddDays(1), Description = "Local artists showcase their work outdoors with live painting." },
            new EventItem { Id = 9, Title = "Craft Workshop", Category = "Art", Date = DateTime.Now.AddDays(6), Description = "Learn pottery, painting, and handmade crafts with local artists." },
            new EventItem { Id = 10, Title = "Street Vendor Day", Category = "Market", Date = DateTime.Now.AddDays(9), Description = "Enjoy unique stalls, art, and music in the town square." },
            new EventItem { Id = 11, Title = "Local Food Fair", Category = "Market", Date = DateTime.Now.AddDays(11), Description = "Sample delicious foods from local vendors." }
        };

        public IActionResult Index(string search)
        {
            var filtered = string.IsNullOrEmpty(search)
                ? _events
                : _events.Where(e =>
                    e.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    e.Category.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            // Determine the dominant category of search results
            var mainCategory = filtered
                .GroupBy(e => e.Category)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key;

            // Generate recommendations that are relevant but not already shown
            var recommendations = _events
                .Where(e => e.Category == mainCategory && !filtered.Contains(e))
                .OrderBy(e => e.Date)
                .Take(3)
                .ToList();

            ViewBag.Search = search;

            var model = new EventSearchModel
            {
                Query = search,
                Results = filtered,
                Recommendations = recommendations
            };

            return View(model);
        }
    }
}