using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutBuddy.BusinessLogic.Implementation.ManageSplits.Models;

namespace WorkoutBuddy.BusinessLogic.Implementation.Home
{
    public class HomeModel
    {
        public UserOfTheWeekModel User { get; set; }
        public SplitListItem Split { get; set; }
    }
}
