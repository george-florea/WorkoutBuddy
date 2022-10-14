using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WorkoutBuddy.Common.DTOs;

namespace WorkoutBuddy.BusinessLogic.Implementation.ManageExercises.Models
{
    public class EditExerciseModel
    {
        public Guid ExerciseId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<ListItemModel<string, int>> ExerciseTypes { get; set; }
        public int SelectedType { get; set; }
        public List<SelectListItem> MuscleGroups { get; set; }
        public List<int>? SelectedMuscleGroups { get; set; }
        public IFormFile? Image { get; set; }
    }
}
