using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkoutBuddy.BusinessLogic.Implementation;
using WorkoutBuddy.BusinessLogic.Implementation.Exercises.Models;
using WorkoutBuddy.BusinessLogic.Implementation.ManageExercises.Models;
using WorkoutBuddy.Common.DTOs;
using WorkoutBuddy.Entities.Enums;
using WorkoutBuddy.WebApp.Code.Base;

namespace WorkoutBuddy.WebApp.Controllers
{
    [Authorize]
    public class ExerciseController : BaseController
    {
        private readonly ExerciseService exerciseService;
        private readonly IConfiguration Configuration;
        public ExerciseController(ControllerDependencies dependencies, ExerciseService service, IConfiguration configuration) : base(dependencies)
        {
            exerciseService = service;
            Configuration = configuration;
        }

        [HttpGet]
        public IActionResult ExercisesList(int index = 0)
        {
            var exercisesList = exerciseService.GetExercises(index, Int32.Parse(Configuration["ExercisesPageSize"]));
            ViewData["NoOfPages"] = exerciseService.NumberOfPages(Int32.Parse(Configuration["ExercisesPageSize"]));
            ViewData["Index"] = index;

            return View(exercisesList);
        }

        public IActionResult ViewExercise(Guid id)
        {
            var exercise = exerciseService.GetExercise(id);
            return View(exercise);
        }

        public IActionResult AddExercise()
        {
            AddExerciseModel model = PopulateAddExerciseModel();
            return View(model);
        }


        [HttpPost]
        public IActionResult AddExercise([FromForm] AddExerciseModel model)
        {
            exerciseService.AddExercise(model);
            return RedirectToAction("ExercisesList", "Exercise");
        }

        
        public IActionResult EditExercise(Guid id)
        {
            var model = exerciseService.PopulateEditModel(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult EditExercise([FromForm] EditExerciseModel model)
        {
            exerciseService.EditExercise(model);
            return RedirectToAction("ExercisesList", "Exercise");
        }

        [HttpPost]
        public IActionResult DeleteExercise(Guid id)
        {
            exerciseService.DeleteExercise(id);
            return RedirectToAction("ExercisesList", "Exercise");
        }

        [HttpPost]
        public IActionResult ApproveExercise([FromBody]string id)
        {
            exerciseService.ApproveExercise(Guid.Parse(id));
            return Ok();
        }

        [HttpPost]
        public IActionResult RejectExercise([FromBody] string id)
        {
            exerciseService.DeleteExercise(Guid.Parse(id));
            return Ok();
        }

        private static AddExerciseModel PopulateAddExerciseModel()
        {
            var exerciseTypes = Enum.GetValues(typeof(ExerciseTypes)).Cast<ExerciseTypes>()
                            .Select(v => new ListItemModel<string, int>
                            {
                                Text = v.ToString(),
                                Value = ((int)v)
                            }).ToList();
            var muscleGroups = Enum.GetValues(typeof(MuscleGroups)).Cast<MuscleGroups>()
                .Select(v => new System.Web.Mvc.SelectListItem()
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString(),
                }).ToList();
            var model = new AddExerciseModel()
            {
                ExerciseTypes = exerciseTypes,
                MuscleGroups = muscleGroups
            };
            return model;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetExercisesByMuscleGroups(List<string> selectedMusclesString)
        {
            var exercises = exerciseService.GetFilteredExercises(selectedMusclesString);
            return Ok(exercises);
        }


    }
}
