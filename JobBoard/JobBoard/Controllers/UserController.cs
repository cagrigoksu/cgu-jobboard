using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobBoard.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobBoard.Models;
using JobBoard.Models.Data;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace JobBoard.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext DB;

        public UserController(ILogger<HomeController> logger, AppDbContext context)
        {
            DB = context;
            _logger = logger;
        }
        
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogIn(string email, string pwd)
        {
            var user = DB.Users.
                FirstOrDefault(x => x.Email == email && x.Password == pwd);
            if (user != null)
            {
                Globals.userId=user.Id;
                HttpContext.Session.SetInt32("Id", user.Id);
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetInt32("CompanyUser", Convert.ToInt32(user.CompanyUser));

                var jobPosts = new HomeController(_logger,DB).BringAllJobs();
                return View("Index", new IndexViewModel(){JobPosts = jobPosts});
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogOn(IFormCollection formCollection)
        {
            UserDataModel user = new()
            {
                Email = formCollection["email"],
                Password = formCollection["pwd"],
                CompanyUser = formCollection["hiring"] == "on"
            };

            if (user.Password == formCollection["pwdConf"])
            {   
                user.LogOnDate = DateTime.Now;
                DB.Add(user);
                DB.SaveChanges();

                return Redirect("/Home/Index");
            }
            else
            {
                return View("Index", new IndexViewModel());
            }
        }

        public IActionResult LogOut()
        {
            // HttpContext.Session.Remove("Id");
            // HttpContext.Session.Remove("Email");
            HttpContext.Session.Clear();

            return View("LogIn");
        }

        // // GET: User
        // public async Task<IActionResult> Index()
        // {
        //     return View(await _context.Users.ToListAsync());
        // }

        // // GET: User/Details/5
        // public async Task<IActionResult> Details(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var userDataModel = await _context.Users
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (userDataModel == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(userDataModel);
        // }

        // // GET: User/Create
        // public IActionResult Create()
        // {
        //     return View();
        // }

        // // POST: User/Create
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("Id,Email,Password,CompanyUser,LogOnDate,Deleted,DeleteDate")] UserDataModel userDataModel)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Add(userDataModel);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(userDataModel);
        // }

        // // GET: User/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var userDataModel = await _context.Users.FindAsync(id);
        //     if (userDataModel == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(userDataModel);
        // }

        // // POST: User/Edit/5
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password,CompanyUser,LogOnDate,Deleted,DeleteDate")] UserDataModel userDataModel)
        // {
        //     if (id != userDataModel.Id)
        //     {
        //         return NotFound();
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(userDataModel);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!UserDataModelExists(userDataModel.Id))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(userDataModel);
        // }

        // // GET: User/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var userDataModel = await _context.Users
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (userDataModel == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(userDataModel);
        // }

        // // POST: User/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var userDataModel = await _context.Users.FindAsync(id);
        //     if (userDataModel != null)
        //     {
        //         _context.Users.Remove(userDataModel);
        //     }

        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }

        // private bool UserDataModelExists(int id)
        // {
        //     return _context.Users.Any(e => e.Id == id);
        // }
    }
}
