using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutBuddy.BusinessLogic.Implementation.UserSplitColection.Models
{
    public class ExercisesProgressModel
    {
        public Guid WorkoutId { get; set; }
        public string ExerciseName { get; set; }
        public List<DateProgressModel> Days { get; set; }
        public int? MaxSets { get; set; }

    }
}
