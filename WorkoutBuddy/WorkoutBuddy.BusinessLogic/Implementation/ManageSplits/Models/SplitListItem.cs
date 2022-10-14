using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutBuddy.BusinessLogic.Implementation.ManageSplits.Models
{
    public class SplitListItem
    {
        public Guid SplitId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int WorkoutsNo { get; set; }
        public float Rating { get; set; }
        public string CreatorName { get; set; }
    }
}
