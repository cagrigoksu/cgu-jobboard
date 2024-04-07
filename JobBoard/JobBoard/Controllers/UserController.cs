using Microsoft.AspNetCore.Mvc;
using JobBoard.Models.Data;
using JobBoard.Models.View;
using JobBoard.Models.Classes;
using JobBoard.Repositories.Interfaces;

namespace JobBoard.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository? _userRepository;
        private readonly IJobPostRepository? _jobPostRepository;

        public UserController(IUserRepository userRepository, IJobPostRepository jobPostRepository)
        {
            _userRepository = userRepository;
            _jobPostRepository = jobPostRepository;
        }
        
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogIn(string email, string pwd)
        {
            var user = _userRepository.GetUser(email, pwd);

            if (user != null)
            {
                Globals.UserId = user.Id;
                Globals.CompanyUser = user.CompanyUser;
                HttpContext.Session.SetInt32("Id", user.Id);
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetInt32("CompanyUser", Convert.ToInt32(user.CompanyUser));

                var jobPosts = _jobPostRepository.GetAllJobPosts();
                return View("Index", new IndexViewModel(){UserId = user.Id, CompanyUser = user.CompanyUser,JobPosts = jobPosts});
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
            if (formCollection["pwd"] == formCollection["pwdConf"])
            {
                var user = new UserDataModel()
                {
                    Email = formCollection["email"],
                    Password = formCollection["pwd"],
                    CompanyUser = formCollection["hiring"] == "on",
                };

                _userRepository.AddUser(user);

                var userResult = _userRepository.GetUser(user.Email, user.Password);
                
                if (userResult != null)
                {
                    Globals.UserId = user.Id;
                    HttpContext.Session.SetInt32("Id", user.Id);
                    HttpContext.Session.SetString("Email", user.Email);
                    HttpContext.Session.SetInt32("CompanyUser", Convert.ToInt32(user.CompanyUser));

                    var jobPosts = _jobPostRepository.GetAllJobPosts();
                    return View("Index", new IndexViewModel() { JobPosts = jobPosts });
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return View("LogOn");
            }
        }

        public IActionResult UserProfile()
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            var result = new UserProfileViewModel();

            var profile = _userRepository.GetUserProfile(new UserProfileDataModel(){Id = Globals.UserId });

            if (profile != null)
            {
                result.Name = profile.Name;
                result.Surname = profile.Surname;
            }

            return View(result);
        }

        [HttpPost]
        public IActionResult UserProfile(UserProfileViewModel model)
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            var profile = new UserProfileDataModel();
            profile.UserId = Globals.UserId;
            profile.Name  = model.Name;
            profile.Surname = model.Surname;

            _userRepository.AddUserProfile(profile);

            return View(model);
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
