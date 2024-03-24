using JobBoard.DataContext;
using JobBoard.Models;
using JobBoard.Models.Classes;
using JobBoard.Models.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using NuGet.Protocol;

namespace JobBoard.Controllers
{
    public class JobApplyController : Controller
    {
        private readonly AppDbContext DB;
        private readonly IHostEnvironment env;

        public JobApplyController(AppDbContext context, IHostEnvironment? env)
        {
            DB = context;
            this.env = env;
        }
        public IActionResult ApplyToJob(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobDetails = DB.Jobs.Find(id);
            var view = new JobApplyViewModel
            {
                JobId = id.Value,
                Country = jobDetails.Country,
                City = jobDetails.City,
                Title = jobDetails.Title,
                Description = jobDetails.Description
                
            };


            return View(view);
        }

        [HttpPost]
        public IActionResult Apply(JobApplyViewModel view)
        {
            var jobPosts = new HomeController(null, DB).BringAllJobs();
            if (view.CV != null)
            { 
                string contentPath = env.ContentRootPath;

                string path1 = Path.Combine(env.ContentRootPath, "Uploads");
                string path = Path.Combine(path1, "Resumes");
                var uniqueFileName = Globals.UserId.ToString()+".pdf";
                using FileStream stream = new FileStream(Path.Combine(path, uniqueFileName), FileMode.Create);
                view.CV.CopyTo(stream);

                return View("Index", new IndexViewModel() { UserId = Globals.UserId, CompanyUser = Globals.CompanyUser, JobPosts = jobPosts });
            }

            return NotFound();

        }
    }
}
