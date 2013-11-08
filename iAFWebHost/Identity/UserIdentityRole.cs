using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Identity
{
    public class UserIdentityRole
    {
        public UserIdentityRole()
        {

        }

        public virtual RoleIdentity Role { get; set; }
        public virtual string RoleId { get; set; }
        public virtual UserIdentity User { get; set; }
        public virtual string UserId { get; set; }
    }
}