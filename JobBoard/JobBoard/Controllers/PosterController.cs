using System.Collections.Immutable;
using JobBoard.DataContext;
using Microsoft.AspNetCore.Mvc;
using JobBoard.Models.Classes;
using JobBoard.Models.Data;
using JobBoard.Models.View;
using JobBoard.Repositories.Interfaces;
using System.Diagnostics.Metrics;
using JobBoard.Enums;
using JobBoard.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace JobBoard.Controllers
{
    public class PosterController:Controller
    {
        private readonly IJobPosterRepository _jobPosterRepository;
        private readonly IJobPosterService _jobPosterService;

        private readonly IJobApplicationService _jobApplicationService;

        private readonly IUserService _userService;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public PosterController(IJobPosterRepository jobPosterRepository, IJobApplicationService jobApplicationService, IWebHostEnvironment webHostEnvironment, IJobPosterService jobPosterService, IUserService userService)
        {
            _jobPosterRepository = jobPosterRepository;
            _jobApplicationService = jobApplicationService;
            _webHostEnvironment = webHostEnvironment;
            _jobPosterService = jobPosterService;
            _userService = userService;
        }


        public IActionResult  Dashboard()
        {
             if (new SessionUtils().EmptySession())
             {
                 return View("LogIn");
             }

             var jobPosts = _jobPosterRepository.GetUserBasedJobPosts(Globals.UserId);

             return View(new JobPostViewModel(){jobs = jobPosts});
        }

        public IActionResult AddJobPost()
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            return View(new JobPostViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddJobPost(JobPostDataModel post)
        {
            _jobPosterRepository.AddJobPost(post);

            var jobPosts = _jobPosterRepository.GetUserBasedJobPosts(Globals.UserId);

            return View("Dashboard",new JobPostViewModel() { jobs = jobPosts });
        }


        public IActionResult DetailJobPost(int id, bool detail)
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            if (id == null)
            {
                return NotFound();
            }

            var job = _jobPosterRepository.GetJobPost(id);

            var result = new JobPostViewModel()
            {
                Detail = detail,
                Id = job.Id,
                Title = job.Title,
                LevelId = job.LevelId,
                Country = job.Country,
                City = job.City,
                Description = job.Description
            };
            return View("AddJobPost",result);
        }
        
        public IActionResult EditJobPost(int id, bool edit)
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            var job = _jobPosterRepository.GetJobPost(id);
            var result = new JobPostViewModel()
                {
                    Edit = edit,
                    Id = job.Id,
                    Title = job.Title,
                    LevelId = job.LevelId,
                    Country = job.Country,
                    City = job.City,
                    Description = job.Description
                };
            return View("AddJobPost", result);
        }

        [HttpPost]
        public IActionResult EditJobPost(JobPostViewModel post)
        {
            _jobPosterRepository.UpdateJobPost(new JobPostDataModel()
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                LevelId = post.LevelId,
                Country = post.Country,
                City = post.City,
              });
            
            var jobPosts = _jobPosterRepository.GetUserBasedJobPosts(Globals.UserId);

            return View("Dashboard", new JobPostViewModel() { jobs = jobPosts });
        }

        [HttpPost]
        public IActionResult DeleteJobPost(int id)
        {
            _jobPosterRepository.DeleteJobPost(id);

            var jobPosts = _jobPosterRepository.GetUserBasedJobPosts(Globals.UserId);

            return View("Dashboard", new JobPostViewModel(){jobs = jobPosts} );

        }

        public IActionResult DeleteConfirmation(int deleteId)
        {
            return PartialView("DeleteConfirmationPartialView", new JobPostViewModel(){Id = deleteId});
        }

        [HttpGet]   // Get applicant list for a job post
        public IActionResult JobApplicants(int jobId)
        {

            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            var applicantList = _jobPosterService.GetJobApplicantsList(jobId);
            var job = _jobPosterRepository.GetJobPost(jobId);
            return View(new JobApplicantsViewModel(){Applicants = applicantList, JobId = jobId, JobTitle = job.Title});
        }

        [HttpGet]   // Get applicant detail for a job post
        public IActionResult ApplicantDetail(int applicantId, int jobId)
        {
            var job = _jobPosterService.GetJobPost(jobId);
            var application = _jobApplicationService.GetUserJobApplication(applicantId, jobId);
            var applicantProfile = _userService.GetUserProfile(application.ApplicantId);


            ApplicantDetailViewModel view = new ApplicantDetailViewModel();
            
            view.JobId = jobId;
            view.ApplicantId = applicantId;
            view.Name = applicantProfile.Name;
            view.Surname = applicantProfile.Surname;
            view.Email = applicantProfile.Email;
            view.PhoneNumber = applicantProfile.PhoneNumber;
            view.UrlResume = application.UrlResume;
            view.UrlMotivationLetter = application.UrlMotivationLetter;
            view.ApplicationStatus = application.Status;
            view.JobTitle = job.Title;
            view.ApplicationDate = Convert.ToDateTime(application.ApplicationDate.ToShortDateString());

            return View(view);
        }

        public IActionResult DownloadResumePdf(int jobId, int applicantId)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string outputFilePath = Path.Combine(webRootPath, "Uploads/Resumes/Application/", applicantId.ToString()+"_"+jobId.ToString()+".pdf");

            if (!System.IO.File.Exists(outputFilePath))
            {
                // Return a 404 Not Found error if the file does not exist
                return NotFound();
            }

            var fileInfo = new System.IO.FileInfo(outputFilePath);
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "attachment;filename=\"" + fileInfo.Name + "\"");
            Response.Headers.Add("Content-Length", fileInfo.Length.ToString());

            // Send the file to the client
            return File(System.IO.File.ReadAllBytes(outputFilePath), "application/pdf", fileInfo.Name);
        }
        
        public IActionResult DownloadMotivationLetterPdf(int jobId, int applicantId)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string outputFilePath = Path.Combine(webRootPath, "Uploads/MotivationLetters/Application/", applicantId.ToString()+"_"+jobId.ToString()+".pdf");

            if (!System.IO.File.Exists(outputFilePath))
            {
                // Return a 404 Not Found error if the file does not exist
                return NotFound();
            }

            var fileInfo = new System.IO.FileInfo(outputFilePath);
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "attachment;filename=\"" + fileInfo.Name + "\"");
            Response.Headers.Add("Content-Length", fileInfo.Length.ToString());

            // Send the file to the client
            return File(System.IO.File.ReadAllBytes(outputFilePath), "application/pdf", fileInfo.Name);
        }

        [HttpPost]
        public int ChangeApplicantStatus(ApplicantDetailViewModel view)
        {
            _jobApplicationService.EditJobApplication(new JobApplicationDataModel()
            {
                ApplicantId = view.ApplicantId,
                JobId = view.JobId,
                Status = (ApplicationStatusEnum)view.ApplicationStatusInt
            });
            
            ModelState.Clear();
            ViewBag.Message = "Successfully saved.";
            return 200;
        }

    }
}
