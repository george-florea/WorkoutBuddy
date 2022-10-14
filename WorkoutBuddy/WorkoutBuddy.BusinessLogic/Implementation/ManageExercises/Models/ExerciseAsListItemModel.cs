using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutBuddy.BusinessLogic.Implementation.Exercises.Models
{
    public class ExerciseAsListItemModel
    {
        public Guid ExerciseId { get; set; }
        public Guid? IdImage { get; set; }
        public string Name { get; set; }
        public string ExerciseType { get; set; }
    }
}
