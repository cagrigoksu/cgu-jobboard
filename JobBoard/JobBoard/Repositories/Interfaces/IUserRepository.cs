using JobBoard.Models.Data;

namespace JobBoard.Repositories.Interfaces
{
    public interface IUserRepository
    {
        UserDataModel GetUser(string email);
        UserProfileDataModel GetUserProfile(int userId);
        void AddUser(UserDataModel user);
        void AddUserProfile(UserProfileDataModel profile);
        void EditUserProfile(UserProfileDataModel userProfile);
        void DeleteUser(int id);
        bool IsUserExist(string email);
    }
}
