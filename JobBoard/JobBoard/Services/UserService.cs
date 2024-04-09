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

        public UserDataModel GetUser(string email)
        {
            var user = _userRepository.GetUser(email);

            return user;
        }

        public bool IsUserExist(string email)
        {
            return _userRepository.IsUserExist(email);
        }

        public UserProfileDataModel GetUserProfile(int userId)
        {
            var user = _userRepository.GetUserProfile(userId);

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

        public void EditUserProfile(UserProfileDataModel userProfile)
        {
            _userRepository.EditUserProfile(userProfile);
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}
