using JobBoard.Models.Data;

namespace JobBoard.Repositories.Interfaces
{
    public interface IUserRepository
    {
        UserDataModel GetUser(string email, string pwd);
        UserProfileDataModel GetUserProfile(int userId);
        void AddUser(UserDataModel user);
        void AddUserProfile(UserProfileDataModel profile);
        void EditUserProfile(UserProfileDataModel userProfile);
        void DeleteUser(int id);
    }
}
