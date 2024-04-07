using JobBoard.DataContext;
using JobBoard.Repositories.Interfaces;

namespace JobBoard.Repositories
{
    public class DBUtilsRepository : IDBUtilsRepository
    {
        private readonly AppDbContext _db;
        public DBUtilsRepository(AppDbContext db)
        {
            _db = db;
        }

        public bool DBConnectionCheck()
        {
            var job = _db.Users.First();
            try
            {
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
    }
}
