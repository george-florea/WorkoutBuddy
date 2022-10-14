using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutBuddy.BusinessLogic.Implementation;
using WorkoutBuddy.BusinessLogic.Implementation.Comments.Models;
using WorkoutBuddy.BusinessLogic.Implementation.ManageSplits.Models;
using WorkoutBuddy.Entities.Enums;
using WorkoutBuddy.WebApp.Code.Attributes;
using WorkoutBuddy.WebApp.Code.Base;

namespace WorkoutBuddy.WebApp.Controllers
{
    [Authorize]
    public class SplitController : BaseController
    {
        private readonly SplitService service;
        private readonly UserAccountService userAccountService;
        public SplitController(ControllerDependencies dependencies, SplitService splitService, UserAccountService userAccountService) : base(dependencies)
        {
            this.service = splitService;
            this.userAccountService = userAccountService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = service.GetSplits();
            return View(model);
        }

        [HttpGet]
        public IActionResult AddSplit()
        {
            var muscleGroups = Enum.GetValues(typeof(MuscleGroups)).Cast<MuscleGroups>()
                .Select(v => new System.Web.Mvc.SelectListItem()
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString(),
                }).ToList();
            var model = new SplitModel()
            {
                MusclesGroups = muscleGroups
            };
            return View(model);
        }


        [HttpPost]
        public IActionResult AddSplit(SplitModel model)
        {
            model.CreatorId = CurrentUser.Id;
            service.AddSplit(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ViewSplit(Guid id)
        {
            var model = service.GetSplit(id);

            if(model == null)
            {
                return View("NotFound");
            }

            return View(model);
        }

        public IActionResult EditSplit(Guid id)
        {
            var model = service.PopulateSplitModel(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult EditSplit(SplitModel model)
        {
            model.CreatorId = CurrentUser.Id;
            service.EditSplit(model);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteSplit([FromBody]Guid id)
        {
            var isDeleted = service.DeleteSplit(id);
            return Ok(isDeleted);
        }

        [HttpPost]
        public IActionResult AddToUserSplits([FromBody]string id)
        {
            var splitId = Guid.Parse(id);
            var isValid = service.AddToUserSplits(splitId, CurrentUser.Id);
            if (isValid)
            {
                return Json(new { message = "Split successfully added to your colection!" });
            }
            else
            {
                return Json(new { message = "The split is already in your colection!" });
            }
        }

       
    }
}
