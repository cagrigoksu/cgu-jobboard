using JobBoard.DataContext;
using JobBoard.Models.Classes;
using JobBoard.Models.Data;
using JobBoard.Repositories.Interfaces;

namespace JobBoard.Repositories
{
    public class JobPosterRepository : IJobPosterRepository
    {
        private readonly AppDbContext _db;
        public JobPosterRepository(AppDbContext db)
        {
           _db = db;
        }
        
        public IQueryable<JobPostDataModel> GetAllJobPosts()
        {
            var jobPosts = from post in _db.JobPosts where post.IsDeleted == false select post;

            return jobPosts;
        }

        public IQueryable<JobPostDataModel> GetAllJobPostsByPage(int pageNumber)
        {
            var take = Globals.MaxItemForJobList;
            var skip = (pageNumber - 1) * Globals.MaxItemForJobList;

            var jobPosts = from post in _db.JobPosts where post.IsDeleted == false select post;
          
            jobPosts = jobPosts.Skip(skip).Take(take);
          
            return jobPosts;
        }


        public IQueryable<JobPostDataModel> GetUserBasedJobPosts(int userId)
        {
            var jobPosts = from jobPost in _db.JobPosts
                where jobPost.CreatedUserId == userId
                    && jobPost.IsDeleted == false
                           select jobPost;

            return jobPosts;
        }

        public JobPostDataModel GetJobPost(int id)
        {
            var job = _db.JobPosts.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);
            return job;
        }

        public void AddJobPost(JobPostDataModel post)
        {
            post.PostDate = DateTime.Now;
            post.CreatedUserId = Globals.UserId;

            _db.Add(post);
            _db.SaveChanges();
        }

        public void UpdateJobPost(JobPostDataModel post)
        {
            var dbPost = _db.JobPosts.First(x => x.Id == post.Id);

            dbPost.Title = post.Title;
            dbPost.Description = post.Description;
            dbPost.LevelId = post.LevelId;
            dbPost.Country = post.Country;
            dbPost.City = post.City;

            _db.Update(dbPost);
            _db.SaveChanges();
        }

        public void DeleteJobPost(int id)
        {
            var post = _db.JobPosts.Find(id);
            // if (post != null)
            // {
            //     _db.Remove(post);
            // }
            post.IsDeleted = true;
            post.DeleteUser = Globals.UserId;
            post.DeleteDate = DateTime.Now;

            _db.Update(post);
            _db.SaveChanges();
        }

        public IQueryable<JobApplicantsListModel> GetJobApplicantsList(int jobId)
        {
            var applicants = from job in _db.JobPosts
                join app in _db.JobApplications on job.Id equals app.JobId
                join user in _db.UserProfiles on app.ApplicantId equals user.UserId
                where job.Id == jobId && job.IsDeleted == false && app.IsDeleted == false
                select new JobApplicantsListModel()
                {
                    ApplicantId = app.ApplicantId,
                    Name = user.Name,
                    Surname = user.Surname,
                    ApplicationDate = app.ApplicationDate,
                    ResumeURL = app.UrlResume,
                    MotivationLetterURL = app.UrlMotivationLetter,
                    Status = app.Status
                };

            return applicants;
        }
    }
}
