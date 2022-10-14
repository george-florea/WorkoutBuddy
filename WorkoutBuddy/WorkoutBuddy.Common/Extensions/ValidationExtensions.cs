using FluentValidation.Results;
using WorkoutBuddy.Common.Exceptions;

namespace WorkoutBuddy.Common.Extensions
{
    public static class ValidationExtensions
    {
        public static void ThenThrow(this ValidationResult result, object model)
        {
            if (!result.IsValid)
            {
                throw new ValidationErrorException(result, model);
            }
        }
    }
}
