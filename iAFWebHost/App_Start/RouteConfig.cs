using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace iAFWebHost
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("v1/{*pathInfo}");

            routes.MapRoute(
                name: "QR",
                url: "qr/{id}/{scale}",
                defaults: new { controller = "Home", action = "Qr",id = UrlParameter.Optional, scale = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Tag",
                url: "tag/{id}",
                defaults: new { controller = "Home", action = "Tag" }
            );

            routes.MapRoute(
                name: "Host",
                url: "site/{id}",
                defaults: new { controller = "Home", action = "Site" }
            );

            routes.MapRoute(
                name: "User",
                url: "user/{username}",
                defaults: new { controller = "Account", action = "UserProfile" }
            );

            routes.MapRoute(
                name: "Go",
                url: "{id}",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
