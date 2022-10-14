using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using WorkoutBuddy.Common.Exceptions;
using WorkoutBuddy.WebApp.Models;

namespace WorkoutBuddy.WebApp.Code
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionFilterAttribute> logger;
        private readonly IModelMetadataProvider modelMetadataProvider;

        public GlobalExceptionFilterAttribute(ILogger<GlobalExceptionFilterAttribute> logger, IModelMetadataProvider modelMetadataProvider)
        {
            this.logger = logger;
            this.modelMetadataProvider = modelMetadataProvider;
        }

        public override void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;

            switch (context.Exception)
            {
                case NotFoundErrorException notFound:
                    context.Result = new ViewResult
                    {
                        ViewName = "Views/Shared/Error_NotFound.cshtml",
                        ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(modelMetadataProvider, context.ModelState)
                        {
                            Model = new ExceptionModel()
                            {
                                Message = notFound.Message
                            }
                        }
                    };

                    break;
                case UnauthorizedAccessException unauthorizedAccess:
                    context.Result = new ViewResult
                    {
                        ViewName = "Views/LandingPage/Index.cshtml"
                    };

                    break;
                case ValidationErrorException validationError:
                    foreach (var validationResult in validationError.ValidationResult.Errors)
                    {
                        context.ModelState.AddModelError(validationResult.PropertyName, validationResult.ErrorMessage);
                    }

                    var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                    context.Result = new ViewResult
                    {
                        ViewName = $"Views/{descriptor.ControllerName}/{descriptor.ActionName}.cshtml",
                        ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(modelMetadataProvider, context.ModelState)
                        {
                            Model = validationError.Model
                        }
                    };
                    break;
                case ForbiddenErrorException forbiden:
                    context.Result = new ViewResult
                    {
                        ViewName = "Views/Shared/Error_Forbidden.cshtml",
                        ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(modelMetadataProvider, context.ModelState)
                        {
                            Model = new ExceptionModel()
                            {
                                Message = forbiden.Message
                            }
                        }
                    };
                    break;
                default:
                    context.Result = new ViewResult
                    {
                        ViewName = "Views/Shared/Error_InternalServerError.cshtml"
                    };
                    break;

            }
        }
    }

    public class UserIsDisabledModel
    {
        public bool UserIsDisabled { get; set; }
    }
}
