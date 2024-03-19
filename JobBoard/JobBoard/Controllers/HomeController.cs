using JobBoard.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using JobBoard.DataContexts;
using JobBoard.Services;

namespace JobBoard.Controllers
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
            return View();
        }


        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(string email, string pwd)
        {
            return Redirect("/");
        }

        public IActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogOn(IFormCollection formCollection)
        {
            UserDataModel user = new()
            {
                Email = formCollection["email"],
                Password = formCollection["pwd"],
                CompanyUser = formCollection["hiring"] == "on"
            };

            if (user.Password == formCollection["pwdConf"])
            {
                return Redirect("/");
            }
            else
            {
                return View("LogOn");
            }
            
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
