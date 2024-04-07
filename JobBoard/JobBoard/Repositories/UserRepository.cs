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
                FirstOrDefault(x => x.Email == email && x.Password == pwd && x.IsDeleted == false);

            return user;
        }

        public UserProfileDataModel GetUserProfile(UserProfileDataModel userProfile)
        {
            var profile = _db.UserProfiles.FirstOrDefault(x => x.UserId == userProfile.Id && x.IsDeleted == false);

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
            var user = _db.Users.Find(id);

            user.IsDeleted = true;
            user.DeleteUser = Globals.UserId;
            user.DeleteDate = DateTime.Now;

            _db.Update(user);
            _db.SaveChanges();
        }
    }
}
