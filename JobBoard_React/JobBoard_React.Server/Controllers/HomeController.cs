using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using JobBoard_React.Server.Context;
using JobBoard_React.Server.Models;
using JobBoard_React.Server.Models.Classes;
using JobBoard_React.Server.Models.View;

namespace JobBoard_React.Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly JobboardContext DB;

        public HomeController(ILogger<HomeController> logger, JobboardContext context)
        {
            _logger = logger;
            DB = context;
        }

        public IQueryable<IndexViewModel> BringAllJobs()
        {
            var jobPosts = from j in DB.JobPosts
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

        public IActionResult JobPostDetail(int jobId)
        {
            var job = DB.JobPosts.SingleOrDefault(x => x.Id == jobId);
            var isApplied = DB.JobApplications.SingleOrDefault(x=>x.ApplicantId== Globals.UserId);

            var result = new JobApplyViewModel()
            {
                JobId = job.Id,
                Title = job.Title,
                LevelId = job.LevelId,
                Country = job.Country,
                City = job.City,
                Description = job.Description,
                
            };

            if (isApplied != null)
            {
                result.isApplied = true;
                result.Status = isApplied.Status;
            }
            return PartialView("JobDetailPartialView", result);
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
