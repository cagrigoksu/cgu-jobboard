using JobBoard.Models.Classes;
using JobBoard.Models.Data;
using JobBoard.Repositories.Interfaces;
using JobBoard.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
