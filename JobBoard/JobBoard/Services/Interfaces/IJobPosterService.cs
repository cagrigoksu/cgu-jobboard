using JobBoard.Models.Classes;
using JobBoard.Models.Data;

namespace JobBoard.Services.Interfaces
{
    public interface IJobPosterService
    {
        IQueryable<JobPostDataModel> GetAllJobPosts();
        IQueryable<JobPostDataModel> GetAllJobPostsByPage(int pageNumber);
        int GetJobPostsMaxPageNumber(int listCount);
        IQueryable<JobPostDataModel> GetUserBasedJobPosts(int userId);
        JobPostDataModel GetJobPost(int id);
        void AddJobPost(JobPostDataModel post);
        void UpdateJobPost(JobPostDataModel post);
        void DeleteJobPost(int id);
        IQueryable<JobApplicantsListModel> GetJobApplicantsList(int jobId);

    }
}
