using JobBoard.Models.Classes;
using JobBoard.Models.Data;

namespace JobBoard.Services.Interfaces
{
    public interface IJobApplicationService
    {
        JobApplicationDataModel GetUserJobApplication(int userId, int jobId);
        void AddJobApplication(JobApplicationDataModel jobApplication);
        IQueryable<AppliedJobsListModel> GetUserBasedJobApplications(int userId);
        void WithdrawJobApplication(int jobId);
        IQueryable<JobApplicantsListModel> GetJobApplicantsList(int jobId);


    }
}
