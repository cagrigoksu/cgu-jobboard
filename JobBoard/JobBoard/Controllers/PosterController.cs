using System.Collections;
using JobBoard.DataContext;
using JobBoard.Enums;
using Microsoft.AspNetCore.Mvc;
using JobBoard.Models.Classes;
using JobBoard.Models.Data;
using JobBoard.Models.View;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

namespace JobBoard.Controllers
{
    public class PosterController:Controller
    {
        private readonly AppDbContext _context;

        public PosterController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult  Dashboard()
        {
             if (new SessionUtils().EmptySession())
             {
                 return View("LogIn");
             }
             
             return View(_context.Jobs.Where(x => x.CreatedUserId == HttpContext.Session.GetInt32("Id")));
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
            post.City = "test";
            post.Country = "test";
            post.PostDate = DateTime.Now;
            post.LevelId = Convert.ToInt32(JobLevelEnum.EntryLevel);
            post.CreatedUserId = Globals.userId;

            _context.Add(post);
            _context.SaveChanges();

            return View("Dashboard", _context.Jobs.Where(x => x.CreatedUserId == Globals.userId));
        }


        public IActionResult DetailJobPost(int? id, bool detail)
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var job = _context.Jobs.SingleOrDefault(x => x.Id == id);
                var result = new JobPostViewModel()
                {
                    Detail = detail,
                    Id = job.Id,
                    Title = job.Title,
                    Description = job.Description
                };
                return View("AddJobPost",result);


            }
        }

        public IActionResult EditJobPost(int? id, bool edit)
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var job = _context.Jobs.SingleOrDefault(x => x.Id == id);
                var result = new JobPostViewModel()
                {
                    Edit = edit,
                    Id = job.Id,
                    Title = job.Title,
                    Description = job.Description
                };
                return View("AddJobPost", result);


            }
        }

        [HttpPost]
        public IActionResult EditJobPost(JobPostViewModel post)
        {
            var dbPost = _context.Jobs.First(x => x.Id == post.Id);

            dbPost.Title = post.Title;
            dbPost.Description = post.Description;

            _context.Update(dbPost);
            _context.SaveChanges();
            return View("Dashboard", _context.Jobs.Where(x => x.CreatedUserId == Globals.userId));
        }

        public IActionResult DeleteJobPost(int id)
        {
            var post = _context.Jobs.Find(id);
            if (post != null)
            {
                _context.Jobs.Remove(post);
            }
            _context.SaveChanges();
            return View("Dashboard", _context.Jobs.Where(x=>x.CreatedUserId == HttpContext.Session.GetInt32("Id")));

        }
    }
}
