using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Identity
{
    public class UserIdentityClaim
    {
        public UserIdentityClaim()
        {
        }

        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }
        public virtual int Id { get; set; }
        public virtual UserIdentity User { get; set; }
    }
}