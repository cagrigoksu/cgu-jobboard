using JobBoard.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using JobBoard.Models.View;
using JobBoard.Repositories.Interfaces;
using JobBoard.Services.Interfaces;

namespace JobBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJobPosterService? _jobPosterService;
        private readonly IJobApplicationService? _jobApplicationService;
        private readonly IDBUtilsRepository? _dbUtilsRepository;

        public HomeController(IJobPosterService? jobPosterService, IJobApplicationService? jobApplicationService, IDBUtilsRepository? dbUtilsRepository)
        {
            _jobPosterService = jobPosterService;
            _jobApplicationService = jobApplicationService;
            _dbUtilsRepository = dbUtilsRepository;
        }


        public IActionResult Index(int pageNumber = 1)
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            var pagedJobPosts = _jobPosterService.GetAllJobPostsByPage(pageNumber);
            //var maxPageNumber = _jobPosterService.GetJobPostsMaxPageNumber(jobPosts.Count());

            return View(new IndexViewModel() { UserId = Globals.UserId, CompanyUser = Globals.CompanyUser, JobPosts = pagedJobPosts, PageNumber = pageNumber});
        }

        public IActionResult JobPostDetail(int jobId, bool companyUser)
        {
            var job = _jobPosterService.GetJobPost(jobId);
            var isApplied = _jobApplicationService.GetUserJobApplication(Globals.UserId, jobId);

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
