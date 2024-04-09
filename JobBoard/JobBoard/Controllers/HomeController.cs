using JobBoard.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using JobBoard.Models.View;
using JobBoard.Repositories.Interfaces;

namespace JobBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJobPosterRepository? _jobPostRepository;
        private readonly IJobApplicationRepository? _jobApplicationRepository;
        private readonly IDBUtilsRepository? _dbUtilsRepository;

        public HomeController(IJobPosterRepository? jobPostRepository, IJobApplicationRepository? jobApplicationRepository, IDBUtilsRepository? dbUtilsRepository)
        {
            _jobPostRepository = jobPostRepository;
            _jobApplicationRepository = jobApplicationRepository;
            _dbUtilsRepository = dbUtilsRepository;
        }

        public IActionResult Index()
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            var jobPosts = _jobPostRepository.GetAllJobPosts();

            return View(new IndexViewModel() { UserId = Globals.UserId, CompanyUser = Globals.CompanyUser, JobPosts = jobPosts });
        }

        public IActionResult JobPostDetail(int jobId, bool companyUser)
        {
            var job = _jobPostRepository.GetJobPost(jobId);
            var isApplied = _jobApplicationRepository.GetUserJobApplication(Globals.UserId, jobId);

            var result = new JobApplyViewModel()
            {
                JobId = job.Id,
                Title = job.Title,
                LevelId = job.LevelId,
                Country = job.Country,
                City = job.City,
                Description = job.Description,
                CompanyUser = companyUser
            };

            if (isApplied != null)
            {
                result.isApplied = true;
                result.Status = isApplied.Status;
            }
            return PartialView("JobDetailPartialView", result);
        }

        public IActionResult Status()
        {
            return View();
        }

        [HttpGet]
        public bool DbStatusCheck()
        {
            return _dbUtilsRepository.DBConnectionCheck();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
