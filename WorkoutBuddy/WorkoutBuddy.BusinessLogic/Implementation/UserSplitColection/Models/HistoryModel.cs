using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutBuddy.BusinessLogic.Implementation.UserSplitColection.Models
{
    public class HistoryModel
    {
        public Guid WorkoutId { get; set; }
        public List<DateTime> Dates { get; set; }
        public List<decimal?> CoefList { get; set; }
        public WorkoutHistoryModel FirstWorkout { get; set; }
    }
}
