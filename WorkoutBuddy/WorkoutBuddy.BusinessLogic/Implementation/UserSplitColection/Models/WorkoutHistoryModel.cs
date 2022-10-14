using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutBuddy.BusinessLogic.Implementation.UserSplitColection.Models
{
    public class WorkoutHistoryModel
    {
        public WorkoutHistoryModel()
        {
            Exercises = new List<ExerciseHistoryModel>();
        }
        public Guid WorkoutId { get; set; }
        public DateTime Date { get; set; }
        public List<ExerciseHistoryModel>? Exercises { get; set; }

    }
}
