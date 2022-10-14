using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkoutBuddy.BusinessLogic.Implementation;
using WorkoutBuddy.BusinessLogic.Implementation.Account.Models;
using WorkoutBuddy.BusinessLogic.Implementation.Models;
using WorkoutBuddy.Common.DTOs;
using WorkoutBuddy.Entities.Enums;
using WorkoutBuddy.WebApp.Code.Base;
using WorkoutBuddy.WebApp.Code.Utils;

namespace WorkoutBuddy.WebApp.Controllers
{

    public class UserAccountController : BaseController
    {
        private readonly UserAccountService Service;
        private readonly AuthenticationUtils utils;

        public UserAccountController(ControllerDependencies dependencies, UserAccountService service)
           : base(dependencies)
        {
            Service = service;
            utils = new AuthenticationUtils();
        }
        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (model == null)
            {
                return View("Error_NotFound");
            }

            Service.RegisterNewUser(model);

            var user = Service.Login(model.Email, model.PasswordString);

            await utils.LogIn(user, HttpContext);

            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {

            var user = Service.Login(model.Email, model.Password);

            if (user.IsDisabled)
            {
                model.IsDisabled = true;
                return View(model);
            }

            if (!user.IsAuthenticated)
            {
                model.AreCredentialsInvalid = true;
                return View(model);
            }

            await utils.LogIn(user, HttpContext);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await utils.LogOut(HttpContext);

            return RedirectToAction("Index", "LandingPage");
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddWeight()
        {
            var model = Service.GetWeightHistory(CurrentUser.Id);
            model.UserId = CurrentUser.Id;
            return View(model);
        }

        [HttpPost]
        public IActionResult AddWeight(AddWeightModel model)
        {
            model.UserId = CurrentUser.Id;
            Service.AddWeight(model);
            return RedirectToAction("AddWeight");
        }

        [Authorize]
        public IActionResult ProfilePage()
        {
            var model = Service.GetUserInfo(CurrentUser.Id);
            return View(model);
        }

        [Authorize]
        public IActionResult EditProfile()
        {
            var model = Service.GetEditModel();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileModel model)
        {
            var newUsername = Service.EditProfile(model);
            if(newUsername != CurrentUser.Username)
            {
                var user = CurrentUser;
                await utils.LogOut(HttpContext);
                user.Username = newUsername;
                await utils.LogIn(user, HttpContext);
            }
            return RedirectToAction("ProfilePage");
        }


        [Authorize]
        public IActionResult EditUserProfile(Guid userId)
        {
            var model = Service.GetUserEditModel(userId);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserModel model)
        {
            Service.EditUserProfile(model);
            return RedirectToAction("Index", "Admin");
        }


        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangePassword([FromBody]PasswordChangeModel model)
        {

            var isValid = Service.ChangePassword(model);
            
            return Ok(isValid);
        }

        [Authorize]
        [HttpPost]
        public IActionResult VerifyOldPassword([FromBody]string oldPassword)
        {
            var isValid = Service.VerifyOldPassword(oldPassword);
            return Ok(isValid);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DisableUser([FromBody] string id)
        {
            var userId = Guid.Parse(id);
            if (userId == Guid.Empty)
            {
                userId = CurrentUser.Id;
            }
            var isDisabled = Service.ChangeAvailability(userId, true);
            return Ok(isDisabled);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult ActivateUser([FromBody] string id)
        {
            var userId = Guid.Parse(id);
            var isActivated = Service.ChangeAvailability(userId, false);
            return Ok(isActivated);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GrantRole()
        {
            var isEligible = Service.IsUserOfTheWeek();
            if (isEligible)
            {
                var user = CurrentUser;
                await utils.LogOut(HttpContext);
                user.Roles.Add(RoleTypes.UserOfTheWeek.ToString());
                await utils.LogIn(user, HttpContext);
            }
            return Ok(isEligible);
        }


        
    }
}
