using WorkoutBuddy.Common.DTOs;

namespace WorkoutBuddy.WebApp.Code.Base
{
    public class ControllerDependencies
    {
        public CurrentUserDto CurrentUser { get; set; }

        public ControllerDependencies(CurrentUserDto currentUser)
        {
            CurrentUser = currentUser;
        }
    }
}
