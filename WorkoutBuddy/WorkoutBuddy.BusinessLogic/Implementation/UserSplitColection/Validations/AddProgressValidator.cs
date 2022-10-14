using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutBuddy.BusinessLogic.Implementation.UserSplitColection.Models;
using WorkoutBuddy.DataAccess;

namespace WorkoutBuddy.BusinessLogic.Implementation.UserSplitColection
{
    public class AddProgressValidator : AbstractValidator<UserWorkoutModel>
    {
        private readonly UnitOfWork uow;
        public AddProgressValidator(UnitOfWork uow)
        {
            this.uow = uow;
            RuleFor(r => r.Date)
                .Must(SameDay).WithMessage("You already added a progress today!")
                .Must(CheckDate).WithMessage("You cannot add a progress older than 7 days");

            RuleFor(r => r.Exercises)
              .Must(e => e != null && e.Count > 0).WithMessage("you cannot add a progress without some data in it!");
        }

        
        private bool CheckDate(DateTime date)
        {
            var res = (DateTime.UtcNow - date).TotalDays;
            return res < 7;
        }
        private bool SameDay(UserWorkoutModel model, DateTime date)
        {
            
            var userWorkouts = uow.UserWorkouts.Get()
                                .Where(us => us.Id.IduserNavigation.Iduser == model.UserId)
                                .ToList();
            var todaysWorkouts = new List<Guid>();
            foreach(var workout in userWorkouts)
            {
                if ((date - workout.Date).Days == 0)
                    todaysWorkouts.Add(workout.Idworkout);
            }
            return !todaysWorkouts.Contains(model.WorkoutId);
        }
    }
}
