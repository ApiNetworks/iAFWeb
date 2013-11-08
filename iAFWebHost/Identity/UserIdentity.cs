using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Identity
{
    public class UserIdentity : IUser
    {
        public string Id
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }
    }

}