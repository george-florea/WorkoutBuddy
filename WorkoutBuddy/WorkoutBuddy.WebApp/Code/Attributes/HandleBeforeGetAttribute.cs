using WorkoutBuddy.BusinessLogic.Implementation;
using WorkoutBuddy.BusinessLogic.Implementation.UserSplitColection;
using WorkoutBuddy.Common.DTOs;

namespace WorkoutBuddy.WebApp.Code.Attributes
{

    [AttributeUsage(AttributeTargets.Method)]
    public class HandleBeforeGetAttribute : Attribute
    {
        private readonly UserAccountService Service;
        private readonly CurrentUserDto CurrentUser;
        private readonly HttpContext HttpContext;
        public HandleBeforeGetAttribute(UserAccountService service, HttpContext httpContext, CurrentUserDto currentUser)
        {
            Service = service;
            HttpContext = httpContext;
            CurrentUser = currentUser;


        }
    }
}
