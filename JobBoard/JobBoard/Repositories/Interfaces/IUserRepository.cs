using JobBoard.Models.Data;

namespace JobBoard.Repositories.Interfaces
{
    public interface IUserRepository
    {
        UserDataModel GetUser(string email, string pwd);
        UserProfileDataModel GetUserProfile(UserProfileDataModel profile);
        void AddUser(UserDataModel user);
        void AddUserProfile(UserProfileDataModel profile);
        void DeleteUser(int id);
    }
}
