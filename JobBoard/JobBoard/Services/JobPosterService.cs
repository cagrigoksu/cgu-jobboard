using JobBoard.Models.Classes;
using JobBoard.Models.Data;
using JobBoard.Repositories.Interfaces;
using JobBoard.Services.Interfaces;

namespace JobBoard.Services
{
    public class JobPosterService : IJobPosterService
    {
        private readonly IJobPosterRepository _jobPosterRepository;
        public JobPosterService(IJobPosterRepository jobPosterRepository)
        {
            _jobPosterRepository = jobPosterRepository;
        }

        public IQueryable<JobPostDataModel> GetAllJobPosts()
        {
            var data = _jobPosterRepository.GetAllJobPosts();
            return data;
        }

        public IQueryable<JobPostDataModel> GetAllJobPostsByPage(int pageNumber)
        {
            var data = _jobPosterRepository.GetAllJobPostsByPage(pageNumber);
            return data;
        }

        public int GetJobPostsMaxPageNumber(int listCount)
        {
            var _temp = listCount % Globals.MaxItemForJobList;
            return _temp == 0 ? listCount / Globals.MaxItemForJobList : (listCount / Globals.MaxItemForJobList) + 1;
        }

        public IQueryable<JobPostDataModel> GetUserBasedJobPosts(int userId)
        {
            var data = _jobPosterRepository.GetUserBasedJobPosts(userId);
            return data;
        }

        public JobPostDataModel GetJobPost(int id)
        {
            var data = _jobPosterRepository.GetJobPost(id);
            return data;
        }

        public void AddJobPost(JobPostDataModel post)
        {
            _jobPosterRepository.AddJobPost(post);
        }

        public void UpdateJobPost(JobPostDataModel post)
        {
            _jobPosterRepository.UpdateJobPost(post);
        }

        public void DeleteJobPost(int id)
        {
            _jobPosterRepository.DeleteJobPost(id);
        }

        public IQueryable<JobApplicantsListModel> GetJobApplicantsList(int jobId)
        {
            var applicant_list = _jobPosterRepository.GetJobApplicantsList(jobId);
            return applicant_list;
        }
    }
}
