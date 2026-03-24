using HyMall_App.Data;
using HyMall_App.Models;
using Microsoft.AspNetCore.Mvc;

namespace HyMall_App.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(new ContactForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ContactForm form)
        {
            if (string.IsNullOrWhiteSpace(form.FirstName) ||
                string.IsNullOrWhiteSpace(form.LastName) ||
                string.IsNullOrWhiteSpace(form.Email) ||
                string.IsNullOrWhiteSpace(form.Subject) ||
                string.IsNullOrWhiteSpace(form.Message))
            {
                ModelState.AddModelError("", "Please fill in all required fields.");
                return View(form);
            }

            if (!IsValidEmail(form.Email))
            {
                ModelState.AddModelError("Email", "Please enter a valid email.");
                return View(form);
            }

            // Save to SQLite
            var message = new ContactMessage
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email,
                Phone = form.Phone,
                Subject = form.Subject,
                Message = form.Message
            };

            _context.ContactMessages.Add(message);
            _context.SaveChanges();

            TempData["Success"] = "Thank you for contacting us! We'll review your message shortly.";
            return RedirectToAction("Index");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}