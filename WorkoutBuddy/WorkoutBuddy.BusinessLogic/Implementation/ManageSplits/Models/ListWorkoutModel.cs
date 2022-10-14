using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutBuddy.Common.DTOs;

namespace WorkoutBuddy.BusinessLogic.Implementation.ManageSplits.Models
{
    public class ListWorkoutModel
    {
        public string WorkoutName { get; set; }
        public List<string> ExercisesList { get; set; }
        
    }
}
