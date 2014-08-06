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
                defaults: new { controller = "Home", action = "Qr", id = UrlParameter.Optional, scale = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Email",
                url: "email",
                defaults: new { controller = "Email", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Catchall",
                url: "email/catchall",
                defaults: new { controller = "Email", action = "catchall", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Inbox",
                url: "email/{id}",
                defaults: new { controller = "Email", action = "Inbox", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Mailgun",
                url: "mailgun/gateway",
                defaults: new { controller = "Mailgun", action = "Gateway" }
            );

            routes.MapRoute(
                name: "Ad",
                url: "ad/{id}",
                defaults: new { controller = "Home", action = "Ad" }
            );

            routes.MapRoute(
                name: "Stats",
                url: "stats/{id}",
                defaults: new { controller = "Home", action = "Stats", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Requests",
                url: "requests/{id}",
                defaults: new { controller = "Home", action = "Requests", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Referrals",
                url: "referrals/{id}",
                defaults: new { controller = "Home", action = "Referrals", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Errors",
                url: "Errors",
                defaults: new { controller = "Error", action = "Errors" }
            );

            routes.MapRoute(
                name: "TraceUrl",
                url: "trace/{id}",
                defaults: new { controller = "Home", action = "Trace" }
            );

            routes.MapRoute(
                name: "About",
                url: "about",
                defaults: new { controller = "Home", action = "About" }
            );

            routes.MapRoute(
                name: "News",
                url: "news",
                defaults: new { controller = "Home", action = "News" }
            );

            routes.MapRoute(
                name: "Contact",
                url: "Contact",
                defaults: new { controller = "Home", action = "Contact" }
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
