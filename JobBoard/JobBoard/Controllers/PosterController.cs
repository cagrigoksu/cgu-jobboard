using System.Collections;
using JobBoard.DataContexts;
using JobBoard.Enums;
using Microsoft.AspNetCore.Mvc;
using JobBoard.Models.Classes;
using JobBoard.Models.Data;
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

             return View(_context.Jobs.ToList());
        }

        public IActionResult AddJobPost()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddJobPost(JobPostDataModel post)
        {
            post.City = "test";
            post.Country = "test";
            post.PostDate = DateTime.Now;
            post.LevelId = Convert.ToInt32(JobLevelEnum.EntryLevel);

            _context.Add(post);
            _context.SaveChanges();

            return View("Dashboard", _context.Jobs.ToList());
        }

        public IActionResult DeleteJobPost(int id)
        {
            var post = _context.Jobs.Find(id);
            if (post != null)
            {
                _context.Jobs.Remove(post);
            }
            _context.SaveChanges();
            return View("Dashboard", _context.Jobs.ToList());

        }
    }
}
