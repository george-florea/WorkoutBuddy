using Microsoft.AspNetCore.Mvc;
using WorkoutBuddy.BusinessLogic.Implementation;
using WorkoutBuddy.BusinessLogic.Implementation.Comments;
using WorkoutBuddy.BusinessLogic.Implementation.Comments.Models;
using WorkoutBuddy.WebApp.Code.Base;

namespace WorkoutBuddy.WebApp.Controllers
{
    public class CommentController : BaseController
    {
        private readonly CommentService service;
        public CommentController(ControllerDependencies dependencies, CommentService commentService) : base(dependencies)
        {
            this.service = commentService;
        }

        [HttpPost]
        public IActionResult AddComment([FromBody]CommentModel model)
        {
            model.Author = CurrentUser.Username;
            service.AddComment(model);
            return Ok();
        }
        
        [HttpPost]
        public IActionResult DeleteComment([FromBody]string id)
        {
            var isDeleted = service.DeleteComment(Guid.Parse(id));

            return Ok(isDeleted);
        }
    }
}
