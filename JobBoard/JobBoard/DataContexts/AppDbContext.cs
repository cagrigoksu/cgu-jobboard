using Microsoft.EntityFrameworkCore;
using System;
using JobBoard.Models.Data;

namespace JobBoard.DataContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserDataModel> Users { get; set; }

    }
}
