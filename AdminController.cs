using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HyMall_App.Data;
using HyMall_App.Models;
using System.Linq;

namespace HyMall_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard() => View();

       // public IActionResult ViewTenants() => View();

     //   public IActionResult EventApprovals() => View();

        public IActionResult Calendar() => View();

        public IActionResult Settings() => View();



        // ✅ Announcements handled here
        public IActionResult Announcements(string search = "")
        {
            var messages = _context.ContactMessages
                .Where(m => string.IsNullOrEmpty(search) || m.Subject.Contains(search))
                .OrderByDescending(m => m.SubmittedAt)
                .ToList();

            ViewBag.Search = search;
            return View(messages);
        }

        [HttpPost]
        public IActionResult DeleteAnnouncement(int id)
        {
            var message = _context.ContactMessages.Find(id);
            if (message != null)
            {
                _context.ContactMessages.Remove(message);
                _context.SaveChanges();
            }
            return RedirectToAction("Announcements");
        }


        // handles event approvals
        public IActionResult EventApprovals(string search = "", string status = "")
        {
            var events = _context.Events.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                events = events.Where(e => e.Title.Contains(search) || e.Description.Contains(search));

            if (!string.IsNullOrEmpty(status) && status != "All")
                events = events.Where(e => e.Status == status);

            ViewBag.Search = search;
            ViewBag.Status = status;

            return View(events.OrderByDescending(e => e.CreatedAt).ToList());
        }

        [HttpPost]
        public IActionResult ApproveEvent(int id)
        {
            var @event = _context.Events.Find(id);
            if (@event != null)
            {
                @event.Status = "Approved";
                _context.SaveChanges();
            }
            return RedirectToAction("EventApprovals");
        }

        [HttpPost]
        public IActionResult RejectEvent(int id)
        {
            var @event = _context.Events.Find(id);
            if (@event != null)
            {
                @event.Status = "Rejected";
                // Optional: capture reason via modal or separate page — for now, just reject
                _context.SaveChanges();
            }
            return RedirectToAction("EventApprovals");
        }


        // handles tenant profiles
        public IActionResult ViewTenants()
        {
            var profiles = _context.TenantProfiles
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
            return View(profiles);
        }

        // Optional: Detailed view (if you want "Click Here" to go somewhere)
        public IActionResult ViewProfile(int id)
        {
            var profile = _context.TenantProfiles.Find(id);
            if (profile == null) return NotFound();
            return View(profile);
        }

    }
}