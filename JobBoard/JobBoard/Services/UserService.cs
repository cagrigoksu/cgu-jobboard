using JobBoard.Models.Data;
using JobBoard.Repositories.Interfaces;
using JobBoard.Services.Interfaces;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JobBoard.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDataModel GetUser(string email, string pwd)
        {
            var user = _userRepository.GetUser(email, pwd);

            return user;
        }

        public UserProfileDataModel GetUserProfile(UserProfileDataModel profile)
        {
            var user = _userRepository.GetUserProfile(profile);

            return user;
        }

        public void AddUser(UserDataModel user)
        {
            _userRepository.AddUser(user);
        }

        public void AddUserProfile(UserProfileDataModel profile)
        {
            _userRepository.AddUserProfile(profile);
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}
