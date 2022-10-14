using Microsoft.EntityFrameworkCore;
using WorkoutBuddy.DataAccess;
using WorkoutBuddy.WebApp.Code;
using WorkoutBuddy.WebApp.Code.ExtensionMethods;
using WorkoutBuddy.BusinessLogic.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(GlobalExceptionFilterAttribute));
    options.Filters.Add(typeof(CheckUserDetailsFilter));
});

builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(BaseService).Assembly);
//builder.Configuration.AddJsonFile("appsettings.json", false, true);

builder.Services.AddDbContext<ProiectAcademieContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDbContext<WorkoutBuddyContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<UnitOfWork>();

builder.Services.AddWorkoutBuddyCurrentUser();

builder.Services.AddPresentation();
builder.Services.AddWorkoutBuddyBusinessLogic();


builder.Services.AddAuthentication("WorkoutBuddyCookies")
       .AddCookie("WorkoutBuddyCookies", options =>
       {
           options.AccessDeniedPath = new PathString("/Home");
           options.LoginPath = new PathString("/UserAccount/Login");
       });
        


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LandingPage}/{action=Index}/{id?}");

app.Run();
