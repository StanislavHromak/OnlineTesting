using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineTesting.Models;

namespace OnlineTesting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if(User.IsInRole("Dean")) return RedirectToAction("Index", "Discipline");
            if (User.IsInRole("Teacher")) return RedirectToAction("Index", "Questions");
            if (User.IsInRole("Student")) return RedirectToAction("Index", "StudentTest");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
