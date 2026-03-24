using Microsoft.AspNetCore.Mvc;
using MunicipalityApp.Models;
using MunicipalityApp.Services;
using System.Linq;

namespace MunicipalityApp.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly IServiceRequestService _service;

        public ServiceRequestController(IServiceRequestService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var all = _service.GetAll().ToList();
            var bst = _service.InOrderBST();
            var mst = _service.MinimumSpanningTree();

            ViewBag.BST = bst;
            ViewBag.MST = mst;
            return View("~/Views/ServiceRequest/Index.cshtml", all);
        }

        [HttpPost]
        public IActionResult SubmitRequest(string title, string details, int priority)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(details))
                return RedirectToAction("Index");

            var request = new ServiceRequest
            {
                Title = title,
                Details = details,
                Priority = priority,
                CreatedAt = DateTime.Now,
                Status = "Pending"
            };

            _service.Add(request);
            _service.InsertIntoBST(request);

            TempData["Message"] = "Service request added successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            var req = _service.GetById(id);
            if (req == null)
                return NotFound();

            req.Status = status;
            TempData["Message"] = $"Status for request '{req.Title}' updated to '{status}'";
            return RedirectToAction("Index");
        }
    }
}




