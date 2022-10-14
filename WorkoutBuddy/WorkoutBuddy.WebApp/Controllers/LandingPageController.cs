using Microsoft.AspNetCore.Mvc;
using WorkoutBuddy.WebApp.Code.Base;

namespace WorkoutBuddy.WebApp.Controllers
{
    public class LandingPageController : BaseController
    {
        public LandingPageController(ControllerDependencies dependencies) : base(dependencies)
        {
        }

        public IActionResult Index()
        {
            if (CurrentUser.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
