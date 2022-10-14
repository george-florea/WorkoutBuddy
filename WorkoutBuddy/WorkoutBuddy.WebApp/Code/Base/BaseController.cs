using Microsoft.AspNetCore.Mvc;
using WorkoutBuddy.Common.DTOs;

namespace WorkoutBuddy.WebApp.Code.Base
{
    public class BaseController : Controller
    {
        protected readonly CurrentUserDto CurrentUser;

        public BaseController(ControllerDependencies dependencies)
            : base()
        {
            CurrentUser = dependencies.CurrentUser;
        }
    }
}
