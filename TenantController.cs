using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HyMall_App.Models;
using Microsoft.AspNetCore.Identity;

namespace HyMall_App.Controllers
{
    [Authorize(Roles = "Tenant")]
    public class TenantController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public TenantController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.UserName = user?.Name ?? "Tenant";
            return View();
        }
    }
}
