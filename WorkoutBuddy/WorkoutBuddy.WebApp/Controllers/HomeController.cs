using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkoutBuddy.WebApp.Code.Base;
using WorkoutBuddy.BusinessLogic.Implementation;
using Microsoft.AspNetCore.Authorization;
using WorkoutBuddy.BusinessLogic.Implementation.Home;
using WorkoutBuddy.BusinessLogic.Implementation.Rating;

namespace WorkoutBuddy.WebApp.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly UserAccountService service;
        private readonly HomeService homeService;
        public HomeController(ControllerDependencies dependencies, UserAccountService service, HomeService homeService) : base(dependencies)
        {
            this.service = service;
            this.homeService = homeService;
        }

        public IActionResult Index()
        {
            var isUserOfTheWeek = service.IsUserOfTheWeek();
            ViewData["IsUserOfTheWeek"] = isUserOfTheWeek;
            var model = homeService.GetHomeItems();
            return View(model);
        }

    }
}