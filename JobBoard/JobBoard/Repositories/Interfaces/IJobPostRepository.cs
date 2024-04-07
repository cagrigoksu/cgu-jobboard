using JobBoard.Models.Data;

namespace JobBoard.Repositories.Interfaces
{
    public interface IJobPostRepository
    {
        IQueryable<JobPostDataModel> GetAllJobPosts();
        IQueryable<JobPostDataModel> GetUserBasedJobPosts(int userId);
        JobPostDataModel GetJobPost(int id);
        void AddJobPost(JobPostDataModel post);
        void UpdateJobPost(JobPostDataModel post);
        void DeleteJobPost(int id);
    }
}
