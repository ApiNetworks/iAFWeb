using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Identity
{
    public class RoleIdentity : IRole
    {
        public RoleIdentity()
        {
        }

        public RoleIdentity(string roleName)
        {
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}