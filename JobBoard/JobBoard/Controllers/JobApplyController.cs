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
        private readonly IHostEnvironment env;
        private readonly IJobPostRepository? _jobPostRepository;
        private readonly IJobApplicationRepository? _jobApplicationRepository;

        public JobApplyController(IHostEnvironment env, IJobPostRepository? jobPostRepository, IJobApplicationRepository? jobApplicationRepository)
        {
            this.env = env;
            _jobPostRepository = jobPostRepository;
            _jobApplicationRepository = jobApplicationRepository;
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

            string filenameMotivation = "";

            if (view.CV != null)
            {
                string root = Path.Combine(env.ContentRootPath, "Uploads");
                string path = Path.Combine(root, "Resumes");
                var filenameCV = Globals.UserId.ToString() +"_"+ view.JobId.ToString() + ".pdf";

                using FileStream stream = new FileStream(Path.Combine(path, filenameCV), FileMode.Create);
                view.CV.CopyTo(stream);

                // TODO: if(view.Motivation){}

                var jobApp = new JobApplicationDataModel()
                {
                    JobId = view.JobId,
                    ApplicantId = Globals.UserId,
                    UrlResume = Path.Combine(path, filenameCV),
                    UrlMotivationLetter = ""
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
    }
}
