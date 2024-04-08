using System.Collections.Immutable;
using JobBoard.DataContext;
using Microsoft.AspNetCore.Mvc;
using JobBoard.Models.Classes;
using JobBoard.Models.Data;
using JobBoard.Models.View;
using JobBoard.Repositories.Interfaces;
using System.Diagnostics.Metrics;
using JobBoard.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace JobBoard.Controllers
{
    public class PosterController:Controller
    {
        private readonly IJobPostRepository _jobPostRepository;
        private readonly IJobApplicationService _jobApplicationService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PosterController(IJobPostRepository jobPostRepository, IJobApplicationService jobApplicationService, IWebHostEnvironment webHostEnvironment)
        {
            _jobPostRepository = jobPostRepository;
            _jobApplicationService = jobApplicationService;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult  Dashboard()
        {
             if (new SessionUtils().EmptySession())
             {
                 return View("LogIn");
             }

             var jobPosts = _jobPostRepository.GetUserBasedJobPosts(Globals.UserId);

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
            _jobPostRepository.AddJobPost(post);

            

            var jobPosts = _jobPostRepository.GetUserBasedJobPosts(Globals.UserId);

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

            var job = _jobPostRepository.GetJobPost(id);

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

            var job = _jobPostRepository.GetJobPost(id);
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
            _jobPostRepository.UpdateJobPost(new JobPostDataModel()
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                LevelId = post.LevelId,
                Country = post.Country,
                City = post.City,
              });
            
            var jobPosts = _jobPostRepository.GetUserBasedJobPosts(Globals.UserId);

            return View("Dashboard", new JobPostViewModel() { jobs = jobPosts });
        }

        [HttpPost]
        public IActionResult DeleteJobPost(int id)
        {
            _jobPostRepository.DeleteJobPost(id);

            var jobPosts = _jobPostRepository.GetUserBasedJobPosts(Globals.UserId);

            return View("Dashboard", new JobPostViewModel(){jobs = jobPosts} );

        }

        public IActionResult DeleteConfirmation(int deleteId)
        {
            return PartialView("DeleteConfirmationPartialView", new JobPostViewModel(){Id = deleteId});
        }

        [HttpGet]
        public IActionResult JobApplicants(int jobId)
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            var applicant_list = _jobApplicationService.GetJobApplicantsList(jobId);
            return View(new JobApplicantsViewModel(){Applicants = applicant_list, JobId = jobId});
        }

        public IActionResult DownloadResumePdf(int jobId, int applicantId)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string outputFilePath = Path.Combine(webRootPath, "Uploads/Resumes/", applicantId.ToString()+"_"+jobId.ToString()+".pdf");

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
            string outputFilePath = Path.Combine(webRootPath, "Uploads/MotivationLetters/", applicantId.ToString()+"_"+jobId.ToString()+".pdf");

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

    }
}
