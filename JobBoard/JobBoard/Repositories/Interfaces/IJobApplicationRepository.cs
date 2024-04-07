using JobBoard.Enums;
using JobBoard.Models.Classes;
using JobBoard.Models.Data;

namespace JobBoard.Repositories.Interfaces
{
    public interface IJobApplicationRepository
    {
        JobApplicationDataModel GetUserJobApplication(int id);
        void AddJobApplication(JobApplicationDataModel jobApplication);
        IQueryable<AppliedJobsListModel> GetUserBasedJobApplications(int userId);
    }
}
