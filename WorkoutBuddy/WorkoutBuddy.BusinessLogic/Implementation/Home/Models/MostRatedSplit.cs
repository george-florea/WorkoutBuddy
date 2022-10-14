using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutBuddy.BusinessLogic.Implementation.Home
{
    public class MostRatedSplit
    {
        public string SplitName { get; set; }
        public string Description { get; set; }
        public int WorkoutsNo { get; set; }
        public int UserSplitsNo { get; set; }
        public string Creator { get; set; }
        public float Rating { get; set; }

    }
}
