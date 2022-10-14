using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutBuddy.DataAccess
{
    public class WorkoutBuddyContext : ProiectAcademieContext
    {
        public WorkoutBuddyContext()
        {
        }

        public WorkoutBuddyContext(DbContextOptions<ProiectAcademieContext> options) : base(options)
        {
        }
    }
}
