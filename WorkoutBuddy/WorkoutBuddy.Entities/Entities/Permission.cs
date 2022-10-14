using System;
using System.Collections.Generic;

namespace WorkoutBuddy.Entities
{
    public partial class Permission
    {
        public Permission()
        {
            Idroles = new HashSet<Role>();
        }

        public short Idpermission { get; set; }
        public string? PermissionDescription { get; set; }

        public virtual ICollection<Role> Idroles { get; set; }
    }
}
