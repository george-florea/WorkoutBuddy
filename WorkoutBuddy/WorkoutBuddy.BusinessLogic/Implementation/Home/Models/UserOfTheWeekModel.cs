using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutBuddy.BusinessLogic.Implementation.Home
{
    public class UserOfTheWeekModel
    {
        public string Username { get; set; }
        public int PointsNo { get; set; }
        public List<string> SplitsNames { get; set; }
    }
}
