using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MunicipalityApp.Models;
using Microsoft.AspNetCore.Mvc;
using MunicipalityApp.Services;
using Microsoft.AspNetCore.Hosting;


namespace MunicipalityApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

