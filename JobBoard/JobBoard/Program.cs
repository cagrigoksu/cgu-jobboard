using JobBoard.DataContext;
using JobBoard.Repositories;
using JobBoard.Repositories.Interfaces;
using JobBoard.Services;
using JobBoard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Azure SQL and PostgreSQL Configuration
var configuration = builder.Configuration;
// AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("AzureSQLConnection")));

// Session
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// HttpClient
builder.Services.AddHttpClient();

builder.Services.AddMvc();

builder.Services.AddScoped<IDBUtilsRepository, DBUtilsRepository>();

builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IJobPosterRepository, JobPosterRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();
builder.Services.AddScoped<IJobPosterService, JobPosterService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISecurityService, SecurityService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

System.IO.Directory.CreateDirectory("./wwwroot/Uploads/Resumes/Application");
System.IO.Directory.CreateDirectory("./wwwroot/Uploads/Resumes/Profile");
System.IO.Directory.CreateDirectory("./wwwroot/Uploads/MotivationLetters/Application");
System.IO.Directory.CreateDirectory("./wwwroot/Uploads/MotivationLetters/Profile");
app.Run();
