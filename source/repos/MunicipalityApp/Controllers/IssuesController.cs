using Microsoft.AspNetCore.Mvc;
using MunicipalityApp.Models;
using MunicipalityApp.Services;

namespace MunicipalityApp.Controllers
{
    public class IssuesController : Controller
    {
        private readonly IIssueService _issueService;
        private readonly IWebHostEnvironment _env;
        public IssuesController(IIssueService issueService, IWebHostEnvironment env)
        {
            _issueService = issueService;
            _env = env;
        }

        [HttpGet]
        public IActionResult Report()
        {
            return View(new ReportIssueViewModel());
        }

        [HttpPost("/Issues/Report")]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> Report(ReportIssueViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var uploads = new List<string>();
            if (vm.Attachments != null && vm.Attachments.Any())
            {
                var uploadRoot = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadRoot)) Directory.CreateDirectory(uploadRoot);

                foreach (var f in vm.Attachments)
                {
                    if (f.Length > 0)
                    {
                        var name = Path.GetRandomFileName() + Path.GetExtension(f.FileName);
                        var path = Path.Combine(uploadRoot, name);
                        using var fs = new FileStream(path, FileMode.Create);
                        await f.CopyToAsync(fs);
                        uploads.Add("/uploads/" + name);
                    }
                }
            }

            var issue = new Issue
            {
                Location = vm.Location,
                Category = vm.SelectedCategory,
                Description = vm.Description,
                Attachments = uploads,
                CreatedAt = DateTime.UtcNow,
                Status = "Submitted"
            };

            var added = _issueService.Add(issue);

            return RedirectToAction(nameof(Success), new { id = added.Id });
        }

        [HttpGet]
        public IActionResult Success(int id)
        {
            var issue = _issueService.GetById(id);
            if (issue == null) return RedirectToAction(nameof(Report));
            return View(issue);
        }

        [HttpGet]
        public IActionResult AllReports()
        {
            var list = _issueService.GetAll();
            return View(list);
        }

        public IActionResult Details(int id)
        {
            var issue = _issueService.GetById(id);
            if (issue == null) return NotFound();
            return View(issue);
        }
    }
}
