using System.Collections;
using JobBoard.DataContext;
using JobBoard.Enums;
using Microsoft.AspNetCore.Mvc;
using JobBoard.Models.Classes;
using JobBoard.Models.Data;
using JobBoard.Models.View;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace JobBoard.Controllers
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

             var jobPosts = from j in DB.Jobs
                 where j.CreatedUserId == Globals.userId
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
            post.CreatedUserId = Globals.userId;

            DB.Add(post);
            DB.SaveChanges();

            var jobPosts = from j in DB.Jobs
                where j.CreatedUserId == Globals.userId
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

            var job = DB.Jobs.SingleOrDefault(x => x.Id == id);
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

        public PartialViewResult ModalTest()
        {
            return new PartialViewResult
            {
                ViewName = "Partial",
            };
        }

        public IActionResult EditJobPost(int id, bool edit)
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            var job = DB.Jobs.SingleOrDefault(x => x.Id == id);
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
            var dbPost = DB.Jobs.First(x => x.Id == post.Id);

            dbPost.Title = post.Title;
            dbPost.Description = post.Description;
            dbPost.LevelId = post.LevelId;
            dbPost.Country = post.Country;
            dbPost.City = post.City;

            DB.Update(dbPost);
            DB.SaveChanges();

            var jobPosts = from j in DB.Jobs
                where j.CreatedUserId == Globals.userId
                select new JobPostViewModel
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    PostDate = j.PostDate
                };

            return View("Dashboard", new JobPostViewModel() { jobs = jobPosts });
        }

        public IActionResult DeleteJobPost(int id)
        {
            var post = DB.Jobs.Find(id);
            if (post != null)
            {
                DB.Jobs.Remove(post);
            }
            DB.SaveChanges();

            var jobPosts = from j in DB.Jobs
                                                    where j.CreatedUserId == Globals.userId
                                                    select new JobPostViewModel { 
                                                        Id = j.Id, 
                                                        Title = j.Title, 
                                                        Description = j.Description, 
                                                        PostDate = j.PostDate};

            return View("Dashboard", new JobPostViewModel(){jobs = jobPosts} );

        }

        public IActionResult Modal()
        {
            return PartialView("ModalView");
        }
    }
}
