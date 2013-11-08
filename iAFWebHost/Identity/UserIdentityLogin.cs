using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Identity
{
    public class UserIdentityLogin
    {
        public UserIdentityLogin()
        {
        }

        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual UserIdentity User { get; set; }
        public virtual string UserId { get; set; }
    }
}