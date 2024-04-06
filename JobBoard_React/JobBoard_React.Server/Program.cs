using JobBoard_React.Server.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Azure SQL Configuration
var configuration = builder.Configuration;
builder.Services.AddDbContext<JobboardContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("AzureSQLConnection")));

// Session
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Added
app.UseSession();
app.UseStaticFiles();
app.UseRouting();

app.MapControllers();

app.MapFallbackToFile("/index.html");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

Directory.CreateDirectory("Uploads/Resumes");
Directory.CreateDirectory("Uploads/MotivationLetters");

app.Run();
