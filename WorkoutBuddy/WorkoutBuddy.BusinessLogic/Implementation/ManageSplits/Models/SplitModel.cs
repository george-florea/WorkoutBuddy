using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WorkoutBuddy.BusinessLogic.Implementation.ManageSplits.Models
{
    public class SplitModel
    {
        public Guid SplitId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<SelectListItem>? MusclesGroups { get; set; }
        public Guid CreatorId { get; set; }
        public List<WorkoutModel>? Workouts { get; set; }
        public bool? IsPrivate { get; set; }
    }
}
