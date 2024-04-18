using Microsoft.AspNetCore.Mvc;
using JobBoard.Models.Data;
using JobBoard.Models.View;
using JobBoard.Models.Classes;
using JobBoard.Services.Interfaces;
using Newtonsoft.Json;

namespace JobBoard.Controllers
{
    public class UserController : Controller
    {
        private readonly IJobPosterService? _jobPosterService;
        private readonly IWebHostEnvironment? _webHostEnvironment;

        private readonly IHttpClientFactory? _httpClientFactory;
        

        public UserController(IJobPosterService? jobPosterService, IWebHostEnvironment? webHostEnvironment, IHttpClientFactory? httpClientFactory)
        {
            _jobPosterService = jobPosterService;
            _webHostEnvironment = webHostEnvironment;
            _httpClientFactory = httpClientFactory;
        }


        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(string email, string pwd)
        {
            // prepare form-data
            IEnumerable<KeyValuePair<string, string>> content = new List<KeyValuePair<string, string>>
            {
                new("email", email),
                new("password", pwd)
            };

            // create client and post 
            var apiClient = _httpClientFactory.CreateClient("api-gateway");

            var result = apiClient.PostAsync("gateway/User/login", new FormUrlEncodedContent(content)).Result;

            if (result.IsSuccessStatusCode)
            {
                var data = await result.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserDataModel>(data);
                Globals.UserId = user.Id;
                Globals.CompanyUser = user.CompanyUser;
                HttpContext.Session.SetInt32("Id", user.Id);
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetInt32("CompanyUser", Convert.ToInt32(user.CompanyUser));
            
                // get job posts 
                var jobPosts = _jobPosterService.GetAllJobPostsByPage(1);
                return View("Index",
                    new IndexViewModel()
                        { UserId = user.Id, CompanyUser = user.CompanyUser, JobPosts = jobPosts, PageNumber = 1 });
            }
            else
            {
                ModelState.Clear();
                ViewBag.Message = "Incorrect email or password.";
                return View();
            }
        }

        public IActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOn(IFormCollection formCollection)
        {
            // prepare data
            IEnumerable<KeyValuePair<string, string>> content = new List<KeyValuePair<string, string>>
            {
                new("email", formCollection["email"]),
                new("password", formCollection["pwd"]),
                new("passwordconf", formCollection["pwdConf"])
            };

            // create client and post
            var apiClient = _httpClientFactory.CreateClient("api-gateway");
            var result = apiClient.PostAsync("gateway/User/logon", new FormUrlEncodedContent(content)).Result;

            if (result.IsSuccessStatusCode)
            {
                var data = await result.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserDataModel>(data);

                Globals.UserId = user.Id;
                Globals.CompanyUser = user.CompanyUser;
                HttpContext.Session.SetInt32("Id", user.Id);
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetInt32("CompanyUser", Convert.ToInt32(user.CompanyUser));

                // get job posts 
                var jobPosts = _jobPosterService.GetAllJobPostsByPage(1);
                return View("Index", new IndexViewModel() { JobPosts = jobPosts, PageNumber = 1 });
            }

            ModelState.Clear();
            ViewBag.Message = "Error: Please check your details.";
            return View("LogOn");
        }

        public async Task<IActionResult> UserProfile()
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            var result = new UserProfileViewModel();

            var apiClient = _httpClientFactory.CreateClient("api-gateway");
            var response = await apiClient.GetAsync($"gateway/User/get-user-profile/{Globals.UserId}").Result.Content.ReadAsStringAsync();
            var profile = JsonConvert.DeserializeObject<UserProfileDataModel>(response);
            if (profile != null)
            {
                result.Name = profile.Name;
                result.Surname = profile.Surname;
                result.PhoneNumber = profile.PhoneNumber;
                result.Email = profile.Email;
            };

            result.CompanyUser = Convert.ToBoolean(HttpContext.Session.GetInt32("CompanyUser"));
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> UserProfile(UserProfileViewModel model)
        {
            if (new SessionUtils().EmptySession())
            {
                return View("LogIn");
            }

            // Copy CV and ML files to server
            var urlResume = "";
            var urlMotivationLetter = "";

            if (model.CV != null)
            {
                string root = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
                string pathCV = Path.Combine(root, "Resumes\\Profile");

                var filenameCV = Globals.UserId.ToString() + ".pdf";

                using FileStream streamCV = new FileStream(Path.Combine(pathCV, filenameCV), FileMode.Create);

                model.CV.CopyTo(streamCV);

                urlResume = Path.Combine(pathCV, filenameCV);
            }
            if (model.MotivationLetter != null)
            {
                string root = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
                string pathML = Path.Combine(root, "MotivationLetters\\Profile");

                var filenameMotivation = Globals.UserId.ToString() + ".pdf";

                using FileStream streamML = new FileStream(Path.Combine(pathML, filenameMotivation), FileMode.Create);

                model.MotivationLetter.CopyTo(streamML);

                urlMotivationLetter = Path.Combine(pathML, filenameMotivation);
            }

            // http get user-profile
            var apiClient = _httpClientFactory.CreateClient("api-gateway");
            var userProfileResponse = apiClient.GetAsync($"gateway/User/get-user-profile/{Globals.UserId}").Result;

            var content = new List<KeyValuePair<string, string>>
            {
                new("UserId", Globals.UserId.ToString()),
                new("Name", model.Name),
                new("Surname", model.Surname),
                new("Email", model.Email),
                new("PhoneNumber", model.PhoneNumber)
            };

            if (urlResume != "")
                content.Add(new("UrlResume", urlResume));
            if (urlMotivationLetter != "")
                content.Add(new("UrlMotivationLetter", urlMotivationLetter));

            if (userProfileResponse.IsSuccessStatusCode)
            {
                // Update existing user profile
                var editResponse = apiClient.PostAsync("gateway/User/edit-user-profile", new FormUrlEncodedContent(content));

                if (editResponse.Result.IsSuccessStatusCode)
                {
                    ModelState.Clear();
                    ViewBag.Message = "Successfully edited.";
                    model.CompanyUser = Convert.ToBoolean(HttpContext.Session.GetInt32("CompanyUser"));

                    return View(model);
                }
            }
            else
            {
                // Add new user profile
                var addResponse = apiClient.PostAsync("gateway/User/add-user-profile", new FormUrlEncodedContent(content));

                if (addResponse.Result.IsSuccessStatusCode)
                {
                    ModelState.Clear();
                    ViewBag.Message = "Successfully saved.";
                    model.CompanyUser = Convert.ToBoolean(HttpContext.Session.GetInt32("CompanyUser"));

                    return View(model);
                }
            }

            // TODO: create error code list
            ModelState.Clear();
            ViewBag.Message = "Error occured while connecting to server.";
            model.CompanyUser = Convert.ToBoolean(HttpContext.Session.GetInt32("CompanyUser"));

            return View(model);

        }

        public IActionResult LogOut()
        {
            // HttpContext.Session.Remove("Id");
            // HttpContext.Session.Remove("Email");
            HttpContext.Session.Clear();

            return View("LogIn");
        }
       
    }
}
