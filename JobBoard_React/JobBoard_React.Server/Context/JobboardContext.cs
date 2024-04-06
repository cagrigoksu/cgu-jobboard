using System;
using System.Collections.Generic;
using JobBoard_React.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobBoard_React.Server.Context;

public partial class JobboardContext : DbContext
{
    public JobboardContext()
    {
    }

    public JobboardContext(DbContextOptions<JobboardContext> options)
        : base(options)
    {
    }

    public DbSet<UserDataModel> Users { get; set; }

    public DbSet<JobPostDataModel> JobPosts { get; set; }
    public DbSet<JobApplicationDataModel> JobApplications { get; set; }
    public DbSet<UserProfileDataModel> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
