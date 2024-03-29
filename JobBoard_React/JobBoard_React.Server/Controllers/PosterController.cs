using JobBoard_React.Server;
using JobBoard_React.Server.Context;
using JobBoard_React.Server.Entities;
using JobBoard_React.Server.Models.Classes;
using JobBoard_React.Server.Models.View;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard_React.Server.Controllers
{
    public class PosterController:Controller
    {
        private readonly AppDbContext DB;

        public PosterController(AppDbContext context)
        {
            DB = context;
        }

        public IActionResult  Dashboard()
        {
             if (new SessionUtils().EmptySession())
             {
                 return View("LogIn");
             }

             var jobPosts = from j in DB.JobPosts
                 where j.CreatedUserId == Globals.UserId
                 select new JobPostViewModel
                 {
                     Id = j.Id,
                     Title = j.Title,
                     Description = j.Description,
                     PostDate = j.PostDate
                 };

            return View(new JobPostViewModel(){jobs = jobPosts});
        }

        public IActionResult AddJobPost()
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            return View(new JobPostViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddJobPost(JobPostDataModel post)
        {
            post.PostDate = DateTime.Now;
            post.CreatedUserId = Globals.UserId;

            DB.Add(post);
            DB.SaveChanges();

            var jobPosts = from j in DB.JobPosts
                where j.CreatedUserId == Globals.UserId
                select new JobPostViewModel
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    PostDate = j.PostDate
                };

            return View("Dashboard",new JobPostViewModel() { jobs = jobPosts });
        }


        public IActionResult DetailJobPost(int id, bool detail)
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            if (id == null)
            {
                return NotFound();
            }

            var job = DB.JobPosts.SingleOrDefault(x => x.Id == id);
            var result = new JobPostViewModel()
            {
                Detail = detail,
                Id = job.Id,
                Title = job.Title,
                LevelId = job.LevelId,
                Country = job.Country,
                City = job.City,
                Description = job.Description
            };
            return View("AddJobPost",result);
        }
        
        public IActionResult EditJobPost(int id, bool edit)
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            var job = DB.JobPosts.SingleOrDefault(x => x.Id == id);
            var result = new JobPostViewModel()
                {
                    Edit = edit,
                    Id = job.Id,
                    Title = job.Title,
                    LevelId = job.LevelId,
                    Country = job.Country,
                    City = job.City,
                    Description = job.Description
                };
            return View("AddJobPost", result);
        }

        [HttpPost]
        public IActionResult EditJobPost(JobPostViewModel post)
        {
            var dbPost = DB.JobPosts.First(x => x.Id == post.Id);

            dbPost.Title = post.Title;
            dbPost.Description = post.Description;
            dbPost.LevelId = post.LevelId;
            dbPost.Country = post.Country;
            dbPost.City = post.City;

            DB.Update(dbPost);
            DB.SaveChanges();

            var jobPosts = from j in DB.JobPosts
                where j.CreatedUserId == Globals.UserId
                select new JobPostViewModel
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    PostDate = j.PostDate
                };

            return View("Dashboard", new JobPostViewModel() { jobs = jobPosts });
        }

        [HttpPost]
        public IActionResult DeleteJobPost(int id)
        {
            var post = DB.JobPosts.Find(id);
            if (post != null)
            {
                DB.JobPosts.Remove(post);
            }
            DB.SaveChanges();

            var jobPosts = from j in DB.JobPosts
                                                    where j.CreatedUserId == Globals.UserId
                                                    select new JobPostViewModel { 
                                                        Id = j.Id, 
                                                        Title = j.Title, 
                                                        Description = j.Description, 
                                                        PostDate = j.PostDate};

            return View("Dashboard", new JobPostViewModel(){jobs = jobPosts} );

        }

        public IActionResult DeleteConfirmation(int deleteId)
        {
            return PartialView("DeleteConfirmationPartialView", new JobPostViewModel(){Id = deleteId});
        }
    }
}
