using Microsoft.AspNetCore.Mvc.Filters;
using System.Web.Mvc;
using WorkoutBuddy.BusinessLogic.Implementation;
using WorkoutBuddy.Common.DTOs;
using WorkoutBuddy.Common.Exceptions;
using WorkoutBuddy.WebApp.Code.Utils;
using ActionFilterAttribute = Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute;

namespace WorkoutBuddy.WebApp.Code
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CheckUserDetailsFilter : ActionFilterAttribute, IFilterMetadata
    {
        private readonly UserAccountService Service;
        private readonly CurrentUserDto CurrentUser;
        private readonly AuthenticationUtils utils;

        public CheckUserDetailsFilter(UserAccountService service, CurrentUserDto currentUser)
        {
            Service = service;
            CurrentUser = currentUser;
            utils = new AuthenticationUtils();
        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Method == HttpMethods.Get)
            {
                var needsUpdate = Service.UserNeedsUpdate(CurrentUser);
                if (needsUpdate)
                {
                    utils.LogOut(filterContext.HttpContext).Wait();
                    var user = Service.UpdateUserLoginDate(CurrentUser.Id);
                    if (!user.IsDisabled)
                    {
                        utils.LogIn(user, filterContext.HttpContext).Wait();
                    }
                    else
                    {
                        utils.LogIn(user, filterContext.HttpContext).Wait();
                        utils.LogOut(filterContext.HttpContext).Wait();
                        throw new UnauthorizedAccessException();
                    }
                }

            }
            base.OnActionExecuting(filterContext);
        }
        //public override async Task OnActionExecutionAsync(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    if (context.HttpContext.Request.Method == HttpMethods.Get)
        //    {
        //        var needsUpdate = Service.UserNeedsUpdate(CurrentUser);
        //        if (needsUpdate)
        //        {
        //            utils.LogOut(context.HttpContext).Wait();
        //            var user = Service.UpdateUserLoginDate(CurrentUser.Id);
        //            if (!user.IsDisabled)
        //            {
        //                utils.LogIn(user, context.HttpContext).Wait();
        //            }
        //        }

        //    }
        //    await base.OnActionExecutionAsync(context, next);
        //}
    }
}
