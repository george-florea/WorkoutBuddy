using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutBuddy.BusinessLogic.Implementation;
using WorkoutBuddy.BusinessLogic.Implementation.AdminDashboard;
using WorkoutBuddy.WebApp.Code.Base;

namespace WorkoutBuddy.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly AdminService service;
        private readonly ExerciseService exerciseService;

        public AdminController(ControllerDependencies dependencies, AdminService service, ExerciseService _service) : base(dependencies)
        {
            this.service = service;
            this.exerciseService = _service;
        }

        public IActionResult Index()
        {
            var list = service.GetUsers();
            return View(list);
        }

        public IActionResult PendingExercises()
        {
            var list = exerciseService.GetPendingExercises();
            return View(list);
        }

    }
}
