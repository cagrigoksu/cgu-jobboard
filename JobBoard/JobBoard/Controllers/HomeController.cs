using JobBoard.Models;
using JobBoard.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using JobBoard.DataContext;

namespace JobBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext DB;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            DB = context;
        }

        public IQueryable<IndexViewModel> BringAllJobs()
        {
            var jobPosts = from j in DB.Jobs
                select new IndexViewModel
                {
                    JobId = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    PostDate = j.PostDate
                };

            return jobPosts;
        }
        
        public IActionResult Index()
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            var jobPosts = BringAllJobs();
            return View(new IndexViewModel() { UserId = Globals.UserId, CompanyUser = Globals.CompanyUser, JobPosts = jobPosts });
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
