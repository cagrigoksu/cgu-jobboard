using JobBoard.DataContext;
using JobBoard.Models.Data;
using JobBoard.Models.View;
using JobBoard.Repositories.Interfaces;

namespace JobBoard.Repositories
{
    public class JobPostRepository : IJobPostRepository
    {
        private readonly AppDbContext _db;
        public JobPostRepository(AppDbContext db)
        {
           _db = db;
        }
        
        public IQueryable<JobPostDataModel> GetAllJobPosts()
        {
            var jobPosts = from post in _db.JobPosts select post;

            return jobPosts;
        }

        public IQueryable<JobPostDataModel> GetUserBasedJobPosts(int userId)
        {
            var jobPosts = from jobPost in _db.JobPosts
                where jobPost.CreatedUserId == userId
                select jobPost;

            return jobPosts;
        }

        public JobPostDataModel GetJobPost(int id)
        {
            var job = _db.JobPosts.SingleOrDefault(x => x.Id == id);
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
            if (post != null)
            {
                _db.Remove(post);
            }
            _db.SaveChanges();
        }
    }
}
