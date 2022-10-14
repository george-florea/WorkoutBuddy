using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WorkoutBuddy.Common.DTOs;
using System;
using System.Linq;
using WorkoutBuddy.WebApp.Code.Base;
using WorkoutBuddy.BusinessLogic.Base;
using System.Security.Claims;
using WorkoutBuddy.BusinessLogic.Implementation.Comments;
using WorkoutBuddy.BusinessLogic.Implementation.UserSplitColection;
using WorkoutBuddy.BusinessLogic.Implementation;
using WorkoutBuddy.BusinessLogic.Implementation.Rating;
using WorkoutBuddy.BusinessLogic.Implementation.AdminDashboard;
using WorkoutBuddy.BusinessLogic.Implementation.Home;

namespace WorkoutBuddy.WebApp.Code.ExtensionMethods
{
    public static class ServiceCollectionExtensionMethods
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddScoped<ControllerDependencies>();

            return services;
        }

        public static IServiceCollection AddWorkoutBuddyBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<ServiceDependencies>();
            services.AddScoped<UserAccountService>();
            services.AddScoped<ExerciseService>();
            services.AddScoped<SplitService>();
            services.AddScoped<ImageService>();
            services.AddScoped<CommentService>();
            services.AddScoped<UserSplitService>();
            services.AddScoped<RatingService>();
            services.AddScoped<AdminService>();
            services.AddScoped<HomeService>();
            return services;
        }

        public static IServiceCollection AddWorkoutBuddyCurrentUser(this IServiceCollection services)
        {
            services.AddScoped(s =>
            {
                var accessor = s.GetService<IHttpContextAccessor>();
                var httpContext = accessor.HttpContext;
                var claims = httpContext.User.Claims;

                var userIdClaim = claims?.FirstOrDefault(c => c.Type == "Id")?.Value;
                var isParsingSuccessful = Guid.TryParse(userIdClaim, out Guid id);
                var usernameClaim = claims?.FirstOrDefault(c => c.Type == "UserName")?.Value;
                var isDisabledClaim = claims?.FirstOrDefault(c => c.Type == "IsDisabled")?.Value;
                var nameClaim = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var emailClaim = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var rolesClaim = claims?.Where(c => c.Type == ClaimTypes.Role)?.Select(c => c.Value).ToList();

                //var activeRoleId = claims?.FirstOrDefault(c => c.Type == "ActiveRoleId")?.Value;

                //var newClaims = new List<Claim>
                //{
                //    new Claim("ActiveRoleId", "3")
                //};

                //var appIdentity = new ClaimsIdentity(newClaims);
                //httpContext.User.AddIdentity(appIdentity);

                return new CurrentUserDto
                {
                    Id = id,
                    IsAuthenticated = httpContext.User.Identity.IsAuthenticated,
                    Name = nameClaim,
                    Username = usernameClaim,
                    Email = emailClaim,
                    Roles = rolesClaim,
                    IsDisabled = isDisabledClaim == "True"
                };
            });

            return services;
        }
    }
}
