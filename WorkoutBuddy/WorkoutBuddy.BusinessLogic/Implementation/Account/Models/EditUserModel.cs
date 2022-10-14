using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutBuddy.BusinessLogic.Implementation.Account.Models
{
    public class EditUserModel
    {
        public EditUserModel()
        {
            Roles = new List<int>();
        }

        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public List<int>? Roles { get; set; }
    }
}
