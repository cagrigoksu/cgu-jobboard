using JobBoard.Models.Classes;
using JobBoard.Models.Data;
using JobBoard.Repositories.Interfaces;
using JobBoard.Services.Interfaces;

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
            var data = _jobApplicationRepository.GetUserJobApplication(userId, jobId);
            return data;
        }

        public void AddJobApplication(JobApplicationDataModel jobApplication)
        {
            _jobApplicationRepository.AddJobApplication(jobApplication);
        }

        public IQueryable<AppliedJobsListModel> GetUserBasedJobApplications(int userId)
        {
            var data = _jobApplicationRepository.GetUserBasedJobApplications(userId);
            return data;
        }

        public void WithdrawJobApplication(int jobId)
        {
            _jobApplicationRepository.WithdrawJobApplication(jobId);
        }

        public void EditJobApplication(JobApplicationDataModel model)
        {
            _jobApplicationRepository.EditJobApplication(model);
        }
    }
}
