using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutBuddy.BusinessLogic.Implementation.Rating;
using WorkoutBuddy.BusinessLogic.Implementation.Rating.Models;
using WorkoutBuddy.WebApp.Code.Base;

namespace WorkoutBuddy.WebApp.Controllers
{
    [Authorize]
    public class RatingController : BaseController
    {
        private readonly RatingService service;
        private readonly IConfiguration configuration;
        public RatingController(ControllerDependencies dependencies, RatingService service, IConfiguration configuration) : base(dependencies)
        {
            this.service = service;
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = service.GetRanking();
            ViewData["startDate"] = service.StartOfWeek(DateTime.Now, DayOfWeek.Monday).ToString("dd/MM/yyyy");
            ViewData["endDate"] = service.EndOfWeek(DateTime.Now, DayOfWeek.Sunday).ToString("dd/MM/yyyy");
            return View(model);
        }

        [HttpPost]
        public IActionResult PostRating([FromBody]AddRateModel model)
        {
            var isValid = service.PostRating(model.Rating, Guid.Parse(model.SplitId), CurrentUser.Id, Int32.Parse(configuration["RatingMultiplier"]));
            if (isValid)
            {
                return Json(new { message = "Thank you for the rate!", itemClass = "text-success" });
            }
            else
            {
                return Json(new { message = "Something went wrong! Please try again", itemClass = "text-danger" });
            }
        }
    }
}
