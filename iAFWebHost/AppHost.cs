using Funq;
using iAF.Entities;
using iAFWebHost.Api;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.Configuration;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost
{
    public class AppHost : AppHostBase
    {
        public AppHost() //Tell ServiceStack the name and where to find your web services
            : base("i.af rest services api v1.0", typeof(ApiService).Assembly) { }

        public override void Configure(Container container)
        {

        }

        public static void Start()
        {
            new AppHost().Init();
        }
    }
}