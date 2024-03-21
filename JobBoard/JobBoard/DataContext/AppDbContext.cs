using Microsoft.EntityFrameworkCore;
using System;
using JobBoard.Models.Data;

namespace JobBoard.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserDataModel> Users { get; set; }

        public DbSet<JobPostDataModel> Jobs { get; set; }

    }
}
