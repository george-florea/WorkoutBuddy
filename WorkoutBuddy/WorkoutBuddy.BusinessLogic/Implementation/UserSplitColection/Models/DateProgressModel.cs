using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutBuddy.BusinessLogic.Implementation.UserSplitColection.Models
{
    public class DateProgressModel
    {
        public DateTime? Date { get; set; }
        public int? SetsNo { get; set; }
        public List<SetModel> Sets { get; set; }
        public decimal ExerciseCoef { get; set; }
    }
}
