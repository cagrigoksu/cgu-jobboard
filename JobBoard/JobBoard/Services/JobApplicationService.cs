using JobBoard.Models.Classes;
using JobBoard.Models.Data;
using JobBoard.Repositories.Interfaces;
using JobBoard.Services.Interfaces;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JobBoard.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;

        public JobApplicationService(IJobApplicationRepository jobApplicationRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
        }


        public JobApplicationDataModel GetUserJobApplication(int userId, int jobId)
        {
            throw new NotImplementedException();
        }

        public void AddJobApplication(JobApplicationDataModel jobApplication)
        {
            throw new NotImplementedException();
        }

        public IQueryable<AppliedJobsListModel> GetUserBasedJobApplications(int userId)
        {
            throw new NotImplementedException();
        }

        public void WithdrawJobApplication(int jobId)
        {
            _jobApplicationRepository.WithdrawJobApplication(jobId);
        }
    }
}
