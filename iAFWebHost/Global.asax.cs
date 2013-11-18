using iAFWebHost.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NLog.Config;
using iAFWebHost.Services;
using iAFWebHost.Entities;

namespace iAFWebHost
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AppHost.Start();

            // Register custom NLog Layout renderers
            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("utc_date", typeof(iAFWebHost.NLog.UtcDateRenderer));
            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("web_variables", typeof(iAFWebHost.NLog.WebVariablesRenderer));

            MvcHandler.DisableMvcResponseHeader = true;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var httpContext = ((MvcApplication)sender).Context;
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
            var currentController = " ";
            var currentAction = " ";

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null && !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                    currentController = currentRouteData.Values["controller"].ToString();

                if (currentRouteData.Values["action"] != null && !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                    currentAction = currentRouteData.Values["action"].ToString();
            }

            var ex = Server.GetLastError();
            var controller = new ErrorController();
            var routeData = new RouteData();

            var actionPrefix = "";
            if (httpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                actionPrefix = "Json";
            
            var action = actionPrefix + "Index";

            if (ex is HttpException)
            {
                var httpEx = ex as HttpException;
                switch (httpEx.GetHttpCode())
                {
                    case 404:
                        action = actionPrefix + "NotFound";
                        break;
                    default:
                        action = actionPrefix + "Index";
                        break;
                }
            }
            else
            {
                // we don't want to log http errors due to URL spam
                // also skip exceptions generated at the serivce level
                // we already logged them earlier
                if (ex.GetType() != typeof(ServiceError))
                {
                    ILogger logger = new LogService();
                    logger.Error(ex);
                }
            }

            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
            httpContext.Response.TrySkipIisCustomErrors = true;
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;

            controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }
    }
}
