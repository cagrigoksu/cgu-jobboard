using JobBoard.DataContext;
using JobBoard.Enums;
using JobBoard.Models.Classes;
using JobBoard.Models.Data;
using JobBoard.Models.View;
using JobBoard.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Controllers
{
    public class JobApplyController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IJobPostRepository? _jobPostRepository;
        private readonly IJobApplicationRepository? _jobApplicationRepository;

        public JobApplyController(IJobPostRepository? jobPostRepository, IJobApplicationRepository? jobApplicationRepository, IWebHostEnvironment webHostEnvironment)
        {
            _jobPostRepository = jobPostRepository;
            _jobApplicationRepository = jobApplicationRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult ApplyToJob(int? id)
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            if (id == null)
            {
                return NotFound();
            }

            var jobDetails = _jobPostRepository.GetJobPost(id.Value);

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
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            if (view.CV != null && view.MotivationLetter != null)
            {
                string root = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
                string pathCV = Path.Combine(root, "Resumes/Application");
                string pathML = Path.Combine(root, "MotivationLetters/Application");

                var filenameCV = Globals.UserId.ToString() +"_"+ view.JobId.ToString() + ".pdf";
                var filenameMotivation = Globals.UserId.ToString() +"_"+ view.JobId.ToString() + ".pdf";

                using FileStream streamCV = new FileStream(Path.Combine(pathCV, filenameCV), FileMode.Create);
                using FileStream streamML = new FileStream(Path.Combine(pathML, filenameMotivation), FileMode.Create);
                view.CV.CopyTo(streamCV);
                view.MotivationLetter.CopyTo(streamML);

                // TODO: if(view.Motivation){}

                var jobApp = new JobApplicationDataModel()
                {
                    JobId = view.JobId,
                    ApplicantId = Globals.UserId,
                    UrlResume = Path.Combine(pathCV, filenameCV),
                    UrlMotivationLetter = Path.Combine(pathML, filenameMotivation)
            };

                _jobApplicationRepository.AddJobApplication(jobApp);

                var jobPosts = _jobPostRepository.GetAllJobPosts();
                return View("Index", new IndexViewModel() { UserId = Globals.UserId, CompanyUser = Globals.CompanyUser, JobPosts = jobPosts });
            }

            return NotFound();

        }

        [HttpGet]
        public IActionResult AppliedJobsPartialView(ApplicationStatusEnum? status)
        {
            var jobList = _jobApplicationRepository.GetUserBasedJobApplications(Globals.UserId);

            if (status != null)
            {
                var a = jobList.Where(x=>(int)x.Status == (int)status);
                return PartialView("AppliedJobsPartialView", new AppliedJobsPartialViewModel() { AppliedJobList = a, FilterStatus = status.Value });
            }
            else
            {
                return PartialView("AppliedJobsPartialView", new AppliedJobsPartialViewModel() { AppliedJobList = jobList, FilterStatus = null });

            }
        }

        public IActionResult AppliedJobs(AppliedJobsViewModel model)
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            var jobList = _jobApplicationRepository.GetUserBasedJobApplications(Globals.UserId);

            return View(new AppliedJobsViewModel(){AppliedJobList = jobList, });
        }

        public IActionResult WithdrawJobApplication(int jobId)
        {
            _jobApplicationRepository.WithdrawJobApplication(jobId);

            var jobPosts = _jobPostRepository.GetAllJobPosts();
            return View("Index", new IndexViewModel() { UserId = Globals.UserId, CompanyUser = Globals.CompanyUser, JobPosts = jobPosts });

        }
    }
}
