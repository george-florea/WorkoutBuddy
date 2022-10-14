using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutBuddy.BusinessLogic.Implementation.Rating
{
    public class RankingListItem
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public int PointsNo { get; set; }
    }
}
