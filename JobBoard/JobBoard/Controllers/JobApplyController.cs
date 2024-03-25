﻿using System.Runtime.InteropServices.JavaScript;
using JobBoard.DataContext;
using JobBoard.Enums;
using JobBoard.Models;
using JobBoard.Models.Classes;
using JobBoard.Models.Data;
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
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            if (id == null)
            {
                return NotFound();
            }

            var jobDetails = DB.JobPosts.Find(id);
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

            var jobPosts = new HomeController(null, DB).BringAllJobs();

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
                    ApplicationDate = DateTime.Now,
                    JobId = view.JobId,
                    ApplicantId = Globals.UserId,
                    UrlResume = Path.Combine(path, filenameCV),
                    UrlMotivationLetter = ""
            };

                DB.Add(jobApp);
                DB.SaveChanges();
                
                return View("Index", new IndexViewModel() { UserId = Globals.UserId, CompanyUser = Globals.CompanyUser, JobPosts = jobPosts });
            }

            return NotFound();

        }

        [HttpGet]
        public IActionResult AppliedJobsPartialView(ApplicationStatusEnum? status)
        {
            var jobList = from post in DB.JobPosts
                join application in DB.JobApplications on post.Id equals application.JobId
                where application.ApplicantId == Globals.UserId 
                      && !application.IsDeleted && !post.IsDeleted
                      && application.Status == status
                select new AppliedJobsListModel()
                {
                    Title = post.Title,
                    ApplicationDate = application.ApplicationDate,
                    City = post.City,
                    CompanyId = post.CompanyId,
                    Id = application.Id,
                    JobId = application.JobId
                };
            // var jobList = DB.JobApplications.ToList().Where(x => x.ApplicantId == Globals.UserId
            //                                                      && !x.IsDeleted);

            // await Task.CompletedTask;
            var a = jobList.Count();
            var b = a;
            return PartialView("AppliedJobsPartialView",new AppliedJobsPartialViewModel() { AppliedJobList = jobList });
        }

        public IActionResult AppliedJobs(AppliedJobsViewModel model)
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            var jobList =
                from j in DB.JobPosts
                join i in DB.JobApplications on j.Id equals i.JobId 
                where i.ApplicantId == Globals.UserId && !i.IsDeleted && !j.IsDeleted
                select new AppliedJobsListModel()
                {
                    Id = i.Id,
                    JobId = i.JobId,
                    Title = j.Title,
                    ApplicationDate = i.ApplicationDate,
                    City = j.City,
                    CompanyId = j.CompanyId,
                    Status = i.Status
                };

            return View(new AppliedJobsViewModel(){AppliedJobList = jobList});
        }
    }
}
