using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WorkoutBuddy.BusinessLogic.Implementation.UserSplitColection;
using WorkoutBuddy.BusinessLogic.Implementation.UserSplitColection.Models;
using WorkoutBuddy.WebApp.Code.Base;

namespace WorkoutBuddy.WebApp.Controllers
{
    [Authorize]
    public class UserSplitController : BaseController
    {
        private readonly UserSplitService service;
        private readonly IConfiguration Configuration;
        public UserSplitController(ControllerDependencies dependencies, UserSplitService service, IConfiguration configuration) : base(dependencies)
        {
            this.service = service;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            var idUser = CurrentUser.Id;
            var model = service.GetListOfSplits(idUser);
            return View(model);
        }

        public IActionResult ViewUserSplit(Guid id)
        {
            var model = service.GetUserSplit(id, CurrentUser.Id);
            return View(model);
        }

        [HttpGet]
        public IActionResult AddProgress(Guid id)
        {
            var model = service.PopulateUserWorkoutModel(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult AddProgress(UserWorkoutModel model)
        {
            model.UserId = CurrentUser.Id;
            var splitId = service.AddProgress(model, Int32.Parse(Configuration["NoOfPoints"]));
            var splitModel = service.GetUserSplit(splitId, CurrentUser.Id);

            return View("ViewUserSplit", splitModel);
        }

        [HttpGet]
        public IActionResult WorkoutHistory(Guid id)
        {
            var model = service.GetHistory(id, CurrentUser.Id);
            ViewData["DatesNo"] = Int32.Parse(Configuration["NoOfDates"]);
            return View(model);
        }

        [HttpPost]
        public IActionResult GetDates([FromBody]DatesModel model)
        {
            var dates = service.GetDates(model.Index, Guid.Parse(model.WorkoutId), CurrentUser.Id, Int32.Parse(Configuration["NoOfDates"]));
            return Ok(dates);
        }

        [HttpGet]
        public IActionResult GetHistory(HistoryRequestModel model)
        {
            var workoutId = Guid.Parse(model.WorkoutId);
            var date = DateTime.Parse(model.Date);
            var workout = service.GetHistoryFor(workoutId, date, CurrentUser.Id);
            return Ok(workout);
        }

        [HttpGet]
        public IActionResult ExercisesProgress(Guid id, int index)
        {
            var model = service.GetProgress(id, CurrentUser.Id, index, Int32.Parse(Configuration["NoOfDates"]));
            var x = service.ComputeNoOfPages(id, CurrentUser.Id, Int32.Parse(Configuration["NoOfDates"]));
            ViewData["pagesNo"] = x - 1;
            ViewData["index"] = index;
            return View(model);
        }

        [HttpPost]
        public IActionResult RemoveUserSplit(Guid id)
        {
            service.RemoveSplit(id, CurrentUser.Id);
            return RedirectToAction("Index");
        }
    }
}
