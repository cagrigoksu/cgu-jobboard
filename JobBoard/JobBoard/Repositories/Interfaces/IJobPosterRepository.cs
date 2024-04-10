using JobBoard.Models.Classes;
using JobBoard.Models.Data;

namespace JobBoard.Repositories.Interfaces
{
    public interface IJobPosterRepository
    {
        IQueryable<JobPostDataModel> GetAllJobPosts();
        IQueryable<JobPostDataModel> GetAllJobPostsByPage(int pageNumber);
        IQueryable<JobPostDataModel> GetUserBasedJobPosts(int userId);
        JobPostDataModel GetJobPost(int id);
        void AddJobPost(JobPostDataModel post);
        void UpdateJobPost(JobPostDataModel post);
        void DeleteJobPost(int id);
        IQueryable<JobApplicantsListModel> GetJobApplicantsList(int jobId);

    }
}
