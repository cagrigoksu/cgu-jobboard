using JobBoard.DataContexts;
using Microsoft.AspNetCore.Mvc;

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
            return View(_context.Jobs.ToList());
        }
    }
}
