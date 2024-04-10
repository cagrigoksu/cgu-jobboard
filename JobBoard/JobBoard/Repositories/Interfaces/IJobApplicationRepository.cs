using JobBoard.Models.Classes;
using JobBoard.Models.Data;

namespace JobBoard.Repositories.Interfaces
{
    public interface IJobApplicationRepository
    {
        JobApplicationDataModel GetUserJobApplication(int userId, int jobId);
        void AddJobApplication(JobApplicationDataModel jobApplication);
        IQueryable<AppliedJobsListModel> GetUserBasedJobApplications(int userId);
        void WithdrawJobApplication(int jobId);

        void EditJobApplication(JobApplicationDataModel model);
    }
}
