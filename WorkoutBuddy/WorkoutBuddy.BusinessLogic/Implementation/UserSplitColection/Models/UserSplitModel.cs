using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutBuddy.BusinessLogic.Implementation.UserSplitColection.Models
{
    public class UserSplitModel
    {
        public Guid Iduser { get; set; }
        public string SplitName { get; set; }
        public string Description { get; set; }
        public Guid Idsplit { get; set; }
        public int? Rating { get; set; }
        public List<WorkoutsListModel> Workouts { get; set; }
    }
}
