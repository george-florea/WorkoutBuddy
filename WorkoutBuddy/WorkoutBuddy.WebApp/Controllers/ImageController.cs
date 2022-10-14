using Microsoft.AspNetCore.Mvc;
using WorkoutBuddy.BusinessLogic.Implementation;
using WorkoutBuddy.WebApp.Code.Base;

namespace WorkoutBuddy.WebApp.Controllers
{
    public class ImageController : BaseController
    {
        private readonly ImageService service;
        public ImageController(ControllerDependencies dependencies, ImageService service) : base(dependencies)
        {
            this.service = service;
        }

        public IActionResult GetImgContent(Guid id)
        {
            var model = service.GetImgContent(id);

            return File(model, "image/jpg");
        }
    }
}
