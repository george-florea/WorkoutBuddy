using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutBuddy.BusinessLogic.Implementation.UserSplitColection.Models
{
    public class UserExerciseModel
    {
        public Guid ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public int? SetsNo { get; set; }
        public int? ExerciseType { get; set; }
        public List<SetModel> Sets { get; set; }
    }
}
