using Microsoft.EntityFrameworkCore;
using JobBoard.Models.Data;

namespace JobBoard.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserDataModel> Users { get; set; }

        public DbSet<JobPostDataModel> JobPosts { get; set; }
        public DbSet<JobApplicationDataModel> JobApplications { get; set; }
        public DbSet<UserProfileDataModel> UserProfiles {get;set;}

    }
}
