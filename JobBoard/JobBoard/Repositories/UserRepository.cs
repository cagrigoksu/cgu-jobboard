using JobBoard.DataContext;
using JobBoard.Models.Data;
using JobBoard.Repositories.Interfaces;

namespace JobBoard.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public UserDataModel GetUser(string email, string pwd)
        {
            var user = _db.Users.
                FirstOrDefault(x => x.Email == email && x.Password == pwd);

            return user;
        }

        public UserProfileDataModel GetUserProfile(UserProfileDataModel userProfile)
        {
            var profile = _db.UserProfiles.FirstOrDefault(x => x.UserId == userProfile.Id);

            return profile;
        }

        public void AddUser(UserDataModel user)
        {
            user.LogOnDate = DateTime.Now;
            _db.Add(user);
            _db.SaveChanges();
        }

        public void AddUserProfile(UserProfileDataModel userProfile)
        {
            userProfile.LastEditDate = DateTime.Now;
            _db.Add(userProfile);
            _db.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}
