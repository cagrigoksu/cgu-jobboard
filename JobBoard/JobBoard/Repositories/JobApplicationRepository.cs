using JobBoard.DataContext;
using JobBoard.Models.Data;
using JobBoard.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IO;
using JobBoard.Enums;
using JobBoard.Models.Classes;

namespace JobBoard.Repositories
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly AppDbContext _db;
        public JobApplicationRepository(AppDbContext db)
        {
            _db = db;
        }


        public JobApplicationDataModel GetUserJobApplication(int userId)
        {
            var application = _db.JobApplications.SingleOrDefault(x => x.ApplicantId == userId);
            return application;
        }

        public void AddJobApplication(JobApplicationDataModel jobApplication)
        {
            jobApplication.ApplicationDate = DateTime.Now;
                
            _db.Add(jobApplication);
            _db.SaveChanges();
        }

        public IQueryable<AppliedJobsListModel> GetUserBasedJobApplications(int userId)
        {
            var jobList =
                from post in _db.JobPosts
                join application in _db.JobApplications on post.Id equals application.JobId
                where application.ApplicantId == userId
                      && !application.IsDeleted
                      && !post.IsDeleted
                select new AppliedJobsListModel()
                {
                    Title = post.Title,
                    ApplicationDate = application.ApplicationDate,
                    City = post.City,
                    CompanyId = post.CompanyId,
                    Id = application.Id,
                    JobId = application.JobId,
                    Status = application.Status
                };

            return jobList;
        }
    }
}
