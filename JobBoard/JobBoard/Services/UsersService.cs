using JobBoard.DataContexts;
using JobBoard.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Services
{
    public class UsersService
    {
        private readonly AppDbContext _context;

        public UsersService(AppDbContext context)
        {
            _context = context;
        }
        public string LogOnUser(UserDataModel user)
        {
            user.LogOnDate = DateTime.Now;
            _context.Users.Add(user);

            return "selam";

        }

    }
}
